using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace SqLiteEditor
{
    /// <summary>
    /// Interaction logic for importCSVfile.xaml
    /// </summary>
    public partial class importCSVfile : Window
    {
        public importCSVfile()
        {
            InitializeComponent();
            userClk.Visibility = Visibility.Collapsed;
        }

        public string strConnection = "";
        public string strSQLCreateTable = "";
        public string strTableName = "";

        public string strSQLInsertData = "";

        // Event Handler ------------------------------------------------------------------------------------------------------------------
        // Create Event for sending "Style" outside UserControlWindow ...

        public delegate void routedEventHandler(object sender);

        public event routedEventHandler readySQLCreateTable;
        public event routedEventHandler readySQLDataInsert;

        // Event Handler - END -------------------------------------------------------------------------------------------------------------



        private TextBox newTextBox(string strInit)
        {
            TextBox tb = new TextBox();
            tb.Name = "textCsv";
            tb.AcceptsReturn = true;
            tb.FontSize = 14.0;
            tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            tb.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            tb.SpellCheck.IsEnabled = false;
            tb.TextWrapping = TextWrapping.NoWrap;
            tb.UndoLimit = 1000;
            tb.Text = strInit;
            tb.Visibility = Visibility.Visible;
            tb.Background = Brushes.White;
            return tb;
        }

        private string fileExtension = "";
        private void openFiles(String[] files)
        {
            if (files == null)
            {
                return;
            }
            if (files.Length != 1)
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    fileExtension = "";
                    MessageBox.Show(this, "Please DROP only ONE file ...");
                }));

                return;
            }

            StringBuilder sbAll = new StringBuilder();
            string fileName = "";

            try
            {
                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                Dispatcher.Invoke(new Action(() =>
                {
                    userClk.Visibility = Visibility.Visible;
                }));

                Action MainThreadLoadFile = new Action(() =>
                {
                    string f = files[0];
                    FileInfo fileTmp = new FileInfo(f);

                    fileName = fileTmp.Name;
                    fileExtension = fileTmp.Extension.ToUpper();

                    if (fileTmp.Extension.ToUpper() == ".CSV" || fileTmp.Extension.ToUpper() == ".XML")
                    {
                        try
                        {   
                         using (StreamReader sr = new StreamReader(f))
                        {
                            while (sr.Peek() >= 0)
                            {
                                sbAll.Append(sr.ReadLine()).AppendLine();
                            }
                        }                       
                        }
                        catch 
                        {
                            Dispatcher.Invoke(new Action(() =>
                            {
                                fileExtension = "";
                                MessageBox.Show(this, "Another process using this file ...");
                            }));
                        }
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            fileExtension = "";
                            MessageBox.Show(this, "Only  CSV | XML files ...");
                        }));
                    }
                    ;

                });

                Action FinalThreadDoWOrk = new Action(() =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        userClk.Visibility = Visibility.Collapsed;

                        txtOriginal.Text = sbAll.ToString();

                        string strName = fileName.Replace(" ", "");
                        strTableName = strName.Substring(0, strName.IndexOf('.'));

                    }));
                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadFile());
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch
            {
                gridData.Visibility = Visibility.Visible;
                txtOriginal.Text = "";

                Dispatcher.Invoke(new Action(() =>
                {
                    fileExtension = "";
                    MessageBox.Show(this, "Could not Allocated in managed memory ... try again different file ... (openFiles)...");
                }));


            }
        }

        private void gridMain_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            String[] files = (String[])data.GetData(DataFormats.FileDrop);

            openFiles(files);
        }

        private void btnTransferTo_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)gridData.Children[0];
            DataSet ds = new DataSet();

            try
            {
                try
                {
                    if (fileExtension == ".XML" || (fileExtension == "" && tb.Text.Trim() != ""))  // try if paste ...
                    {
                        classCsvXmlConvertReadWrite xml = new classCsvXmlConvertReadWrite();
                        ds = xml.GetDataSetFromXml(tb.Text);
                        ds.DataSetName = strTableName;
                        ds.AcceptChanges();
                    }
                }
                catch
                { }



                if ((fileExtension == ".CSV" || fileExtension == "") && ds.Tables.Count == 0)   // try CSV if above false ...
                {
                    classCsvXmlConvertReadWrite csv = new classCsvXmlConvertReadWrite();
                    ds.Tables.Add(csv.GetDataTableFromCsvString(tb.Text, true));
                    ds.Tables[0].TableName = strTableName;
                    ds.AcceptChanges();
                }

                if (ds.Tables.Count > 0)
                {
                    Truncated(ref ds);    // truncated empty row(s) column(s) - Excel usually created extra column(s)/row(s)
                    CreateDataGrids(ds);
                    btnCreateSqlInsert.IsEnabled = false;
                    btnCreateTable.IsEnabled = true;
                    btnTransferTo.IsEnabled = false;
                    chkPK.IsEnabled = true;
                    fileExtension = "";
                }
                else
                {
                    fileExtension = "";
                    throw new Exception("Wrong EXT ...");
                };

            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    fileExtension = "";
                    MessageBox.Show(this, "Attention! \nSomthing Wrong With CSV/XML data! \nTry Again! \n" + ex.Message);
                }));



                btnCreateSqlInsert.IsEnabled = false;
                btnCreateTable.IsEnabled = false;
                btnTransferTo.IsEnabled = true;
                chkPK.IsEnabled = false;
            }

        }

        private void Truncated(ref DataSet ds)    // truncated empty row(s) column(s) - Excel usually created extra column(s)/row(s)
        {
         

            foreach (DataTable dt in ds.Tables)
            {
                List<DataColumn> listOfColumns = new List<DataColumn>();
                foreach (DataColumn dc in dt.Columns)  // If column does not has a name - logic automatically assign Name - Column + Number
                {
                    string strColumn = dc.ColumnName.Trim();
                    if (strColumn.IndexOf("Column") == 0)
                    {
                        string subNumber = strColumn.Substring(6);
                        int number = 0;
                        if (Int32.TryParse(subNumber, out number))
                        {
                            listOfColumns.Add(dc);
                        }
                    }
                }
                foreach (DataColumn dc in listOfColumns)
                {
                    dt.Columns.Remove(dc.ColumnName);
                }
            }

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)  // If column does not has a name - logic automatically assign Name - Column + Number
                {
                    String strSum = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        strSum += (dr[dc.ColumnName] ?? "");
                    }

                    if (strSum.Trim() == "") dr.Delete();
                }

            }
        }


        private void CreateDataGrids(DataSet ds)
        {
            gridDrop.Visibility = Visibility.Collapsed; // Hide ability to Drop File(s);

            TabControl tabMain = new TabControl();
            tabMain.Name = "tabMain";
            gridData.Children.Clear(); // Clear GridData ...
            gridData.Children.Add(tabMain);

            int iTable = 0;
            foreach (DataTable dt in ds.Tables)
            {
                TabItem ti = new TabItem();
                ti.Header = dt.TableName.ToString();
                DataGrid dG = new DataGrid();

                dG.AutoGenerateColumns = true;
                dG.ItemsSource = dt.DefaultView;
                ti.Content = dG;

                tabMain.Items.Add(ti);
                iTable++;
            }
        }

        private classHelper cH = new classHelper();
        private void btnCreateTable_Click(object sender, RoutedEventArgs e)
        {
            TabControl tc = (TabControl)gridData.Children[0];
            TabItem ti = (TabItem)tc.SelectedItem;
            bool blnPK = chkPK.IsChecked == true;

            DataGrid dG = (DataGrid)ti.Content;
            DataView dView = (DataView)dG.ItemsSource;
            DataTable dt = dView.Table;
            dt.AcceptChanges();

            strSQLCreateTable = cH.createCreateTableSQLscript(dt, blnPK);

            btnCreateTable.IsEnabled = false;
            btnCreateSqlInsert.IsEnabled = true;
            chkPK.IsEnabled = false;

            readySQLCreateTable(this);  // Event - top level code will subscribe this....
        }

        private void btnCreateSqlInsert_Click(object sender, RoutedEventArgs e)
        {
            TabControl tc = (TabControl)gridData.Children[0];
            TabItem ti = (TabItem)tc.SelectedItem;

            DataGrid dG = (DataGrid)ti.Content;
            DataView dView = (DataView)dG.ItemsSource;
            DataTable dt = dView.Table;
            dt.AcceptChanges();

            // Next step -> count existing rows in current table ... 

            int intEffected = 0;
            string strError = "";

            bool blnPK = chkPK.IsChecked == true;
            int intMaxPK = 0;
            if(blnPK == false) intMaxPK = cH.intMaxPKIndex(strConnection, dt.TableName, out intEffected, out strError);

            if (strError != "")
            {
                if (MessageBox.Show(strError, "Are You create table, continue  ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                { } else { return; }
            }
            if (blnPK == false) strSQLInsertData = cH.createCreateInsertSQLscriptAutoIncriment(dt, intMaxPK + 1);
            if (blnPK == true) strSQLInsertData = cH.createCreateInsertSQLscriptBaseOnPK(dt);

            btnCreateSqlInsert.IsEnabled = false;
            readySQLDataInsert(this);  // Event - top level code will subscribe this....
        }


    }
}
