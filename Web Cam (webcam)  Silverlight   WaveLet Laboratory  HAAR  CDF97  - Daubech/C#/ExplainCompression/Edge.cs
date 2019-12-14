using System;
using System.Numerics;

using System.Windows.Media.Imaging;

namespace TransformMethods
{
    public class Edge
    {


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

        public Complex[,] filterGaussian(Complex[,] intImg, int intOddOrder, double sigma)
        {
            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            double[,] gaussK = gaussFilterKoefficiant(intOddOrder, sigma);

            Complex[,] zOut = new Complex[X, Y];

            int n = (intOddOrder - 1) / 2;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    // Gauss section
                    Complex sum = 0.0;
                    Complex sumG = 0.0;

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
                    zOut[i, j] = sumG / sum;
                    // --------------
                }
            }

            return zOut;
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


        private int intMaxTotal(int int00, int int10, int int01, int int11)
        {
            int intMax = 0;

            if (intMax < int00) intMax = int00;
            if (intMax < int10) intMax = int10;
            if (intMax < int01) intMax = int01;
            if (intMax < int11) intMax = int11;

            return intMax;
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

        public Complex[,] filterSobel(int[,] intImg)    // x +iy
        {
            // X,Y Dimensions

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

            return edge;
        }

        public Complex[,] filterSobel(Complex[,] intImg, out double maxMagnitude)    // x +iy
        {
            // X,Y Dimensions

            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            maxMagnitude = 0.0;

            Complex[,] edge = new Complex[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    double dY = ((intImg[i - 1, j + 1] + 1 * intImg[i, j + 1] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i, j - 1] + intImg[i + 1, j - 1])).Magnitude;
                    double dX = ((intImg[i + 1, j - 1] + 1 * intImg[i + 1, j] + intImg[i + 1, j + 1]) - (intImg[i - 1, j - 1] + 1 * intImg[i - 1, j] + intImg[i - 1, j + 1])).Magnitude;

                    //       -1  0 +1            +1 +2 +1      | Y j
                    //  Dx   -2  0 +2      Dy     0  0  0      |
                    //       -1  0 +1            -1 -2 -1      |______ X i

                    edge[i, j] = new Complex(dX, dY);

                    maxMagnitude = maxMagnitude < edge[i, j].Magnitude ? edge[i, j].Magnitude : maxMagnitude;
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

        public int[,] filterSobel(int[,] intImg, int intInd)    // x +iy
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

        public int[,] nonMaximumSurpress(Complex[,] zImg)   // out - very narrow edge
        {
            int X = zImg.GetLength(0);
            int Y = zImg.GetLength(1);

            // tan Y/X
            // 0
            // tan +/22.5    0.4142
            // 45
            // tan 67.5    2.4142
            // 90
            // tan 112.5   -2.4142
            // 135
            // tan 157.5   -0.4142
            // 0

            //  | Y j
            //  |
            //  |______ X i

            int[,] edge = new int[X, Y];

            for (int i = 1; i < X - 1; i++)
            {
                for (int j = 1; j < Y - 1; j++)
                {
                    double ph = zImg[i, j].Phase;

                    if (Math.Abs(ph) > 2.748 || Math.Abs(ph) <= 0.393) { edge[i, j] = intEdge(zImg[i - 1, j], zImg[i, j], zImg[i + 1, j]); continue; };          //  0 , +-180 degree

                    if (ph > 0.393 && ph <= 1.178) { edge[i, j] = intEdge(zImg[i + 1, j + 1], zImg[i, j], zImg[i - 1, j - 1]); continue; };                    //  45        degree
                    if (ph < -1.963 && ph >= -2.748) { edge[i, j] = intEdge(zImg[i + 1, j + 1], zImg[i, j], zImg[i - 1, j - 1]); continue; };                    // -135       degree

                    if (Math.Abs(ph) > 1.178 && ph <= Math.Abs(1.963)) { edge[i, j] = intEdge(zImg[i, j + 1], zImg[i, j], zImg[i, j - 1]); continue; };          //  90 , -90  degree

                    if (ph > 1.963 && ph <= 2.748) { edge[i, j] = intEdge(zImg[i + 1, j - 1], zImg[i, j], zImg[i - 1, j + 1]); continue; };                    //  135       degree
                    if (ph < -0.393 && ph >= -1.178) { edge[i, j] = intEdge(zImg[i + 1, j - 1], zImg[i, j], zImg[i - 1, j + 1]); continue; };                    //  -45       degree
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

        private int intEdge(Complex A, Complex B, Complex C)
        {
            if (B.Magnitude > A.Magnitude && B.Magnitude > C.Magnitude) { return (int)B.Magnitude; }
            else { return 0; }
        }

 

        private WriteableBitmap convertByteToGrayImg(byte[,] bZ)
        {
            int X = bZ.GetLength(0);
            int Y = bZ.GetLength(1);
            WriteableBitmap img = new WriteableBitmap(X, Y);

            for (int i = 0; i < X * Y; i++)
            {
                byte b = bZ[i % X, i / X];
                img.Pixels[i] = 255 << 24 | b << 16 | b << 8 | b;
            }

            return img;
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
                        intC = (img.Pixels[i] & (255 << 16)) >> 8;
                        break;

                    case "G":
                        intC = (img.Pixels[i] & (255 << 8)) >> 8;
                        break;

                    case "B":
                        intC = (img.Pixels[i] & (255 << 0)) >> 0;
                        break;

                    case "RGB":    // Gray ->  Y = .2126 * R^gamma + .7152 * G^gamma + .0722 * B^gamma
                        intC = (int)(0.2126 * ((img.Pixels[i] & (255 << 16)) >> 8) + 0.7152 * ((img.Pixels[i] & (255 << 8)) >> 8) + 0.0722 * ((img.Pixels[i] & (255 << 0)) >> 0));
                        break;

                    default:
                        intC = img.Pixels[i] & 255;
                        break;
                }

                intImg[i % X, i / X] = intC & 255;
            }

            return intImg;
        }

        public Complex[,] sobelRealPhaseFiltr(int[,] intImg, out double maxMagnitude)
        {
            int X = intImg.GetLength(0);
            int Y = intImg.GetLength(1);

            int[,] intReal = new int[X, Y];
            int[,] intImaginary = new int[X, Y];

            Complex[,] zOut = new Complex[X, Y];

            //    Real   - X coordinate
            //   -1  0  +1
            //   -2  0  +2
            //   -1  0  +1

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    if (j == 0)
                    {
                        intReal[i, 0] = 2 * intImg[i, 0] + 1 * intImg[i, 1];
                        continue;
                    }

                    if (j == Y - 1)
                    {
                        intReal[i, Y - 1] = 2 * intImg[i, Y - 1] + 1 * intImg[i, Y - 2];
                        continue;
                    }

                    intReal[i, j] = intImg[i, j - 1] + 1 * intImg[i, j] + intImg[i, j + 1];
                }
            }; //   Real

            for (int j = 0; j < Y; j++)
            {
                for (int i = 0; i < X; i++)
                {
                    if (i == 0)
                    {
                        zOut[i, j] = intReal[1, j] - intReal[0, j];
                        continue;
                    }

                    if (i == X - 1)
                    {
                        zOut[X - 1, j] = intReal[X - 1, j] - intReal[X - 2, j];
                        continue;
                    }

                    zOut[i, j] = intReal[i + 1, j] - intReal[i - 1, j];
                }
            }; //   Real

            //    intImaginary   -  Y - coordinate
            //   -1  -2  -1
            //    0   0   0
            //    1   2   1

            for (int j = 0; j < Y; j++)
            {
                for (int i = 0; i < X; i++)
                {
                    if (i == 0)
                    {
                        intImaginary[0, j] = 2 * intImg[0, j] + 1 * intImg[1, j];
                        continue;
                    }

                    if (i == X - 1)
                    {
                        intImaginary[X - 1, j] = 2 * intImg[X - 1, j] + 1 * intImg[X - 2, j];
                        continue;
                    }

                    intImaginary[i, j] = intImg[i - 1, j] + 1 * intImg[i, j] + intImg[i + 1, j];
                }
            }; //  Real

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    if (j == 0)
                    {
                        zOut[i, 0] += (intImaginary[i, 1] - intImaginary[i, 0]) * Complex.ImaginaryOne;
                        continue;
                    }

                    if (j == Y - 1)
                    {
                        zOut[i, Y - 1] += (intImaginary[i, Y - 1] - intImaginary[i, Y - 2]) * Complex.ImaginaryOne;
                        continue;
                    }

                    zOut[i, j] += (intImaginary[i, j + 1] - intImaginary[i, j - 1]) * Complex.ImaginaryOne;
                }
            }; //  Real

