using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Controls;


namespace SqLiteEditor
{
    class classHelper
    {
        private DataBase db = new DataBase();
        private int intEffected = 0;
        private string strError = "";

        public DataSet BringAlltables(string strConnection)
        {

            DataSet dsSysTables = new DataSet();
            DataColumn[] keys = new DataColumn[1];

            StringBuilder sbSql = new StringBuilder();

            // Bring only users TABLEs (VIEWs)
            sbSql.Append(@"SELECT tbl_name  FROM sqlite_master 
                             WHERE type IN('table', 'view') AND tbl_name  NOT LIKE 'sqlite_%'
                            
                            UNION ALL
                             
                            SELECT tbl_name  FROM sqlite_temp_master
                             WHERE type IN('table', 'view') 
                            
                            ORDER BY 1");
            try
            {
                dsSysTables = db.FindDataSet(ref sbSql, strConnection, out intEffected, out strError);
                keys[0] = dsSysTables.Tables[0].Columns["TNAME"];
                dsSysTables.Tables[0].PrimaryKey = keys;
                dsSysTables.AcceptChanges();
            }
            catch
            {
                dsSysTables = null;
            }

            return dsSysTables;
        }

        public DataSet BringSql(string strConnection, string strTableName)
        {

            DataSet dsSqlTables = new DataSet();
            DataColumn[] keys = new DataColumn[1];
            int intEffected = 0;

            StringBuilder sbSql = new StringBuilder();

            // Bring schema for table ...
            sbSql.Append(@"SELECT sql, type  FROM sqlite_master WHERE tbl_name =""" + strTableName + @""" AND type IN('table', 'view') "); // Do not insert extra space...

            try
            {
                dsSqlTables = db.FindDataSet(ref sbSql, strConnection, out intEffected, out strError);
                dsSqlTables.AcceptChanges();
            }
            catch
            {
                dsSqlTables = null;
            }

            return dsSqlTables;
        }

        public string BringAlterSql(string strConnection, string strTableName)
        {
            DataSet ds = BringSql(strConnection, strTableName);
            string strCreate = ds.Tables[0].Rows[0][0].ToString() + ";";
            string strType = ds.Tables[0].Rows[0][1].ToString().ToUpper();
            string strOldTableName = strTableName + "_old";
            string strColumnsName = BringColumnNameString(strConnection, strTableName);

            string strAlter = "";

            if (strType == "TABLE")
            {
                strAlter = " PRAGMA foreign_keys=off; " + Environment.NewLine; ;
                strAlter += " BEGIN TRANSACTION; " + Environment.NewLine; ;
                strAlter += @" ALTER TABLE """ + strTableName + @""" RENAME TO """ + strOldTableName + @""" ; " + Environment.NewLine + Environment.NewLine;
                strAlter += strCreate + Environment.NewLine + Environment.NewLine;
                strAlter += @" INSERT INTO """ + strTableName + @""" (" + strColumnsName + ") " + Environment.NewLine;
                strAlter += " SELECT  " + strColumnsName + Environment.NewLine;
                strAlter += @"   FROM """ + strOldTableName + @""";" + Environment.NewLine + Environment.NewLine;
                strAlter += " COMMIT; " + Environment.NewLine;
                strAlter += " PRAGMA foreign_keys=on;" + Environment.NewLine + Environment.NewLine;

                strAlter += @"DROP TABLE IF EXISTS """ + strOldTableName + @""";";
            }
            else
            {
                strAlter = @" DROP VIEW IF EXISTS """ + strTableName + @""";" + Environment.NewLine;
                strAlter += strCreate;
            }
            return strAlter;
        }

        public string BringSelectSql(string strConnection, string strTableName)
        {
            string strColumnsName = BringColumnNameString(strConnection, strTableName);

            string strSelect = "";
            strSelect = Environment.NewLine; ;
            strSelect += @" SELECT " + Environment.NewLine;
            strSelect += strColumnsName.Replace(",", ", " + Environment.NewLine) + Environment.NewLine;
            strSelect += " FROM " + Environment.NewLine;
            strSelect += @"""" + strTableName + @"""" + Environment.NewLine;
            strSelect += " -- WHERE [CONDITION]" + Environment.NewLine;
            return strSelect;
        }

        //http://www.tutorialspoint.com/sqlite/sqlite_insert_query.htm
        public string BringInsertSql(string strConnection, string strTableName)
        {
            string strColumnsName = BringColumnNameString(strConnection, strTableName);
            string strInsert = "";

            strInsert += @" INSERT INTO """ + strTableName + @""" (" + Environment.NewLine;
            strInsert += " (" + Environment.NewLine;
            strInsert += strColumnsName.Replace(",", ", " + Environment.NewLine) + Environment.NewLine;
            strInsert += " )" + Environment.NewLine;
            strInsert += " VALUES (value1, value2, value3,...valueN); " + Environment.NewLine;

            return strInsert;
        }
        //http://www.tutorialspoint.com/sqlite/sqlite_update_query.htm
        public string BringUpdateSql(string strConnection, string strTableName)
        {
            string strColumnsName = BringColumnNameString(strConnection, strTableName);
            string strUpdate = "";

            strUpdate = Environment.NewLine; ;
            strUpdate += @"UPDATE  " + @"""" + strTableName + @"""" + Environment.NewLine;
            strUpdate += "SET " + Environment.NewLine;
            strUpdate += (strColumnsName.Replace(",", " = '...',") + "  = '...' ").Replace(",", "," + Environment.NewLine) + Environment.NewLine;
            strUpdate += "WHERE [CONDITION]" + Environment.NewLine;

            return strUpdate;
            ;
        }

        //http://www.tutorialspoint.com/sqlite/sqlite_delete_query.htm
        public string BringDeleteSql(string strConnection, string strTableName)
        {
            string strColumnsName = BringColumnNameString(strConnection, strTableName);
            string strDelete = "";

            strDelete = Environment.NewLine; ;
            strDelete += @"DELETE FROM  " + @"""" + strTableName + @"""" + Environment.NewLine;
            strDelete += "WHERE " + Environment.NewLine;
            strDelete += (strColumnsName.Replace(",", " = '...',") + "  = '...' ").Replace(",", " AND " + Environment.NewLine) + Environment.NewLine;
            strDelete += "WHERE [CONDITION]" + Environment.NewLine;

            return strDelete;
            ;
        }


        public string BringColumnNameString(string strConnection, string strTableName) // one line with comma separate 
        {
            int intEffected = 0;
            StringBuilder sbSql = new StringBuilder(@"SELECT * FROM """ + strTableName + @"""  WHERE 1<>1;");
            DataSet dsColumns = new DataSet();

            try
            {
                dsColumns = db.FindDataSet(ref sbSql, strConnection, out intEffected, out strError);
                dsColumns.AcceptChanges();
            }
            catch
            {
                dsColumns = null;
            }

            string strColumns = "";
            if (dsColumns != null)
            {
                foreach (DataColumn dc in dsColumns.Tables[0].Columns)
                {
                    strColumns += @"""" + dc.ColumnName + @""" ,";
                }
            }
            strColumns = strColumns.TrimEnd(',', ' ');

            return strColumns;
        }

