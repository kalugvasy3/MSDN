// Just links - See also different solutions 

// http://stackoverflow.com/questions/10738161/is-it-possible-to-rearrange-tab-items-in-tab-control-in-wpf
// https://msdn.microsoft.com/en-us/library/system.windows.dragdrop.dodragdrop(v=vs.100).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
// https://blogs.msdn.microsoft.com/jaimer/2007/07/12/drag-amp-drop-in-wpf-explained-end-to-end/
// https://msdn.microsoft.com/en-us/library/aa289508(vs.71).aspx


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Interop;

using MyUserControl;

namespace MyUserControl
{
    public partial class TabUserControl : UserControl
    {
        //----------------------------------------------------------------------------------------------------------------------------
        #region ParentWindow parameter using for Detached (Separate), Attached, Move, Preliminary Selection - logic.      

        private Window parentWindow;
        public Window ParentWindow
        {
            get { return parentWindow; }
            set
            {
                parentWindow = value;
                CommonStaticLibrary.WinList.Add(parentWindow);
                ChangeTitleAll();
                parentWindow.Loaded += FullWindowFromMessages_Loaded;  //from messages
            }
        }

        private void ChangeTitleAll()
        {
            int intTotalWindows = CommonStaticLibrary.WinList.Count;
            foreach (Window w in CommonStaticLibrary.WinList)
            {
                w.Title = "MainWindow ( Total Windows = " + intTotalWindows.ToString() + " )";
            }
        }

        // Unloaded Window ...
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CommonStaticLibrary.WinList.Remove(parentWindow);
            ChangeTitleAll();
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------

        public TabUserControl()
        {
            InitializeComponent();
        }

        //----------------------------------------------------------------------------------------------------------------------------
        #region   TabItem New/Add/Remove/Create

        /// <summary>
        /// Add to TabItem all needed Events and add it  to TabContrlol.
        /// </summary>
        /// <param name="ti"></param>
        public void addTabItem(TabItem ti)
        {
            Label lbl = ti.Header as Label;
            lbl.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            lbl.MouseLeave += lbl_MouseLeave;
            lbl.MouseEnter += lbl_MouseEnter;
            tabControl.Items.Add(ti);
        }

        /// <summary>
        ///  Add New Tabitem with "content", add all needed Events/Name(s)
        ///  Create label for header and assign some events for this label
        /// </summary>
        /// <param name="content"></param>
        /// <param name="Name"></param>
        public void addNewTabItem(object content, string Name)
        {
            TabItem ti = new TabItem();
            Label lbl = new Label();
            lbl.Content = Name;
            lbl.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
            lbl.MouseLeave += lbl_MouseLeave;
            lbl.MouseEnter += lbl_MouseEnter;
            ti.Content = content;
            ti.Header = lbl;
            ti.Name = Name;
            tabControl.Items.Add(ti);
        }

        /// <summary>
        /// Remove All Events from TabItem
        /// </summary>
        /// <param name="tabItem"></param>
        /// <returns></returns>

        private TabItem removeTabItemEvents(TabItem tabItem)
        {
            Label lbl = tabItem.Header as Label;

            lbl.MouseLeftButtonDown -= lbl_MouseLeftButtonDown;
            lbl.MouseLeave -= lbl_MouseLeave;
            lbl.MouseEnter -= lbl_MouseEnter;

            return tabItem;
        }

        // Mouse Events  for TabItem 
        private Point pStart = new Point(0.0, 0.0);

        /// <summary>
        /// Remember Mouse Start Point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pStart = e.GetPosition(tabControl);
        }

