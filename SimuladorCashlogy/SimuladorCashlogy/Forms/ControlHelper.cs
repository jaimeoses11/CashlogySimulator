using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimuladorCashlogy.Forms
{
    public static class ControlHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control Target)
        {
            SendMessage(Target.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        }

        public static void ResumeDrawing(Control Target)
        {
            SendMessage(Target.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            Target.Invalidate(true);
            Target.Update();
        }
    }
}
