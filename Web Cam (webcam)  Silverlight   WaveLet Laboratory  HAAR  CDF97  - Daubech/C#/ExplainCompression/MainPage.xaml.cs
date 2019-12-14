using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TransformMethods;

namespace ExplainCompression
{
    public partial class MainPage : UserControl
    {
        //----------------------MAIN PROG----------------------------------------------------------------------------------------------------------------------

        private float _deltaImgSec = 0.25f;     // period
        private int _countImage = 0;            // counter
        private int _countRImage = 0;

        private int _intPackImage = 4;          // number of images in collection

        private byte bLoss = 1;     // if differences between pixel less then - replace to 0   // try to use 0, 1, 2 ... 8
        private int vHaar = 128;     // vertical      it should be 2**n
        private int hHaar = 64;     // horizontal    it should be  2**n   vertical and horizontal not necessary should be same ...

        private CaptureSource _captureSource = null;

        private ObservableCollection<WriteableBitmap> _imagesSend = new ObservableCollection<WriteableBitmap>();  //  send collection
        private ObservableCollection<WriteableBitmap> _imagesReceive = new ObservableCollection<WriteableBitmap>();  // receive collection

        private ObservableCollection<WriteableBitmap> _images = new ObservableCollection<WriteableBitmap>();  // tmp  collection
        private WriteableBitmap imgOut1024x256;
        private WriteableBitmap imgReceive1024x256;
        private WriteableBitmap imgRecieveHaar;

        private DispatcherTimer _timerSend;      // Timer
        private DispatcherTimer _timerReceive;   // Timer

        private bool blnStart = false;
        private bool blnStarReceive = false;

        public List<byte> outByte;

        DateTime dtG = DateTime.Now;

        public bool flagReady = false;

        public MainPage() {
            InitializeComponent();

            _captureSource = new CaptureSource();
            _captureSource.CaptureImageCompleted += (s, args) => {
                _images.Add(args.Result);
            };

            _timerSend = new DispatcherTimer();
            _timerSend.Interval = TimeSpan.FromSeconds(_deltaImgSec);
            _timerSend.Tick += new EventHandler(_timerSend_Tick);

            _timerReceive = new DispatcherTimer();
            _timerReceive.Interval = TimeSpan.FromSeconds(_deltaImgSec);
            _timerReceive.Tick += new EventHandler(_timerReceive_Tick);

            MouseRightButtonDown += (sender, e) => { e.Handled = true; };     // do not run event by default
            MouseRightButtonUp += (sender, e) => { e.Handled = true; };
        }

        private void _timerSend_Tick(object sender, EventArgs e) {
            if (_images.Count == 1) {
                //    _timerSend.Stop();

                TransformMethods.TransferTo objT = new TransferTo();

                _imagesSend.Add(objT.convertImgAnySizeTo256x256(_images[0]));

                rSend.Fill = new ImageBrush() {
                    ImageSource = _imagesSend[_countImage]
                };

                switch (_countImage) {
                    case 0:
                    rS0.Fill = new ImageBrush() {
                        ImageSource = _imagesSend[_countImage]
                    };
                    break;
                    case 1:
                    rS1.Fill = new ImageBrush() {
                        ImageSource = _imagesSend[_countImage]
                    };
                    break;
                    case 2:
                    rS2.Fill = new ImageBrush() {
                        ImageSource = _imagesSend[_countImage]
                    };

                    break;
                    case 3:
                    rS3.Fill = new ImageBrush() {
                        ImageSource = _imagesSend[_countImage]
                    };

                    break;
                    default: ;
                    break;
                }
                _countImage += 1;
                _images.Clear();
            }

            TransformMethods.WaveLetHaar objH = new WaveLetHaar();
            TransformMethods.WaveLetCDF97 objD = new WaveLetCDF97();

            //START NEW CICKLE----------------------------------------------------------------------------------------------------------------------

            if (_countImage == _intPackImage) {
                DateTime dt0 = DateTime.Now;

                TransformMethods.TransferTo objT = new TransferTo();
                imgOut1024x256 = objT.convertGrayImgToGray1024x256(_imagesSend);

                rSendCombine.Fill = new ImageBrush() {
                    ImageSource = imgOut1024x256
                };
                //========================================================================
                sbyte[,] sbHaar;
                sbHaar = objT.convertGrayImgToSByte256(imgOut1024x256);

                if (rbH.IsChecked == true) {
                    objH.Haar2DByteForward(ref sbHaar, bLoss, vHaar, hHaar);
                }
                else {
                    objD.CDF2DForward(ref sbHaar, bLoss, vHaar, hHaar);
                }

                rSendHaar.Fill = new ImageBrush() {
                    ImageSource = objT.convertSByte256ToGrayImg(sbHaar)
                };

                TransformMethods.MyArc objA = new MyArc();
                List<byte> outBt = objA.archiveWithTable(sbHaar);

                lblSize.Content = outBt.Count.ToString() + " bytes ... (" + (256 * 256 * 4).ToString() + ")";

                outByte = new List<byte>(outBt);

                flagReady = true;

                DateTime dt1 = DateTime.Now;
                lblDelta.Content = dt1.Subtract(dt0).Milliseconds.ToString() + " ms";

                _countImage = 0;
                _imagesSend.Clear();
            }

            if (_captureSource.State == System.Windows.Media.CaptureState.Started) {
                _captureSource.CaptureImageAsync();    // Start Image Async...

                //    _timerSend.Start();                     // Start Timer
            }

            //START NEW CICKLE----------------------------------------------------------------------------------------------------------------------
        }

