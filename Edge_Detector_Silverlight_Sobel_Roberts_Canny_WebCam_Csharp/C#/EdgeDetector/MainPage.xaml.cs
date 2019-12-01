using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Threading;

using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

using TransformMethods;

namespace EdgeDetector
{
    public partial class MainPage : UserControl
    {

        private float _deltaImgSecond = 0.125f;  // the period selection of images

        private int _intPackImage = 1;          // number of images in the collection
        private int _countImage = 0;            // counter collections
        private ObservableCollection<WriteableBitmap> _images = new ObservableCollection<WriteableBitmap>();  // collection for images

        private DispatcherTimer _timerImg;      // Timer
        private CaptureSource _captureSource = null;

        private bool blnStart = false;         // WebCam - was Started

        private WriteableBitmap imgOrigin;     // original image 

        private WriteableBitmap imgSelected;   // image  

        private WriteableBitmap imgEdge;       // image  

        private double dX = 64;
        private double dY = 64;

        private WriteableBitmap img256;
        private int intS256 = 128;


        private string strColor = "";




        public MainPage()
        {
            InitializeComponent();

            _captureSource = new CaptureSource();
            _captureSource.CaptureImageCompleted += (s, args) =>
            {
                _images.Add(args.Result);
            };
            _timerImg = new DispatcherTimer();
            _timerImg.Interval = TimeSpan.FromSeconds(_deltaImgSecond);
            _timerImg.Tick += new EventHandler(_timerImg_Tick);

            MouseRightButtonDown += (sender, e) => { e.Handled = true; };     // not fulfill the default event
            //    MouseRightButtonUp += (sender, e) => { e.Handled = true; };

            imgOrigin = new WriteableBitmap(256, 256);
            imgSelected = new WriteableBitmap(128, 128);

        }

        private ImgManipulation objManip = new ImgManipulation();
        private Edge objEdge = new Edge();
        private WriteableBitmap img;

        void _timerImg_Tick(object sender, EventArgs e)
        {
            if (_images.Count == _countImage + 1)
            {
                //VIDEO----------------------------------------------------------------------------------------------------------------------

                _timerImg.Stop();
                               
                //WriteableBitmap img = new WriteableBitmap(objManip.convertBitmapToGray(_images[_countImage], strColor,out histTotal));
                img = new WriteableBitmap(objManip.convertImgAnySizeTo256x256(_images[_countImage], 255, strColor)); //"RGB"  "R" "G" "B" " "

                run();

            }


            //START NEW CICKLE----------------------------------------------------------------------------------------------------------------------
            _countImage += 1;
            if (_countImage == _intPackImage)
            {
                _countImage = 0;
                _images.Clear();
            }

            if (_captureSource.State == System.Windows.Media.CaptureState.Started)
            {
                _captureSource.CaptureImageAsync();    // Start Image Async...
                _timerImg.Start();                     // Start Timer
            }
            //START NEW CICKLE----------------------------------------------------------------------------------------------------------------------

        }

        private void run()
        {
            WebCamDefault.Fill = new ImageBrush()    // display the main image
            {
                ImageSource = objManip.addRec(WebCamDefault, dX, dY, intS256, img, out img256)
            };

            rec256.Fill = new ImageBrush()
            {
                ImageSource = img256
            };
            //VIDEO----------------------------------------------------------------------------------------------------------------------


            if (rbNone.IsChecked == true)
            {
                imgEdge = img256;
            }

            if (rbSobel.IsChecked == true)
            {
                imgEdge = objEdge.convertIntToGrayImg(objEdge.filterSobel(objEdge.convertImgToInt(img256)));
            }

            if (rbRoberts.IsChecked == true)
            {
                imgEdge = objEdge.convertIntToGrayImg(objEdge.filterRoberts(objEdge.convertImgToInt(img256)));
            }

            if (rbCanny.IsChecked == true)
            {
                imgEdge = objEdge.imgCannyEdge(img256, 3, 1.4); //  Gauss Filter order - 3 , sigma usually 1.4
            }

            if (chkContrast.IsChecked == true)
            {
                imgEdge = objEdge.convertIntToGrayImg(objManip.contrast(objManip.convertBitmapToIntArrayTwoDimARGB(imgEdge)));
            };

            recEdge.Fill = new ImageBrush()
            {
                ImageSource = imgEdge
            };

        }



        private void WebCamDefault_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

                //  isBlock = true;
                Point pCapture = e.GetPosition(WebCamDefault); 
                dX = (int)pCapture.X;
                dY = (int)pCapture.Y;
        
            run();

        }



        private void WebCamDefault_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                intS256 = intS256 * 2;
                if (intS256 >= 256) intS256 = 256;
            }
            else
            {
                intS256 = intS256 / 2;
                if (intS256 <= 1) intS256 = 1;
            }

            run();
        }

        private void btnWebCamStart_Click(object sender, RoutedEventArgs e)
        {
            if (_captureSource != null & blnStart == false)
            {
                try
                {
                    _captureSource.Stop(); // stop whatever device may be capturing
                    _captureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                    _captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
                }
                catch
                {
                    _captureSource = null;
                }
                try
                {
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                    {
                        _captureSource.Start();
                        if (_captureSource.State == System.Windows.Media.CaptureState.Started)
                        {
                            // capture the current frame and add it to our observable collection                
                            _images.Clear();
                            _captureSource.CaptureImageAsync();
                            _timerImg.Start();
                            blnStart = true;

                            btnWebCamStart.Content = "WebCam STOP";
                        }
                        return;
                    }
                }
                catch
                {
                }
            }
            else
            {
                _captureSource.Stop();
                blnStart = false;
                btnWebCamStart.Content = "WebCam START";
            }
        }

        private void WebCamDefault_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _timerImg.Stop();
                _captureSource.Stop();
                _captureSource = null;
            }
            catch
            {
            }
        }

        private void WebCamDefault_Drop(object sender, DragEventArgs e)
        {
            TransformMethods.DropFile objDrop = new DropFile();

            System.IO.FileInfo[] fi = (System.IO.FileInfo[])e.Data.GetData(DataFormats.FileDrop);

            img = objDrop.CreateImage(fi[0]);
            img = new WriteableBitmap(objManip.convertImgAnySizeTo256x256(img, 255, strColor)); //"RGB"  "R" "G" "B" " "

            run();


        }

        private void chkContrast_Checked(object sender, RoutedEventArgs e)
        {
            if (img != null) run();
        }

        private void rbNone_Checked(object sender, RoutedEventArgs e)
        {
            if (img != null) run();
        }

        private void rbSobel_Checked(object sender, RoutedEventArgs e)
        {
            if (img != null) run();
        }

        private void rbRoberts_Checked(object sender, RoutedEventArgs e)
        {
            if (img != null) run();
        }

        private void rbCanny_Checked(object sender, RoutedEventArgs e)
        {
            if (img != null) run();
        }
    }
}
