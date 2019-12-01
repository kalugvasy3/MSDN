using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Text;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

using WpfControlTabTest;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Interop;

using MyUserControl;


namespace WpfControlTabTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           // Uncomment IsThisRun() if you want to use prevent running duplicate application instance ...
           
            // Uses only Debug Mode,if IsThisRun() uncommented ... or close Visual Studio ans Run Again (This application hosted in VS -> you always have one working instance)
             IsThisRun();

            InitTabControl();
        }

        // This use for sending message to Running process (Application had run already (first instance))
  
        private void IsThisRun()
        {
            // http://stackoverflow.com/questions/7182949/how-to-check-if-a-wpf-application-is-already-running

            Process currentProcess = Process.GetCurrentProcess();

            Process[] processes = Process.GetProcesses();
            List<int> sameProcessId = new List<int>();    // Do not create List Of Process - will not be working below logic ...

            foreach (Process p in processes)
            {
                string[] strName = p.ProcessName.Split('.');  // Any process
                string[] strCurrentName = currentProcess.ProcessName.Split('.');
                if (strName[0].IndexOf(strCurrentName[0]) >= 0)    // if do not restrict by "host" - no RUN Without Debug    && strName[1].IndexOf("host") < 0
                {
                    sameProcessId.Add(p.Id);
                }
            }

            if (sameProcessId.Count > 1)
            {
                String stAllProcess = "";
                foreach (int intId in sameProcessId)
                {
                    stAllProcess += intId.ToString() + ";";
                }
                MessageBox.Show(" All Processes Id's - " + stAllProcess);

                sameProcessId.Remove(currentProcess.Id); // Remove Current Process From List and Send Message to Last Process ...

                Process runnigProcess = Process.GetProcessById(sameProcessId[0]);
                MessageBox.Show(" Message Send To Process Id - " + runnigProcess.Id.ToString());
                
                MyUserControl.CommonStaticLibrary.SendMessage(runnigProcess.MainWindowHandle, MyUserControl.CommonStaticLibrary.MESSAGE_CREATE_NEW_FULL_WINDOW, IntPtr.Zero, IntPtr.Zero); // MESSAGE_CREATE_NEW_FULL_WINDOW = 0xA123;
                MessageBox.Show(" Will Kill Process Id ", currentProcess.Id.ToString());
                currentProcess.Kill();
            }
        }


        // Populate TabControl with some contents
        private void InitTabControl()
        {
            for (int i = 0; i < 7; i++)  // create five tab with different background color ...
            {
                MyTabControl.addNewTabItem(initObject(), "TAB" + i.ToString());
            }

            // MUST set UserControl parameters "ParentWindow"  Very Important !!!!
            MyTabControl.ParentWindow = this;
        }


        // Set some object to new TAB (it can be any object - image, table, text, any...)
        private object initObject()
        {
            Grid grd = new Grid();
            grd.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, MyTabControl.rnd(), MyTabControl.rnd(), MyTabControl.rnd()));
            return grd;
        }

    }
}

