using System.Runtime.InteropServices;

namespace Control_PC
{
    internal class MouseSimulator
    {
        private static uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private static uint MOUSEEVENTF_LEFTUP = 0x0004;
        private static uint MOUSEEVENTF_DOUBLECLICK = 0x0020;
        private static uint MOUSEEVENTF_WHEEL = 0x0800;
        private static uint WHEEL_DELTA = 120;

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        public static void Press()
        {
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
        }

        public static void Release()
        {
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public void click()
        {
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        internal void doubleClick()
        {
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP | MOUSEEVENTF_DOUBLECLICK, X, Y, 0, 0);
        }

        internal void scroll(int scrollAmount)
        {
            uint X = (uint)System.Windows.Forms.Cursor.Position.X;
            uint Y = (uint)System.Windows.Forms.Cursor.Position.Y;
            int scrollData = scrollAmount * (int)WHEEL_DELTA;
            mouse_event(MOUSEEVENTF_WHEEL, X, Y, (uint)scrollData, 0);
        }

        internal void pressLeft()
        {
            Press();
        }

        internal void releaseLeft()
        {
            Release();
        }
    }
}
