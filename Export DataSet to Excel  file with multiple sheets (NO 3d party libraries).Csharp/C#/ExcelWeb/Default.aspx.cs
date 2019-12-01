using System;

using System.Data;

using System.Text;
using System.Web;


namespace CsvWebC
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            String strDownloadFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

            // Your Function for Retrieving Data
            DataSet DS = RetrieveData();


            exportDataSetToExcel(Response, DS, strDownloadFileName);
            Response.End();
        }

        protected DataSet RetrieveData()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DataTable - TEST...

            dt.TableName = "TEST 1";
            dt.Columns.Add("TestA");
            dt.Columns.Add("TestB");
            dt.Columns.Add("TestC");
            dt.Columns.Add("TestD,E");

            DataRow dr = dt.NewRow();
            dr["TestA"] = "A' ";
            dr["TestB"] = @"B"" ";
            dr["TestC"] = "C<> ";
            dr["TestD,E"] = ",C, ";

            dt.Rows.Add(dr);
            ds.Tables.Add(dt);

            DataTable dt2 = new DataTable();
            //DataTable - TEST...

            dt2.TableName = "TEST TEST 2";
            dt2.Columns.Add("TestA2");
            dt2.Columns.Add("TestB2");
            dt2.Columns.Add("TestC2");
            dt2.Columns.Add("TestD2,E2");

            DataRow dr2 = dt2.NewRow();
            dr2["TestA2"] = "A'2 ";
            dr2["TestB2"] = @"B""2 ";
            dr2["TestC2"] = "C<>2 ";
            dr2["TestD2,E2"] = ",C,2 ";

            dt2.Rows.Add(dr2);
            ds.Tables.Add(dt2);

            return ds;
        }

        //https://www.codeproject.com/Tips/19840/Export-to-Excel-using-VB-Net
        //https://en.wikipedia.org/wiki/Microsoft_Office_XML_formats
        //https://en.wikipedia.org/wiki/Microsoft_Excel
        //https://msdn.microsoft.com/en-us/library/aa140066(office.10).aspx

        void exportDataSetToExcel(HttpResponse respObj, DataSet DS, string fileName)
        {

            fileName = fileName + ".xml";

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?> ");
            sb.AppendLine(@"<?mso-application progid=""Excel.Sheet""?> ");
            sb.AppendLine(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" ");
            sb.AppendLine(@"xmlns:x=""urn:schemas-microsoft-com:office:excel"" ");
            sb.AppendLine(@"xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" ");
            sb.AppendLine(@"xmlns:html=""http://www.w3.org/TR/REC-html40""> ");
            sb.AppendLine("");

            foreach (DataTable tbl in DS.Tables)
            {
                System.Data.DataRow dr;

                sb.AppendLine(@"<Worksheet ss:Name=""" + tbl.TableName + @""">");
                sb.AppendLine("<Table>");

                // see XML Spreadsheet Reference https://msdn.microsoft.com/en-us/library/aa140066(office.10).aspx
                sb.AppendLine(@"<Column ss:Index=""1"" ss:AutoFitWidth=""0"" ss:Width=""110""/>");

                // First Row as Column's Name
                sb.AppendLine("<Row>");
                foreach (DataColumn dc in tbl.Columns)
                {
                    sb.AppendLine(@"<Cell><Data ss:Type=""String"">" + HttpUtility.HtmlEncode(dc.ColumnName.ToString()) + "</Data></Cell>");
                }

                sb.AppendLine("</Row>");

                // Add each Row

                foreach (DataRow dr2 in tbl.Rows)
                {
                    sb.AppendLine("<Row>");
                    foreach (DataColumn dc in tbl.Columns)
                    {
                        sb.AppendLine(@"<Cell><Data ss:Type=""String"">" + HttpUtility.HtmlEncode(dr2[dc.ColumnName].ToString()) + "</Data></Cell>");
                    }
                    sb.AppendLine("</Row>");
                }

                sb.AppendLine("</Table>");
                sb.AppendLine("</Worksheet>");

            }
            sb.AppendLine("</Workbook>");

            respObj.Clear();
            respObj.ClearHeaders();
            respObj.ContentType = "application/vnd.ms-excel";
            respObj.AppendHeader("content-disposition", "attachment; filename=" + fileName);

            byte[] myData = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            respObj.BinaryWrite(myData); // Binary data - see myData -  

        }

    }
}