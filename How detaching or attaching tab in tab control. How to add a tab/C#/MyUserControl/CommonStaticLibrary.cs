using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace MyUserControl
{
    static public class CommonStaticLibrary
    {

        // Static List of Opened MainWindow's
        public static List<Window> WinList = new List<Window>();
        public static int IntActiveWindow = -1;

        // We need below just for one simple trick - mouse must to jump to new created temporary window ...

        // Mouse Event -------------------------------------------------------------------------------
        // http://stackoverflow.com/questions/8739523/directing-mouse-events-dllimportuser32-dll-click-double-click
        // http://www.pinvoke.net/default.aspx/user32.mouse_event


        // Logic just programatical change Left Click from Down to Up and again Down ...
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        static public void sendMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        static public void sendMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        // http://ryanfarley.com/blog/archive/2004/05/10/605.aspx
        // http://stackoverflow.com/questions/6779731/c-sharp-using-sendmessage-problem-with-wm-copydata

        public const int MESSAGE_CREATE_NEW_FULL_WINDOW = 0xA123;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);



    }
}
