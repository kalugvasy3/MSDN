using System;
using System.Numerics;

using System.Windows.Media.Imaging;

namespace TransformMethods
{
    public class Edge
    {
        public WriteableBitmap imgCannyEdge(WriteableBitmap imgColor, int nOrder, double sigma)
        {
            int Y = imgColor.PixelHeight;
            int X = imgColor.PixelWidth;

            int[,] intImg = new int[X, Y];
            intImg = convertImgToGrayInt(imgColor, ""); // Image already Gray ...

            Complex[,] zImg = filterSobelReal(filterGaussian(intImg, nOrder, sigma));
           
            return convertIntToGrayImg(nonMaximumSurpressReal(zImg));
          
        }

        public WriteableBitmap convertIntToGrayImg(int[,] Z)
        {
            int X = Z.GetLength(0);
            int Y = Z.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                int z = Z[i % X, i / X];

                //byte b = (byte)(z < 0 ? 0 : (z > 255 ? 255 : z));
                byte b = (byte)(z > 255 ? 255 : (z < 0 ? 255 + z : z));
                img.Pixels[i] = 255 << 24 | b << 16 | b << 8 | b;
            }

            return img;
        }

        public int[,] filterRoberts(int[,] zimg)
        {
            int X = zimg.GetLength(0);
            int Y = zimg.GetLength(1);

            int[,] edge = new int[X, Y];

            for (int i = 0; i < X - 1; i++)
            {
                for (int j = 0; j < Y - 1; j++)
                {
                    int P1 = zimg[i, j];
                    int P2 = zimg[i + 1, j];
                    int P3 = zimg[i, j + 1];
                    int P4 = zimg[i + 1, j + 1];

                    edge[i, j] = (Int16)(Math.Abs(P1 - P4) + Math.Abs(P2 - P3));

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
                edge[X - 1, j] = edge[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                edge[i, Y - 1] = edge[i, Y - 2];
            }
            return edge;
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


        public int[,] filterGaussian(int[,] intImg, int intOddOrder, double sigma)
        {
            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            double[,] gaussK = gaussFilterKoefficiant(intOddOrder, sigma);

            int[,] intOut = new int[X, Y];

            int n = (intOddOrder - 1) / 2;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    // Gauss section
                    double sum = 0.0;
                    double sumG = 0.0;

                    for (int ig = 0; ig < intOddOrder; ig++)
                    {
                        for (int jg = 0; jg < intOddOrder; jg++)
                        {
                            if (i - n + ig < 0 || i - n + ig >= X) continue;
                            if (j - n + jg < 0 || j - n + jg >= Y) continue;
                            sum += gaussK[ig, jg];
                            sumG += gaussK[ig, jg] * intImg[i - n + ig, j - n + jg];
                        }
                    }
                    intOut[i, j] = (int)(sumG / sum);
                    // --------------
                }
            }

            return intOut;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="intImg"></param>
        /// <returns></returns>
        ///
        public Complex[,] filterSobelReal(int[,] intImg)    // x +iy
        {
            // X,Y Dimension

            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            Complex[,] edge = new Complex[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    double dY = (intImg[i - 1, j + 1] + 1 * intImg[i, j + 1] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i, j - 1] + intImg[i + 1, j - 1]);
                    double dX = (intImg[i + 1, j - 1] + 1 * intImg[i + 1, j] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i - 1, j] + intImg[i - 1, j + 1]);

                    //       -1  0 +1            +1 +2 +1      | Y j
                    //  Dx   -2  0 +2      Dy     0  0  0      |
                    //       -1  0 +1            -1 -2 -1      |______ X i

                    edge[i, j] = new Complex(dX, dY);
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                edge[0, j] = edge[1, j];
                edge[X - 1, j] = edge[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                edge[i, 0] = edge[i, 1];
                edge[i, Y - 1] = edge[i, Y - 2];
            }

            double pi8 = Math.PI / 8.0;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    double phaseRange = edge[i, j].Phase / pi8;

                    if (phaseRange < 1 && phaseRange > -1) { phaseRange = 0; goto l3; }      //  →
                    if (phaseRange >= 1 && phaseRange < 3) { phaseRange = 2; goto l3; }      //  ↗
                    if (phaseRange >= 3 && phaseRange < 5) { phaseRange = 4; goto l3; }      //  ↑
                    if (phaseRange >= 5 && phaseRange < 7) { phaseRange = 6; goto l3; }      //  ↖
                    if (phaseRange >= 7 || phaseRange < -7) { phaseRange = 8; goto l3; }     //  ←
                    if (phaseRange <= -5 & phaseRange > -7) { phaseRange = -6; goto l3; }    //  ↙
                    if (phaseRange <= -3 & phaseRange > -5) { phaseRange = -4; goto l3; }    //  ↓
                    if (phaseRange <= -1 & phaseRange > -3) { phaseRange = -2; goto l3; }    //  ↘

                    l3: edge[i, j] = edge[i, j].Magnitude + phaseRange * Complex.ImaginaryOne;
                }
            }; //  Real

