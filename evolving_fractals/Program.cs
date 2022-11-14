using System;
using System.Drawing;

namespace MagPend
{
    class Program
    {
        public static void Main(string[] args)
        {
            Renderer r = new(0.1f, 0.2f, 0.22f, 1280, 720, 3);
            int i = 0;
            Bitmap b;

            string path = @"out";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            while (i < 500)
            {
                b = r.RenFrame();
                b.Save("out/file" + i + ".png");

                Console.WriteLine("File " + i + " done");
                i++;
            }
        }
    }
}