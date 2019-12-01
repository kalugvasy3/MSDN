using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace SqLiteEditor
{
    class  DataBase
    {
        private System.Data.SQLite.SQLiteConnection connToDB;

        private int executeSqlNonQuery(StringBuilder sbSql, string strConnection) {
            int intEffected = -1;  // return number rows effected 

                try {
                connToDB = new System.Data.SQLite.SQLiteConnection(strConnection);
                connToDB.Open();
                }
                catch (Exception ex) {
                    string msg = "Open Connection Exception / Check Connection String.";
                    throw new Exception(msg, ex);

                }

                try {
                    System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sbSql.ToString(), connToDB);
                    intEffected = cmd.ExecuteNonQuery();
                }
                catch (Exception ex) {
                    throw (ex);
                }
 
            return intEffected;
        }

        private DataSet getDataSet(ref StringBuilder sbSql, string strConnection, out int intEffected) {
            intEffected = -1;

                try {
                    connToDB = new System.Data.SQLite.SQLiteConnection(strConnection);
                    connToDB.Open();
                }
                catch (Exception ex) {
                    string msg = "Open Connection Exception / Check Connection String.";
                    throw new Exception(msg, ex);

                }

                DataSet dataSet = new DataSet();
                try {
                    System.Data.SQLite.SQLiteDataAdapter dbAdapter = new System.Data.SQLite.SQLiteDataAdapter();
                    
                    dbAdapter.SelectCommand = new System.Data.SQLite.SQLiteCommand(sbSql.ToString(), connToDB);
                    dbAdapter.FillLoadOption = LoadOption.PreserveChanges;
                    intEffected = dbAdapter.Fill(dataSet);
                }
                catch (Exception ex) {
                   
                    throw (ex);
                }
                return dataSet;
        }


        public bool blnTestConnection(string strConnection)
        {

            try
            {
                connToDB = new System.Data.SQLite.SQLiteConnection(strConnection);
                connToDB.Open();
                connToDB.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int ExecuteSqlNonQuery(StringBuilder sbSql, string strConnection, out string strError)
        {
            strError = "";
            try
            {
                return executeSqlNonQuery(sbSql, strConnection);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return  -1;
            }
        }

 
        public DataSet FindDataSet(ref StringBuilder sbSql, string strConnection,out int intEffected, out string strError )
        {
            strError = "";
            intEffected = -1;

            try
            {
                return getDataSet(ref sbSql, strConnection,  out intEffected);
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return null;
            }
        }

        public void closeConnection()
        {
            try
            {
                connToDB.Close();
            }
            catch 
            {
            }
        }
    }
}
