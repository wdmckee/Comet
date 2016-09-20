using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet
{
    class Program
    {


        #region Imports
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        #endregion



        #region Constants
     


        #endregion








        static void Main(string[] args)
        {
            

            // Get window information
            var handle = GetConsoleWindow();
            Console.WriteLine(string.Format("handle: {0}", handle.ToString()));


            // hide window
            Window consoleWindow = new Window();
            //consoleWindow.HideConsole(handle);


            Keyboard keyboard = new Keyboard();
            Keyboard.KeyboardAction += new EventHandler(KeyboardEvent);


            Mouse mouse = new Mouse();
            Mouse.MouseAction += new EventHandler(MouseEvent);



            Application.Run();// used windows form to force a message loop (see refernces)


        }


        private static void MouseEvent(object sender, EventArgs e)
        {
            Console.WriteLine( ((Mouse.MouseEventArgs)e).Data);
        }

        private static void KeyboardEvent(object sender, EventArgs e)
        {
            Console.WriteLine(((Keyboard.KeyboardEventArgs)e).Data);
        }















    }
}
