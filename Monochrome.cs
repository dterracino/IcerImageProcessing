using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace IcerImageProcessing
{
    /// <summary>
    /// 二值化算法
    /// </summary>
    public  static class Monochrome
    {       
        /// <summary>
        /// 图像二值化
        /// </summary>
        /// <param name="img">输入图像</param>
        /// <param name="brightThreshole">亮度阈值</param>
        /// <returns>输出图像</returns>
        public static Image Binary(Image img, int brightThreshole)
        {
            Color ColorOrigin = new Color();
            double Red, Green, Blue, Y;
            Bitmap Bmp1 = new Bitmap(img);

            //Graphics g = pictureBox1.CreateGraphics();
            for (int i = 0; i <= img.Width - 1; i++)
            {
                for (int j = 0; j <= img.Height - 1; j++)
                {
                    ColorOrigin = Bmp1.GetPixel(i, j);
                    Red = ColorOrigin.R;
                    Green = ColorOrigin.G;
                    Blue = ColorOrigin.B;
                    Y = 0.59 * Red + 0.3 * Green + 0.11 * Blue;
                    if (Y > brightThreshole)
                    {
                        Color ColorProcessed = Color.FromArgb(255, 255, 255);
                        Bmp1.SetPixel(i, j, ColorProcessed);
                    }
                    if (Y <= brightThreshole)
                    {
                        Color ColorProcessed = Color.FromArgb(0, 0, 0);
                        Bmp1.SetPixel(i, j, ColorProcessed);
                    }

                }
            }
            return Bmp1;

        }

        public static  byte[] GradientBytes(Image img, int gradientThreshold, int brightThreshold)
        {
            int h = img.Height, w = img.Width;
            MemoryStream ms = new MemoryStream();
            int BPP = 4;
            //if (img.PixelFormat.ToString().IndexOf("32") > -1) BPP = 4;
            Bitmap b = new Bitmap(img);
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bout = new byte[h * w];
            byte[] buffers = ms.ToArray();
            for (int j = 0; j < w - 1; ++j)
            {
                for (int i = h - 1; i > 0; --i)
                {
                    // 取得当前点的RGB值
                    byte pb = buffers[0 + 54 + (j + (h - i) * w) * BPP];
                    byte pg = buffers[1 + 54 + (j + (h - i) * w) * BPP];
                    byte pr = buffers[2 + 54 + (j + (h - i) * w) * BPP];
                    // 取得当前点上面那一点的RGB值
                    byte upb = buffers[0 + 54 + (j + (h - i - 1) * w) * BPP];
                    byte upg = buffers[1 + 54 + (j + (h - i - 1) * w) * BPP];
                    byte upr = buffers[2 + 54 + (j + (h - i - 1) * w) * BPP];
                    // 取得当前点右面那一点的RGB值
                    byte rpb = buffers[0 + 54 + (j + 1 + (h - i) * w) * BPP];
                    byte rpg = buffers[1 + 54 + (j + 1 + (h - i) * w) * BPP];
                    byte rpr = buffers[2 + 54 + (j + 1 + (h - i) * w) * BPP];
                    // 取得当前点右上面那一点的RGB值
                    byte cpb = buffers[0 + 54 + (j + 1 + (h - i - 1) * w) * BPP];
                    byte cpg = buffers[1 + 54 + (j + 1 + (h - i - 1) * w) * BPP];
                    byte cpr = buffers[2 + 54 + (j + 1 + (h - i - 1) * w) * BPP];
                    // 分别计算两个点的亮度值
                    double YB = 0.3 * pr + 0.59 * pg + 0.11 * pb;
                    double YU = 0.3 * upr + 0.59 * upg + 0.11 * upb;
                    double YR = 0.3 * rpr + 0.59 * rpg + 0.11 * rpb;
                    double YC = 0.3 * cpr + 0.59 * cpg + 0.11 * cpb;
                    // 求出两个点的亮度差值
                    //double tmpDelta = YU - YB > 0 ? YU - YB : YB - YU;
                    //double tmpDelta = YB - YU;
                    double tmpDelta = Math.Abs(YB - YU) + Math.Abs(YB - YR) + Math.Abs(YB - YC) + Math.Abs(YR - YU);
                    if (tmpDelta > gradientThreshold) // 与预设的阈值数据进行比较
                    {
                        bout[j + (i) * w] = 1;//大于，则表示有梯度
                    }
                    else
                    {
                        if (YU > brightThreshold)
                        {
                            bout[j + (i) * w] = 0;//小于，则表示无梯度
                        }
                        else
                        {
                            bout[j + (i) * w] = 1;//大于，则表示有梯度
                        }
                    }
                }
            }
            return bout;
        }

    }
}