        public string createCreateTableSQLscript(DataTable dt, bool blnPK)
        {
            //https://www.sqlite.org/datatype3.html 

            //NULL. The value is a NULL value.
            //INTEGER. The value is a signed integer, stored in 1, 2, 3, 4, 6, or 8 bytes depending on the magnitude of the value.
            //REAL. The value is a floating point value, stored as an 8-byte IEEE floating point number.
            //TEXT. The value is a text string, stored using the database encoding (UTF-8, UTF-16BE or UTF-16LE).
            //BLOB. The value is a blob of data, stored exactly as it was input.

            var tableStruct = new List<Tuple<string, string, string, int>>(); // columnName, Type, Length, isNull>

            string strTableName = dt.TableName;

            string strColumnPK = "";
            foreach (DataColumn dc in dt.Columns)
            {

                string strColumnName = @"""" + dc.ColumnName + @"""";

                if (dc.ColumnName.IndexOf("PK_") == 0) { strColumnPK += strColumnName + " ,"; }

                string strType = "";
                string strNull = "";

                bool isNull = false;  // if found one NULL - it will be true;
                bool isInt = true;   // if found one NOT integer - false 
                bool isReal = true;   // if found one NOT real - false 

                int iMax = 0;
                bool blnObj = false;  //use to indicate then logic comes into ( obj != null )

                // bool isBlob = false;  // for thousands bytes it will be true; 

                foreach (DataRow r in dt.Rows)
                {
                  
                    object obj = (object)r[dc.ColumnName];

                    if (obj == null) isNull = true;

                    if (obj != null)
                    {
                        blnObj = true;
                        blnObj = true; // if false type will be TEXT only...
                        string s = obj.ToString().Trim();

                        if (s.Trim() == "") isNull = true;
                        if (s.Length > iMax) iMax = s.Length;

                        int iOut = 0;
                        if (s.Trim() != "" && !int.TryParse(s, out iOut)) isInt = false;

                        float floatOut = 0;
                        if (s.Trim() != "" && !float.TryParse(s, out floatOut)) isReal = false;
                    }
                }

                if (!isNull) strNull = "NOT NULL";
                if (blnObj && isReal) strType = "REAL";
                if (blnObj && isInt && isReal) strType = "INTEGER";
                if (!isInt && !isReal) strType = "TEXT";
                if (!blnObj) strType = "TEXT"; // if type was not determinate from data - it must be TEXT only ...

                var newT = new Tuple<string, string, string, int>(strColumnName, strType, strNull, iMax);
                tableStruct.Add(newT);

            }
            string strSQL = "PRAGMA foreign_keys=off;" + Environment.NewLine;
            strSQL += "BEGIN TRANSACTION;" + Environment.NewLine;

            strSQL += @"DROP TABLE IF EXISTS """ + strTableName + @""";" + Environment.NewLine;

            string strPrimary = @" ""PK_" + strTableName + @"""";


            strSQL += @"CREATE TABLE """ + strTableName + @""" (" + Environment.NewLine;
            if (!blnPK) { strSQL += strPrimary + " INTEGER  NOT NULL," + Environment.NewLine; }

            foreach (var t in tableStruct)
            {
                strSQL += t.Item1 + "  " + t.Item2 + "  " + t.Item3 + "," + Environment.NewLine; ;
            }
            if (blnPK)
            { strSQL += "PRIMARY KEY (" + strColumnPK.Substring(0, strColumnPK.Length - 1) + " ASC) );" + Environment.NewLine; }
            else {  strSQL += "PRIMARY KEY (" + strPrimary + " ASC) );" + Environment.NewLine;}
            strSQL += Environment.NewLine;

            strSQL += "COMMIT; " + Environment.NewLine;
            strSQL += "PRAGMA foreign_keys=on;" + Environment.NewLine;

            return strSQL;
        }


        public int intMaxPKIndex(string strConnection, string strTableName, out int intEffected, out string strError)
        {
            long longMax = 0;

            strError = " ";
            intEffected = 0;
            StringBuilder sb = new StringBuilder();

            sb.Append(@"SELECT MAX(PK_" + strTableName + @") FROM """ + strTableName + @"""");

            try
            {
                DataTable dt = db.FindDataSet(ref sb, strConnection, out intEffected, out strError).Tables[0];
                longMax = dt.Rows[0][0] is DBNull ? 0 : (long)dt.Rows[0][0];
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                longMax = 0;
            }
            return (int)longMax;
        }

        public string createCreateInsertSQLscriptAutoIncriment(DataTable dtEntier, int intPK = 0) //if needed append ... MAX + 1;
        {

            string strEntireCode = "";    // below code build INSERT by block from 100 rows ... Need to combine together ...
            DataTable dt = dtEntier.Clone(); // Must Clone Schema ..

            while (dtEntier.Rows.Count > 0)
            {
                dt.Clear(); // Clear for using NEXT 100 rows ...

                int iLocalCount = 0;

                foreach (DataRow dr in dtEntier.Rows)    // Split  dtEntier by smaller DT (100 rows)
                {
                    dt.ImportRow(dr); //Imports (copies) the row from the original table to the new one
                    dtEntier.Rows[iLocalCount].Delete(); ////Marks row for deletion
                    if (iLocalCount > 100) break;
                    iLocalCount++;
                }
                dtEntier.AcceptChanges(); //Removes the rows we marked for deletion 

                string strSqlInsert = @"INSERT INTO """ + dt.TableName + @""" " + Environment.NewLine;
                string strSqlPK = @"""PK_" + dt.TableName + @""" ";

                string strColumnNames = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    strColumnNames += @"""" + dc.ColumnName + @""",";
                }
                strColumnNames = strColumnNames.Substring(0, strColumnNames.Length - 1);