        /// <summary>
        /// If Mouse Enter to new TAB rewrite pStart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_MouseEnter(object sender, MouseEventArgs e)
        {

            current = (sender as Label).Background;
            (sender as Label).Background = Brushes.LightBlue;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                pStart = e.GetPosition(tabControl);
            }
        }

        Vector pDelta;
        Brush current;

        /// <summary>
        /// If Mouse Leave TAB's header (Left,Right) -> switch TABs, (Up,Down) -> detached TAB 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Background = current;
            Point pLeave = e.GetPosition(tabControl);
            pDelta = pLeave - pStart; // calculate direction base on ".X"
            int iS = tabControl.SelectedIndex;
            int iCount = tabControl.Items.Count;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Switch TABs
                if (Math.Abs(pDelta.Y) <= lbl.ActualHeight / 4.0)      // Mouse MUST leave label from left or right side 
                {
                    bool blnDirection = false;             // move right  
                    if (pDelta.X < 0) blnDirection = true; // move left  ; (mousse moved from left side)
                    if (blnDirection && iS > 0)  // move left
                    {
                        var temp = tabControl.SelectedItem;
                        tabControl.Items.RemoveAt(iS);
                        tabControl.Items.Insert(iS - 1, temp);
                        tabControl.SelectedIndex = iS - 1;
                    }
                    if (!blnDirection && iS < iCount - 1)  // move right - if last one do not move also...
                    {
                        var temp = tabControl.SelectedItem;
                        tabControl.Items.RemoveAt(iS);
                        tabControl.Items.Insert(iS + 1, temp);
                        tabControl.SelectedIndex = iS + 1;
                    }
                }
                else  // Create new TempWindow and start moving ...
                {
                    Dispatcher.Invoke(new Action(() => CreateWinTemp(e)));
                }
            }
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------------
        #region Create Temporary Window (using for attached/detached/move logic), Add Mouse Events to Temporary Window.  
       
        private Window WinTemp;  // It is temporary window  which include  TabControl with only one TabItem

        /// <summary>
        /// Create New temporary Window
        /// </summary>
        private void CreateWinTemp(MouseEventArgs e)
        {
            TabControl tabTemp = new TabControl();
            WinTemp = new Window();
            WinTemp.Name = "WinTmp";
            Grid grd = new Grid();
            grd.Children.Add(tabTemp);
            WinTemp.Content = grd;
            WinTemp.WindowStyle = WindowStyle.None;
            WinTemp.AllowsTransparency = true;         // We can set Transparency only for Windows NONE
            WinTemp.Background = Brushes.Transparent;
            TabItem ti = tabControl.SelectedItem as TabItem;
            ti = removeTabItemEvents(ti);  // removed events ...
            tabControl.Items.RemoveAt(tabControl.SelectedIndex);  // remove selected tabItem from TabContrlo 
            // Add to the temporary window events - Moving (Mouse still pressed) and Creating new MainWindow (Mouse Second UP)
            WinTemp.MouseLeftButtonDown += WinTemp_MouseLeftButtonDown;
            WinTemp.MouseLeftButtonUp += WinTemp_MouseLeftButtonUp;
            tabTemp.Items.Add(ti);
            tabTemp.Background = Brushes.Transparent;
            // Sew the size and location
            WinTemp.Left = ParentWindow.RestoreBounds.Left + e.GetPosition(this).X - 20.0;
            if (pDelta.Y > 0)
            {
                WinTemp.Top = ParentWindow.RestoreBounds.Top + e.GetPosition(this).Y + SystemParameters.CaptionHeight ; // Down
            } else
            {
                WinTemp.Top = ParentWindow.RestoreBounds.Top + e.GetPosition(this).Y ;
            }
            
            WinTemp.Width = ParentWindow.ActualWidth;
            WinTemp.Height = ParentWindow.ActualHeight;
            WinTemp.Show();
            WinTemp.Focus();
            intUpCount = 0;
            CommonStaticLibrary.sendMouseUp();   // Release Mouse - UP  (First Up - just for jump to new window during moving process)    [DllImport("user32.dll")]
            System.Threading.Thread.Sleep(10);
            CommonStaticLibrary.sendMouseDown(); // Press again ... press on new created window ... catch new created window - WinTmp     [DllImport("user32.dll")]
            initTimer(); // Init and Start Timer - This will be logic for transition process ... from paren window -> temp window -> new full window.
            if (tabControl.Items.Count == 0)
            {
                parentWindow.Close();
            }
        }

        
        private int intUpCount = 0;  // Counts "Mouse Up"

        /// <summary>
        /// Second "Up" Create Full Main Window and Delete Temporary Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinTemp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            intUpCount += 1;
            if (intUpCount == 2) // only once for each TAB will be created FullWindow
            {
                if (CommonStaticLibrary.IntActiveWindow == -1)
                {
                    //Create fullWindow from Temporary (Attached/Detached logic )

                    Window fullWindow = new Window();
                    fullWindow.Title = "MainWindow";

                    TabUserControl tuc = new TabUserControl();
                    tuc.ParentWindow = fullWindow; // WE MUST SETUP/REMEMBER  NEW WINDOW INSIDE STATIC VALUE (in USER CONTROL)

                    Uri iconUri = new Uri("pack://application:,,,/MyUserControl;component/AP.ico");
                    fullWindow.Icon = BitmapFrame.Create(iconUri);
                    Grid grdtc = WinTemp.Content as Grid;
                    TabItem ti = new TabItem();
                    TabControl tc = grdtc.Children[0] as TabControl;
                    ti = tc.Items[0] as TabItem;
                    tc.Items.Remove(ti);
                    tuc.addTabItem(ti);
                    Grid grd = new Grid();
                    grd.Children.Add(tuc); 
                    fullWindow.Content = grd;
                    fullWindow.Left = WinTemp.RestoreBounds.Left - 20;
                    fullWindow.Top = WinTemp.RestoreBounds.Top;
                    fullWindow.Width = WinTemp.ActualWidth;
                    fullWindow.Height = WinTemp.ActualHeight;
                    fullWindow.Name = "FullWindow" + (ti.Header as Label).Content.ToString();
                    fullWindow.WindowStyle = WindowStyle.SingleBorderWindow;

                    fullWindow.Show();
                    fullWindow.Focus();


                } else
                {
                    Window w = CommonStaticLibrary.WinList[CommonStaticLibrary.IntActiveWindow];
                    Grid grd = w.Content as Grid;
                    TabUserControl existUC = grd.Children[0] as TabUserControl;
                    Grid grdTc = WinTemp.Content as Grid;
                    TabControl tc = grdTc.Children[0] as TabControl;
                    TabItem ti = new TabItem();
                    ti = tc.Items[0] as TabItem;
                    tc.Items.Remove(ti);
                    existUC.addTabItem(ti);
                    existUC.Opacity = 1.0;
                    w.Focus();
                }
                aTimer.Enabled = false; // Stop Timer
                WinTemp.Close();
                CommonStaticLibrary.IntActiveWindow = -1;
                ChangeTitleAll();
            }
        }

        private void FullWindowFromMessages_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSourceParameters parameters = new HwndSourceParameters();
            HwndSource source = new HwndSource(parameters);
            source = HwndSource.FromHwnd(new WindowInteropHelper(sender as Window).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private  IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // address the messages you are receiving using msg, wParam, lParam
            if (msg == MyUserControl.CommonStaticLibrary.MESSAGE_CREATE_NEW_FULL_WINDOW)
            {
                MessageBox.Show(" Add Window to running process.");
                createFullWindow("TAB");
            }
            return IntPtr.Zero;
        }

        private void createFullWindow(string tabName)
        {

            Window fullWindow = new Window();
            fullWindow.Title = "MainWindow";

            TabUserControl tuc = new TabUserControl();
            tuc.ParentWindow = fullWindow; // WE MUST SETUP/REMEMBER  NEW WINDOW INSIDE STATIC VALUE (in USER CONTROL)

            Uri iconUri = new Uri("pack://application:,,,/MyUserControl;component/AP.ico");
            fullWindow.Icon = BitmapFrame.Create(iconUri);

            TabItem ti = new TabItem();
            Label lbl = new Label();
            lbl.Content = tabName;
            ti.Header = lbl;

            Window firstWin = CommonStaticLibrary.WinList[0];

            tuc.addTabItem(ti);
            Grid tabGrid = new Grid();
            tabGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, rnd(), rnd(), rnd()));
            ti.Content = tabGrid;

            Grid grdtc = new Grid();
            grdtc.Children.Add(tuc);

            fullWindow.Content = grdtc;
            fullWindow.Left = firstWin.RestoreBounds.Left + 44;
            fullWindow.Top = firstWin.RestoreBounds.Top + 44;
            fullWindow.Width = firstWin.ActualWidth;
            fullWindow.Height = firstWin.ActualHeight;
           // fullWindow.Name = "FullWindow" + CommonStaticLibrary.WinList.Count.ToString();
            fullWindow.WindowStyle = WindowStyle.SingleBorderWindow;

            fullWindow.Show();
            fullWindow.Focus();
        }

        // Below using just create Random Color ...
        private int nextWaiting = 111;
        // help function random generator ...
        public byte rnd()
        {
            Thread.Sleep(nextWaiting);
            var random = new Random((byte)System.DateTime.Now.Millisecond);
            nextWaiting = (byte)random.Next(111, 255);
            return (byte)nextWaiting;
        }

        /// <summary>
        ///  Using for Move Temporary Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinTemp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            WinTemp.DragMove();
        }

        private System.Timers.Timer aTimer;
        private void initTimer()
        {
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        // Find Parent Window - if WinTemp over specific region of parent window it will be parent ... apply opacity ...
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                parentWindow.Title = WinTemp.RestoreBounds.ToString();

                var tempY = WinTemp.RestoreBounds.Top;
                var tempX = WinTemp.RestoreBounds.Left;

                int countList = CommonStaticLibrary.WinList.Count;
                for (int intW = 0; intW < countList; intW++) 
                {
                    Window win = (Window)CommonStaticLibrary.WinList[intW];  // need to cast to Window - because MainWindow is MainWindow class.
                    var winCap = SystemParameters.CaptionHeight;
                    // Header Height for Current Window

                    Label lb;
                    TabUserControl tUc;

                    Grid grd = win.Content as Grid;
                    tUc = grd.Children[0] as TabUserControl;
                    var ti = tUc.tabControl.Items[0] as TabItem;
                    lb = ti.Header as Label;

                    var tabHeaderH = lb.ActualHeight;

                    var winY = win.RestoreBounds.Top;
                    var winX = win.RestoreBounds.Left;
                    var winW = win.RestoreBounds.Width;

                    CommonStaticLibrary.IntActiveWindow = -1;
                    tUc.Opacity = 1.0;

                    if ((tempY > winY + winCap) && (tempY < winY + winCap + tabHeaderH))
                    {
                        if ((tempX < winX + winW) && (tempX > winX))
                        {
                            tUc.Opacity = 0.5;
                            CommonStaticLibrary.IntActiveWindow = intW;
                            return;
                        }
                    } 
                }

            }));
        }

        #endregion
        //----------------------------------------------------------------------------------------------------------------------------
    }
}