            return edge;
        }

        public int[,] filterSobel(int[,] intImg)    // x +iy
        {
            // X,Y Dimensions

            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            int[,] edge = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    double dY = (intImg[i - 1, j + 1] + 1 * intImg[i, j + 1] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i, j - 1] + intImg[i + 1, j - 1]);
                    double dX = (intImg[i + 1, j - 1] + 1 * intImg[i + 1, j] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i - 1, j] + intImg[i - 1, j + 1]);

                    //       -1  0 +1            +1 +2 +1      | Y j
                    //  Dx   -2  0 +2      Dy     0  0  0      |
                    //       -1  0 +1            -1 -2 -1      |______ X i

                    double xx = Math.Sqrt(dX * dX + dY * dY);
                    edge[i, j] = (xx > 255 ? 255 : (int)xx);
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                edge[0, j] = edge[1, j];
                edge[X - 1, j] = edge[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                edge[i, 0] = edge[i, 1];
                edge[i, Y - 1] = edge[i, Y - 2];
            }
            return edge;
        }
 

        public int[,] nonMaximumSurpressReal(Complex[,] zImg)   // out - very narrow edge
        {
            int X = zImg.GetLength(0);
            int Y = zImg.GetLength(1);

            //   0   →
            //   2  ↗
            //   4   ↑
            //   6  ↖
            //   8   ←
            //  -6  ↙
            //  -4  ↓
            //   -2 ↘

            int[,] edge = new int[X, Y];
            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    int intPH = (int)zImg[i, j].Imaginary;

                    switch (intPH)
                    {
                        case 0: edge[i, j] = intEdge(zImg[i - 1, j], zImg[i, j], zImg[i + 1, j]); break;            //      0
                        case 2: edge[i, j] = intEdge(zImg[i + 1, j + 1], zImg[i, j], zImg[i - 1, j - 1]); break;    //     45
                        case 4: edge[i, j] = intEdge(zImg[i, j + 1], zImg[i, j], zImg[i, j - 1]); break;            //     90
                        case 6: edge[i, j] = intEdge(zImg[i + 1, j - 1], zImg[i, j], zImg[i - 1, j + 1]); break;    //    135
                        case 8: edge[i, j] = intEdge(zImg[i - 1, j], zImg[i, j], zImg[i + 1, j]); break;            //  +-180
                        case -6: edge[i, j] = intEdge(zImg[i + 1, j + 1], zImg[i, j], zImg[i - 1, j - 1]); break;   //   -135
                        case -4: edge[i, j] = intEdge(zImg[i, j + 1], zImg[i, j], zImg[i, j - 1]); break;           //    -90
                        case -2: edge[i, j] = intEdge(zImg[i + 1, j - 1], zImg[i, j], zImg[i - 1, j + 1]); break;   //    -45

                        default: break;
                    }
                }
            }

            for (int j = 1; j < Y - 1; j++)
            {
                edge[0, j] = edge[1, j];
                edge[X - 1, j] = edge[X - 2, j];
            }

            for (int i = 1; i < X; i++)
            {
                edge[i, 0] = edge[i, 1];
                edge[i, Y - 1] = edge[i, Y - 2];
            }
            return edge;
        }

      
        private double[,] gaussFilterKoefficiant(int sizeOdd, double sigma) // Size must be Odd   sigma usual 1.4
        {   // filter coefficient
            double[,] gauss = new double[sizeOdd, sizeOdd];
            // set standard deviation
            double r;
            double s = 2.0 * sigma * sigma;
            // sumG is for normalization
            double sum = 0.0;

            //generate sizeOdd x  sizeOdd kernel
            int n = (sizeOdd - 1) / 2;

            for (int x = -n; x <= n; x++)
            {
                for (int y = -n; y <= n; y++)
                {
                    r = Math.Sqrt(x * x + y * y);
                    gauss[x + n, y + n] = (Math.Exp(-(r * r) / s)) / (Math.PI * s);
                    sum += gauss[x + n, y + n];
                }
            }

            // normalize the Gauss
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    gauss[i, j] /= sum;

            return gauss;
        }

        private int intEdge(Complex A, Complex B, Complex C)
        {
            if (B.Magnitude > A.Magnitude && B.Magnitude > C.Magnitude) { return (int)B.Magnitude; }
            else { return 0; }
        }

 
        private int[,] convertImgToGrayInt(WriteableBitmap img, string strColor)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] intImg = new int[X, Y];
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

                    case "RGB":    // Gray ->  Y = .2126 * R^gamma + .7152 * G^gamma + .0722 * B^gamma
                        intC = (int)(0.2126 * ((img.Pixels[i] & (255 << 16)) >> 16) + 0.7152 * ((img.Pixels[i] & (255 << 8)) >> 8) + 0.0722 * ((img.Pixels[i] & (255 << 0)) >> 0));
                        break;

                    default:
                        intC = img.Pixels[i] & 255;
                        break;
                }

                intImg[i % X, i / X] = intC & 255;
            }

            return intImg;
        }
    }
}