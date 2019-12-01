using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Globalization;

using System.Data.Common;

namespace WpfHelperCopyData
{


    // https://msdn.microsoft.com/en-us/library/9hy8csk1(v=vs.110).aspx
    // https://msdn.microsoft.com/en-us/library/dd0w4a2z(v=vs.110).aspx
    // https://msdn.microsoft.com/en-us/library/wda6c36e(v=vs.110).aspx


    class DataBase
    {

        private System.Data.Common.DbConnection conn;


        // Given a provider name and connection string, 
        // create the DbProviderFactory and DbConnection.
        // Returns a DbConnection on success; null on failure.

        private DbConnection CreateDbConnection(string providerName, string connectionString, out string strError)
        {
            // Assume failure.
            conn = null;
            strError = "";

            // Create the DbProviderFactory and DbConnection.
            if (connectionString != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                    conn  = factory.CreateConnection();
                    conn.ConnectionString = connectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (conn  != null)
                    {
                        conn  = null;
                    }
                    strError = ex.Message;
                }
            }
            // Return the connection.
            return conn ;
        }


        public int ExecuteSqlNonQuery(StringBuilder sbSql, string providerName, string connectionString, out string strError)
        {
            int intEffected = -1;  // return number rows effected 
            strError = "";

            try
            {
                conn = CreateDbConnection(providerName, connectionString, out strError);
                conn.Open();
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return -1;
            }

            try
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sbSql.ToString();
                cmd.CommandType = CommandType.Text;  
                cmd.Connection = conn;

                intEffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return -1;
            }
            return intEffected;

        }


        public DataSet GetDataSet(ref StringBuilder sbSql, string providerName, string connectionString, out string strError, out int intEffected)
        {
            intEffected = -1;
            strError = "";

            try
            {
                conn = CreateDbConnection(providerName, connectionString, out strError);
                conn.Open();
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return null;
            }

            DataSet dataSet = new DataSet();
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
            
            try
            {    //System.Data.Common.DbCommand
                DbDataAdapter dbAdapter = factory.CreateDataAdapter();

                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sbSql.ToString();   
                cmd.CommandType = CommandType.Text;   // If procedure you can use -> CALL procName (@param1,...,@paramN)
                cmd.Connection = conn;

                dbAdapter.SelectCommand = cmd;
                dbAdapter.FillLoadOption = LoadOption.PreserveChanges;
                intEffected = dbAdapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }
            return dataSet;

        }

        public bool TestConnection(string providerName, string connectionString, out string strError)
        {
            strError = "";
            try
            {
                conn = CreateDbConnection(providerName, connectionString, out strError);
                conn.Open();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }
     
        public void closeConnection() // Use it if you need to brake long RUN;
        {
            try
            {
                conn.Close();
            }
            catch
            {
            }
        }


      

        public List<StringBuilder> BuildDeleteSQL(DataSet ds)
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
        public List<StringBuilder> BuildInsertSQL(DataTable dt, string providerNameTo)  // dt.Name MUST be same like in OleDbDataBase ...
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

            if (int.TryParse(strOneTransaction, out intOneTransaction))
            { }
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

                for (int iRowCount = intLoBoundery; iRowCount < intHiBoundery; iRowCount++)
                {
                    DataRow row = dt.Rows[iRowCount];

                    sb.Append(" SELECT ");

                    for (int i = 0; i < intClmn; i++)
                    {
                        if (row[i] == null || row[i].ToString().Trim() == "")
                        {
                            sb.Append("NULL AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                        }
                        else
                        {
                            string str = row[i].ToString();
                            //string strOut;
                            //if (ifTime(str, out strOut))
                            //{
                            //    sb.Append(strOut + " AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                            //}
                            //else
                            //{
                                sb.Append("'" + str.Replace("'", "''").Replace(';', ',').Replace('!', '|') + "' AS " + dt.Columns[i].ColumnName + (i == intClmn - 1 ? " " : ","));
                          //  }
                        }
                    }

                    if (providerNameTo.LastIndexOf("IBM") > -1) { sb.Append(" FROM sysibm.sysdummy1 "); }
                    if (providerNameTo.LastIndexOf("ORA") > -1) { sb.Append(" FROM dual "); }



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

        //private bool ifTime(string str, out string strOut)
        //{
        //    DateTime dateValue;

        //    if (DateTime.TryParseExact(str, "%M/%d/yyyy %h:mm:ss tt", cultureSource, DateTimeStyles.None, out dateValue))
        //    {
        //        strOut = "to_date('" + dateValue.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS') ";
        //        return true;
        //    }

        //    else
        //    {
        //        strOut = "";
        //        return false;
        //    }
        //}


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


        public string extractTableName(string str)
        {
            string tablename = str;

            tablename = tablename.Replace(";", "").Replace("\r\n", " ").Trim(); // make one line
            tablename = tablename.Replace("   ", " ").Replace("  ", " ").Replace("  ", " "); // remove extra spaces ...

            int intFirst = tablename.ToUpper().IndexOf("FROM");  // FROM length = 4

            if (intFirst <= 0) { return ""; }

            tablename = tablename.Substring(intFirst + 4).Trim();

            int intLast = tablename.IndexOf(" ", 0); // It is possible that first char is space ...
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


