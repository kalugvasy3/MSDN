using System;
using System.Collections.Generic;

namespace TransformMethods
{
    public class WaveLetCDF97
    {
        //*  fwt97 - Forward biorthogonal 9/7 wavelet transform (lifting implementation)
        //*
        //*  x is an input signal, which will be replaced by its output transform.
        //*  n is the length of the signal, and must be a power of 2.
        //*
        //*  The first half part of the output signal contains the approximation coefficients.
        //*  The second half part contains the detail coefficients (aka. the wavelets coefficients).
        //*
        //*  See also iwt97.

        private static List<Double> fwt97(List<double> x, int loss, int rvalue) {
            int n = x.Count;
            if (n <= rvalue | n <= 4) return x;

            double a;
            int i;

            // Predict 1
            a = -1.586134342;
            for (i = 1; i < n - 2; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[n - 1] += 2 * a * x[n - 2];

            // Update 1
            a = -0.05298011854;
            for (i = 2; i < n; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[0] += 2 * a * x[1];

            // Predict 2
            a = 0.8829110762;
            for (i = 1; i < n - 2; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[n - 1] += 2 * a * x[n - 2];

            // Update 2
            a = 0.4435068522;
            for (i = 2; i < n; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[0] += 2 * a * x[1];

            // Scale
            a = 1 / 1.149604398;

            for (i = 0; i < n; i++) {
                if (i % 2 == 0) x[i] *= a;
                else x[i] /= a;
            }

            double[] tempbank = new double[n];

            // Pack

            for (i = 0; i < n; i++) {
                if (i % 2 == 0) tempbank[i / 2] = x[i];
                else tempbank[n / 2 + i / 2] = x[i];
            }

            List<Double> RetVal = new List<Double>();
            List<Double> TmpArr = new List<Double>();

            for (i = n / 2; i < n; i++) {
                if (Math.Abs(tempbank[i]) < loss) {
                    RetVal.Add(0.0);
                }
                else {
                    RetVal.Add(tempbank[i]);
                }
            }

            for (i = 0; i < n / 2; i++) {
                if (Math.Abs(tempbank[i]) < loss) {
                    TmpArr.Add(0.0);
                }
                else {
                    TmpArr.Add(tempbank[i]);
                }
            }

            RetVal.AddRange(fwt97(TmpArr, loss, rvalue));

            return RetVal;
        }

        //*  iwt97 - Inverse biorthogonal 9/7 wavelet transform
        //*
        //*  This is the inverse of fwt97 so that iwt97(fwt97(x,n),n)=x for every signal x of length n.
        //*
        //*  See also fwt97.

        public static List<Double> inverse97(List<Double> SourceList, int rvalue) {
            int n = SourceList.Count;

            if (n <= rvalue | n <= 4) return SourceList;

            List<double> RetVal = new List<double>();
            List<double> TmpPart = new List<double>();

            for (int i = n / 2; i < n; i++) TmpPart.Add(SourceList[i]);
            List<Double> SecondPart = inverse97(TmpPart, rvalue);

            for (int i = 0; i < n / 2; i++) RetVal.Add(SourceList[i]);
            for (int i = 0; i < n / 2; i++) RetVal.Add(SecondPart[i]);

            return iwt97(RetVal);
        }

        public static List<double> iwt97(List<double> x) {
            int n = x.Count;

            double a;
            int i;

            List<double> tempbank = new List<double>();

            // Unpack

            for (i = 0; i < n / 2; i++) {
                tempbank.Add(x[i + n / 2]);
                tempbank.Add(x[i]);
            }
            for (i = 0; i < n; i++) x[i] = tempbank[i];

            // Undo scale
            a = 1.149604398;

            for (i = 0; i < n; i++) {
                if (i % 2 == 0) x[i] *= a;
                else x[i] /= a;
            }

            // Undo update 2
            a = -0.4435068522;
            for (i = 2; i < n; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[0] += 2 * a * x[1];

            // Undo predict 2
            a = -0.8829110762;
            for (i = 1; i < n - 2; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[n - 1] += 2 * a * x[n - 2];

            // Undo update 1
            a = 0.05298011854;
            for (i = 2; i < n; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[0] += 2 * a * x[1];

            // Undo predict 1
            a = 1.586134342;
            for (i = 1; i < n - 2; i += 2) {
                x[i] += a * (x[i - 1] + x[i + 1]);
            }
            x[n - 1] += 2 * a * x[n - 2];

            return x;
        }

        public void CDF2DForward(ref sbyte[,] Z, int loss, int vD, int hD) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = fwt97(zRow, loss, hD); //for each rows

                for (i = 0; i < nX; i++) {
                    Z[i, j] = (sbyte)(zRow[i]);  //1.586134342
                }
            }

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = fwt97(zCol, loss, vD);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (sbyte)(zCol[j]);/// 1.586134342

                    if (Z[i, j] > 127) Z[i, j] = 127;
                    if (Z[i, j] < -127) Z[i, j] = -127;
                }
            }
        }

        public void CDF2DForward(ref int[,] Z, int loss, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = fwt97(zRow, loss, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    Z[i, j] = (int)zRow[i];
                }
            }

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = fwt97(zCol, loss, rvalue);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (int)zCol[j];
                }
            }
        }

        public void CDF2DForward(ref sbyte[,] Z, int loss, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = fwt97(zRow, loss, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    Z[i, j] = (sbyte)(zRow[i] / 1.586134342);  //1.586134342
                }
            }

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = fwt97(zCol, loss, rvalue);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (sbyte)(zCol[j] / 1.586134342);

                    if (Z[i, j] > 127) Z[i, j] = 127;
                    if (Z[i, j] < -127) Z[i, j] = -127;
                }
            }
        }

        public void CDF2DInverse(ref int[,] Z, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = inverse97(zCol, rvalue);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (int)zCol[j];
                }
            }

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = inverse97(zRow, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    Z[i, j] = (int)zRow[i] > 255 ? 255 : (int)zRow[i];
                }
            }
        }

        public void CDF2DInverse(ref sbyte[,] Z, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j] * 1.586134342);
                }

                zCol = inverse97(zCol, rvalue);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (sbyte)zCol[j];
                }
            }

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j] * 1.586134342);
                }

                zRow = inverse97(zRow, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    if (zRow[i] > 127) zRow[i] = 127;
                    if (zRow[i] < -127) zRow[i] = -127;

                    Z[i, j] = (sbyte)zRow[i];
                }
            }
        }

        public void CDF2DInverse(ref sbyte[,] Z, int vD, int hD) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]); // * 1.586134342
                }

                zCol = inverse97(zCol, vD);  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (sbyte)zCol[j];
                }
            }

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]); //* 1.586134342
                }

                zRow = inverse97(zRow, hD); //for each rows

                for (i = 0; i < nX; i++) {
                    if (zRow[i] > 127) zRow[i] = 127;
                    if (zRow[i] < -127) zRow[i] = -127;

                    Z[i, j] = (sbyte)zRow[i];
                }
            }
        }
    }
}