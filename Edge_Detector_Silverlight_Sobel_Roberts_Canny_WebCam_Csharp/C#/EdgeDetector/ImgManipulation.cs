using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TransformMethods
{
    public class ImgManipulation
    {
        /// <summary>
        /// Коректировка цвета, коэффициенты между 0...1
        /// </summary>
        /// <param name="img"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public WriteableBitmap adjustColor(WriteableBitmap img, double red, double green, double blue, double alpha) {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            WriteableBitmap imgOut = new WriteableBitmap(X, Y);     // X-first for  WriteableBitmap

            for (int i = 0; i < X * Y; i++) {
                int r = (int)((double)((img.Pixels[i] & (255 << 16)) >> 16) * red * alpha);
                int g = (int)((double)((img.Pixels[i] & (255 << 8)) >> 8) * green * alpha);
                int b = (int)((double)((img.Pixels[i] & (255 << 0)) >> 0) * blue * alpha);

                imgOut.Pixels[i] = r << 16 | g << 8 | b << 0 | (int)(255.0d * alpha) << 24;
            }

            return imgOut;
        }

        /// <summary>
        /// Строит из цветного изображения Серое Изображение ...
        /// </summary>
        /// <param name="imgT"></param>
        /// <returns></returns>
        public WriteableBitmap convertBitmapToGray(WriteableBitmap imgT, string color, out int[] histogram)      // преобразовать изображение
        {
            int X = imgT.PixelWidth;
            int Y = imgT.PixelHeight;
            histogram = new int[256];

            for (int i = 0; i < X * Y; i++) {
                int colorR = (imgT.Pixels[i] & (255 << 16)) >> 16;     //R
                int colorG = (imgT.Pixels[i] & (255 << 8)) >> 8;       //G
                int colorB = imgT.Pixels[i] & (255 << 0);             //B

                int avr = (int)(colorR * 0.299 + colorG * 0.587 + colorB * 0.114);

                if (color == "R") {
                    avr = colorR;
                }

                if (color == "G") {
                    avr = colorG;
                }

                if (color == "B") {
                    avr = colorB;
                }

                histogram[avr & 255] += 1;
                imgT.Pixels[i] = (avr << 16 | avr << 8 | avr << 0 | (255 << 24));
            }
            return imgT;
        }

        /// <summary>
        /// Функция аналогична следующей функции. Просто возвращает квадратное иображение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <param name="intS"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public WriteableBitmap cutRec(object sender, double dX, double dY, int intS, WriteableBitmap img) {
            WriteableBitmap imgTmp = new WriteableBitmap(intS, intS);

            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            Rectangle rS = sender as Rectangle;

            double dsX = X / rS.ActualWidth;   //коэффициет сжатие - растяжение по X
            double dsY = Y / rS.ActualHeight;  //коэффициет сжатие - растяжение по Y

            int iX = (int)(dX * dsX);
            int jY = (int)(dY * dsY);

            for (int i = iX; (i < iX + intS) && (i < X); i++) {
                for (int j = jY; (j < jY + intS) && (j < Y); j++) {
                    imgTmp.Pixels[(j - jY) * intS + (i - iX)] = img.Pixels[j * X + i];
                }
            }

            return imgTmp;
        }

        /// <summary>
        /// Функция возврашает imgRec + квадрат на imgRec
        /// sender - прямоугольник, x/y координаты квадрата (обычно координаты мышки)
        /// intSquare - размер квадрата в pixel (может быть больше размера изображения)
        /// imgSelected - возвращает вырезанный квадрат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="intSquare"></param>
        /// <param name="imgRec"></param>
        /// <param name="imgSelected"></param>
        /// <returns></returns>
        public WriteableBitmap addRec(object sender, double x, double y, int intSquare, WriteableBitmap imgRec, out WriteableBitmap imgSelected) {
            WriteableBitmap imgTmp = new WriteableBitmap(imgRec);

            imgSelected = new WriteableBitmap(intSquare, intSquare);

            int X = imgTmp.PixelWidth;
            int Y = imgTmp.PixelHeight;

            Rectangle rS = sender as Rectangle;

            double dsX = X / rS.ActualWidth;   //коэффициет сжатие - растяжение по X
            double dsY = Y / rS.ActualHeight;  //коэффициет сжатие - растяжение по Y

            int ixBitmap = (int)(x * dsX);
            int jyBitmap = (int)(y * dsY);

            int i = ixBitmap;
            int j = jyBitmap;

            for (i = ixBitmap; (i < ixBitmap + intSquare) && (i < X); i++) {
                for (j = jyBitmap; (j < jyBitmap + intSquare) && (j < Y); j++) {
                    imgSelected.Pixels[(j - jyBitmap) * intSquare + (i - ixBitmap)] = imgTmp.Pixels[j * X + i];          // вырезанное изображение
                    imgTmp.Pixels[j * X + i] = imgTmp.Pixels[j * X + i] & (224 << 24 | 255 << 16 | 255 << 8 | 255 << 0); // добавить 25% прозрачности
                    int intTmp = imgTmp.Pixels[j * X + i] & 255;
                }
            }

            return imgTmp;
        }

        /// <summary>
        /// Уменьшает любое изображение до (урежит до 128 в соответствии с коэффициентом) x 128
        /// Если нужно конвертировать в серый цвет ...
        /// </summary>
        /// <param name="img"></param>
        /// <param name="blnGray"></param>
        /// <returns></returns>
        public WriteableBitmap convertImgAnySizeTo256x256(WriteableBitmap img, byte alpha, string color) {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            double dky = Y / 256.0;
            WriteableBitmap imgOut = new WriteableBitmap(256, 256);

            double diStart = (X / 2.0 - 128.0 * dky);
            double diStop = (X / 2.0 + 128.0 * dky);

            int inew = 0;
            int jnew = 0;

            double dAlpha = alpha / 255.0;
            int pixel;

            for (double dj = 0; dj < Y - dky; dj += dky)  // do not need to cut Y axel
            {
                for (double di = diStart; di < diStop; di += dky)  // cut X axel
                {
                    pixel = img.Pixels[(int)(dj + 0.5) * X + (int)(di + 0.5)];

                    int iAvr;

                    int iRed = (int)(((pixel & (255 << 16)) >> 16) * dAlpha);
                    int iGreen = (int)(((pixel & (255 << 8)) >> 8) * dAlpha);
                    int iBlue = (int)(((pixel & (255 << 0)) >> 0) * dAlpha);

                    switch (color) {
                        case "R":
                        imgOut.Pixels[256 * jnew + inew] = alpha << 24 | iRed << 16;
                        break;
                        case "G":
                        imgOut.Pixels[256 * jnew + inew] = alpha << 24 | iGreen << 8;
                        break;
                        case "B":
                        imgOut.Pixels[256 * jnew + inew] = alpha << 24 | iBlue << 0;
                        break;
                        case "RGB":
                        imgOut.Pixels[256 * jnew + inew] = alpha << 24 | iRed << 16 | iGreen << 8 | iBlue << 0;
                        break;
                        default:
                        iAvr = (int)(iRed * 0.299 + iGreen * 0.587 + iBlue * 0.114);
                        imgOut.Pixels[256 * jnew + inew] = alpha << 24 | iAvr << 16 | iAvr << 8 | iAvr << 0;
                        break;
                    }

                    inew++;
                }
                inew = 0;
                jnew++;
            }

            return imgOut;
        }


        public WriteableBitmap convertImgAnySizeTo128x128(WriteableBitmap img, byte alpha, string color)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            double dky = Y / 128.0;
            int intDky = (int)dky;
            
            WriteableBitmap imgOut = new WriteableBitmap(128, 128);

            double diStart = (X / 2.0 - 64.0 * dky);
            double diStop = (X / 2.0 + 64.0 * dky);

            int inew = 0;
            int jnew = 0;

            double dAlpha = alpha / 255.0;
            
            int pixel;
            int iAvr;

            for (double dj = 0; dj < Y - dky; dj += dky)  // do not need to cut Y axel
            {
                for (double di = diStart; di < diStop; di += dky)  // cut X axel
                {
                    int intAvr = 0;
                    int intSum = 0;

                    for (int i = 0; i < intDky; i++) //intDky
                    {
                        for (int j = 0; j < intDky; j++) //intDky
                        {
                            if ((int)(dj + j) < Y && (int)(di + i) < X)
                            {
                                pixel = img.Pixels[(int)(dj + j) * X + (int)(di + i)];

                                int iRed = (int)(((pixel & (255 << 16)) >> 16) * dAlpha);
                                int iGreen = (int)(((pixel & (255 << 8)) >> 8) * dAlpha);
                                int iBlue = (int)(((pixel & (255 << 0)) >> 0) * dAlpha);
                                intAvr += (int)(iRed * 0.299 + iGreen * 0.587 + iBlue * 0.114);
                               
                                intSum++;
                            }
                        }

                    }

                    iAvr = intAvr/ intSum;

                    imgOut.Pixels[128 * jnew + inew] = alpha << 24 | iAvr << 16 | iAvr << 8 | iAvr << 0;
 
                    inew++;
                }
                inew = 0;
                jnew++;
            }

            return imgOut;
        }


        public WriteableBitmap convertColorImgAnySizeTo128x128(WriteableBitmap img)
        {
            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            double dky = Y / 128.0;
            int intDky = (int)dky;

            WriteableBitmap imgOut = new WriteableBitmap(128, 128);

            double diStart = (X / 2.0 - 64.0 * dky);
            double diStop = (X / 2.0 + 64.0 * dky);

            int inew = 0;
            int jnew = 0;

            int pixel;

            for (double dj = 0; dj < Y - dky; dj += dky)  // do not need to cut Y axel
            {
                for (double di = diStart; di < diStop; di += dky)  // cut X axel
                {
                    int intRed = 0;
                    int intGreen = 0;
                    int intBlue = 0;

                    int intSum = 0;

                    for (int i = 0; i < intDky; i++) //intDky
                    {
                        for (int j = 0; j < intDky; j++) //intDky
                        {
                            if ((int)(dj + j) < Y && (int)(di + i) < X)
                            {
                                pixel = img.Pixels[(int)(dj + j) * X + (int)(di + i)];

                                int iRed = (int)(((pixel & (255 << 16)) >> 16) );
                                int iGreen = (int)(((pixel & (255 << 8)) >> 8) );
                                int iBlue = (int)(((pixel & (255 << 0)) >> 0) );

                                intRed += iRed;
                                intGreen += iGreen;
                                intBlue += iBlue;
                                
                                intSum++;
                            }
                        }

                    }

                    intRed = (intRed / intSum) & 255;
                    intGreen = (intGreen / intSum) & 255;
                    intBlue = (intBlue / intSum) & 255;

                    imgOut.Pixels[128 * jnew + inew] = 255 << 24 | intRed << 16 | intGreen << 8 | intBlue << 0;

                    inew++;
                }
                inew = 0;
                jnew++;
            }

            return imgOut;
        }


        // http://sernam.ru/book_kir.php?id=7
        // linear contrasting images
        //
        //           X - Xmin
        //     y = ------------- * (Ymax - Ymin) + Ymin
        //           Xmax - Xmin
        //

        public int[,] contrast(int[,] intColor)
        {

            int X = intColor.GetLength(0);
            int Y = intColor.GetLength(1);

            int[,] intContrast = new int[X, Y];

            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];

            // histogram on the green

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int histR = (intColor[i, j] >> 16) & 255;
                    histogramR[histR] += 1;

                    int histG = (intColor[i, j] >> 8) & 255;
                    histogramG[histG] += 1;

                    int histB = (intColor[i, j] >> 0) & 255;
                    histogramB[histB] += 1;
                }
            }

            int maxHistR = 255;
            int minHistR = 0;

            int maxHistG = 255;
            int minHistG = 0;

            int maxHistB = 255;
            int minHistB = 0;

            int total = X * Y;

            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            double persentL = 0.01;
            double persentH = 0.99;


            for (int i = 0; i < 256; i++)
            {
                sumR += histogramR[i];
                if (sumR <= total * persentL) minHistR = i;
                if (sumR <= total * persentH) maxHistR = i;

                sumG += histogramG[i];
                if (sumG <= total * persentL) minHistG = i;
                if (sumG <= total * persentH) maxHistG = i;

                sumB += histogramB[i];
                if (sumB <= total * persentL) minHistB = i;
                if (sumB <= total * persentH) maxHistB = i;
            }

            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    int r = (intColor[i, j] >> 16) & 255;
                    int g = (intColor[i, j] >> 8) & 255;
                    int b = (intColor[i, j] >> 0) & 255;

                    r = ((r - minHistR) * 220) / (maxHistR - minHistR + 1) + 20;
                    g = ((g - minHistG) * 220) / (maxHistG - minHistG + 1) + 20;
                    b = ((b - minHistB) * 220) / (maxHistB - minHistB + 1) + 20;

                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;

                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;

                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;

                    intContrast[i, j] = 255 << 24 | r << 16 | g << 8 | b;
                }
            }


            return intContrast;
        }

        
        // WriteableBitmap ARGB color to IntArray (alpha | R | G | B)
        public int[,] convertBitmapToIntArrayTwoDimARGB(WriteableBitmap img)
        {

            int X = img.PixelWidth;
            int Y = img.PixelHeight;

            int[,] intArr = new int[Y, X];

            for (int i = 0; i < X * Y; i++)
            {
                int x = i % X;
                int y = i / X;
                intArr[x, y] = img.Pixels[i];
            }
            return intArr;
        }


    }
}