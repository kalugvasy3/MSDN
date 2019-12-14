using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows.Media.Imaging;

namespace TransformMethods
{
    public class TransferTo
    {
        /// <summary>
        /// Convert WriteableBitmap to Complex[,]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Complex[,] convertImgToComlex(WriteableBitmap img, string strColor, out double maxMagnitude)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            maxMagnitude = 0;

            Complex[,] cimg = new Complex[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int intC = 0;
                switch (strColor)
                {
                    case "R":
                        intC = (img.Pixels[i] & (255 << 16)) >> 16;
                        break;

                    case "G":
                        intC = (img.Pixels[i] & (255 << 8)) >> 8;
                        break;

                    case "B":
                        intC = (img.Pixels[i] & (255 << 0)) >> 0;
                        break;

                    case "RGB":
                        intC = img.Pixels[i];
                        break;

                    default:
                        intC = img.Pixels[i] & 255;
                        break;
                }

                //intC = bln128 ? (intC & 255) - 128 : intC & 255;

                intC = intC & 255;
                cimg[i % X, i / X] = new Complex(intC, 0.0);

                maxMagnitude = maxMagnitude < cimg[i % X, i / X].Magnitude ? maxMagnitude = cimg[i % X, i / X].Magnitude : maxMagnitude;
            }

            return cimg;
        }

        public Int16[,] convertImgToInt16(WriteableBitmap img, string strColor)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            Int16[,] zimg = new Int16[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int intC = 0;
                switch (strColor)
                {
                    case "R":
                        intC = (img.Pixels[i] & (255 << 16)) >> 16;
                        break;

                    case "G":
                        intC = (img.Pixels[i] & (255 << 8)) >> 8;
                        break;

                    case "B":
                        intC = (img.Pixels[i] & (255 << 0)) >> 0;
                        break;

                    case "RGB":
                        intC = img.Pixels[i];
                        break;

                    default:
                        intC = img.Pixels[i] & 255;
                        break;
                }

                zimg[i % X, i / X] = (Int16)intC;
            }

