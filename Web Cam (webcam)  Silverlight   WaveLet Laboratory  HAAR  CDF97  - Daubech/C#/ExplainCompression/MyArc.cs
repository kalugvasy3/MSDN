using System.Collections.Generic;

namespace TransformMethods
{
    public class MyArc
    {
        public List<byte> arcRepeater(sbyte[,] btArr) {
            List<byte> btOut = new List<byte>();
            List<sbyte> btTmp = new List<sbyte>();

            int iX = btArr.GetLength(0);
            int iY = btArr.GetLength(1);

            btOut.Add((byte)(iX >> 8));   // hi byte
            btOut.Add((byte)(iX & 255));  // lo byte
            btOut.Add((byte)(iY >> 8));   // hi byte
            btOut.Add((byte)(iY & 255));  // lo byte

            for (int i = 0; i < iX; i++) {
                for (int j = 0; j < iY; j++) {
                    btTmp.Add(btArr[i, j]);
                }
            }

            sbyte sprev = btTmp[0];
            int jrep = 0;

            foreach (sbyte sbt in btTmp) {
                if (sbt == sprev) {
                    jrep++;
                }
                else {
                    // code prev here
                    btOut.Add((byte)jrep);
                    btOut.Add((byte)sprev);

                    sprev = sbt;
                    jrep = 1;
                }
            }

            return btOut;
        }

        public List<byte> arcRepeater(List<byte> btArr) {
            List<byte> btOut = new List<byte>();

            int iL = btArr.Count;

            byte prev = btArr[0];
            int jrep = 0;

            foreach (byte bt in btArr) {
                if (bt == prev) {
                    jrep++;
                }
                else {
                    // code prev here
                    btOut.Add((byte)jrep);
                    btOut.Add((byte)prev);

                    prev = bt;
                    jrep = 1;
                }
            }

            return btOut;
        }

        public List<byte> archiveWithTable(sbyte[,] arrOrigin) {
            List<byte> btTmp = new List<byte>();

            List<byte> btImage = new List<byte>();
            List<byte> btFirstTbl = new List<byte>();

            int iX = arrOrigin.GetLength(0);
            int iY = arrOrigin.GetLength(1);

            int intCount = 0;
            byte btOrig = 0;
            int intForTbl = 0;

            for (int i = 0; i < iX; i++) {
                for (int j = 0; j < iY; j++) {
                    intCount++;
                    btOrig = (byte)arrOrigin[i, j];

                    if (btOrig != 0) {
                        btImage.Add(btOrig);
                        intForTbl = intForTbl << 1 | 1;
                    }
                    else {
                        intForTbl = intForTbl << 1 | 0;
                    }

                    if (intCount == 8) {
                        intCount = 0;
                        btFirstTbl.Add((byte)intForTbl);
                    }
                }
            }

            btTmp.AddRange(btFirstTbl);

            List<byte> btOut = new List<byte>();
            List<byte> btSecondTbl = new List<byte>();
            List<byte> btSecondImg = new List<byte>();

            intCount = 0;
            btOrig = 0;
            intForTbl = 0;

            foreach (byte b in btTmp) {
                intCount++;

                if (b != 0) {
                    btSecondImg.Add(b);
                    intForTbl = intForTbl << 1 | 1;
                }
                else {
                    intForTbl = intForTbl << 1 | 0;
                }

                if (intCount == 8) {
                    intCount = 0;
                    btSecondTbl.Add((byte)intForTbl);
                }
            }

            int sTbl = btSecondTbl.Count;

            btOut.Add((byte)(sTbl >> 8));   // hi byte
            btOut.Add((byte)(sTbl & 255));  // lo byte

            btOut.Add((byte)(iX >> 8));   // hi byte
            btOut.Add((byte)(iX & 255));  // lo byte
            btOut.Add((byte)(iY >> 8));   // hi byte
            btOut.Add((byte)(iY & 255));  // lo byte

            btOut.AddRange(btSecondTbl);
            btOut.AddRange(btSecondImg);
            btOut.AddRange(btImage);

            return btOut;
        }