                strSqlInsert = strSqlInsert + "( " + strSqlPK + ", " + strColumnNames + ") " + Environment.NewLine + Environment.NewLine;


                string stringAllData = "";

                foreach (DataRow r in dt.Rows)
                {
                    string strCurrentData = "SELECT " + intPK.ToString() + " AS " + strSqlPK + ", ";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string strData = r[dc.ColumnName].ToString().Trim();
                        strCurrentData += (strData == "" ? "NULL " : "'" + r[dc.ColumnName].ToString().Replace("'", "''") + "'") + @" AS """ + dc.ColumnName + @""",";
                    }
                    strCurrentData = strCurrentData.Substring(0, strCurrentData.Length - 1) + Environment.NewLine;
                    stringAllData += strCurrentData + Environment.NewLine;
                    stringAllData += "UNION" + Environment.NewLine;
                    intPK++;
                }
                stringAllData = stringAllData.Substring(0, stringAllData.Length - 5 - 2); // UNION - 5 / NewLine -2

                strSqlInsert += stringAllData + ";" + Environment.NewLine + Environment.NewLine;
                strEntireCode += strSqlInsert;

            }
            return strEntireCode;
        }

        public string createCreateInsertSQLscriptBaseOnPK(DataTable dtEntier) //if needed append ... MAX + 1;
        {

            string strEntireCode = "";    // below code build INSERT by block from 100 rows ... Need to combine together ...
            DataTable dt = dtEntier.Clone(); // Must Clone Schema ..

            while (dtEntier.Rows.Count > 0)
            {
                dt.Clear(); // Clear for using NEXT 100 rows ...

                int iLocalCount = 0;

                foreach (DataRow dr in dtEntier.Rows)    // Split  dtEntier by smaller DT (100 rows)
                {
                    dt.ImportRow(dr); //Imports (copies) the row from the original table to the new one
                    dtEntier.Rows[iLocalCount].Delete(); ////Marks row for deletion
                    if (iLocalCount > 100) break;
                    iLocalCount++;
                }
                dtEntier.AcceptChanges(); //Removes the rows we marked for deletion 

                string strSqlInsert = @"INSERT INTO """ + dt.TableName + @""" " + Environment.NewLine;

                string strColumnNames = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    strColumnNames += @"""" + dc.ColumnName + @""",";
                }
                strColumnNames = strColumnNames.Substring(0, strColumnNames.Length - 1);

                strSqlInsert = strSqlInsert + "( " + strColumnNames + ") " + Environment.NewLine + Environment.NewLine;


                string stringAllData = "";

                foreach (DataRow r in dt.Rows)
                {
                    string strCurrentData = "SELECT " ;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string strData = r[dc.ColumnName].ToString().Trim();
                        strCurrentData += (strData == "" ? "NULL " : "'" + r[dc.ColumnName].ToString().Replace("'", "''") + "'") + @" AS """ + dc.ColumnName + @""",";
                    }
                    strCurrentData = strCurrentData.Substring(0, strCurrentData.Length - 1) + Environment.NewLine;
                    stringAllData += strCurrentData + Environment.NewLine;
                    stringAllData += "UNION" + Environment.NewLine;
                }
                stringAllData = stringAllData.Substring(0, stringAllData.Length - 5 - 2); // UNION - 5 / NewLine -2

                strSqlInsert += stringAllData + ";" + Environment.NewLine + Environment.NewLine;
                strEntireCode += strSqlInsert;

            }
            return strEntireCode;
        }

    }
}
