//http://en.wikipedia.org/wiki/Discrete_wavelet_transform
//http://en.wikipedia.org/wiki/Cohen-Daubechies-Feauveau_wavelet

using System;
using System.Collections.Generic;

namespace TransformMethods
{
    public class WaveLetHaar
    {
        public static List<Double> DirectTransform(List<Double> SourceList, int loss, int rvalue) {
            List<Double> RetVal = new List<Double>();
            List<Double> TmpArr = new List<Double>();

            if (SourceList.Count <= rvalue) {
                return SourceList;
            }

            for (int j = 0; j < SourceList.Count; j += 2) {
                double Rtmp = (SourceList[j] - SourceList[j + 1]) / 2.0;
                double Atmp = (SourceList[j] + SourceList[j + 1]) / 2.0;

                if (Math.Abs(Rtmp) <= loss) Rtmp = 0;
                if (Math.Abs(Atmp) <= loss) Atmp = 0;

                RetVal.Add(Rtmp);
                TmpArr.Add(Atmp);
            }

            RetVal.AddRange(DirectTransform(TmpArr, loss, rvalue));

            return RetVal;
        }

        public static List<Double> InverseTransform(List<Double> SourceList, int rvalue) {
            if (SourceList.Count <= rvalue)
                return SourceList;

            List<Double> RetVal = new List<Double>();
            List<Double> TmpPart = new List<Double>();

            for (int i = SourceList.Count / 2; i < SourceList.Count; i++)
                TmpPart.Add(SourceList[i]);

            List<Double> SecondPart = InverseTransform(TmpPart, rvalue);

            for (int i = 0; i < SourceList.Count / 2; i++) {
                RetVal.Add(SecondPart[i] + SourceList[i]);
                RetVal.Add(SecondPart[i] - SourceList[i]);
            }

            return RetVal;
        }

        public static List<int> DirectTransform(List<int> SourceList, int loss, int rvalue) {
            List<int> RetVal = new List<int>();
            List<int> TmpArr = new List<int>();

            if (SourceList.Count <= rvalue) {
                return SourceList;
            }

            for (int j = 0; j < SourceList.Count; j += 2) {
                int Rtmp = (SourceList[j] - SourceList[j + 1]) / 2;
                int Atmp = (SourceList[j] + SourceList[j + 1]) / 2;

                if (Math.Abs(Rtmp) <= loss) Rtmp = 0;
                if (Math.Abs(Atmp) <= loss) Atmp = 0;

                RetVal.Add(Rtmp);
                TmpArr.Add(Atmp);
            }

            RetVal.AddRange(DirectTransform(TmpArr, loss, rvalue));

            return RetVal;
        }

        public static List<int> InverseTransform(List<int> SourceList, int rvalue) {
            if (SourceList.Count <= rvalue)
                return SourceList;

            List<int> RetVal = new List<int>();
            List<int> TmpPart = new List<int>();

            for (int i = SourceList.Count / 2; i < SourceList.Count; i++)
                TmpPart.Add(SourceList[i]);

            List<int> SecondPart = InverseTransform(TmpPart, rvalue);

            for (int i = 0; i < SourceList.Count / 2; i++) {
                RetVal.Add(SecondPart[i] + SourceList[i]);
                RetVal.Add(SecondPart[i] - SourceList[i]);
            }

            return RetVal;
        }