        public List<byte> tetraArc(sbyte[,] inByte) {
            List<byte> tbl = new List<byte>();
            List<byte> tetra = new List<byte>();

            List<byte> bOut = new List<byte>();

            int intCount = 0;

            int iTbl = 0;
            int iImg = 0;

            int cimg = 0;

            int iX = inByte.GetLength(0);
            int iY = inByte.GetLength(1);

            for (int i = 0; i < iX; i++) {
                for (int j = 0; j < iY; j++) {
                    byte b = (byte)inByte[i, j];
                    intCount++;

                    int bhi = (b & 240) >> 4;
                    int blo = b & 15;

                    if ((bhi) != 0) {
                        iImg = iImg << 4 | bhi;
                        iTbl = iTbl << 1 | 1;
                        cimg++;

                        if (cimg == 2) {
                            tetra.Add((byte)iImg);
                            cimg = 0;
                            iImg = 0;
                        }
                    }
                    else {
                        iTbl = iTbl << 1 | 0;
                    }

                    if ((blo) != 0) {
                        iImg = iImg << 4 | blo;
                        iTbl = iTbl << 1 | 1;
                        cimg++;

                        if (cimg == 2) {
                            tetra.Add((byte)iImg);
                            cimg = 0;
                            iImg = 0;
                        }
                    }
                    else {
                        iTbl = iTbl << 1 | 0;
                    }

                    if (intCount == 4) {
                        tbl.Add((byte)iTbl);
                        intCount = 0;
                        iTbl = 0;
                    }
                }
            }
            if (cimg == 1) { tetra.Add((byte)iImg); }

            bOut.AddRange(tbl);
            bOut.AddRange(tetra);

            return bOut;
        }

        public List<byte> tetraArc(List<byte> inByte) {
            List<byte> tbl = new List<byte>();
            List<byte> tetra = new List<byte>();

            List<byte> bOut = new List<byte>();

            int intCount = 0;

            int iTbl = 0;
            int iImg = 0;

            int cimg = 0;

            foreach (byte b in inByte) {
                intCount++;

                int bhi = (b & 240) >> 4;
                int blo = b & 15;

                if ((bhi) != 0) {
                    iImg = iImg << 4 | bhi;
                    iTbl = iTbl << 1 | 1;
                    cimg++;

                    if (cimg == 2) {
                        tetra.Add((byte)iImg);
                        cimg = 0;
                        iImg = 0;
                    }
                }
                else {
                    iTbl = iTbl << 1 | 0;
                }

                if ((blo) != 0) {
                    iImg = iImg << 4 | blo;
                    iTbl = iTbl << 1 | 1;
                    cimg++;

                    if (cimg == 2) {
                        tetra.Add((byte)iImg);
                        cimg = 0;
                        iImg = 0;
                    }
                }
                else {
                    iTbl = iTbl << 1 | 0;
                }

                if (intCount == 4) {
                    tbl.Add((byte)iTbl);
                    intCount = 0;
                    iTbl = 0;
                }
            }

            if (cimg == 1) { tetra.Add((byte)iImg); }

            bOut.AddRange(tbl);
            bOut.AddRange(tetra);

            return bOut;
        }

        public sbyte[,] unArcWithTable(List<byte> btArchive) {
            // length second table

            int iScount = btArchive[0];              // btArchive[0] - hi
            iScount = iScount << 8 | btArchive[1];   // btArchive[1] - lo

            int iX = btArchive[2];
            iX = iX << 8 | btArchive[3];
            int iY = btArchive[4];
            iY = iY << 8 | btArchive[5];

            int intTotal = btArchive.Count;

            List<byte> btImage = new List<byte>();
            List<byte> btTmp = new List<byte>();

            List<byte> btSecondTbl = new List<byte>();

            for (int i = 0; i < iScount; i++) {
                btSecondTbl.Add(btArchive[i + 6]);      // 6 bytes - see above
            }

            int iNext = 0;
            foreach (byte b in btSecondTbl) {
                int ib = b;

                if ((ib & 128) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 64) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 32) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 16) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 8) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 4) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 2) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
                if ((ib & 1) > 0) { btTmp.Add(btArchive[6 + iScount + iNext]); iNext++; } else { btTmp.Add(0); }
            }

            for (int i = 6 + iScount + iNext; i < btArchive.Count; i++)  // 6 - bytes see abive - size
            {
                btImage.Add(btArchive[i]);
            }

            List<byte> btOut = new List<byte>();

            iNext = 0;

            for (int i = 0; i < btTmp.Count; i++) {
                int ib = btTmp[i];

                if ((ib & 128) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 64) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 32) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 16) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 8) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 4) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 2) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
                if ((ib & 1) > 0) { btOut.Add(btImage[iNext]); iNext++; } else { btOut.Add(0); }
            }

            sbyte[,] sOut = new sbyte[iX, iY];

            iNext = 0;
            for (int i = 0; i < iX; i++) {
                for (int j = 0; j < iY; j++) {
                    sOut[i, j] = (sbyte)btOut[iNext];
                    iNext++;
                }
            }

            return sOut;
        }
    }
}