        private void WebCamDefault_Unloaded(object sender, RoutedEventArgs e) {
            try {
                _timerSend.Stop();
                _timerReceive.Stop();

                _captureSource.Stop();
                _captureSource = null;
            }
            catch {
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e) {
            if (_captureSource != null & blnStart == false) {
                try {
                    _captureSource.Stop(); // stop whatever device may be capturing
                    _captureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                    _captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
                }
                catch {
                    _captureSource = null;
                }

                try {
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess()) {
                        _captureSource.Start();
                        if (_captureSource.State == System.Windows.Media.CaptureState.Started) {
                            // capture the current frame and add it to our observable collection
                            _imagesSend.Clear();
                            _countImage = 0;
                            _captureSource.CaptureImageAsync();
                            _timerSend.Start();
                            blnStart = true;

                            btnSend.Content = "STOP Sending";
                        }
                        return;
                    }
                }
                catch {
                }
            }
            else {
                _timerSend.Stop();
                _captureSource.Stop();

                blnStart = false;
                btnSend.Content = "START Sending";
            }
        }

        private void _timerReceive_Tick(object sender, EventArgs e) {
            lblDeltaR.Content = DateTime.Now.Subtract(dtG).Milliseconds.ToString() + " ms";
            dtG = DateTime.Now;

            //    _timerReceive.Stop();

            if (flagReady & _countRImage == 0) {
                TransformMethods.MyArc objA = new MyArc();
                sbyte[,] Z = new sbyte[1024, 256];
                Z = objA.unArcWithTable(outByte);

                TransformMethods.TransferTo objT = new TransferTo();

                imgRecieveHaar = new WriteableBitmap(objT.convertSByte256ToGrayImg(Z));

                rRecieveHaar.Fill = new ImageBrush() {
                    ImageSource = imgRecieveHaar
                };

                TransformMethods.WaveLetHaar objH = new WaveLetHaar();
                TransformMethods.WaveLetCDF97 objD = new WaveLetCDF97();

                if (rbH.IsChecked == true) {
                    objH.Haar2DInverse(ref Z, vHaar, hHaar);
                }
                else {
                    objD.CDF2DInverse(ref Z, vHaar, hHaar);
                }

                imgReceive1024x256 = new WriteableBitmap(objT.convertSByte256ToGrayImgReceive(Z));

                rRecieveCombine.Fill = new ImageBrush() {
                    ImageSource = imgReceive1024x256
                };

                _imagesReceive = objT.convertGray1024x256ToGrayImgCollection(imgReceive1024x256);

                flagReady = false;
            }

            rReceive.Fill = new ImageBrush() {
                ImageSource = _imagesReceive[_countRImage]
            };

            _countRImage += 1;

            if (_countRImage == _intPackImage) {
                _countRImage = 0;
            }

            //     _timerReceive.Start();
        }

        private void btnRecieve_Click(object sender, RoutedEventArgs e) {
            if (blnStarReceive == false) {
                _timerReceive.Start();
                blnStarReceive = true;
                btnRecieve.Content = "STOP Receiving";
                _countRImage = 0;
            }
            else {
                _timerReceive.Stop();
                blnStarReceive = false;
                btnRecieve.Content = "START Receiving";
            }
        }

        private void rReceive_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                scaleHaar.ScaleX = scaleHaar.ScaleX * 1.1;
                scaleHaar.ScaleY = scaleHaar.ScaleY * 1.1;

                if (scaleHaar.ScaleX > 2.0)
                    scaleHaar.ScaleX = 2.0;
                if (scaleHaar.ScaleY > 2.0)
                    scaleHaar.ScaleY = 2.0;
            }
            else {
                scaleHaar.ScaleX = scaleHaar.ScaleX / 1.1;
                scaleHaar.ScaleY = scaleHaar.ScaleY / 1.1;

                if (scaleHaar.ScaleX < 1.0)
                    scaleHaar.ScaleX = 1.0;
                if (scaleHaar.ScaleY < 1.0)
                    scaleHaar.ScaleY = 1.0;
            }
        }
    }
}