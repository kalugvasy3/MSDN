using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

// DataCommon class PASS VERACODE security 
class DataCommon
{

    private int gedSQLTimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SQLTimeout"]);
   
    private string connectionString = "";
    private string providerName = "";
 
    private System.Data.Common.DbConnection conn;
    private DbCommand _commandDB;


    /// <summary>
    /// This function initiate DbCommand for current instance - we MUST run it first ...
    /// </summary>
    /// <param name="strError"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public DbCommand commandDB(out String strError, String connectionName = "")
    {
        if (connectionName == "")   // Default Name
        {
           connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStr"].ConnectionString;
           providerName = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStr"].ProviderName;
        } else                     // New Name
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            providerName = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ProviderName;
        }

        conn = CreateDbConnection(out strError);
        _commandDB = conn.CreateCommand();
        _commandDB.CommandTimeout = gedSQLTimeOut;
        return _commandDB;

    }

    // Create the DbProviderFactory and DbConnection.
    // Returns a DbConnection on success; null on failure.

    private DbConnection CreateDbConnection(out string strError)
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

                conn = factory.CreateConnection();
                conn.ConnectionString = connectionString;
            }
            catch (Exception ex)
            {
                // Set the connection to null if it was created.
                if (conn != null)
                {
                    conn = null;
                }
                strError = ex.Message;
            }
        }
        // Return the connection.
        return conn;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="comType"></param>
    /// <param name="parameters"></param>
    /// <param name="strError"></param>
    /// <returns></returns>
    public int ExecuteSqlNonQuery(DbCommand cmd, CommandType comType, Dictionary<string, object> parameters, out string strError)
    {

        int intEffected = -1;
        strError = "";

        try
        {
            conn = cmd.Connection;
            conn.Open();
        }
        catch (Exception ex)
        {
            strError = ex.Message;
            return intEffected;
        }

        try
        {
            cmd.CommandType = comType;
            if ((parameters != null))
            {
                cmd.Parameters.Clear();
                if (parameters.Count > 0)
                {
                    SetCmd(ref cmd, parameters);
                }
            }

            intEffected = cmd.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception ex)
        {
            strError = ex.Message;
            intEffected = -1;
        }
        finally
        {
            conn.Close();
        }

        return intEffected;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="comType"></param>
    /// <param name="parameters"></param>
    /// <param name="strError"></param>
    /// <param name="intEffected"></param>
    /// <returns></returns>
    public DataSet GetDataSet(DbCommand cmd, CommandType comType, Dictionary<string, object> parameters, out string strError, out int intEffected)
    {
        intEffected = -1;
        strError = "";

        try
        {
            conn = cmd.Connection;
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
        {
            //System.Data.Common.DbCommand
            DbDataAdapter dbAdapter = factory.CreateDataAdapter();

            cmd.CommandType = comType;
            if ((parameters != null))
            {
                cmd.Parameters.Clear();
                if (parameters.Count > 0)
                {
                    SetCmd(ref cmd, parameters);
                }
            }

            dbAdapter.SelectCommand = cmd;
            dbAdapter.FillLoadOption = LoadOption.PreserveChanges;
            intEffected = dbAdapter.Fill(dataSet);

            conn.Close();
        }
        catch (Exception ex)
        {
            strError = ex.Message;
            conn.Close();
        }
        return dataSet;

    }

    private void SetCmd(ref DbCommand cmd, Dictionary<string, object> parameters)
    {
        foreach (var pair in parameters)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = pair.Key;
            param.Value = pair.Value;
            cmd.Parameters.Add(param);
        }
    }

    public bool TestConnection(out string strError)
    {
        strError = "";
        try
        {
            // conn = CreateDbConnection(out strError);  Connection has been created if you run  public DbCommand commandDB(out String strError, String connectionName = "")
            
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

    public void closeConnection()
    {
        // Use it if you need to brake long RUN;
        try
        {
            conn.Close();
        }
        catch
        {
        }
    }

}