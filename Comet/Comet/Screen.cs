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




        #endregion





        static Form formBox;
        static Form formCover;
        Graphics graphics;



        public Screen()
        {
            IntPtr desktop = GetDC(IntPtr.Zero);
            graphics = Graphics.FromHdc(desktop);
        }



        public void CreateBox()
        {
            formCover = new Form();
            formCover.Opacity = .05;
            formCover.Width = SystemInformation.VirtualScreen.Width;
            formCover.Height = SystemInformation.VirtualScreen.Height;
            formCover.FormBorderStyle = FormBorderStyle.None;            
            formCover.Show();
            formCover.SendToBack();




            formBox = new Form();
            formBox.FormBorderStyle = FormBorderStyle.None;

            formBox.Opacity = .10;
            formBox.Height = 100; formBox.Width = 100;
            formBox.Show();
        }

        public void ResizeBox(CursorPoint from, CursorPoint to)
        {
            var x = from.X;
            var y = from.Y;

            var x1 = to.X;
            var y1 = to.Y;

            formBox.Location = new Point(x, y);
            formBox.Opacity = .70;
            formBox.Height = y1 - y ;
            formBox.Width = x1 - x;
            //formBox.Height =   to.Y - from.Y/ (Int32)graphics.DpiY;
            //formBox.Width = (to.X - from.X)/ (Int32)graphics.DpiX;
            //Console.WriteLine("{0}:{1}", x, y);
        }
        public void CloseBox()
        {


           formBox.Close();
            formCover.Close();
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





        public void Capture(int index, string path, CursorPoint from, CursorPoint to)
        {

            var x = from.X;/// 2;
            var y = from.Y;// / 2;

            var x1 = to.X;// / 2;
            var y1 = to.Y;// / 2;
            

            Rectangle bounds = new Rectangle(new Point(x*2,y*2), new Size((x1-x)*2,(y1-y)*2));//Screen.GetBounds(Point.Empty);
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



    }
}
