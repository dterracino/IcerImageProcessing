using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IcerImageProcessing
{
    /// <summary>
    /// 形态学算法库
    /// </summary>
    public static class Morphologic
    {
        public enum MapData { Black = 1, None = 0, Marked = 2 };

        #region 膨胀腐蚀
        /// <summary>
        /// 二值腐蚀
        /// </summary>
        //public static void Erosion(ref byte[] data, int x, int y, int width, int height, int w, int h)
        //{
        //    byte[] dataout = (byte[])data.Clone();
        //    Erosion(data, ref dataout, x, y, width, height, w, h);
        //    data = (byte[])dataout.Clone();
        //}

        public static byte[] Erosion(byte[] datain, int w, int h)
        {
            return Erosion(datain, 0, 0, w, h, w, h, 7);
        }

        public static byte[] Erosion(byte[] datain, int w, int h, int minabutblock)
        {
            return Erosion(datain, 0, 0, w, h, w, h, minabutblock);
        }

        public static byte[] Erosion(byte[] datain, int x, int y, int width, int height, int w, int h, int minabutblock)
        {
            byte[] dataout = new byte[datain.Length];
            for (int j = x + 1; j < x + width - 1; j++)
            {
                for (int i = y + 1; i < y + height - 1; i++)
                {
                    int k = (h - i - 1) * w + j;
                    int mn = 0;
                    mn += datain[k - 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k + 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k + 0] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k + w + 0] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k + w - 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k + w + 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k - w - 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k - w + 1] == (byte)MapData.Black ? 1 : 0;
                    mn += datain[k - w + 0] == (byte)MapData.Black ? 1 : 0;

                    if (mn > minabutblock)
                    {
                        dataout[k] = (byte)MapData.Black;
                    }
                    else
                    {
                        dataout[k] = (byte)MapData.None;
                    }
                }
            }
            return dataout;
        }

        /// <summary>
        /// 二值膨胀
        /// </summary>
        //public static void Dilation(ref byte[] data, int x, int y, int width, int height, int w, int h)
        //{
        //    byte[] dataout = (byte[])data.Clone();
        //    Dilation(data, ref dataout, x, y, width, height, w, h);
        //    data = (byte[])dataout.Clone();
        //}

        public static byte[] Dilation(byte[] datain, int w, int h)
        {
            return Dilation(datain, 0, 0, w, h, w, h);
        }

        public static byte[] Dilation(byte[] datain, int x, int y, int width, int height, int w, int h)
        {
            byte[] dataout = new byte[datain.Length];
            for (int j = x + 1; j < x + width - 1; j++)
            {
                for (int i = y + 1; i < y + height - 1; i++)
                {
                    int k = (h - i - 1) * w + j;
                    if (datain[k] == (byte)MapData.Black)
                    {
                        dataout[k - 1] = (byte)MapData.Black;
                        dataout[k + 1] = (byte)MapData.Black;
                        dataout[k + 0] = (byte)MapData.Black;
                        dataout[k + w - 1] = (byte)MapData.Black;
                        dataout[k + w + 1] = (byte)MapData.Black;
                        dataout[k + w + 0] = (byte)MapData.Black;
                        dataout[k - w - 1] = (byte)MapData.Black;
                        dataout[k - w + 1] = (byte)MapData.Black;
                        dataout[k - w + 0] = (byte)MapData.Black;
                    }
                }
            }
            return dataout;
        }
        #endregion

        /// <summary>
        /// 细化
        /// </summary>
        /// <param name="img">输入图片</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="times">细化次数</param>
        /// <returns>输出图片</returns>
        public static Image Thinner(Image img, int width, int height, int times)
        {
            int dwWidth = width;
            int dwHeight = height;
            int i, j, time, change;
            int[,] f = new int[width + 2, height + 2];
            int[,] t = new int[width + 2, height + 2];
            int[,] x = new int[width + 2, height + 2];
            int[,] three = new int[width + 2, height + 2];
            int[,] four = new int[width + 2, height + 2];
            int[,] five1 = new int[width + 2, height + 2];
            int[,] five2 = new int[width + 2, height + 2];
            //Color c = new Color();
            byte c;
            //Bitmap box1 = new Bitmap(img);
            byte[] buff = Basic.getBinaryFromImage(img);
            change = 1;
            for (time = 1; time <= times; time++)
            {
                switch (change)
                {
                    case 1:
                        for (i = 1; i < dwWidth; i++)
                            for (j = 1; j < dwHeight; j++)
                            {
                                c = buff[j * dwWidth + i];
                                //c = box1.GetPixel(i, j);
                                f[i, j] = c;
                            }
                        for (i = 2; i < dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                                x[i, j] = Math.Abs(f[i + 1, j - 1] - f[i + 1, j]) +
                                       Math.Abs(f[i, j - 1] - f[i + 1, j - 1]) +
                                       Math.Abs(f[i - 1, j - 1] - f[i, j - 1]) +
                                       Math.Abs(f[i - 1, j] - f[i - 1, j - 1]) +
                                       Math.Abs(f[i - 1, j + 1] - f[i - 1, j]) +
                                       Math.Abs(f[i, j + 1] - f[i - 1, j + 1]) +
                                       Math.Abs(f[i + 1, j + 1] - f[i, j + 1]);
                        //DEUTSCH
                        for (i = 1; i < dwWidth; i++)
                            for (j = 1; j < dwHeight; j++)
                                t[i, j] = f[i - 1, j - 1] + f[i - 1, j]
                                      + f[i - 1, j + 1] + f[i, j - 1]
                                      + f[i, j + 1] + f[i + 1, j - 1]
                                      + f[i + 1, j] + f[i + 1, j + 1];
                        for (i = 2; i <= dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                            {
                                if (f[i + 1, j] == 0 || f[i, j - 1] == 0 || f[i - 1, j] == 0)
                                    three[i, j] = 1;
                                if (f[i + 1, j] == 0 || f[i, j - 1] == 0 || f[i, j + 1] == 0)
                                    four[i, j] = 1;
                                if ((f[i + 1, j] == 1 && f[i, j + 1] == 1) && (f[i + 1, j - 1] == 1
                                  || f[i - 1, j + 1] == 1) && (f[i, j - 1] == 0 && f[i - 1, j - 1] == 0 && f[i - 1, j] == 0 && f[i + 1, j + 1] == 0))
                                    five1[i, j] = 1;
                                if ((f[i + 1, j] == 1 && f[i, j - 1] == 1) && (f[i - 1, j - 1] == 1
                                  || f[i + 1, j + 1] == 1) && (f[i + 1, j - 1] == 0 && f[i - 1, j] == 0 && f[i - 1, j + 1] == 0 && f[i, j + 1] == 0))
                                    five2[i, j] = 1;
                            }
                        for (i = 2; i < dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                            {
                                if ((x[i, j] == 0 || x[i, j] == 2) && t[i, j] != 1 && three[i, j] == 1 && four[i, j] == 1 ||
                                    (x[i, j] == 4 && t[i, j] != 1 && three[i, j] == 1 && four[i, j] == 1 && (five1[i, j] == 1 || five2[i, j] == 1)))
                                    buff[j * dwWidth + i] = 0;
                            }
                        break;
                    case 2:
                        for (i = 1; i < dwWidth; i++)
                            for (j = 1; j < dwHeight; j++)
                            {
                                c = buff[j * dwWidth + i];
                                //c = box1.GetPixel(i, j);
                                f[i, j] = c;
                            }
                        for (i = 2; i < dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                                x[i, j] = Math.Abs(f[i - 1, j + 1] - f[i - 1, j]) +
                                    Math.Abs(f[i, j + 1] - f[i - 1, j + 1]) +
                                    Math.Abs(f[i + 1, j + 1] - f[i, j + 1]) +
                                    Math.Abs(f[i + 1, j] - f[i + 1, j + 1]) +
                                    Math.Abs(f[i + 1, j - 1]) - f[i + 1, j] +
                                    Math.Abs(f[i, j - 1] - f[i + 1, j - 1]) +
                                    Math.Abs(f[i - 1, j - 1] - f[i, j - 1]);
                        for (i = 1; i < dwWidth; i++)
                            for (j = 1; j < dwHeight; j++)
                                t[i, j] = f[i + 1, j + 1] + f[i + 1, j] + f[i + 1, j - 1] + f[i, j + 1] + f[i, j - 1] + f[i - 1, j + 1] + f[i - 1, j] + f[i - 1, j - 1];
                        for (i = 2; i < dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                            {
                                if (f[i - 1, j] == 0 || f[i, j + 1] == 0 || f[i + 1, j] == 0)
                                    three[i, j] = 1;
                                if (f[i - 1, j] == 0 || f[i, j + 1] == 0 || f[i, j - 1] == 0)
                                    four[i, j] = 1;
                                if ((f[i - 1, j] == 1 && f[i, j - 1] == 1) && (f[i - 1, j + 1] == 1 || f[i + 1, j - 1] == 1) && (f[i, j + 1] == 0 && f[i + 1, j + 1] == 0
                                    && f[i + 1, j] == 0 && f[i - 1, j - 1] == 0))
                                    five1[i, j] = 1;
                                if ((f[i - 1, j] == 1 && f[i, j + 1] == 1) && (f[i + 1, j + 1] == 1 || f[i - 1, j - 1] == 1) && (f[i - 1, j + 1] == 0 && f[i + 1, j] == 0
                                    && f[i + 1, j - 1] == 0 && f[i, j - 1] == 0))
                                    five2[i, j] = 1;
                            }
                        for (i = 2; i < dwWidth; i++)
                            for (j = 2; j < dwHeight; j++)
                            {
                                if ((x[i, j] == 0 || x[i, j] == 2) && t[i, j] != 1 && three[i, j] == 1 && four[i, j] == 1 ||
                                    (x[i, j] == 4 && t[i, j] != 1 && three[i, j] == 1 && four[i, j] == 1 && (five1[i, j] == 1 || five2[i, j] == 1)))
                                    buff[j * dwWidth + i] = 0;
                            }
                        break;
                }
                if (change == 1) change = 2;
                else if (change == 2) change = 1;
            }
            return Basic.getImageFromBinary(buff, img.Width, img.Height);
        }

        public static Image Gaussion(Image img)
        {
            Color c1 = new Color();
            Color c2 = new Color();
            Color c3 = new Color();
            Color c4 = new Color();
            Color c5 = new Color();
            Color c6 = new Color();
            Color c7 = new Color();
            Color c8 = new Color();
            Color c9 = new Color();
            int rr, r1, r2, r3, r4, r5, r6, r7, r8, r9, i, j, fxr;
            Bitmap box1 = new Bitmap(img);
            for (i = 1; i <= img.Width - 2; i++)
            {
                for (j = 1; j <= img.Height - 2; j++)
                {
                    c1 = box1.GetPixel(i, j - 1);
                    c2 = box1.GetPixel(i - 1, j);
                    c3 = box1.GetPixel(i, j);
                    c4 = box1.GetPixel(i + 1, j);
                    c5 = box1.GetPixel(i, j + 1);
                    c6 = box1.GetPixel(i - 1, j - 1);
                    c7 = box1.GetPixel(i - 1, j + 1);
                    c8 = box1.GetPixel(i + 1, j - 1);
                    c9 = box1.GetPixel(i + 1, j + 1);
                    r1 = c1.R;
                    r2 = c2.R;
                    r3 = c3.R;
                    r4 = c4.R;
                    r5 = c5.R;
                    r6 = c6.R;
                    r7 = c7.R;
                    r8 = c8.R;
                    r9 = c9.R;
                    fxr = (r6 + r7 + r8 + r9 + 2 * r1 + 2 * r2 + 2 * r4 + 2 * r5 + 4 * r3) / 16;
                    rr = fxr;
                    if (rr < 0) rr = 0;
                    if (rr > 255) rr = 255;
                    Color cc = Color.FromArgb(rr, rr, rr);
                    box1.SetPixel(i, j, cc);
                }
            }
            return box1;
        }

        public static byte[] Minus(byte[] data1,byte[] data2, int width, int height)
        {
            int wh = width * height;
            byte[] buff = new byte[wh];
            for (int i = 0; i < wh; ++i)
            {
                buff[i] = (byte)Math.Abs(data1[i] - data2[i]);
            }
            return buff;
        }

    }
}
