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

using Microsoft.Win32;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

namespace WpfHelperCopyData
{
    /// <summary>
    /// Interaction logic for RunInsert.xaml
    /// </summary>
    public partial class RunInsert : Window
    {

        public string strConnectionStringTo { get; set; }
        public List<StringBuilder> listSB { get; set; }

        private OleDbDataBase db = new OleDbDataBase();

        public RunInsert()
        {
            InitializeComponent();
        }

        private void runInsertSQL()
        {
            try
            {
                StringBuilder sbException = new StringBuilder();

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                // DataSet
                Action MainThreadLoadSQL = new Action(() =>
                {

                    int iMax = listSB.Count;
                    int iCurent = 0;

                    foreach (StringBuilder sb in listSB)
                    {
                        try
                        {
                            db.ExecuteSqlNonQuery(sb, strConnectionStringTo);
                            Dispatcher.Invoke(new Action(() => prgResult.Value = iCurent++));
                        }
                        catch (Exception ex)
                        {
                            sbException.Append(ex.Message).Append(Environment.NewLine).Append(Environment.NewLine);
                        }

                    }

                });

                // Grid
                Action FinalThreadDoWOrk = new Action(() =>
                {
                    Dispatcher.Invoke(new Action(() => prgResult.Value = 0));

                    if (sbException.ToString() != "")
                    {
                        MessageBox.Show(sbException.ToString());
                    }
                    else
                        MessageBox.Show("DONE - successfully!");
                });


                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadSQL());
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            runInsertSQL();
        }

        // Close Connection if window clossing.

        private void RunInsert_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.closeConnection();
        }

        private void chkBoxUnblock_Click(object sender, RoutedEventArgs e)
        {
            btnRun.IsEnabled = btnRun.IsEnabled ? false : true;
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            // listSB
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "SQL files |*.sql";

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

            try
            {
                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();
                Action MainThreadLoadSQL = new Action(() =>
                {
                    try
                    {
                        int iMax = listSB.Count;
                        int iCurent = 0;

                        Dispatcher.Invoke(new Action(() => prgResult.Value = 0));

                        foreach (StringBuilder sb in listSB)
                        {
                            streamWriter.WriteLine(sb.ToString());
                            streamWriter.WriteLine(Environment.NewLine);
                            streamWriter.WriteLine(Environment.NewLine);
                            Dispatcher.Invoke(new Action(() => prgResult.Value = ++iCurent));
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                });

                Action FinalThreadDoWOrk = new Action(() =>
                {
                    Dispatcher.Invoke(new Action(() => prgResult.Value = 0));

                    streamWriter.Flush();
                    streamWriter.Close();
                });


                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadSQL());
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
