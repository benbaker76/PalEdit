using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PalEdit
{
    public class Win32
    {
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;

        public const uint PM_REMOVE = 0x0001;

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        public static void RemoveMouseUpMessage()
        {
            NativeMessage msg;

            while (Win32.PeekMessage(out msg, IntPtr.Zero, WM_LBUTTONUP, WM_LBUTTONUP, PM_REMOVE)) ;
        }
    }
}
