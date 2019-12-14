using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XMLtoDataSet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DataSet dS = new DataSet();
        private string strErr = "";

        /// <summary>
        /// Open file and read to DataSet
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>

        private string openFiles(String[] files)
        {
            if (files.Length == 0 || files.Length > 1) { return "Please Drop only one File !"; }   //;
            FileInfo fileTmp = new FileInfo(files[0]);

            if (fileTmp.Extension.ToUpper() == ".XML")
            {
                Dispatcher.Invoke(new Action(() =>
                        {
                            using (StreamReader sr = new StreamReader(files[0]))
                            {
                                try
                                {
                                    dS.ReadXml(sr);
                                }
                                catch (Exception ex)
                                {
                                    strErr = ex.Message;
                                }
                            }
                        }
                ));
            }
            else
            {
                return "Please Drop Only XML file!";
            }

            return strErr;
        }


        private string ShowResult()
        {
            if (dS.Tables.Count == 0) return "Zero tables";

            grdResult.Children.Clear();  // Clear Previuse Result
            TabControl tc = new TabControl();

            try
            {
                foreach (DataTable t in dS.Tables)
                {
                    TabItem ti = new TabItem();
                    ti.Name = t.TableName;
                    ti.Header = t.TableName;
                    DataGrid dg = new DataGrid();
                    DataView dv = new DataView(t);
                    dg.ItemsSource = dv;

                    ti.Content = dg;
                    tc.Items.Add(ti);

                }
                    grdResult.Children.Add(tc);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }


        private void Grid_Drop(object sender, DragEventArgs e)
        {       
            IDataObject data = e.Data;
            String[] files = (String[])data.GetData(DataFormats.FileDrop);

            strErr = "";
            string str = openFiles(files);

            lblNotAdmin.Visibility = Visibility.Collapsed;
            if (str == "")
            {
                str = ShowResult();
            } 
            MessageBox.Show(str == "" ? "OK" : str);
        }
    }
}
