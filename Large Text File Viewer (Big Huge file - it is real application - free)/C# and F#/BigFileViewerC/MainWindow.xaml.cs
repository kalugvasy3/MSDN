using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BigFileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyUserControl.BigFileViewer ucMainWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Title = "Big File Viewer";
            this.MinHeight = 256.0;
            this.MinWidth = 512.0;
            this.MaxHeight = 2160.0;  // limit lines per screen 200 lines
            this.MaxWidth = 3000.0;

            this.SizeToContent = SizeToContent.WidthAndHeight;

            ucMainWindow = new MyUserControl.BigFileViewer();
            this.Content = ucMainWindow;

            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
            this.SizeChanged += MainWindow_SizeChanged;

        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            GC.Collect();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ucMainWindow.WinHolder = this;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ucMainWindow.WinHolder = this;
        }


    }
}
