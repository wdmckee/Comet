using System;
using System.Collections.Generic;
using System.IO;
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
        static int index = 0;
        static DateTime today = DateTime.Today;
        static string outputpath;
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
            if (Mouse.wparam == "513" && Keyboard.CurrentKeyPressed == "A") // down
            {
                screen.CreateBox();
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
            }
            if (Mouse.wparam == "512" && Mouse.LeftIsDown == true &&  Keyboard.CurrentKeyPressed == "A") // moving
            {              
                screen.ResizeBox(Mouse.LastDown_physical_location, Mouse.current_pysical_location);
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
            }

            if (Mouse.wparam == "514" && Keyboard.CurrentKeyPressed == "A") // up
            {
                //Console.WriteLine("{0} , {1}  :  {2} , {3} : {4}", Mouse.previous.X.ToString(), Mouse.previous.Y.ToString(), Mouse.current.X.ToString(), Mouse.current.Y.ToString(), Mouse.wparam);
                screen.CloseBox();
                screen.CaptureSave( GetPath(), Mouse.LastDown_physical_location, Mouse.LastUp_physical_location);
                Keyboard.CurrentKeyPressed = "";
                index++;
            }

        }

        private static void KeyboardEvent(object sender, EventArgs e)
        {
            /*Console.WriteLine(((Keyboard.KeyboardEventArgs)e).Data)*/;

             var a = ((Keyboard.KeyboardEventArgs)e).Data;
            if ( a.ToString() == "Space")
            {
                //screen.CaptureApp();
                screen.CaptureAppMenu(GetPath());
                index++;
            }
            if (a.ToString() == "A")
            {
                screen.CreateCover(0);
                //screen.Draw(Mouse.previous, Mouse.current);
            }
            if (a.ToString() == "Z")
            {
                Application.Exit();
            }



        }



        internal static string GetPath()
        {
            var outputpath = string.Format("{0}\\{1}\\{2}.bmp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), today.ToString("yyyyMMdd"), index);
            DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(outputpath));

            return outputpath;

        }











    }
}
