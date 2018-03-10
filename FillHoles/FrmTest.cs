using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FillHolesTest
{
    public unsafe partial class FrmTest : Form
    {

        private Bitmap SrcBmp;
        private Bitmap DestBmp;
        public FrmTest()
        {
            InitializeComponent();
        }

        private void CmdOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap Temp = (Bitmap)Bitmap.FromFile(openFileDialog.FileName);
                if (IsGrayBitmap(Temp) == true)
                    SrcBmp = Temp;
                else
                {
                    SrcBmp = ConvertToGrayBitmap(Temp);
                    Temp.Dispose();
                }
                DestBmp = CreateGrayBitmap(SrcBmp.Width, SrcBmp.Height);
                SrcPic.Image = SrcBmp;
                DestPic.Image = DestBmp;
                DoBinaryzation(SrcBmp, DestBmp);
              
            }
            openFileDialog.Dispose();
        }

        private void CmdFillHole_Click(object sender, EventArgs e)
        {
            FillHole(DestBmp, false);
            DestPic.Invalidate();
        }

        private void CmdFillHoleBack_Click(object sender, EventArgs e)
        {
            FillHole(DestBmp, true);
            DestPic.Invalidate();
        }

        private void CmdRestore_Click(object sender, EventArgs e)
        {
            DoBinaryzation(SrcBmp, DestBmp);
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {
            SrcBmp = global::FillHoles.Properties.Resources.micro;
            DestBmp = CreateGrayBitmap(SrcBmp.Width, SrcBmp.Height);
            SrcPic.Image = SrcBmp;
            DestPic.Image = DestBmp;
            DoBinaryzation(SrcBmp,DestBmp);
        }

        private Bitmap CreateGrayBitmap(int Width, int Height)
        {
            Bitmap Bmp = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed);
            ColorPalette Pal = Bmp.Palette;
            for (int Y = 0; Y < Pal.Entries.Length; Y++) Pal.Entries[Y] = Color.FromArgb(255, Y, Y, Y);
            Bmp.Palette = Pal;
            return Bmp;
        }

        private bool IsGrayBitmap(Bitmap Bmp)
        {
            if (Bmp.PixelFormat != PixelFormat.Format8bppIndexed) return false;
            if (Bmp.Palette.Entries.Length != 256) return false;
            for (int Y = 0; Y < Bmp.Palette.Entries.Length; Y++)
                if (Bmp.Palette.Entries[Y] != Color.FromArgb(255, Y, Y, Y)) return false;
            return true;
        }

        private Bitmap ConvertToGrayBitmap(Bitmap Src)
        {
            Bitmap Dest = CreateGrayBitmap(Src.Width, Src.Height);
            BitmapData SrcData = Src.LockBits(new Rectangle(0, 0, Src.Width, Src.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData DestData = Dest.LockBits(new Rectangle(0, 0, Dest.Width, Dest.Height), ImageLockMode.ReadWrite, Dest.PixelFormat);
            int Width = SrcData.Width, Height = SrcData.Height;
            int SrcStride = SrcData.Stride, DestStride = DestData.Stride;
            byte* SrcP, DestP;
            for (int Y = 0; Y < Height; Y++)
            {
                SrcP = (byte*)SrcData.Scan0 + Y * SrcStride;               
                DestP = (byte*)DestData.Scan0 + Y * DestStride;
                for (int X = 0; X < Width; X++)
                {
                    *DestP = (byte)((*SrcP + (*(SrcP + 1) << 1) + *(SrcP + 2)) >> 2);
                    SrcP += 3;
                    DestP++;
                }
            }
            Src.UnlockBits(SrcData);
            Dest.UnlockBits(DestData);
            return Dest;
        }

        private void GetHistGram(Bitmap Src, int[] HistGram)
        {
            BitmapData SrcData = Src.LockBits(new Rectangle(0, 0, Src.Width, Src.Height), ImageLockMode.ReadWrite, Src.PixelFormat);
            int Width = SrcData.Width, Height = SrcData.Height, SrcStride = SrcData.Stride;
            byte* SrcP;
            for (int Y = 0; Y < 256; Y++) HistGram[Y] = 0;
            for (int Y = 0; Y < Height; Y++)
            {
                SrcP = (byte*)SrcData.Scan0 + Y * SrcStride;
                for (int X = 0; X < Width; X++, SrcP++) HistGram[*SrcP]++;
            }
            Src.UnlockBits(SrcData);
        }

        private void DoBinaryzation(Bitmap Src, Bitmap Dest )
        {
            int[] HistGram = new int[256];
            GetHistGram(SrcBmp, HistGram);
            int Threshold = GetHuangFuzzyThreshold(HistGram);
            BitmapData SrcData = Src.LockBits(new Rectangle(0, 0, Src.Width, Src.Height), ImageLockMode.ReadWrite, Src.PixelFormat);
            BitmapData DestData = Dest.LockBits(new Rectangle(0, 0, Dest.Width, Dest.Height), ImageLockMode.ReadWrite, Dest.PixelFormat);
            int Width = SrcData.Width, Height = SrcData.Height;
            int SrcStride = SrcData.Stride, DestStride = DestData.Stride;
            byte* SrcP, DestP;
            for (int Y = 0; Y < Height; Y++)
            {
                SrcP = (byte*)SrcData.Scan0 + Y * SrcStride;            
                DestP = (byte*)DestData.Scan0 + Y * DestStride;
                for (int X = 0; X < Width; X++, SrcP++, DestP++)
                    *DestP = *SrcP > Threshold ? byte.MaxValue : byte.MinValue;     
            }
            Src.UnlockBits(SrcData);
            Dest.UnlockBits(DestData);
            DestPic.Invalidate();
        }

        public static int GetHuangFuzzyThreshold(int[] HistGram)
        {
            int X, Y;
            int First, Last;
            int Threshold = -1;
            double BestEntropy = Double.MaxValue, Entropy;
            //  Find the first and last non-zero level values
            for (First = 0; First < HistGram.Length && HistGram[First] == 0; First++) ;
            for (Last = HistGram.Length - 1; Last > First && HistGram[Last] == 0; Last--) ;
            if (First == Last) return First;                // There is only one color in the image
            if (First + 1 == Last) return First;            // There are only two colors in the image

            // Calculate the cumulative histogram and the corresponding cumulative histogram with weights
            int[] S = new int[Last + 1];
            int[] W = new int[Last + 1];            // For oversized graphs, the saved data of this array may exceed the int representation range. Consider using the long type instead
            S[0] = HistGram[0];
            for (Y = First > 1 ? First : 1; Y <= Last; Y++)
            {
                S[Y] = S[Y - 1] + HistGram[Y];
                W[Y] = W[Y - 1] + Y * HistGram[Y];
            }

            // Establish lookup tables
            double[] Smu = new double[Last + 1 - First];
            for (Y = 1; Y < Smu.Length; Y++)
            {
                double mu = 1 / (1 + (double)Y / (Last - First));               
                Smu[Y] = -mu * Math.Log(mu) - (1 - mu) * Math.Log(1 - mu);      
            }

            // Iterative calculation of the optimal threshold
            for (Y = First; Y <= Last; Y++)
            {
                Entropy = 0;
                int mu = (int)Math.Round((double)W[Y] / S[Y]);            
                for (X = First; X <= Y; X++)
                    Entropy += Smu[Math.Abs(X - mu)] * HistGram[X];
                mu = (int)Math.Round((double)(W[Last] - W[Y]) / (S[Last] - S[Y]));  
                for (X = Y + 1; X <= Last; X++)
                    Entropy += Smu[Math.Abs(X - mu)] * HistGram[X];       
                if (BestEntropy > Entropy)
                {
                    BestEntropy = Entropy;      // Take the minimum entropy as the best threshold
                    Threshold = Y;
                }
            }
            return Threshold;
        }
        public  void FillHole(Bitmap Src,bool FillBackGround=false )
        {

            int X, Y;
            int Width, Height, Stride;
            byte* Pointer,Scan0;
            BitmapData SrcData = Src.LockBits(new Rectangle(0, 0, Src.Width, Src.Height), ImageLockMode.ReadWrite, Src.PixelFormat);
            Width = SrcData.Width; Height = SrcData.Height; Scan0 = (byte*)SrcData.Scan0; Stride = SrcData.Stride;

            byte Color;
            if (FillBackGround == false)
                Color = 255;
            else
                Color = 0;


            for (Y = 0; Y < Height; Y++)
            {
                Pointer = Scan0 + Y * Stride;
                if (Pointer[0] == Color) FloodFill(Src, Scan0, Stride, new Point(0, Y), 127);
                if (Pointer[Width - 1] == Color) FloodFill(Src, Scan0, Stride, new Point(Width - 1, Y), 127);
            }

            for (X = 0; X < Width; X++)
            {
                Pointer = Scan0 + X;
                if (Pointer[0] == Color) FloodFill(Src, Scan0, Stride, new Point(X, 0), 127);
                if (Pointer[(Height - 1) * Stride] == Color) FloodFill(Src, Scan0, Stride, new Point(X, Height - 1), 127);
            }

            for (Y = 0; Y < Height; Y++)
            {
                Pointer = Scan0 + Y * Stride;
                for (X = 0; X < Width; X++)
                {
                    if (Pointer[X] == 127)
                        Pointer[X] = Color;
                    else
                        Pointer[X] = (byte)(255 - Color);
                }
            }

            Src.UnlockBits(SrcData);
        }

        public void Invert(Bitmap Src)
        {

            int Y;
            int Width, Height, Stride;
            byte* Pointer;
            BitmapData SrcData = Src.LockBits(new Rectangle(0, 0, Src.Width, Src.Height), ImageLockMode.ReadWrite, Src.PixelFormat);
            Width = SrcData.Width; Height = SrcData.Height; Pointer = (byte*)SrcData.Scan0; Stride = SrcData.Stride;

            for (Y = 0; Y < Height * Stride; Y++)
            {
                Pointer[Y] = (byte)(255 - Pointer[Y]);
            }

           
            Src.UnlockBits(SrcData);
        }

        public void FloodFill(Bitmap Src,byte *ptr,int Stride, Point location, byte FillValue)
        {
            
                int w = Src.Width;
                int h = Src.Height;
                int PickColor;
                Stack<Point> fillPoints = new Stack<Point>(w * h);
               
                int bytes = Stride * Src.Height;
                byte[] grayValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)ptr, grayValues, 0, bytes);
                byte[] temp = (byte[])grayValues.Clone();
              
                PickColor = ptr[Stride * location.Y + location.X];
            
                if (location.X < 0 || location.X >= w || location.Y < 0 || location.Y >= h) return ;
                fillPoints.Push(new Point(location.X, location.Y));
                int[,] mask = new int[w, h];
              
                while (fillPoints.Count > 0)
                {
                    Point p = fillPoints.Pop();
                    mask[p.X, p.Y] = 1;
                    temp[p.X + p.Y * Stride] = FillValue;

                    if (p.X > 0 && temp[ (p.X - 1) + p.Y * Stride] ==PickColor)
                    {
                        temp[(p.X - 1) + p.Y * Stride] = FillValue;
                        fillPoints.Push(new Point(p.X - 1, p.Y));
                        mask[p.X - 1, p.Y] = 1;
                    }
                    if (p.X < w - 1 && temp[ (p.X + 1) + p.Y * Stride]==PickColor)
                    {
                        temp[(p.X + 1) + p.Y * Stride] = FillValue;
                        fillPoints.Push(new Point(p.X + 1, p.Y));
                        mask[p.X + 1, p.Y] = 1;
                    }
                    if (p.Y > 0 && temp[p.X + (p.Y - 1) * Stride] == PickColor)
                    {
                        temp[ p.X + (p.Y - 1) * Stride] = FillValue;
                        fillPoints.Push(new Point(p.X, p.Y - 1));
                        mask[p.X, p.Y - 1] = 1;
                    }
                    if (p.Y < h - 1 && temp[p.X + (p.Y + 1) * Stride] == PickColor)
                    {
                        temp[ p.X + (p.Y + 1) * Stride] = FillValue;
                        fillPoints.Push(new Point(p.X, p.Y + 1));
                        mask[p.X, p.Y + 1] = 1;
                    }
                }
                fillPoints.Clear();
                grayValues = (byte[])temp.Clone();
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, (IntPtr)ptr, bytes);
         ;
         
        }

       

    }
}
