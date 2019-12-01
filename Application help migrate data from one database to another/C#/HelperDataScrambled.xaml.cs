using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.IO;

using Microsoft.Win32;


using System.Threading;
//using System.Threading.Tasks;

using System.Data;

namespace WpfHelperCopyData
{
    /// <summary>
    /// Interaction logic for HelperDataScrambled.xaml
    /// </summary>
    public partial class HelperDataScrambled : Window
    {
        public HelperDataScrambled()
        {
            InitializeComponent();

            userClk.Visibility = Visibility.Collapsed;
            dsResult = null;
        }

        public DataSet dsResult  { get; set; }


        private OleDbDataBase db = new OleDbDataBase();    
        private DataTable[] dTable;


        private List<string> findTablesName(StringBuilder sbRun)
        {
            List<string> listTablesName = new List<string>();
            OleDbDataBase db = new OleDbDataBase();

            string strNotCommented = "";
            foreach (string str in sbRun.ToString().Split('\n'))
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


            string[] strSQL = strNotCommented.Split(';');
            foreach (string str in strSQL)
            {

                string strName = db.extractTableName(str);
                if (strName != "" && !listTablesName.Exists(x => x == strName) )
                {
                    listTablesName.Add(strName);
                }
            }

            return listTablesName;
        }

        
        public void runSQL( string strConnectionString, StringBuilder sbRun)
        {

            //  int? intElement = null;

            try
            {

                int intEffected = 0;

                DataSet ds = new DataSet();

                TextBlock txtException = new TextBlock();
                txtException.TextWrapping = TextWrapping.Wrap;
                TextBox txtSQL = new TextBox();
                txtSQL.TextWrapping = TextWrapping.Wrap;

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                Dispatcher.Invoke(new Action(() => userClk.Visibility = Visibility.Visible));

                // DataSet
                Action MainThreadLoadSQL = new Action(() =>
                {


                    try
                    {
                        if (sbRun.ToString().Trim() == "")
                        {
                            Dispatcher.Invoke(new Action(() => txtException.Text = "No SQL Codes / SQL  is empty."));
                            ds = null;
                        }
                        else
                        {
                            ds = db.FindDataSet(sbRun, strConnectionString,  out intEffected);  
                            //ds = db.FindDataSetWithReader(sbRun, strConnectionString);
                            dsResult = ds;

                            int intTable = 0;
                            foreach (string str in  findTablesName(sbRun))
                            {
                                dsResult.Tables[intTable].TableName = str;
                                intTable++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtException.Text = ex.Message ;

                        }));

                    }

                });

                // Grid
                Action FinalThreadDoWOrk = new Action(() =>
                {
                    //   if (intElement != null) Dispatcher.Invoke(new Action(() => gridResult.Children.RemoveAt((int)intElement)));  // It is Clock

                    userClk.Visibility = Visibility.Collapsed;



                    if (txtException.Text == "")
                    {

                        string strEff = "Effected rows - ";
                        TabControl tabMain = new TabControl();
                        gridResult.Children.Add(tabMain);

                        dTable = new DataTable[ds.Tables.Count];
                        int iTable = 0;
                        foreach (DataTable dt in ds.Tables)
                        {
                            
                            dTable[iTable] = dt;
                            
                            TabItem ti = new TabItem();
                            ti.Header = dTable[iTable].TableName;

                            //dt.Rows.InsertAt(dt.NewRow(),0);

                            DataGrid dG = new DataGrid();
                            dG.IsReadOnly = false;
                            dG.AutoGenerateColumns = true;
                            dG.ItemsSource = dt.DefaultView;

                            ti.Content = dG;                          

                            ti.PreviewKeyDown += (s, e) => { csvSaveAs(s, e); e.Handled = true; };

                            tabMain.Items.Add(ti);
                            strEff += "  [" + dt.TableName + "] = " + dt.Rows.Count + "; ";
                            iTable++;
                        }


                        TabItem tFinal = new TabItem();
                        tFinal.Header = "SQL - OLE DB ";

                        strEff += "\n\r(First Table = " + intEffected.ToString() + ")\n\r\n\r";

                        if (sbRun.ToString().Length < 10000) txtSQL.Text = strEff + sbRun.ToString();
                        else txtSQL.Text = strEff + sbRun.ToString(0, 10000) + "  ......................." + "\n\r......................." + "\n\r..........etc..........";

                        txtSQL.IsEnabled = true;

                        tFinal.Content = txtSQL;
                        tabMain.Items.Add(tFinal);

                        tabMain.Focus();
                        tabMain.SelectedIndex = 0;

                    }
                    else
                    {
                        gridResult.Children.Add(txtException);
                    }



                });


                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadSQL());
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
             //   db.connToDB.Close();
                db = null;
            }
            catch
            {
            }

        }


        private void csvSaveAs(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                TabItem ti = new TabItem();
                ti = sender as TabItem;

                double dResult;
                Double.TryParse(ti.Header.ToString(), out dResult);

                int intT = (int)dResult;

                DataTable dt = dTable[intT];
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = " CSV files |*.csv";

                Nullable<bool> result = sfd.ShowDialog();

                string filename = "";
                if (result == true)
                {
                    filename = sfd.FileName;
                }
                else
                {
                    return;
                }

                TextWriter streamWriter = new StreamWriter(filename);

                //--------Columns Name---------------------------------------------------------------------------

                StringBuilder sb = new StringBuilder();
                int intClmn = dTable[intT].Columns.Count;

                for (int i = 0; i < intClmn; i++)
                {
                    sb.Append((dt.Columns[i].ColumnName.ToString() + (i == intClmn - 1 ? " " : ",")));
                }

                streamWriter.WriteLine(sb.ToString());

                //--------Data By  Columns---------------------------------------------------------------------------

                foreach (DataRow row in dTable[intT].Rows)
                {
                    sb = new StringBuilder("");
                    for (int i = 0; i < intClmn; i++)
                    {
                        sb.Append((row[i].ToString().Replace(System.Environment.NewLine, "; ") + (i == intClmn - 1 ? " " : ",")));
                    }
                    streamWriter.WriteLine(sb.ToString());
                }
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                db.closeConnection();
        }

 
    }
}
