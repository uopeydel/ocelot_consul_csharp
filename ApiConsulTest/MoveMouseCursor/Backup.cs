using System;
using System.Runtime.InteropServices;

namespace MoveMouseCursor
{
    //class Backup
    //{
    //    static void Backup_(string[] args)
    //    {
    //        while (true)
    //        {

    //            Win32.POINT p = new Win32.POINT();
    //            p.x = Convert.ToInt16("600");
    //            p.y = Convert.ToInt16("600");

    //            //Win32.ClientToScreen(this.Handle, ref p);
    //            Win32.SetCursorPos(p.x, p.y);

    //            Console.ReadKey();

    //            Win32.POINT point;
    //            Win32.GetCursorPos(out point);

    //            Console.Clear();
    //            Console.WriteLine("(x => {0} , y => {1})", point.x, point.y);

    //        }

    //    }
    //}

    //public static class Win32
    //{
    //    [DllImport("user32.dll")]
    //    public static extern bool GetCursorPos(out POINT lpPoint);

    //    [DllImport("User32.Dll")]
    //    public static extern long SetCursorPos(int x, int y);

    //    [DllImport("User32.Dll")]
    //    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

    //    [StructLayout(LayoutKind.Sequential)]
    //    public struct POINT
    //    {
    //        public int x;
    //        public int y;
    //    }
    //}
}
