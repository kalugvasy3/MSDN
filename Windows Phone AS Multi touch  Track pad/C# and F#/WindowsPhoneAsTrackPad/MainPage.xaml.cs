using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.ServiceModel;
using System.ServiceModel.Description;



using Windows.Web.Http;


namespace TrackPadSilverlight

{
    public partial class MainPage : PhoneApplicationPage
    {

        Int32 MOUSEEVENTF_ABSOLUTE = 0x8000;
        Int32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        Int32 MOUSEEVENTF_LEFTUP = 0x0004;
        Int32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        Int32 MOUSEEVENTF_MIDDLEUP = 0x0040;
        Int32 MOUSEEVENTF_MOVE = 0x0001;
        Int32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        Int32 MOUSEEVENTF_RIGHTUP = 0x0010;
        Int32 MOUSEEVENTF_XDOWN = 0x0080;
        Int32 MOUSEEVENTF_XUP = 0x0100;
        Int32 MOUSEEVENTF_WHEEL = 0x0800;
        Int32 MOUSEEVENTF_HWHEEL = 0x01000;

        // Common Values

        Point lastTouchedPoint0 = new Point(0, 0);
        WindowsPhoneMouseService.MouseEventClient mClient;

        public MainPage()
        {
            InitializeComponent();
            Touch.FrameReported += Touch_FrameReported;
        }

        private double deltaX = 0.0;
        private double deltaY = 0.0;
        private bool blnWas2or3 = false;


        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            TouchPointCollection touchPoints = e.GetTouchPoints(rectPad);



            if (touchPoints.Count == 1 && blnWas2or3==false)
            {

                if (touchPoints[0].Position.Y > rectPad.ActualHeight - 55)
                {

                    if (touchPoints[0].Position.X < this.ActualWidth * 0.5)
                    {
                        txtTap.Text = "  Left Button Click.";

                        if (touchPoints[0].Action == TouchAction.Down)
                        {
                            mClient.MouseMoveTupleAsync(MOUSEEVENTF_LEFTDOWN.ToString(), "0", "0", "0", "0");
                        }

                        if (touchPoints[0].Action == TouchAction.Up)
                        {
                            mClient.MouseMoveTupleAsync(MOUSEEVENTF_LEFTUP.ToString(), "0", "0", "0", "0");
                        }
                    }
                    else
                    {
                        txtTap.Text = "  Right Button Click.";

                        if (touchPoints[0].Action == TouchAction.Down)
                        {
                            mClient.MouseMoveTupleAsync(MOUSEEVENTF_RIGHTDOWN.ToString(), "0", "0", "0", "0");
                        }

                        if (touchPoints[0].Action == TouchAction.Up)
                        {
                            mClient.MouseMoveTupleAsync(MOUSEEVENTF_RIGHTUP.ToString(), "0", "0", "0", "0");
                        }
                    }
                }

                else

                {
                    deltaX = (touchPoints[0].Position.X - lastTouchedPoint0.X);
                    deltaY = (touchPoints[0].Position.Y - lastTouchedPoint0.Y);


                    mClient.MouseMoveTupleAsync(MOUSEEVENTF_MOVE.ToString(), ((Int32)deltaX).ToString(), ((Int32)deltaY).ToString(), "0", "0");
                    txtTap.Text = "  MOUSE MOVE";

                    if (touchPoints[0].Action == TouchAction.Up)
                    {
                        mClient.MouseMoveTupleAsync(MOUSEEVENTF_LEFTUP.ToString(), "0", "0", "0", "0");
                    }
                }

                lastTouchedPoint0 = touchPoints[0].Position;

            }
            else if (touchPoints.Count == 2)
            {
                deltaX = touchPoints[0].Position.X - (touchPoints[1].Position.X);
                blnWas2or3 = true;

                if (Math.Abs(deltaX) < 200)   // WHEEL
                {
                    deltaY = (touchPoints[0].Position.Y - lastTouchedPoint0.Y) * 3.0; // 3.0 increase speed ...if needed 
                    mClient.MouseMoveTupleAsync(MOUSEEVENTF_WHEEL.ToString(), "0", "0", ((Int32)deltaY).ToString(), "0");
                    lastTouchedPoint0 = touchPoints[0].Position;
                }

                lastTouchedPoint0 = touchPoints[0].Position;
            }

            else if (touchPoints.Count == 3)

            {

                deltaX = (touchPoints[0].Position.X - lastTouchedPoint0.X);
                deltaY = (touchPoints[0].Position.Y - lastTouchedPoint0.Y);
                blnWas2or3 = true;

                mClient.MouseMoveTupleAsync((MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN).ToString(), ((Int32)deltaX).ToString(), ((Int32)deltaY).ToString(), "0", "0");
                txtTap.Text = "  MOUSE MOVE";

                if (touchPoints[0].Action == TouchAction.Up)
                {
                    mClient.MouseMoveTupleAsync((MOUSEEVENTF_LEFTUP).ToString(), "0", "0", "0", "0");
                    txtTap.Text = "";
                }

                lastTouchedPoint0 = touchPoints[0].Position;
            }
        }

        private void rectPad_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            lastTouchedPoint0 = new Point(e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
            blnWas2or3 = false;
        }

        private void rectPad_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            //use Touch_FrameReported instead 
        }
        private void rectPad_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //use Touch_FrameReported instead 
        }

        // LOADED 
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // MUST INSERT YOUR DESKTOP NAME 
                // YOU MUST FORWARD PORT (see router setting to this DESKTOP)
                String url = "http://desktop-6727b93:8187/WindowsPhoneMouseService";

                EndpointAddress address = new EndpointAddress(url);
                BasicHttpBinding binding = new BasicHttpBinding();

                //FOR EMULATOR  use this ->
                //mClient = new WindowsPhoneMouseService.MouseEventClient();

                //FOR DEVICE use this ->
                mClient = new WindowsPhoneMouseService.MouseEventClient(binding, address);
                mClient.OpenAsync();
            }
            catch (Exception ex)
            {
            }
        }

        // UNLOADED 
        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            mClient.CloseAsync();
        }

    }
}