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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SqLiteEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainUserControl.styleChanged += new UserControlWindow.routedEventHandler(object_StyleChanged);
            this.WindowStyle = WindowStyle.SingleBorderWindow; 
        }

        private void object_StyleChanged(object sender, UserControlWindow.routedEventArgs e)
        {
            UserControlWindow obj = sender as UserControlWindow;           

            if (e.intStyle == 0) SqLiteEditor.WindowStyle = WindowStyle.None;
            if (e.intStyle == 1) SqLiteEditor.WindowStyle = WindowStyle.SingleBorderWindow;  // Default
            if (e.intStyle == 2) SqLiteEditor.WindowStyle = WindowStyle.ToolWindow;
 //           if (e.intStyle == 3) SqLiteEditor.WindowStyle = WindowStyle.ThreeDBorderWindow;

        }

        private void SqLiteEditor_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Topmost = false;
        }



    }
}
