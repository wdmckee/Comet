using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet
{
    class Image
    {



  

        public  void ShowDiff(string function, string folder)
        {
           // string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            var files = GetAllBitmaps(folder);
            Bitmap image1, image2, new_image = new Bitmap(1, 1);




            for (int i = 0; i < files.Count-1; i++)
            {
                int fileIndex = i;
                int fileIndex2 = i+1;

                var fileName = (i).ToString();
                var x_path = Path.Combine(folder, string.Format("out_{0}.bmp", fileName));


                if (i==0) { fileName = (i + 1).ToString(); }



                //if (i == 0)
                //{

                var file1 = files[fileIndex];
                var file2 = files[fileIndex2];


          


                    image1 = (Bitmap)Bitmap.FromFile(file1);
                    image2 = (Bitmap)Bitmap.FromFile(file2);
                    new_image = Controller_Function(function, image1, image2, x_path, i + 1);
                    

                //}
         

           

















            }

            Controller_Function(function, new_image, new_image, Path.Combine(folder, string.Format("out_{0}.bmp",  "0")), 0);
            // new_image.Save(x_path);
        }

        
        public  List<string> GetAllBitmaps(string x_path)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string[] files = System.IO.Directory.GetFiles(x_path, "*.bmp");
            List<string> file_paths = new List<string>();


            return files.OrderBy(x => x).ToList();
        }



        private  Bitmap Controller_Function(string function, Bitmap imag1, Bitmap imag2, string x_path, int iteration)
        {
            Bitmap new_image = new Bitmap(1, 1);
            
                    new_image = DiffOnly_Recursive(imag1, imag2);
                    if (iteration != 0) { new_image.Save(x_path); }
                    return new_image;
              

            }



        















        private  Bitmap DiffOnly_Recursive(Bitmap imag1, Bitmap imag2)  //, Bitmap imag2
        {

            var colors = Enum.GetValues(typeof(KnownColor))
          .Cast<KnownColor>()
          .Select(Color.FromKnownColor);


            int rows, cols, row, col;         

            rows = imag1.Height;
            cols = imag1.Width;



            
            Bitmap outmap = new Bitmap(cols, rows, PixelFormat.Format32bppRgb);

            //convert to grayscale of a single byte
            for (row = 0; row < rows; row++)
            {
                for (col = 0; col < cols; col++)
                {
                    Color pixel1 = imag1.GetPixel(col, row);
                    Color pixel2 = imag2.GetPixel(col, row);



                    var mappedColor1 = pixel1;  // colors.Aggregate(Color.Black, (accu, curr) => ColorDiff(pixel1, curr) < ColorDiff(pixel1, accu) ? curr : accu);
                    var mappedColor2 = pixel2;  // colors.Aggregate(Color.Black, (accu, curr) => ColorDiff(pixel2, curr) < ColorDiff(pixel2, accu) ? curr : accu);





                    if (mappedColor1 == mappedColor2)
                    {
                        //outmap.SetPixel(col, row, pixel1);
                    }
                    else
                    {
                        
                        //if (mappedColor1 == mappedColor2 && pixel1.Name == Color.White.Name)
                        //{
                        //    outmap.SetPixel(col, row, pixel1);
                        //}
                        //else {
                            outmap.SetPixel(col, row, pixel2);
                        //}
                            
                        //outmap.SetPixel(col, row, Color.Red);
                    }

                }

               

            }


            outmap.MakeTransparent();
            return outmap;
        }









        private int ColorDiff(Color color, Color curr)
        {
            return Math.Abs(color.R - curr.R) + Math.Abs(color.G - curr.G) + Math.Abs(color.B - curr.B);
        }



    }
}
