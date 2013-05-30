using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace IcerImageProcessing
{
    public static class Transformation
    {
        public static Image ResizeImage(Image img, double percent)
        {
            int w = (int)(img.Width * percent);
            int h = (int)(img.Height * percent);
            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(img, new Rectangle(0, 0, w, h));
            g.Flush();
            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public static Image RotateImage(Image img, float angle, Color bkColor)
        {
            int w = img.Width + 2;
            int h = img.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = img.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(img, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        public static Image InnerMoveImage(Image img, int x, int y, Color bkColor)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(bkColor);
            g.DrawImage(img, x, y);
            g.Flush();
            g.Dispose();
            return bmp;
        }
    }
}
