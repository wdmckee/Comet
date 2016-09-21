using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Comet
{
    class Mouse
    {
        #region Imports

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetPhysicalCursorPos(ref CursorPoint lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref CursorPoint lpPoint);

  

        #endregion

        #region Constants
        private const int WH_MOUSE_LL = 14;

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_MOUSEMOVE = 0x0200;

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static UIntPtr _zero = UIntPtr.Zero;

        #endregion

        #region Delegates

        internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Events
        public static event EventHandler MouseAction = delegate { };


        public class MouseEventArgs : EventArgs
        {
            private CursorPoint m_PrevPts;
            private CursorPoint m_CurrentPts;
            public MouseEventArgs(CursorPoint _CurrentPts, CursorPoint _PrevPts)
            {
                m_CurrentPts = _CurrentPts;
                m_PrevPts = _PrevPts;
            } // eo ctor

           
            public CursorPoint CurrentPts { get { return m_CurrentPts; } }
            public CursorPoint PrevPts { get { return m_PrevPts; } }
        }
        #endregion

        public static CursorPoint current_pysical_location { get; set; }
        public static CursorPoint previous_physical_location { get; set; }

        public static CursorPoint LastDown_physical_location { get; set; }
        public static CursorPoint LastUp_physical_location { get; set; }




        public static CursorPoint current_location { get; set; }
        public static CursorPoint previous_location { get; set; }

        public static CursorPoint LastDown_location { get; set; }
        public static CursorPoint LastUp_location { get; set; }






        public static bool LeftIsDown { get; set; }

        public static string wparam { get; set; }

        #region Methods


        public Mouse()
        {
            var retVal = SetHook(_proc);
            current_pysical_location = new CursorPoint();
            previous_physical_location = new CursorPoint();
        }


        public static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }



        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            //{

            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            
            previous_physical_location = current_pysical_location;
            current_pysical_location = hookStruct.pt;

            previous_location = current_location;
            current_location = hookStruct.pt;

            CursorPoint lpp = new Comet.CursorPoint();
            Mouse.GetPhysicalCursorPos(ref lpp);

            CursorPoint lp = new Comet.CursorPoint();
            Mouse.GetCursorPos(ref lp);

            current_pysical_location =  lpp;
            current_location = lp;






            wparam = wParam.ToString();

            if (wparam == "513")
            {              
                LastDown_physical_location = current_pysical_location;
                LastDown_location = current_location;
                LeftIsDown = true;
            }
            if (wparam == "514")
            {
                LastUp_physical_location = current_pysical_location;
                LastUp_location = current_location;
                LeftIsDown = false;
            }

            MouseAction(null, new MouseEventArgs(current_pysical_location, previous_physical_location));
            
          //  }



            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }





        /*
        void sendMouseRightclick(Point p)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, p.X, p.Y, 0, 0);
        }

        void sendMouseDoubleClick(Point p)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);

            Thread.Sleep(150);

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);
        }

        void sendMouseRightDoubleClick(Point p)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, p.X, p.Y, 0, 0);

            Thread.Sleep(150);

            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, p.X, p.Y, 0, 0);
        }
        */



        public CursorPoint GetPhysicalLocation()
        {
            CursorPoint cursorPos = new CursorPoint();
            GetPhysicalCursorPos(ref cursorPos);
            return cursorPos;

        }

        public CursorPoint GetLocation()
        {
            CursorPoint cursorPos = new CursorPoint();
            GetCursorPos(ref cursorPos);
            return cursorPos;

        }


        public bool ShowUsage()
        {
            CursorPoint cursorPos = new CursorPoint();
            try
            {
                Console.WriteLine(cursorPos.X.ToString());
                return GetPhysicalCursorPos(ref cursorPos);
                
            }
            catch (EntryPointNotFoundException) // Not Windows Vista
            {
                return false;
            }
        }


        public void sendMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 50, 50, 0, _zero);
        }

        void sendMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 50, 50, 0, _zero);
        }



        #endregion










    }



    public struct CursorPoint
    {
        public int X;
        public int Y;
    }

    struct MSLLHOOKSTRUCT
    {
        public CursorPoint pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }


}
