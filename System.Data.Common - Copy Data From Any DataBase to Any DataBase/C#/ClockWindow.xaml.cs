
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.Media3D.Converters;

namespace WpfHelperCopyData
{
    public partial class ClockWindow : UserControl
    {

        public DateTime dateTimeStarted = DateTime.Now;  

        public ClockWindow()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            InitializeComponent();

            DateTime date = DateTime.Now;
            TimeZone time = TimeZone.CurrentTimeZone;
            TimeSpan difference = time.GetUtcOffset(date);

            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                TimeSpan tmpTime =  DateTime.Now.Subtract(dateTimeStarted);

                int Second01 = tmpTime.Seconds - (tmpTime.Seconds / 10) * 10;
                int Second10 = tmpTime.Seconds / 10;
                int Minute01 = tmpTime.Minutes - (tmpTime.Minutes / 10) * 10;

                switch (Minute01)
                {
                    case 1:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_1");
                        break;
                    case 2:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_2");
                        break;
                    case 3:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_3");
                        break;
                    case 4:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_4");
                        break;
                    case 5:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_5");
                        break;
                    case 6:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_6");
                        break;
                    case 7:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_7");
                        break;
                    case 8:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_8");
                        break;
                    case 9:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_9");
                        break;
                    case 0:
                        GeometryModel3DGeometry_1m.Geometry = (Geometry3D)TryFindResource("Digit_0");
                        break;
                }

                switch (Second10)
                {
                    case 1:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_1");
                        break;
                    case 2:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_2");
                        break;
                    case 3:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_3");
                        break;
                    case 4:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_4");
                        break;
                    case 5:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_5");
                        break;
                    case 6:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_6");
                        break;
                    case 7:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_7");
                        break;
                    case 8:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_8");
                        break;
                    case 9:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_9");
                        break;
                    case 0:
                        GeometryModel3DGeometry_10s.Geometry = (Geometry3D)TryFindResource("Digit_0");
                        break;
                }
                switch (Second01)
                {
                    case 1:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_1");
                        break;
                    case 2:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_2");
                        break;
                    case 3:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_3");
                        break;
                    case 4:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_4");
                        break;
                    case 5:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_5");
                        break;
                    case 6:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_6");
                        break;
                    case 7:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_7");
                        break;
                    case 8:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_8");
                        break;
                    case 9:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_9");
                        break;
                    case 0:
                        GeometryModel3DGeometry_1s.Geometry = (Geometry3D)TryFindResource("Digit_0");
                        break;
                }

            }));
        }

        private void GeometryModel3D_1sChanged(object sender, EventArgs e)
        {
            Storyboard Storyboard1s = (Storyboard)TryFindResource("Storyboard_1s");
            Storyboard1s.Begin();

        }

        private void GeometryModel3D_10sChanged(object sender, EventArgs e)
        {
            Storyboard Storyboard10s = (Storyboard)TryFindResource("Storyboard_10s");
            Storyboard10s.Begin();
        }

        private void GeometryModel3D_1mChanged(object sender, EventArgs e)
        {
            Storyboard Storyboard1m = (Storyboard)TryFindResource("Storyboard_1m");
            Storyboard1m.Begin();
        }


        private void Shadow_0(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 0;
        }
        private void Shadow_1(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 1;
        }
        private void Shadow_2(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 2;
        }
        private void Shadow_3(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 3;
        }
        private void Shadow_4(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 4;
        }
        private void Shadow_5(object sender, RoutedEventArgs e)
        {
            DigitDropShadow.ShadowDepth = 5;
        }



    }

}