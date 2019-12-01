using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Globalization;

namespace WpfHelperCopyData
{
    class OleDbDataBase
    {

        private System.Data.OleDb.OleDbConnection conn;

        private int executeSqlNonQuery(StringBuilder sbSql, string strConnection)
        {
            int intEffected = -1;  // return number rows effected 

            try
            {
                conn = new System.Data.OleDb.OleDbConnection(strConnection);
                conn.Open();
            }
            catch (Exception ex)
            {
                string msg = "Open Connection Exception / Check Connection String.";
                throw new Exception(msg, ex);

            }

            try
            {
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(sbSql.ToString(), conn);
                intEffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            return intEffected;

        }



        private DataSet getDataSet(ref StringBuilder sbSql, string strConnection, out int intEffected)
        {
            intEffected = -1;

            try
            {
                conn = new System.Data.OleDb.OleDbConnection(strConnection);
                conn.Open();
            }
            catch (Exception ex)
            {
                string msg = "Open Connection Exception / Check Connection String.";
                throw new Exception(msg, ex);
            }

            DataSet dataSet = new DataSet();
            try
            {
                System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter();
                dbAdapter.SelectCommand = new System.Data.OleDb.OleDbCommand(sbSql.ToString(), conn);
                dbAdapter.FillLoadOption = LoadOption.PreserveChanges;
                intEffected = dbAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dataSet;

        }


        private DataTable getDataReaderSet(ref StringBuilder sbSql, string strConnection)
        { 
            try
            {
                conn = new System.Data.OleDb.OleDbConnection(strConnection);
                conn.Open();
            }
            catch (Exception ex)
            {
                string msg = "Open Connection Exception / Check Connection String.";
                throw new Exception(msg, ex);
            }

            DataTable dataTable = new DataTable();
            try
            {
                System.Data.OleDb.OleDbCommand sqlCmd = new System.Data.OleDb.OleDbCommand(sbSql.ToString(),conn);
                System.Data.OleDb.OleDbDataReader dbReader;
                dbReader = sqlCmd.ExecuteReader();
                dataTable.Load(dbReader);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dataTable;

        }

    
        private DataSet getFillSchema(ref StringBuilder sbSql, string strConnection)
        {
            try
            {
                conn = new System.Data.OleDb.OleDbConnection(strConnection);
                conn.Open();
            }
            catch (Exception ex)
            {
                string msg = "Open Connection Exception / Check Connection String.";
                throw new Exception(msg, ex);
            }

            DataSet dataSet = new DataSet();
            try
            {
                System.Data.OleDb.OleDbDataAdapter dbAdapter = new System.Data.OleDb.OleDbDataAdapter();
                dbAdapter.SelectCommand = new System.Data.OleDb.OleDbCommand(sbSql.ToString(), conn);
                dbAdapter.FillSchema(dataSet, SchemaType.Source);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dataSet;

        }



        /// <summary>
        /// COMP MSSQL MySQL ORA
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnection"></param>
        /// <param name="strDBname"></param>
        /// <returns></returns>
        public int ExecuteSqlNonQuery(StringBuilder sbSql, string strConnection)
        {
            try
            {
                return executeSqlNonQuery(sbSql, strConnection);
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }

        /// <summary>
        /// COMP MSSQL MySQL ORA
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnection"></param>
        /// <param name="strDBname"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataSet FindDataSet(StringBuilder sbSql, string strConnection, out int intEffected)
        {
            intEffected = 0;

            try
            {
                  return getDataSet(ref sbSql, strConnection, out intEffected);
            }
            catch (Exception ex)
            {
                throw(ex);
            }


        }


        public DataSet FindDataSetWithReader(StringBuilder sbSql, string strConnection)
        {

            try
            {

                string strNotCommented = "";
                foreach (string str in sbSql.ToString().Split('\n'))
                {
                    int intFirstComment = str.IndexOf("--");
                    if (intFirstComment >= 0)
                    {
                        strNotCommented += str.Substring(0, intFirstComment) + Environment.NewLine;
                    }
                    else
                    {
                        strNotCommented += str + Environment.NewLine;
                    }
                }
                
                DataSet ds = new DataSet();
                foreach (string str in strNotCommented.Split(';'))
                {
                    string strTmp = str.Replace(Environment.NewLine, " ").Replace(';',' ').Trim();
                    if (strTmp !="")
                    {
                       StringBuilder sb = new StringBuilder(str);
                       ds.Tables.Add(getDataReaderSet(ref sb,strConnection));         
                    }
                }                    

                return ds;

             //   return getDataSet(ref sbSql, strConnection, out intEffected);
            }
            catch (Exception ex)
            {
                throw(ex);
            }


        }
        public DataSet FindSchema(StringBuilder sbSql, string strConnection)
        {
            try
            {
                return getFillSchema(ref sbSql, strConnection);
            }
            catch
            {
                return null;
            }


        }






        /// <summary>
        /// /// COMP MSSQL MySQL ORA
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnection"></param>
        /// <param name="strDBname"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public List<StringBuilder> FindStringSB(StringBuilder sbSql, string strConnection, out string strEffected)
        {
            DataSet ds = new DataSet();
            List<StringBuilder> listSB = new List<StringBuilder>();

            int intEffected = 0;
            strEffected = "";


            try
            {
                ds = getDataSet(ref sbSql, strConnection, out intEffected);
            }
            catch
            {
                throw;
            }

            int intRow = 0;
            int intClmn = 0;

            for (int iTab = 0; iTab < ds.Tables.Count; iTab++)
            {
                //-------Table Name-------------------------------------------------------------------------------
                DataTable dt = ds.Tables[iTab];

                listSB.Add(new StringBuilder(""));
                listSB.Add(new StringBuilder("Table [" + iTab.ToString() + "]"));
                listSB.Add(new StringBuilder(""));

                strEffected += "Table [" + iTab.ToString() + "]" + "  " + dt.Rows.Count + "  rows" + "\n";

                try
                {                                                     //prevent exception - if NOTHING
                    intRow = dt.Rows.Count;
                    intClmn = dt.Columns.Count;
                }
                catch
                {
                }

                //-------MAX-------------------------------------------------------------------------------------

                int[] intMax = new int[intClmn];
                for (int i = 0; i < intClmn; i++)
                {
                    intMax[i] = dt.Columns[i].ColumnName.Length;
                }

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < intClmn; i++)
                    {
                        int intTmp = (row[i].ToString()).Length;
                        intMax[i] = intTmp > intMax[i] ? intTmp : intMax[i];
                    }
                }

                //--------Columns Name---------------------------------------------------------------------------

                {
                    StringBuilder sb = new StringBuilder("");
                    for (int i = 0; i < intClmn; i++)
                    {
                        sb.Append((dt.Columns[i].ColumnName.ToString() + (i == intClmn - 1 ? " " : ",")).PadLeft(intMax[i] + 1, ' '));
                    }
                    listSB.Add(sb);
                }

                //--------Columns Data--------------------------------------------------------------------------
                if (intRow != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StringBuilder sb = new StringBuilder("");
                        for (int i = 0; i < intClmn; i++)
                        {
                            sb.Append((row[i].ToString().Replace(System.Environment.NewLine, "; ") + (i == intClmn - 1 ? " " : ",")).PadLeft(intMax[i] + 1, ' '));
                        }
                        listSB.Add(sb);
                    }
                }
                else
                {
                    listSB.Add(new StringBuilder(""));
                }
            }

            return listSB;
        }


        public List<StringBuilder> BuildDB2DeleteSQL(DataSet ds)
        {
            List<StringBuilder> listSB = new List<StringBuilder>();

            int counTables = ds.Tables.Count;

            // First table PRIMARY next CHILD and CHILD of CHILD ...

            for (int i = counTables - 1; i >= 0; i--)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" DELETE FROM " + ds.Tables[i].TableName + " ;" + Environment.NewLine);
                listSB.Add(sb);
            }

            return listSB;
        }

