using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace IcerImageProcessing
{
    /// <summary>
    /// 图像基本处理库
    /// </summary>
    public static class Basic
    {
        #region 图像与二进制转换函数
        /// <summary>
        /// 将图片转换为二进制格式
        /// </summary>
        /// <param name="img">输入图片</param>
        /// <returns>二进制数组</returns>
        public static byte[] getBinaryFromImage(Image img)
        {
            int BPP = 3;
            Bitmap b = new Bitmap(img);
            if (b.PixelFormat.ToString().IndexOf("32") > -1) BPP = 4;
            int width = b.Width;
            int height = b.Height;

            //Bitmap dstImage = new Bitmap(width, height);

            BitmapData srcData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //BitmapData dstData = dstImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            byte[] buff = new byte[img.Width * img.Height];
            int k = 0;

            int stride = srcData.Stride;
            int offset = stride - width * BPP;

            unsafe
            {
                byte* src = (byte*)srcData.Scan0;
                //byte* dst = (byte*)dstData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //dst[0] = (byte)(255 - src[0]);
                        //dst[1] = (byte)(255 - src[1]);
                        //dst[2] = (byte)(255 - src[2]);
                        //dst[3] = src[3];

                        buff[k++] = src[0] == 255 ? (byte)0 : (byte)1;
                        src += BPP;
                        //dst += BPP;
                    } // x

                    src += offset;
                    //dst += offset;
                } // y
            }

            b.UnlockBits(srcData);
            //dstImage.UnlockBits(dstData);

            b.Dispose();
            return buff;
        }

        /// <summary>
        /// 将二进制格式转换为图片格式
        /// </summary>
        /// <param name="buff">二进制数组</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>图片</returns>
        public static Image getImageFromBinary(byte[] buff, int width, int height)
        {
            int BPP = 4;
            Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //if (b.PixelFormat.ToString().IndexOf("32") > -1) BPP = 4;

            BitmapData dstData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int k = 0;

            int stride = dstData.Stride;
            int offset = stride - width * BPP;

            unsafe
            {
                byte* dst = (byte*)dstData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        dst[0] = buff[k] == 1 ? (byte)0 : (byte)255;
                        dst[1] = buff[k] == 1 ? (byte)0 : (byte)255;
                        dst[2] = buff[k] == 1 ? (byte)0 : (byte)255;
                        dst[3] = (byte)255;

                        k++;
                        dst += BPP;
                    } // x

                    dst += offset;
                } // y
            }

            b.UnlockBits(dstData);
            return b;
        }
        #endregion

        #region 图像与灰度转换函数
        /// <summary>
        /// 将图片转换为灰度格式
        /// </summary>
        /// <param name="img">输入图片</param>
        /// <returns>灰度数组</returns>
        public static byte[] getGrayFromImage(Image img)
        {
            int BPP = 3;
            Bitmap b = new Bitmap(img);
            if (b.PixelFormat.ToString().IndexOf("32") > -1) BPP = 4;
            int width = b.Width;
            int height = b.Height;

            //Bitmap dstImage = new Bitmap(width, height);

            BitmapData srcData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //BitmapData dstData = dstImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            byte[] buff = new byte[img.Width * img.Height];
            int k = 0;

            int stride = srcData.Stride;
            int offset = stride - width * BPP;

            unsafe
            {
                byte* src = (byte*)srcData.Scan0;
                //byte* dst = (byte*)dstData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //dst[0] = (byte)(255 - src[0]);
                        //dst[1] = (byte)(255 - src[1]);
                        //dst[2] = (byte)(255 - src[2]);
                        //dst[3] = src[3];
                        int pb = src[0];
                        int pg = src[1];
                        int pr = src[2];
                        buff[k++] = (byte)(pr * 0.3 + pg * 0.59 + pb * 0.11);
                        src += BPP;
                        //dst += BPP;
                    } // x

                    src += offset;
                    //dst += offset;
                } // y
            }

            b.UnlockBits(srcData);
            //dstImage.UnlockBits(dstData);

            b.Dispose();
            return buff;
        }

        /// <summary>
        /// 将灰度格式转换为图片格式
        /// </summary>
        /// <param name="buff">灰度数组</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>图片</returns>
        public static Image getImageFromGray(byte[] buff, int width, int height)
        {
            int BPP = 4;
            Bitmap b = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //if (b.PixelFormat.ToString().IndexOf("32") > -1) BPP = 4;

            BitmapData dstData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int k = 0;

            int stride = dstData.Stride;
            int offset = stride - width * BPP;

            unsafe
            {
                byte* dst = (byte*)dstData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        dst[0] = buff[k];
                        dst[1] = buff[k];
                        dst[2] = buff[k];
                        dst[3] = (byte)255;

                        k++;
                        dst += BPP;
                    } // x

                    dst += offset;
                } // y
            }

            b.UnlockBits(dstData);
            return b;
        }
        #endregion
    }
}
