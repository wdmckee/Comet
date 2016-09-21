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


        static Screen screen;
        //static CursorPoint current_cusor;
        //static CursorPoint prev_cusor;

        

        static void Main(string[] args)
        {
            screen = new Screen();
            //current_cusor = new CursorPoint();
            //prev_cusor = new CursorPoint();

            // Get window information
            var handle = GetConsoleWindow();
            Console.WriteLine(string.Format("handle: {0}", handle.ToString()));


            // hide window
            Window consoleWindow = new Window();
           consoleWindow.HideConsole(handle);


            Keyboard keyboard = new Keyboard();
            Keyboard.KeyboardAction += new EventHandler(KeyboardEvent);


            Mouse mouse = new Mouse();
            Mouse.MouseAction += new EventHandler(MouseEvent);


       


            Application.Run();// used windows form to force a message loop (see refernces)


        }

        
        private static void MouseEvent(object sender, EventArgs e)
        {

            //current_cusor = ((Mouse.MouseEventArgs)e).CurrentPts;
            //prev_cusor = ((Mouse.MouseEventArgs)e).PrevPts;
            //var a = (uint)Mouse.wparam;
            if (Mouse.wparam == "513") // down
            {
                screen.CreateBox();
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
            }
            if (Mouse.wparam == "512" && Mouse.LeftIsDown == true) // moving
            {              
                screen.ResizeBox(Mouse.LastDown_physical_location, Mouse.current_pysical_location);
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
            }

            if (Mouse.wparam == "514") // up
            {
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
                screen.CloseBox();
                screen.Capture(0, @"C:\Users\derek.mckee\Desktop\img", Mouse.LastDown_physical_location, Mouse.LastUp_physical_location);

            }

        }

        private static void KeyboardEvent(object sender, EventArgs e)
        {
            /*Console.WriteLine(((Keyboard.KeyboardEventArgs)e).Data)*/;

            var a = ((Keyboard.KeyboardEventArgs)e).Data;
            if ( a.ToString() == "C")
            {
                //screen.Capture(0, @"C:\Users\derek.mckee\Desktop\img");
            }
            if (a.ToString() == "A")
            {
                //screen.Draw(Mouse.previous, Mouse.current);
            }


        }















    }
}
