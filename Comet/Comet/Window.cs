using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Comet
{
    class Window
    {

        #region Imports

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion



        #region Constants
        const int SW_HIDE = 0;


        #endregion






        public Window()
        {


        }


        public  void HideConsole(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_HIDE);

        }



    }
}