            // Convert to Magnitude - in Real
            //            Phase (by PI/8) - in Imaginary  (need to know the range of angles only  0.. + 8  0 .. -6)

            double pi8 = Math.PI / 8;
            maxMagnitude = 0;

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int phaseRange = (int)(zOut[i, j].Phase / pi8);

                    maxMagnitude = maxMagnitude < zOut[i, j].Magnitude ? zOut[i, j].Magnitude : maxMagnitude;

                    if (phaseRange < 1 && phaseRange > -1) { phaseRange = 0; goto l3; }      //  →
                    if (phaseRange >= 1 && phaseRange < 3) { phaseRange = 2; goto l3; }      //  ↗
                    if (phaseRange >= 3 && phaseRange < 5) { phaseRange = 4; goto l3; }      //  ↑
                    if (phaseRange >= 5 && phaseRange < 7) { phaseRange = 6; goto l3; }      //  ↖
                    if (phaseRange >= 7 || phaseRange < -7) { phaseRange = 8; goto l3; }     //  ←
                    if (phaseRange <= -5 & phaseRange > -7) { phaseRange = -6; goto l3; }    //  ↙
                    if (phaseRange <= -3 & phaseRange > -5) { phaseRange = -4; goto l3; }    //  ↓
                    if (phaseRange <= -1 & phaseRange > -3) { phaseRange = -2; goto l3; }    //  ↘

                    l3: zOut[i, j] = zOut[i, j].Magnitude + phaseRange * Complex.ImaginaryOne;
                }
            }; //  Real
            return zOut;

            // ↑ ↓ → ← ↖ ↗ ↘ ↙ ∙   ↑↗→↘↓↙←↖
        }
    }
}