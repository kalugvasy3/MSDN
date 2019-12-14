using System;
using System.Threading.Tasks;
using System.Windows;


using System.IO;

using System.Diagnostics;
using Microsoft.Win32;

namespace BenchmarkReadFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private String[] strAll;
        private System.Diagnostics.Stopwatch stopW;

        public MainWindow()
        {
            InitializeComponent();
            usrClk.Visibility = Visibility.Hidden;
        }

        // - Begin- Read File Using Thread Synchronization ...
        private void btnReadFileThread_Click(object sender, RoutedEventArgs e)
        {
            strAll = null;
            GC.Collect();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            String fileName = openFileDialog.FileName;
            if (fileName == "") return;

            FileInfo f = new FileInfo(fileName);
            i64 = f.Length;
            lblThread.Content = i64.ToString();

            readFileThread(fileName);

        }

        private void readFileThread(String fileName)
        {
            TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

            Action MainThreadLoadFile = new Action(() =>
                {

                    Dispatcher.Invoke(new Action(() => { usrClk.Visibility = Visibility.Visible; }));

                    stopW = Stopwatch.StartNew();
                    strAll = File.ReadAllLines(fileName);
                    stopW.Stop();

                    Dispatcher.Invoke(new Action(() => 
                    {
                        lblReadFileThread.Content = stopW.ElapsedMilliseconds.ToString() + " ms.  ";
                        lblThread.Content = (i64 / stopW.ElapsedMilliseconds / 1000).ToString() + " MB/S" ;
                    }));

                });

            Action FinalThreadDoWOrk = new Action(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    usrClk.Visibility = Visibility.Hidden;
                }));
            });


            Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadFile());
            MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

        }

        // - End- Read File Using Thread Synchronization ...
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        // - Begin - Read File Using Async/Await

        private Int64 i64 = 0;

        private async void btnReadFileAsync_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            String fileName = openFileDialog.FileName;
            if (fileName == "") return;
            strAll = null;

            FileInfo f = new FileInfo(fileName);
            i64 = f.Length;
            lblAsync.Content = i64.ToString();

            await MainLoadFileAsync(fileName);

        }

        private async Task MainLoadFileAsync(String fileName)
        {
            Dispatcher.Invoke(new Action(() => { usrClk.Visibility = Visibility.Visible; }));

            Action MainThreadLoadFile = new Action(() =>
                {
                    stopW = Stopwatch.StartNew();
                    strAll = File.ReadAllLines(fileName);
                    stopW.Stop();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        lblReadFileAsync.Content = stopW.ElapsedMilliseconds.ToString() + " ms.  ";
                        lblAsync.Content = (i64 / stopW.ElapsedMilliseconds / 1000).ToString() + " MB/S";
                        usrClk.Visibility = Visibility.Hidden;
                    }));

                });

            await Task.Factory.StartNew(() => MainThreadLoadFile());
        }

        // - End- Read File Using Async/Await ...
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        // - Begin - Read File Buffered ...

        private async void btnReadAllText_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            String fileName = openFileDialog.FileName;
            if (fileName == "") return;
            strAll = null;

            // Do not open file with size > 2G - - string size limit for ReadAllText ... 

            FileInfo f = new FileInfo(fileName);
            i64 = f.Length;
            lblTextAll.Content = i64.ToString(); 

            if (f.Length >= 2147483647) return;
            
            await MainLoadFileAllText(fileName);
        }

        private async Task MainLoadFileAllText(String fileName)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                usrClk.Visibility = Visibility.Visible;
            }));

            Action MainThreadLoadFile = new Action(() =>
            {
                stopW = Stopwatch.StartNew();

                String str = File.ReadAllText(fileName);
 
                stopW.Stop();

                Dispatcher.Invoke(new Action(() =>
                {
                    lblReadAllText.Content = stopW.ElapsedMilliseconds.ToString() + " ms.  ";
                    lblTextAll.Content = (i64 / stopW.ElapsedMilliseconds / 1000).ToString() + " MB/S";
                    usrClk.Visibility = Visibility.Hidden;
                }));

            });

            await Task.Factory.StartNew(() => MainThreadLoadFile());
        }

    }
}
