using System;
using System.IO;
using System.Data;
using System.Text;

using Microsoft.VisualBasic.FileIO;

public class classCsvXmlConvertReadWrite
{
    public  string GetXMLString(DataTable dtOneRow)
    {
        string strXML;
        DataSet ds = new DataSet();
        ds.DataSetName = "XML";
        ds.Tables.Add(dtOneRow);
        try
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ds.WriteXml(writer);
            strXML = writer.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
        return strXML;
    }
    public  string GetXMLString(DataSet ds)
    {
        string strXML;
        try
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ds.WriteXml(writer);
            strXML = writer.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
        return strXML;
    }
    public  DataSet GetDataSetFromXml(string strXml)
    {
        DataSet ds = new DataSet();
        try
        {
            ds.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(strXml)));
        }
        catch (Exception ex)
        {
            throw;
        }
        return ds;
    }
    public  DataSet GetDataSetFromXml(DataSet ds)
    {
        DataTable dt = ds.Tables[0];
        DataSet dsOut = new DataSet();
        foreach (DataRow dr in dt.Rows)
        {
            string strXML = dr["XML"].ToString();
            dsOut.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(strXML)));
        }
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        else
        {
            dsOut.DataSetName = "XML";
            dsOut.Tables[0].TableName = dt.TableName;
            dsOut.Tables[0].Columns.Add("PK_INDEX");
        }
        int iCurrent = 0;
        foreach (DataRow dr in dt.Rows)
        {
            string strXML = dr["XML"].ToString();
            dsOut.Tables[0].Rows[iCurrent]["PK_INDEX"] = dr["PK_INDEX"];
            iCurrent += 1;
        }
        return dsOut;
    }
    public  DataTable GetDataTableFromCsvString(string csvBody, bool isHeadings)
    {
        DataTable MethodResult = null;
        try
        {
            MemoryStream MemoryStream = new MemoryStream();
            StreamWriter StreamWriter = new StreamWriter(MemoryStream);
            StreamWriter.Write(csvBody);
            StreamWriter.Flush();
            MemoryStream.Position = 0;
            using (TextFieldParser TextFieldParser = new TextFieldParser(MemoryStream))
            {
                if (isHeadings)
                {
                    MethodResult = GetDataTableFromTextFieldParser(TextFieldParser);
                }
                else
                {
                    MethodResult = GetDataTableFromTextFieldParserNoHeadings(TextFieldParser);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return MethodResult;
    }
    private  DataTable GetDataTableFromTextFieldParser(TextFieldParser textFieldParser)
    {
        DataTable MethodResult = null;
        try
        {
            textFieldParser.SetDelimiters(new string[] { "," });
            textFieldParser.HasFieldsEnclosedInQuotes = true;
            string[] ColumnFields = textFieldParser.ReadFields();
            DataTable dt = new DataTable();
            foreach (string ColumnField in ColumnFields)
            {
                DataColumn DataColumn = new DataColumn(ColumnField);
                DataColumn.AllowDBNull = true;
                dt.Columns.Add(DataColumn);
            }
            while (!textFieldParser.EndOfData)
            {
                string[] Fields = textFieldParser.ReadFields();
                for (int i = 0; i <= Fields.Length - 1; i++)
                {
                    if (Fields[i] == "")
                    {
                        Fields[i] = null;
                    }
                }
                dt.Rows.Add(Fields);
            }
            MethodResult = dt;
        }
        catch (Exception ex)
        {
            throw;
        }
        return MethodResult;
    }
    private  DataTable GetDataTableFromTextFieldParserNoHeadings(TextFieldParser textFieldParser)
    {
        DataTable MethodResult = null;
        try
        {
            textFieldParser.SetDelimiters(new string[] { "," });
            textFieldParser.HasFieldsEnclosedInQuotes = true;
            bool FirstPass = true;
            DataTable dt = new DataTable();
            while (!textFieldParser.EndOfData)
            {
                string[] Fields = textFieldParser.ReadFields();
                if (FirstPass)
                {
                    for (int i = 0; i <= Fields.Length - 1; i++)
                    {
                        DataColumn DataColumn = new DataColumn("Column " + i);
                        DataColumn.AllowDBNull = true;
                        dt.Columns.Add(DataColumn);
                    }
                    FirstPass = false;
                }
                for (int i = 0; i <= Fields.Length - 1; i++)
                {
                    if (Fields[i] == "")
                    {
                        Fields[i] = null;
                    }
                }
                dt.Rows.Add(Fields);
            }
            MethodResult = dt;
        }
        catch (Exception ex)
        {
            throw;
        }
        return MethodResult;
    }
}
