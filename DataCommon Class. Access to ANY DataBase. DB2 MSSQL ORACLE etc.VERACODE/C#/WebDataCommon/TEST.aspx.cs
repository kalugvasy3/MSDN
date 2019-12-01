// EXAMPLE - HOW TO USE CLASS DataCommon.cs
// One Class for ALL data providers which installed on your computer ...

// DataCommon.cs Class PASS VERACODE  test ...

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Just add Data.Common
using System.Data.Common;

namespace WebDataCommon
{
    public partial class TEST : System.Web.UI.Page
    {

        string strError = "";
        int intEffected = 0;

        const string procDB2 = @"SFCWD035.""procColumnsCQI""";   //Execute like PROC -  No parameters
        const string procMSSQL = "CALL procMSSQL (@pA, @pB)";  // Execute like SQL statement with parameters
        const string procMYSQL = "SELECT * FROM tableA";    // Select from tableA


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
            else { }
        }

        //Access to IBM.Data.DB2 - see web.config  (In this example - it is default value)
        protected void btnA_Click(object sender, EventArgs e)
        {
            DataCommon db = new DataCommon();
            DbCommand cmd = db.commandDB(out strError);  // Use Default Name - no second parameter

            // You will not be able pass SQL code like parameter - VERACODE return ERROR with NO PASS testing 
            cmd.CommandText = procDB2;
            var dic = new Dictionary<String, Object> { { "@pDataCatalog", "Grievance" } };
            grdView.DataSource = db.GetDataSet(cmd, System.Data.CommandType.StoredProcedure, dic, out strError, out intEffected);
            grdView.DataBind();
        }

        //Access to System.Data.SqlClient - see web.config
        protected void btnB_Click(object sender, EventArgs e)
        {
            DataCommon db = new DataCommon();
            DbCommand cmd = db.commandDB(out strError, "connectionB");  // Use connectionB   - see web.config

            cmd.CommandText = procMSSQL;
            var dic = new Dictionary<String, Object> { { "@pA", "A" }, { "@pB", "B" } };
            grdView.DataSource = db.GetDataSet(cmd, System.Data.CommandType.Text, dic, out strError, out intEffected);
            grdView.DataBind();
        }

        //Access to MySql.Data.MySqlClient - see web.config
        protected void btnC_Click(object sender, EventArgs e)
        {
            DataCommon db = new DataCommon();
            DbCommand cmd = db.commandDB(out strError, "connectionC");  // Use connectionC   - see web.config        }

            cmd.CommandText = procMYSQL;
            // Use NULL if no parameters
            grdView.DataSource = db.GetDataSet(cmd, System.Data.CommandType.Text, null, out strError, out intEffected);
            grdView.DataBind();
        }
    }
}