            return zimg;
        }

        public int[,] convertImgToInt(WriteableBitmap img, string strColor)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] zimg = new int[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int intC = 0;
                switch (strColor)
                {
                    case "R":
                        intC = (img.Pixels[i] & (255 << 16)) >> 16;
                        break;

                    case "G":
                        intC = (img.Pixels[i] & (255 << 8)) >> 8;
                        break;

                    case "B":
                        intC = (img.Pixels[i] & (255 << 0)) >> 0;
                        break;

                    case "RGB":
                        intC = (int)img.Pixels[i];
                        break;

                    default: // Gray ->  Y = .2126 * R^gamma + .7152 * G^gamma + .0722 * B^gamma
                        intC = (int)(0.2126 * ((img.Pixels[i] & (255 << 16)) >> 16) + 0.7152 * ((img.Pixels[i] & (255 << 8)) >> 8) + 0.0722 * ((img.Pixels[i] & (255 << 0)) >> 0));
                        break;
                }

                zimg[i % X, i / X] = (int)intC;
            }

            return zimg;
        }

        public int[,] convertImgToInt(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] zimg = new int[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                // Gray ->  Y = .2126 * R^gamma + .7152 * G^gamma + .0722 * B^gamma
                int intC = (int)(0.2126 * ((img.Pixels[i] & (255 << 16)) >> 16) + 0.7152 * ((img.Pixels[i] & (255 << 8)) >> 8) + 0.0722 * ((img.Pixels[i] & (255 << 0)) >> 0));

                zimg[i % X, i / X] = (int)intC;
            }

            return zimg;
        }


        public int[,] convertImgToColorInt(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] zimg = new int[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
               zimg[i % X, i / X] = img.Pixels[i];
            }

            return zimg;
        }

        public int[,] convertImgToMaxGrayInt(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] zimg = new int[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int pixel = img.Pixels[i];

                int r = (pixel >> 16) & 255;
                int g = (pixel >> 8) & 255;
                int b = (pixel >> 0) & 255;

                int max = Math.Max(r, Math.Max(g, b));
            //    int min = Math.Min(r, Math.Min(g, b));

                pixel = max;

           //     pixel = (255 << 24) | (pixel & 255) << 16 | (pixel & 255) << 8 | (pixel & 255);

               zimg[i % X, i / X] = pixel;
            }

            return zimg;
        }
    

        public sbyte[,] convertImgToSByte(WriteableBitmap img, string strColor)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            sbyte[,] zimg = new sbyte[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int intC = 0;
                switch (strColor)
                {
                    case "R":
                        intC = (img.Pixels[i] & (255 << 16)) >> 16;
                        break;

                    case "G":
                        intC = (img.Pixels[i] & (255 << 8)) >> 8;
                        break;

                    case "B":
                        intC = (img.Pixels[i] & (255 << 0)) >> 0;
                        break;

                    case "RGB":
                        intC = img.Pixels[i];
                        break;

                    default:
                        intC = img.Pixels[i] & 255;
                        break;
                }

                zimg[i % X, i / X] = (sbyte)((intC & 255) >> 1);
            }

            return zimg;
        }

   

        public int[,] filterGaussian(int[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] gauss = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    int P1 = 16 * zimg[i - 1, j - 1];
                    int P2 = 26 * zimg[i, j - 1];
                    int P3 = 16 * zimg[i + 1, j + 1];
                    int P4 = 26 * zimg[i - 1, j];
                    int P5 = 41 * zimg[i, j];
                    int P6 = 26 * zimg[i + 1, j];
                    int P7 = 16 * zimg[i - 1, j + 1];
                    int P8 = 26 * zimg[i, j + 1];
                    int P9 = 16 * zimg[i + 1, j + 1];

                    gauss[i, j] = (int)((P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9) / 209);

                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                gauss[0, j] = gauss[1, j];
                gauss[X - 1, j] = gauss[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                gauss[i, 0] = gauss[i, 1];
                gauss[i, Y - 1] = gauss[i, Y - 2];
            }
            return gauss;
        }

        public Int16[,] filterGaussian(Int16[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            Int16[,] gauss = new Int16[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    int P1 = 16 * zimg[i - 1, j - 1];
                    int P2 = 26 * zimg[i, j - 1];
                    int P3 = 16 * zimg[i + 1, j + 1];
                    int P4 = 26 * zimg[i - 1, j];
                    int P5 = 41 * zimg[i, j];
                    int P6 = 26 * zimg[i + 1, j];
                    int P7 = 16 * zimg[i - 1, j + 1];
                    int P8 = 26 * zimg[i, j + 1];
                    int P9 = 16 * zimg[i + 1, j + 1];

                    gauss[i, j] = (Int16)((P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9) / 209);
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                gauss[0, j] = gauss[1, j];
                gauss[X - 1, j] = gauss[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                gauss[i, 0] = gauss[i, 1];
                gauss[i, Y - 1] = gauss[i, Y - 2];
            }
            return gauss;
        }


        public sbyte[,] filterGaussian(sbyte[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            sbyte[,] gauss = new sbyte[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    int P1 = 16 * zimg[i - 1, j - 1];
                    int P2 = 26 * zimg[i, j - 1];
                    int P3 = 16 * zimg[i + 1, j + 1];
                    int P4 = 26 * zimg[i - 1, j];
                    int P5 = 41 * zimg[i, j];
                    int P6 = 26 * zimg[i + 1, j];
                    int P7 = 16 * zimg[i - 1, j + 1];
                    int P8 = 26 * zimg[i, j + 1];
                    int P9 = 16 * zimg[i + 1, j + 1];

                    gauss[i, j] = (sbyte)((P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9) / 209);
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                gauss[0, j] = gauss[1, j];
                gauss[X - 1, j] = gauss[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                gauss[i, 0] = gauss[i, 1];
                gauss[i, Y - 1] = gauss[i, Y - 2];
            }
            return gauss;
        }

        public void filterGaussian(ref Complex[,] Z, out double maxMagnitude)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            maxMagnitude = 0;

            Complex[,] gauss = new Complex[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    Complex P1 = 16 * Z[i - 1, j - 1];
                    Complex P2 = 26 * Z[i, j - 1];
                    Complex P3 = 16 * Z[i + 1, j + 1];
                    Complex P4 = 26 * Z[i - 1, j];
                    Complex P5 = 41 * Z[i, j];
                    Complex P6 = 26 * Z[i + 1, j];
                    Complex P7 = 16 * Z[i - 1, j + 1];
                    Complex P8 = 26 * Z[i, j + 1];
                    Complex P9 = 16 * Z[i + 1, j + 1];

                    gauss[i, j] = (P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9) / 209;

                    maxMagnitude = maxMagnitude < gauss[i, j].Magnitude ? maxMagnitude = gauss[i, j].Magnitude : maxMagnitude;
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                gauss[0, j] = gauss[1, j];
                gauss[X - 1, j] = gauss[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                gauss[i, 0] = gauss[i, 1];
                gauss[i, Y - 1] = gauss[i, Y - 2];
            }
            Z = gauss;
        }

        public sbyte[,] blackAndWhite(sbyte[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            sbyte[,] baw = new sbyte[X, Y];
            double sum = 0;

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    sum += zimg[i, j];
                }
            }

            sbyte avr = (sbyte)(sum / (X * Y));

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = (sbyte)(zimg[i, j] > avr ? 127 : 0);
                }
            }

            return baw;
        }

        public Int16[,] blackAndWhite(Int16[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            Int16[,] baw = new Int16[X, Y];
            double sum = 0;

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    sum += zimg[i, j];
                }
            }

            Int16 avr = (Int16)(sum / (X * Y));

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = (Int16)(zimg[i, j] > avr ? 255 : 0);
                }
            }

            return baw;
        }

        public Int16[,] inversWaveLet(Int16[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            Int16[,] inv = new Int16[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    inv[i, j] = (Int16)(255 - zimg[i, j]);
                }
            }

            return inv;
        }

        public sbyte[,] inversWaveLet(sbyte[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            sbyte[,] inv = new sbyte[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    inv[i, j] = (sbyte)(127 - zimg[i, j]);
                }
            }

            return inv;
        }

        public int[,] blackAndWhite(int[,] zimg, int orderGauss, double sigma)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] Z = new int[X, Y];

            Edge objE = new Edge();
            Z = objE.filterGaussian(zimg, orderGauss, sigma);

            int[,] baw = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = zimg[i, j] > Z[i, j] ? 255 : 0;
                }
            }

            return baw;
        }

        public int[,] blackGrayWhite(int[,] zimg, int orderGauss, double sigma)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] Zgauss = new int[X, Y];

            Edge objE = new Edge();
            Zgauss = objE.filterGaussian(zimg, orderGauss, sigma);   // order 5   sigma 2 - best result

            int[,] baw = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = 255; // Zgauss[i, j];

                    if (zimg[i, j] > Zgauss[i, j] + 2) { baw[i, j] = 255; continue; }
                    if (zimg[i, j] < Zgauss[i, j] - 2) { baw[i, j] = 0; continue; }
                }
            }

            return baw;

        }


        public int[,] removeTrend(int[,] zimg, int orderGauss, double sigma)  // trend 
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] Zgauss = new int[X, Y];

            Edge objE = new Edge();
            Zgauss = objE.filterGaussian(zimg, orderGauss, sigma);   // order 5   sigma 2 - best result

            int[,] baw = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = zimg[i,j] - Zgauss[i, j] ;
                }
            }

            return baw;

        }


        public int[,] OnlyBlackWhite(int[,] zimg, int orderGauss, double sigma)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] Zgauss = new int[X, Y];

            Edge objE = new Edge();
            Zgauss = objE.filterGaussian(zimg, orderGauss, sigma);   // order 5   sigma 2 - best result

            int[,] baw = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = Zgauss[i, j];

                    if (zimg[i, j] > Zgauss[i, j] + 2) { baw[i, j] = 255; continue; }
                    if (zimg[i, j] < Zgauss[i, j] - 2) { baw[i, j] = 0; continue; }
                }
            }

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    if ((baw[i, j] == 0 || baw[i, j] == 255)) continue;
                    baw[i, j] = 255;
                    if (zimg[i - 1, j] + zimg[i - 1, j - 1] + zimg[i, j - 1] + zimg[i + 1, j - 1] + zimg[i + 1, j] + zimg[i + 1, j + 1] + zimg[i, j + 1] + zimg[i - 1, j + 1] < Zgauss[i, j] * 8  ) { baw[i, j] = 0; } else { baw[i, j] = 255; }
                }
            }

            return baw;

        }


        public int[,] blackAndGray(int[,] zimg, int orderGauss, double sigma)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] Z = new int[X, Y];

            Edge objE = new Edge();
            Z = objE.filterGaussian(zimg, orderGauss, sigma);

            int[,] baw = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = zimg[i, j] > Z[i, j] ? zimg[i, j] : 0;
                }
            }

            return baw;
        }

        public Complex[,] blackAndWhite(Complex[,] zimg, int orderGauss, double sigma)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            Complex[,] Z = new Complex[X, Y];

            Edge objE = new Edge();
            Z = objE.filterGaussian(zimg, orderGauss, sigma);

            Complex[,] baw = new Complex[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = zimg[i, j].Magnitude > Z[i, j].Magnitude ? 255 : 0;
                }
            }

            return baw;
        }

        public int[,] blackAndWhite(int[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] baw = new int[X, Y];
            double sum = 0;

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    sum += zimg[i, j];
                }
            }

            int avr = (int)(sum / (X * Y));

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    baw[i, j] = zimg[i, j] > avr ? 255 : 0;
                }
            }

            return baw;
        }

        public int[,] inversWaveLet(int[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] inv = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    inv[i, j] = (int)(255 - zimg[i, j]);
                }
            }

            return inv;
        }

        public sbyte[,] filterGaussian3X3(sbyte[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            sbyte[,] gauss = new sbyte[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    int P1 = 16 * zimg[i - 1, j - 1];
                    int P2 = 26 * zimg[i, j - 1];
                    int P3 = 16 * zimg[i + 1, j + 1];
                    int P4 = 26 * zimg[i - 1, j];
                    int P5 = 41 * zimg[i, j];
                    int P6 = 26 * zimg[i + 1, j];
                    int P7 = 16 * zimg[i - 1, j + 1];
                    int P8 = 26 * zimg[i, j + 1];
                    int P9 = 16 * zimg[i + 1, j + 1];

                    gauss[i, j] = (sbyte)((P1 + P2 + P3 + P4 + P5 + P6 + P7 + P8 + P9) / 209);

                    //if (inv[i, j] >= 127)
                    //{
                    //    inv[i, j] = 0;
                    //}
                    //else
                    //{
                    //    inv[i, j] = 255;
                    //}
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                gauss[0, j] = gauss[1, j];
                gauss[X - 1, j] = gauss[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                gauss[i, 0] = gauss[i, 1];
                gauss[i, Y - 1] = gauss[i, Y - 2];
            }
            return gauss;
        }

    

        public WriteableBitmap convertIntToGrayImg(int[,] Z, out int zero)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            zero = 0;
            for (int i = 0; i < X * Y; i++)
            {
                int z = Z[i % X, i / X];
                if (z == 0) { zero += 1; }
                //byte b = (byte)(z < 0 ? 0 : (z > 255 ? 255 : z));
                byte b = (byte)(z > 255 ? 255 : (z < 0 ? 255 + z : z));
                img.Pixels[i] = 255 << 24 | b << 16 | b << 8 | b;
            }

            return img;
        }


        public WriteableBitmap convertIntToGrayImg(int[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);


            for (int i = 0; i < X * Y; i++)
            {
                int z = Z[i % X, i / X];
                byte b = (byte)(z > 255 ? 255 : (z < 0 ? 255 + z : z));
                img.Pixels[i] = 255 << 24 | b << 16 | b << 8 | b;
            }

            return img;
        }



        public WriteableBitmap convertIntToRGBImg(int[,] Z, out int zero)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            zero = 0;
            for (int i = 0; i < X * Y; i++)
            {
                int z = Z[i % X, i / X];
                if (z == 0) { zero += 1; }
                img.Pixels[i] = 255 << 24 | z;
            }

            return img;
        }

        public WriteableBitmap convertIntToColorImg(int[,] intZ)
        {
            int X = intZ.GetLength(0);
            int Y = intZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                img.Pixels[i] = intZ[i % X, i / X];
            }

            return img;
        }


        public Complex[,] convertIntToComplex(int[,] intZ)
        {
            int X = intZ.GetLength(0);
            int Y = intZ.GetLength(1);
            Complex[,] zout = new Complex[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    zout[i, j] = intZ[i, j];
                }
            }
            return zout;
        }

        //-------------------------------------------------------------------------------------------------------------------

        public int[,] reduceXY_To_XY(int[,] inZ, int intNumImages, double delta)  // Size same but image/int reduced
        {
            int X = inZ.GetLength(0);
            int Y = inZ.GetLength(1);

            double dky = Math.Pow(delta, intNumImages); // X /(double)(X - intReduce);
            int[,] intOut = new int[X, Y]; // same size but int will be reduced;

 
            for (int j = 0; j < Y; j++)  // do not need to cut Y axel
            {
                int jnew = (int)(j / dky + Y / 2.0 - Y / 2.0 / dky);
                for (int i = 0; i < X; i++)  // cut X axel
                {
                    intOut[(int)(i / dky + X / 2.0 - X / 2.0 / dky), jnew] = inZ[i, j];
                }
            }

            return intOut;
        }

        public int[,] sum_reduceXY_To_XY(int[,] inInt, int intNumImages, double deltaDistance, bool blnOr)  // Size same but image/int reduced
        {
            int X = inInt.GetLength(0);
            int Y = inInt.GetLength(1);

            int[,] intSum = new int[X, Y];

            int intTotal = 0;

            for (int j = 0; j < intNumImages + 1; j++)  // do not need to cut Y axel
            {
                intSum = sum(intSum, reduceXY_To_XY(inInt, j, deltaDistance), blnOr);
                intTotal++;
            }

            if (blnOr)
            {
                return intSum;
            }
            else
            {
                return dev(intSum, intTotal);
            }
        }

        private int[,] sum(int[,] int1, int[,] int2, bool blnOr)
        {
            int X = int1.GetLength(0);
            int Y = int1.GetLength(1);

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int1[i, j] = blnOr ? (int1[i, j] | int2[i, j]) : (int1[i, j] + int2[i, j]);
                }
            }

            return int1;
        }

        private int[,] dev(int[,] int1, int intDev)
        {
            int X = int1.GetLength(0);
            int Y = int1.GetLength(1);

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int1[i, j] = int1[i, j] / intDev;
                }
            }

            return int1;
        }

        //--------------------------------------------------------------------------------------------------------------------

        public int[,] reduceXY_To_XY(int[,] inZ, int intNumImg)  // Size same but image/int reduced
        {
            int X = inZ.GetLength(0);
            int Y = inZ.GetLength(1);

            intNumImg = intNumImg == 0 ? 1 : intNumImg;

            double dky = intNumImg; // X /(double)(X - intReduce);
            int[,] intOut = new int[X, Y]; // same size but int will be reduced;

            for (int j = 0; j < Y; j++)  // do not need to cut Y axel
            {
                int jnew = (int)(j / dky + Y / 2.0 - Y / 2.0 / dky);
                for (int i = 0; i < X; i++)  // cut X axel
                {
                    intOut[(int)(i / dky + X / 2.0 - X / 2.0 / dky), jnew] = inZ[i, j];
                }
            }

            return intOut;
        }

        public int[,] modifyIntToNumInt(int[,] Z, int num)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);

            int[,] Zout = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int z = Z[i, j];
                    Zout[i, j] = 255 << 24 | z;
                }
            }

            return Zout;
        }

        public WriteableBitmap convertIntToGrayImg(Int16[,] Z, out int zero)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            zero = 0;
            for (int i = 0; i < X * Y; i++)
            {
                int z = (byte)(Z[i % X, i / X]);
                if (z == 0) { zero += 1; }
                byte b = (byte)(z < 0 ? 255 : z);
                img.Pixels[i] = 255 << 24 | b << 16 | b << 8 | b;
            }

            return img;
        }

        public WriteableBitmap convertSByteToGrayImg(sbyte[,] Z, out int zero)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            zero = 0;
            for (int i = 0; i < X * Y; i++)
            {
                byte tmp = (byte)(Z[i % X, i / X] << 1);

                if (tmp == 0) { zero += 1; }
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public void convertLowPass(ref Complex[,] cZ, int intBnd)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);

            for (int i = 0; i < X * Y; i++)
            {
                double ix = Math.Abs(-X / 2 + i % X);
                double iy = Math.Abs(-X / 2 + i / X);

                if (ix <= intBnd || iy <= intBnd)
                {
                }
                else
                {
                    cZ[i % X, i / X] = new Complex(0.0, 0.0);
                }
            }
        }

        public void convertHiPass(ref Complex[,] cZ, int intBnd)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);

            for (int i = 0; i < X * Y; i++)
            {
                double ix = Math.Abs(-X / 2 + i % X);
                double iy = Math.Abs(-X / 2 + i / X);

                if (ix <= intBnd || iy <= intBnd)
                {
                    cZ[i % X, i / X] = new Complex(0.0, 0.0);
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// Convert Complex[X,Y].Magnitude - (normalized data - 255)  to  WriteableBitmap
        /// maxReal has to be >= 1.0
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public WriteableBitmap convertComlexToLog10GrayImg(Complex[,] cZ, double maxMagnitude, double minMagnitude)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                double logx = Math.Log10(cZ[i % X, i / X].Magnitude) - Math.Log10(minMagnitude);
                double logdelta = Math.Log10(maxMagnitude) - Math.Log10(minMagnitude);

                byte tmp = (byte)((logx < 0 ? 0 : logx) / logdelta * 255); // normalized Log10(data)
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        /// <summary>
        /// Convert Complex[X,Y].Phase - (normalized data - 255)  to  WriteableBitmap
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public WriteableBitmap convertComlexToPhaseGrayImg(Complex[,] cZ)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                byte tmp = (byte)((cZ[i % X, i / X].Phase + Math.PI) / (2.0 * Math.PI) * 255); // normalized - 2*PI
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public WriteableBitmap convertComlexRowToImg(ref WriteableBitmap img, int Jrow)    // uses ref for modify image on fly;
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            WriteableBitmap imgOut = new WriteableBitmap(X, 256);  // because Color 0..255
            byte[] byteRow = new byte[X];

            for (int i = X * (Jrow - 1); i < X * Jrow; i++)
            {
                byteRow[i - (X * (Jrow - 1))] = (byte)(img.Pixels[i] & 255);
                img.Pixels[i] = (255 << 16 | 255 << 8 | 255) & img.Pixels[i]; // create line
            }

            for (int i = 0; i < X; i++)
            {
                byte itemp = (byte)byteRow[i];
                for (int j = 0; j < itemp; j++)
                {
                    imgOut.Pixels[(255 - j) * X + i] = 255 << 24 | 0;
                }
            }

            return imgOut;
        }

        public WriteableBitmap convertComlexColumnToImg(ref WriteableBitmap img, int Jcol)   // uses ref for modify image on fly;
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            WriteableBitmap imgOut = new WriteableBitmap(Y, 256);  // because Color 0..255
            byte[] byteCol = new byte[Y];

            for (int i = Jcol - 1; i < X * Y; i += X)
            {
                byteCol[i / X] = (byte)(img.Pixels[i] & 255);
                img.Pixels[i] = (255 << 16 | 255 << 8 | 255) & img.Pixels[i]; // create line
            }

            for (int i = 0; i < X; i++)
            {
                byte itemp = (byte)byteCol[i];
                for (int j = 0; j < itemp; j++)
                {
                    imgOut.Pixels[(255 - j) * X + i] = 255 << 24 | 0;
                }
            }
            return imgOut;
        }

        /// <summary>
        /// Convert Complex[X,Y].Magnitude - (normalized data - 255)  to  WriteableBitmap
        /// maxReal has to be >= 1.0
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public WriteableBitmap convertComlexToGrayImg(Complex[,] cZ, double maxMagnitude, bool blnInvert, bool blnBlackWhite)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            int boundary = 127;

            if (blnBlackWhite)
            {
                double sum = 0.0;
                for (int di = 0; di < X * Y; di++)
                {
                    sum += cZ[di % X, di / X].Magnitude;
                }
                boundary = (int)sum / (X * Y);
            }

            for (int i = 0; i < X * Y; i++)
            {
                byte tmp;
                tmp = (byte)((255.0 * cZ[i % X, i / X].Magnitude) / maxMagnitude); // normalized Log10(data)

                if (blnInvert)
                {
                    tmp = (byte)(255 - tmp);
                    boundary = 255 - boundary;
                }

                if (blnBlackWhite)
                {
                    if (tmp >= boundary)
                    {
                        tmp = 255;
                    }
                    else
                    {
                        tmp = 0;
                    }
                }

                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public WriteableBitmap convertComlexToGrayImg(Complex[,] cZ)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);
            double maxMagnitude = 0;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    maxMagnitude = maxMagnitude < cZ[i, j].Magnitude ? cZ[i, j].Magnitude : maxMagnitude;
                }
            }

            for (int i = 0; i < X * Y; i++)
            {
                byte tmp;
                tmp = (byte)((255.0 * cZ[i % X, i / X].Magnitude) / maxMagnitude); // normalized Log10(data)
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public WriteableBitmap convertComlexRealOnlyToGrayImg(Complex[,] cZ)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);
            double maxReal = 0;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    maxReal = maxReal < cZ[i, j].Real ? cZ[i, j].Real : maxReal;
                }
            }

            for (int i = 0; i < X * Y; i++)
            {
                byte tmp;
                tmp = (byte)((255.0 * cZ[i % X, i / X].Real) / maxReal); // normalized Log10(data)
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public Complex[,] flipXComplex(Complex[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);

            Complex[,] flip = new Complex[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    flip[X - 1 - i, j] = Z[i, j];
                }
            }
            return flip;
        }

        public int[,] flipXComplex(int[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);

            int[,] flip = new int[X, Y];

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    flip[X - 1 - i, j] = Z[i, j];
                }
            }
            return flip;
        }

        public WriteableBitmap convert2DSByteArrayToHistogram(ref sbyte[,] Z, out int intSum)
        {
            WriteableBitmap img = new WriteableBitmap(256, 256);

            int[] H = new int[256];
            for (int i = 0; i < Z.GetLength(0); i++)
            {
                for (int j = 0; j < Z.GetLength(1); j++)
                {
                    byte bt = (byte)Z[i, j];
                    H[bt] = H[bt] + 1;
                    if (H[bt] > 255) H[bt] = 255;
                }
            }

            intSum = 0;

            for (int i = 0; i < 256; i++)
            {
                if (H[i] > 0) { intSum += 1; }

                byte itemp = (byte)H[i];
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }

            return img;
        }

        public WriteableBitmap convertImgToHistogram(WriteableBitmap imgIn)
        {
            WriteableBitmap img = new WriteableBitmap(256, 256);

            int X = imgIn.PixelWidth;
            int Y = imgIn.PixelHeight;

            int[] H = new int[256];
            for (int i = 0; i < X * Y; i++)
            {
                byte tmp = (byte)(imgIn.Pixels[i] & 255);
                H[tmp] = H[tmp] + 1;
            }

            for (int i = 0; i < 256; i++)
            {
                byte itemp = (byte)H[i];
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }

            return img;
        }

        public WriteableBitmap convertArcToImg(List<byte> tmpArr)
        {
            int X = tmpArr.Count;

            int pwr = (int)(Math.Log(X, 2) + 0.5);
            int imgSize = (int)Math.Sqrt(Math.Pow(2.0, pwr));

            WriteableBitmap img = new WriteableBitmap(imgSize, imgSize);

            for (int i = 0; i < imgSize; i++)
            {
                for (int j = 0; j < imgSize; j++)
                {
                    int intT = j * imgSize + i;
                    if (intT < X)
                    {
                        int intTem = tmpArr[intT];
                        int tmp = 0;

                        if ((intTem & 240) == 0)
                        {
                            tmp = 255 << 24 | tmpArr[intT] << 16;
                        }

                        if ((intTem & 15) == 0)
                        {
                            tmp = 255 << 24 | tmpArr[intT] << 8;
                        }

                        img.Pixels[intT] = tmp;
                    }
                    else
                    {
                        img.Pixels[intT] = 255 << 24 | 255;
                    }
                }
            }

            return img;
        }

        public WriteableBitmap convertImgToHistogram(List<byte> tmpArr)
        {
            WriteableBitmap img = new WriteableBitmap(256, 256);

            int X = tmpArr.Count;

            int[] H = new int[256];

            for (int i = 0; i < X; i++)
            {
                byte tmp = tmpArr[i];
                H[tmp] = H[tmp] + 1;
            }

            for (int i = 0; i < 256; i++)
            {
                byte itemp = (byte)H[i];
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }

            return img;
        }

        public WriteableBitmap convert2DByteArrayToHistogramWithMaxOut(ref int[,] Z, out int intMin, out int intMax)
        {
            intMin = 0;
            intMax = 0;

            WriteableBitmap img = new WriteableBitmap(256, 256);

            int[] H = new int[256];
            for (int i = 0; i < Z.GetLength(0); i++)
            {
                for (int j = 0; j < Z.GetLength(1); j++)
                {
                    H[(byte)Z[i, j]] = H[(byte)Z[i, j]] + 1;
                    if (H[(byte)Z[i, j]] > 255) { H[(byte)Z[i, j]] = 255; };

                    if (intMin < Z[i, j]) intMin = Z[i, j];
                    if (intMax > Z[i, j]) intMax = Z[i, j];
                }
            }

            for (int i = 0; i < 256; i++)
            {
                byte itemp = (byte)H[i];
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }

            return img;
        }

        public WriteableBitmap convert2DByteArrayToHistogramWithMaxOut(ref Int16[,] Z, out int intMin, out int intMax)
        {
            intMin = 0;
            intMax = 0;

            byte bt = 0;

            WriteableBitmap img = new WriteableBitmap(256, 256);

            int[] H = new int[256];
            for (int i = 0; i < Z.GetLength(0); i++)
            {
                for (int j = 0; j < Z.GetLength(1); j++)
                {
                    bt = (byte)Z[i, j];
                    H[bt] += 1;
                    if (H[bt] >= 255) H[bt] = 255;  // cut Histogram

                    if (intMin > Z[i, j]) intMin = Z[i, j];
                    if (intMax < Z[i, j]) intMax = Z[i, j];
                }
            }

            for (int i = 0; i < 256; i++)
            {
                byte itemp = (byte)(H[i]);
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }

            return img;
        }

        public WriteableBitmap convertArrayToHistogram256WithMaxOut(int[] H, out int maxHist)
        {
            WriteableBitmap img = new WriteableBitmap(256, 256);
            int max = 0;
            for (int i = 0; i < H.Length; i++)
            {
                if (H[i] > max) max = H[i];
            }

            maxHist = max;

            for (int i = 0; i < 256; i++)
            {
                int itemp = ((H[i] * 255) / max) & 255;
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }
            return img;
        }

        public WriteableBitmap convertArrayToHistogram256WithMaxIn(int[] H, int maxHist)
        {
            WriteableBitmap img = new WriteableBitmap(256, 256);

            for (int i = 0; i < 256; i++)
            {
                int itemp = ((H[i] * 255) / maxHist) & 255;
                for (int j = 0; j < itemp; j++)
                {
                    img.Pixels[(255 - j) * 256 + i] = 255 << 24 | 0;
                }
            }
            return img;
        }

        public void convertComplexArrayToWithLossOfArray(ref Complex[,] cZ, double percentLoss)
        {
            int X = cZ.GetLength(0);
            int Y = cZ.GetLength(1);

            double loss = (cZ[0, 3].Magnitude + cZ[3, 0].Magnitude + cZ[3, 3].Magnitude) * percentLoss / 3 / 100.0;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    if (cZ[i, j].Magnitude < loss) { cZ[i, j] = Complex.Zero; }
                }
            }
        }

        public WriteableBitmap convertImgAnySizeTo256x256(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            double dky = Y / 256.0;
            WriteableBitmap imgOut = new WriteableBitmap(256, 256);

            double diStart = (X / 2.0 - 128.0 * dky);
            double diStop = (X / 2.0 + 128.0 * dky);

            int inew = 0;
            int jnew = 0;

            for (double dj = 0; dj < Y - dky; dj += dky)  // do not need to cut Y axil
            {
                for (double di = diStart; di < diStop; di += dky)  // cut X axil
                {
                    int pixel = img.Pixels[(int)(dj + 0.5) * X + (int)(di + 0.5)];

                    int iAvr;

                    int iRed = (int)(((pixel & (255 << 16)) >> 16));
                    int iGreen = (int)(((pixel & (255 << 8)) >> 8));
                    int iBlue = (int)(((pixel & (255 << 0)) >> 0));

                    iAvr = (int)(iRed * 0.299 + iGreen * 0.587 + iBlue * 0.114);
                    iAvr = (iAvr >> 2) << 2; // keep only 64 grade
                    imgOut.Pixels[256 * jnew + inew] = 255 << 24 | iAvr << 16 | iAvr << 8 | iAvr << 0;

                    inew++;
                }
                inew = 0;
                jnew++;
            }

            return imgOut;
        }

        public ObservableCollection<WriteableBitmap> convertGray1024x256ToGrayImgCollection(WriteableBitmap imgGray)
        {
            int X = 1024;    // imgGray[0].PixelWidth;
            int Y = 256;    // imgGray[0].PixelHeight;

            ObservableCollection<WriteableBitmap> imgCollection = new ObservableCollection<WriteableBitmap>();

            imgCollection.Add(new WriteableBitmap(256, 256));
            imgCollection.Add(new WriteableBitmap(256, 256));
            imgCollection.Add(new WriteableBitmap(256, 256));
            imgCollection.Add(new WriteableBitmap(256, 256));

            for (int i = 0; i < X; i += 1)
            {
                for (int j = 0; j < Y; j += 1)
                {
                    int pixel = imgGray.Pixels[j * X + i];
                    imgCollection[i & 3].Pixels[(j * X + i) >> 2] = pixel;
                }
            }

            return imgCollection;
        }

        public WriteableBitmap convertGrayImgToGray1024x256(ObservableCollection<WriteableBitmap> imgGray)
        {
            int X = 256;    // imgGray[0].PixelWidth;
            int Y = 256;    // imgGray[0].PixelHeight;

            WriteableBitmap imgOut = new WriteableBitmap(1024, 256);

            for (int i = 0; i < X; i += 1)
            {
                for (int k = 0; k < 4; k += 1)
                {
                    for (int j = 0; j < Y; j += 1)
                    {
                        int pixel = imgGray[k].Pixels[j * X + i];

                        imgOut.Pixels[j * 1024 + 4 * i + k] = pixel;
                    }
                }
            }

            return imgOut;
        }

        public sbyte[,] convertGrayImgToSByte256(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            sbyte[,] zimg = new sbyte[X, Y];
            for (int i = 0; i < X * Y; i++)
            {
                int intC = 0;

                intC = img.Pixels[i] & 255;
                zimg[i % X, i / X] = (sbyte)((intC & 255) >> 2);
            }

            return zimg;
        }

        public WriteableBitmap convertSByte256ToGrayImg(sbyte[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                sbyte sb = Z[i % X, i / X];
                //if (sb < 0) sb = 0;
                //if (sb > 63) sb = 63;

                byte tmp = (byte)((int)sb << 2);
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }

        public WriteableBitmap convertSByte256ToGrayImgReceive(sbyte[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                sbyte sb = Z[i % X, i / X];
                if (sb < 0) sb = 0;
                if (sb > 63) sb = 63;

                byte tmp = (byte)((int)sb << 2);
                img.Pixels[i] = 255 << 24 | tmp << 16 | tmp << 8 | tmp;
            }

            return img;
        }
    }
}