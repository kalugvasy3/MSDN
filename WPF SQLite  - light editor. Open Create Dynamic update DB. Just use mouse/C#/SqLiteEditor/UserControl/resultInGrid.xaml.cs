using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

using System.Threading;
using System.Threading.Tasks;

using System.Data;

namespace SqLiteEditor
{
    /// <summary>
    /// Interaction logic for resultInGrid.xaml  http://www.codeproject.com/Articles/30905/WPF-DataGrid-Practical-Examples#dataset
    /// </summary>
    public partial class resultInGrid : Window
    {

        public resultInGrid()
        {
            InitializeComponent();
            userClk.Visibility = Visibility.Collapsed;

        }

        private DataBase db = new DataBase();
        private DataTable[] dTable;

        public void runSQL(string strConnectionString, StringBuilder sbRun)
        {

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

                    string strError = "";
                    try
                    {
                        if (sbRun.ToString().Trim() == "")
                        {
                            Dispatcher.Invoke(new Action(() => txtException.Text = "No SQL Codes / SQL  is empty."));
                            ds = null;
                        }
                        else
                        {
                            ds = db.FindDataSet(ref sbRun, strConnectionString,  out intEffected, out strError);
                        }

                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtException.Text = strError; 
                   
                        }));

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }

                });

                // Grid
                Action FinalThreadDoWOrk = new Action(() =>
                {
                    //   if (intElement != null) Dispatcher.Invoke(new Action(() => gridResult.Children.RemoveAt((int)intElement)));  // It is Clock

                    userClk.Visibility = Visibility.Collapsed;



                    if (txtException.Text == "" && ds != null )
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
                            ti.Header = iTable.ToString();

                            DataGrid dG = new DataGrid();

                            // https://blogs.msdn.microsoft.com/vinsibal/tag/datagrid-samples/
                            // https://msdn.microsoft.com/en-us/library/system.windows.controls.datagrid.columns(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-3

                            dG.AutoGenerateColumns = true;
                            dG.ItemsSource = dt.DefaultView;
                            ti.Content = dG;
                            //    dG.IsReadOnly = true;


                            ti.PreviewKeyDown += (s, e) => { csvSaveAs(s, e); e.Handled = true; };

                            tabMain.Items.Add(ti);
                            strEff += "  [" + dt.TableName + "] = " + dt.Rows.Count + "; ";
                            iTable++;
                        }


                        TabItem tFinal = new TabItem();
                        tFinal.Header = "SQLite ..." ;

                        strEff += "\n\r(First Table = " + intEffected.ToString() + ")\n\r\n\r";

                        if (sbRun.ToString().Length < 10000) txtSQL.Text = strEff + sbRun.ToString();
                        else txtSQL.Text = strEff + sbRun.ToString(0, 10000) + "  ......................." + "\n\r......................." + "\n\r..........etc..........";

                        txtSQL.IsEnabled = true;
                       
                        tFinal.Content = txtSQL;
                        tabMain.Items.Add(tFinal);

                        tabMain.Focus();
                        tabMain.SelectedIndex = 0;

                    }else{
                        txtException.Background = Brushes.LightPink;
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
                db.closeConnection();
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
                    sb.Append((@"""" + dt.Columns[i].ColumnName.ToString().Replace(@"""",@"""""") + @"""" + (i == intClmn - 1 ? " " : ",")));
                }

                streamWriter.WriteLine(sb.ToString());

                //--------Data By  Columns---------------------------------------------------------------------------

                foreach (DataRow row in dTable[intT].Rows)
                {
                    sb = new StringBuilder("");
                    for (int i = 0; i < intClmn; i++)
                    {
                        sb.Append((@"""" + row[i].ToString().Replace(@"""", @"""""") + @"""" + (i == intClmn - 1 ? " " : ",")));
                    }
                    streamWriter.WriteLine(sb.ToString());
                }
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
    }
}