        public void Haar2DInverse(ref sbyte[,] Z, int vHaar, int hHaar) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = InverseTransform(zCol, vHaar); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    dZ[i, j] = (int)zCol[j];
                }
            }

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(dZ[i, j]);
                }

                zRow = InverseTransform(zRow, hHaar); //for each rows

                for (i = 0; i < nX; i++) {
                    Z[i, j] = (sbyte)zRow[i];
                }
            }
        }

        public void Haar2DByteForward(ref sbyte[,] Z, byte loss, int vHaar, int hHaar) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = DirectTransform(zRow, loss, hHaar); //for each rows

                for (i = 0; i < nX; i++) {
                    dZ[i, j] = (int)zRow[i];
                }
            }

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(dZ[i, j]);
                }

                zCol = DirectTransform(zCol, loss, vHaar); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (sbyte)zCol[j];
                }
            }
        }

        public void Haar2DForward(ref Int16[,] Z, int loss, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = DirectTransform(zRow, loss, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    dZ[i, j] = zRow[i];
                }
            }

            // Transform Columns - nY

            //double zMax = 0.0;
            //double zMin = 0.0;

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(dZ[i, j]);
                }

                zCol = DirectTransform(zCol, loss, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (Int16)zCol[j];
                }
            }
        }

        public void Haar2DForward(ref int[,] Z, int loss, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = DirectTransform(zRow, loss, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    dZ[i, j] = zRow[i];
                }
            }

            // Transform Columns - nY

            //double zMax = 0.0;
            //double zMin = 0.0;

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(dZ[i, j]);
                }

                zCol = DirectTransform(zCol, loss, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    Z[i, j] = (int)zCol[j];
                }
            }
        }

        public void Haar2DByteForward(ref sbyte[,] Z, byte loss, byte rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(Z[i, j]);
                }

                zRow = DirectTransform(zRow, loss, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    dZ[i, j] = (int)zRow[i];
                }
            }

            // Transform Columns - nY

            //double zMax = 0.0;
            //double zMin = 0.0;

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(dZ[i, j]);
                }

                zCol = DirectTransform(zCol, loss, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    if (zCol[j] > 127) zCol[j] = 127;
                    if (zCol[j] < -127) zCol[j] = -127;
                    Z[i, j] = (sbyte)zCol[j];
                }
            }
        }

        //public void Haar2DInverse(ref int[,] Z)
        //{
        //    int nX = Z.GetLength(0);
        //    int nY = Z.GetLength(1);

        //    int i, j;

        //    // Transform Columns - nY

        //    for (i = 0; i < nX; i++)
        //    {
        //        List<int> zCol = new List<int>();
        //        for (j = 0; j < nY; j++)
        //        {
        //            zCol.Add(Z[i, j]);
        //        }

        //        zCol = InverseTransform(zCol); ;  //for each columns

        //        for (j = 0; j < nY; j++)
        //        {
        //            Z[i, j] = zCol[j];
        //        }
        //    }

        //    // Transform Rows  - nX
        //    for (j = 0; j < nY; j++)
        //    {
        //        List<int> zRow = new List<int>();
        //        for (i = 0; i < nX; i++)
        //        {
        //            zRow.Add(Z[i, j]);
        //        }

        //        zRow = InverseTransform(zRow); //for each rows

        //        for (i = 0; i < nX; i++)
        //        {
        //            Z[i, j] = zRow[i];
        //        }
        //    }

        //}

        public void Haar2DInverse(ref sbyte[,] Z, byte rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = InverseTransform(zCol, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    if (zCol[j] > 127) zCol[j] = 127;
                    if (zCol[j] < -127) zCol[j] = -127;

                    dZ[i, j] = (int)zCol[j];
                }
            }

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(dZ[i, j]);
                }

                zRow = InverseTransform(zRow, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    if (zRow[i] > 127) zRow[i] = 127;
                    if (zRow[i] < -127) zRow[i] = -127;

                    Z[i, j] = (sbyte)zRow[i];
                }
            }
        }

        public void Haar2DInverse(ref int[,] Z, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = InverseTransform(zCol, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    dZ[i, j] = zCol[j];
                }
            }

            double zMax = 0.0;
            double zMin = 0.0;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(dZ[i, j]);
                }

                zRow = InverseTransform(zRow, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    if (zRow[i] > 255) zRow[i] = 255;
                    if (zRow[i] > zMax) zMax = zRow[i];
                    if (zRow[j] < zMin) zMin = zRow[i];

                    dZ[i, j] = zRow[i];
                }
            }

            for (i = 0; i < nX; i++) {
                for (j = 0; j < nY; j++) {
                    Z[i, j] = (int)((dZ[i, j] - zMin) * 255.0 / (zMax - zMin));
                }
            }
        }

        public void Haar2DInverse(ref Int16[,] Z, int rvalue) {
            int nX = Z.GetLength(0);
            int nY = Z.GetLength(1);

            double[,] dZ = new double[nX, nY];

            int i, j;

            // Transform Columns - nY

            for (i = 0; i < nX; i++) {
                List<double> zCol = new List<double>();
                for (j = 0; j < nY; j++) {
                    zCol.Add(Z[i, j]);
                }

                zCol = InverseTransform(zCol, rvalue); ;  //for each columns

                for (j = 0; j < nY; j++) {
                    dZ[i, j] = zCol[j];
                }
            }

            double zMax = 0.0;
            double zMin = 0.0;

            // Transform Rows  - nX
            for (j = 0; j < nY; j++) {
                List<double> zRow = new List<double>();
                for (i = 0; i < nX; i++) {
                    zRow.Add(dZ[i, j]);
                }

                zRow = InverseTransform(zRow, rvalue); //for each rows

                for (i = 0; i < nX; i++) {
                    if (zRow[i] > 255) zRow[i] = 255;
                    if (zRow[i] > zMax) zMax = zRow[i];
                    if (zRow[j] < zMin) zMin = zRow[i];

                    dZ[i, j] = zRow[i];
                }
            }

            for (i = 0; i < nX; i++) {
                for (j = 0; j < nY; j++) {
                    Z[i, j] = (Int16)((dZ[i, j] - zMin) * 255.0 / (zMax - zMin));
                }
            }
        }
    }
}