        /// <summary>
        /// Insert DB2
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnection"></param>
        /// <param name="strDBname"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public List<StringBuilder> BuildDB2InsertSQL(DataTable dt)  // dt.Name MUST be same like in OleDbDataBase ...
        {
            List<StringBuilder> listSB = new List<StringBuilder>();

            int intRow = 0;
            int intClmn = 0;

            string tableName = dt.TableName;
            try
            {                                                     //prevent exception - if NOTHING
                intRow = dt.Rows.Count;
                intClmn = dt.Columns.Count;
            }
            catch
            {
            }

            //Will be inserted by 100 rows. 
            int intOneTransaction = 0;
            String strOneTransaction = System.Configuration.ConfigurationManager.AppSettings["rowsInOneTransaction"].ToString().ToUpper();

            if (int.TryParse(strOneTransaction,out intOneTransaction))
            {}
            else
            { intOneTransaction = 100; } // default value 100

            
            for (int intGroup = 0; intGroup * intOneTransaction < intRow; intGroup++)
            {
                //--------Build Columns Names ---------------------------------------------------------------------------

                StringBuilder sb = new StringBuilder("INSERT INTO " + tableName + "( ");
                for (int i = 0; i < intClmn; i++)
                {
                    sb.Append((dt.Columns[i].ColumnName.ToString() + (i == intClmn - 1 ? " " : ",")));
                }
                sb.Append(")   " + Environment.NewLine + Environment.NewLine);

                //--------Columns Data--------------------------------------------------------------------------

                int intLoBoundery = intGroup * intOneTransaction;
                int intHiBoundery = (intGroup + 1) * intOneTransaction <= intRow ? (intGroup + 1) * intOneTransaction : intRow;

                for (int iRowCount = intLoBoundery ; iRowCount < intHiBoundery  ; iRowCount++)
                {
                    DataRow row = dt.Rows[iRowCount];

                    sb.Append(" SELECT ");

                    for (int i = 0; i < intClmn; i++)
                    {
                        if (row[i] == null || row[i].ToString().Trim() =="")
                        {
                            sb.Append("NULL AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                        }
                        else
                        {
                            string str = row[i].ToString();
                            string strOut;
                            if (ifTime(str,out strOut))
                            {
                                sb.Append(strOut + " AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                            }
                            else
                            {
                                sb.Append("'" + str.Replace("'", "''").Replace(';', ',').Replace('!', '|') + "' AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                            }
                        }
                    }

                    sb.Append(" FROM sysibm.sysdummy1 ");

                    if (iRowCount < intHiBoundery - 1)
                    { 
                     sb.Append(System.Environment.NewLine + " UNION ALL " + System.Environment.NewLine);                    
                    }
                }
                sb.Append(" ; ");
                listSB.Add(sb);
            }

            return listSB;
        }

        // DB2 does not like AM/PM format  

        private CultureInfo cultureSource = new CultureInfo("en-US");
        private bool ifTime(string str, out string strOut)
        {
            DateTime dateValue;

            if (DateTime.TryParseExact(str, "%M/%d/yyyy %h:mm:ss tt", cultureSource, DateTimeStyles.None, out dateValue))
            {
                strOut = "to_date('" + dateValue.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS') ";
                return true;
            }

            else
            {
                strOut = "";
                return false;
            }
        }


        public StringBuilder BuildSelectSQL(DataTable dt)
        {
            int intRow = 0;
            int intClmn = 0;

            string tableName = dt.TableName;

            try
            {                                                     //prevent exception - if NOTHING
                intRow = dt.Rows.Count;
                intClmn = dt.Columns.Count;
            }
            catch
            {
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("-- Example for scramble data  - JUST EXAMPLE :" + Environment.NewLine);
            sb.Append("-- For DB2    ---  TRANSLATE(UPPER(ColumnName) , '' , 'AEIOUY' , '_') AS ColumnName, ...  " + Environment.NewLine);
            sb.Append("-- For DB2    ---  TO_CHAR(TS_TIMESTAMP,'YYYY-MM-DD HH24:MI:SS.NNNNNN') AS TS_TIMESTAMP - if your column use MICROSECONDS. " + Environment.NewLine);
            sb.Append("--                 Before NNNNNN MUST be DOT                  .NNNNNN       !!!!" + Environment.NewLine);
           
            sb.Append("  " + Environment.NewLine);
            //sb.Append("-- For MS SQL ---  PATINDEX('%[^0-9.]%', ColumnName) AS ColumnName, ... " + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("SELECT " + Environment.NewLine + Environment.NewLine);

            for (int i = 0; i < intClmn; i++)
            {
                sb.Append("       ").Append((dt.Columns[i].ColumnName.ToString() + (i == intClmn - 1 ? " " : ",")) + Environment.NewLine);
            }
            sb.Append(Environment.NewLine);
            sb.Append(" FROM " + tableName + "  ;");

            return sb;
        }

        public bool blnTestConnection(string strConnection)
        {
            try
            {
                conn = new System.Data.OleDb.OleDbConnection(strConnection);
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void closeConnection()
        {
            try
            {
                conn.Close();
            }
            catch
            {
            }
        }


        public string findTableNameHelpSQL(string str, string strConnectionStringFrom, out DataTable dt)
        {
            StringBuilder sb = new StringBuilder(str);
            DataSet ds = null;
            dt = null;

            try
            {
                ds = FindSchema(sb, strConnectionStringFrom);
            }
            catch
            {
                return null;
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            dt = ds.Tables[0];

            // logic for find table name from SQL script.

            string tablename = extractTableName(sb.ToString());
            dt.TableName = tablename;

            return tablename;
        }


        public string extractTableName(string str)
        {
            string tablename = str;

            tablename = tablename.Replace(";", "").Replace("\r\n", " ").Trim(); // make one line
            tablename = tablename.Replace("   ", " ").Replace("  ", " ").Replace("  ", " "); // remove extra spaces ...

            int intFirst = tablename.ToUpper().IndexOf("FROM");  // FROM length = 4

            if (intFirst <= 0) { return ""; }

            tablename = tablename.Substring(intFirst + 4).Trim();

            int intLast = tablename.IndexOf(" ", 0); // It is possiable that first char is space ...
            if (intLast == -1)
            {
            }
            else
            {
                tablename = tablename.Substring(0, intLast).Trim();
            }

            return tablename;
        }
    }
}


