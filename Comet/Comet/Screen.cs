using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comet
{
    class Screen
    {


        #region Imports
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);



        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);



        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        // public static extern IntPtr GetMenu(HandleRef hWnd);
        static extern IntPtr GetMenu(IntPtr hWnd);







        #endregion





        static Form formBox;
        static Form formCover;
        Graphics graphics;
    


        public Screen()
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            graphics = Graphics.FromHdc(desktop);
        }



        public void CreateCover(int freezeFlag)
        {
            if (formCover == null)
            {
               
                formCover = new Form();
                formCover.ShowInTaskbar = false;
                IntPtr desktop = GetDC(formCover.Handle);
                graphics = Graphics.FromHdc(desktop);

                formCover.Location = new Point(0, 0);
                formCover.BackColor = Color.Red;
                formCover.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width + 50;
                formCover.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                formCover.Opacity = .25;

                // this is not really what I want. it takes an image of whats under the frozen section
                if (freezeFlag == 1)
                {
                    formCover.BackgroundImage = CaptureScreen();
                    formCover.Width = formCover.BackgroundImage.Width;
                    formCover.Height = formCover.BackgroundImage.Height;
                    
                    formCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                    formCover.Opacity = 1;


                    /*minimize all other windows*/
                    const int WM_COMMAND = 0x111;
                    const int MIN_ALL = 419;
                    const int MIN_ALL_UNDO = 416;
                    IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
                    SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
                    //System.Threading.Thread.Sleep(2000);
                    SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
                }



                

                formCover.FormBorderStyle = FormBorderStyle.None;







                formCover.Show();
                formCover.BringToFront();
                //formCover.Focus();
            }

        }

        public void CreateBox()
        {
           
            
            formBox = new Form();
            formBox.FormBorderStyle = FormBorderStyle.None;
            formBox.ShowInTaskbar = false;

            formBox.Opacity = .1;
            formBox.Height = 100; formBox.Width = 100;
            formBox.Show();
            
        }

        public void ResizeBox(CursorPoint from, CursorPoint to)
        {
            formCover.BringToFront();
            formBox.BringToFront();
            var x = from.X;
            var y = from.Y;

            var x1 = to.X;
            var y1 = to.Y;

            formBox.Location = new Point(x, y);
            formBox.Opacity = .1;
            formBox.Height = y1 - y ;
            formBox.Width = x1 - x;

        }
        public void CloseBox()
        {


            formBox.Close();
            formCover.Close();
            formCover = null;

        }





        public void Draw(CursorPoint from, CursorPoint to, IntPtr handle)
        {
            IntPtr win = GetDC(handle);
            //IntPtr desktop = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(win))
            {
                //    //    //g.FillRectangle(Brushes.Red, 0, 0, cp.X, cp.Y);
                //    //    // g.DrawRectangle(new Pen(Brushes.Red), 0, 0, cp.X, cp.Y);
                  Pen pen = new Pen(Brushes.Red);
                  pen.Width = 3;
               g.DrawRectangle(pen, from.X, from.Y, (to.X- from.X), (to.Y-from.Y));
            }
            ReleaseDC(IntPtr.Zero, handle);
        }





        public void CaptureSave(int index, string path, CursorPoint from, CursorPoint to)
        {

            var x = from.X;/// 2;
            var y = from.Y;// / 2;

            var x1 = to.X;// / 2;
            var y1 = to.Y;// / 2;


            Rectangle bounds = new Rectangle(new Point(x * 2, y * 2), new Size((x1 - x) * 2, (y1 - y) * 2));//Screen.GetBounds(Point.Empty);
            if (bounds.Height > 10 && bounds.Width > 10)
            {
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    }
                    var fullpath = string.Format("{0}\\{1}.bmp", path, index);
                    bitmap.Save(fullpath, ImageFormat.Bmp);
                }
            }


        }


        public Bitmap CaptureScreen()
        {

            var x =  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var y =  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;


    
            Rectangle bounds = new Rectangle(new Point(0, 0), new Size(x*2, y*2));//Screen.GetBounds(Point.Empty);
            Bitmap retValue = new Bitmap(bounds.Width, bounds.Height);

            if (bounds.Height > 10 && bounds.Width > 10)
            {
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    }
                    retValue = (Bitmap)bitmap.Clone();
                    var fullpath = string.Format("{0}\\{1}.bmp", @"C:\Users\derek.mckee\Desktop\img", 99);
                    retValue.Save(fullpath, ImageFormat.Bmp);
                }
            }

            return retValue;
        }

        public Bitmap CaptureApp()
        {
            RECT rc;
            IntPtr hwnd = GetForegroundWindow();
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap((rc.Right-rc.Left)*2, (rc.Bottom-rc.Top)*2, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();


            var fullpath = string.Format("{0}\\{1}.bmp", @"C:\Users\derek.mckee\Desktop\img", 99);
            bmp.Save(fullpath, ImageFormat.Bmp);


            return bmp;
           
            



            //var x = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            //var y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            //IntPtr handle = GetForegroundWindow();

            //Rectangle bounds = new Rectangle(new Point(0, 0), new Size(x * 2, y * 2));//Screen.GetBounds(Point.Empty);
            //Bitmap retValue = new Bitmap(bounds.Width, bounds.Height);

            //if (bounds.Height > 10 && bounds.Width > 10)
            //{
            //    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            //    {
            //        using (Graphics g = Graphics.FromHwnd(handle))
            //        {
            //            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            //        }
            //        retValue = (Bitmap)bitmap.Clone();
            //        var fullpath = string.Format("{0}\\{1}.bmp", @"C:\Users\derek.mckee\Desktop\img", 99);
            //        retValue.Save(fullpath, ImageFormat.Bmp);
            //    }
            //}

            //return retValue;
        }

        public Bitmap CaptureAppMenu()
        {
            //RECT rc;
            //IntPtr handle = GetForegroundWindow();
            //IntPtr hwnd = GetMenu(handle);

            //GetWindowRect(hwnd, out rc);

            RECT rc;
            IntPtr hwnd = GetForegroundWindow();
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap((rc.Right - rc.Left) * 2, (rc.Bottom - rc.Top) * 2, PixelFormat.Format32bppArgb);
            
            //Rectangle bounds = new Rectangle(new Point(rc.Left*2, rc.Top*2), new Size(bmp.Width, bmp.Height));//Screen.GetBounds(Point.Empty);
          

          
                using (bmp)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(new Point(rc.Left*2+10, rc.Top*2-2), Point.Empty, new Size(bmp.Width-21, bmp.Height-10));
                    }
                   
                    var fullpath = string.Format("{0}\\{1}.bmp", @"C:\Users\derek.mckee\Desktop\img", 98);
                    bmp.Save(fullpath, ImageFormat.Bmp);
                }
          

            return bmp;



            //Bitmap bmp = new Bitmap((rc.Right - rc.Left) * 2, (rc.Bottom - rc.Top) * 2, PixelFormat.Format32bppArgb);
            //Graphics gfxBmp = Graphics.FromImage(bmp);
            //IntPtr hdcBitmap = gfxBmp.GetHdc();

            //PrintWindow(hwnd, hdcBitmap, 0);

            //gfxBmp.ReleaseHdc(hdcBitmap);
            //gfxBmp.Dispose();


            //var fullpath = string.Format("{0}\\{1}.bmp", @"C:\Users\derek.mckee\Desktop\img", 99);
            //bmp.Save(fullpath, ImageFormat.Bmp);


            //return bmp;



        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

    }



    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }



}
