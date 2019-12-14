using System;
using System.IO;
using Windows.Graphics.Display;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.Threading;


namespace UniversalAudioRepeater
{
    /// <summary>
    /// Universal Audio Repeater ... 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            textBlock.Visibility = Visibility.Collapsed;
        }


        private Windows.Media.Capture.MediaCapture mediaCaptureAudioPrimery;
        private MediaEncodingProfile recordProfile = null;
        private MediaElement playbackElementPrimery = new MediaElement();
        private bool blnStart = false;

        private MemoryStream msIRAS0 = new MemoryStream();
        private MemoryStream msIRAS1 = new MemoryStream();
        private IRandomAccessStream streamIRAS0;
        private IRandomAccessStream streamIRAS1;


        // You can play with this Delay ...
        private int intDelay = 200;  // Record Time in millesecond...
        // Short time - increase repetet noise ...
        // Long time - create echo ...
        // I think 200ms is optimum ...

        // If you use headphones - best time will be 500..1000ms ... 


        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {

            blnStart = true;
            btnStart.Visibility = Visibility.Collapsed;
            btnStop.Visibility = Visibility.Visible;

            textBlock.Visibility = Visibility.Visible;

            mediaCaptureAudioPrimery = new Windows.Media.Capture.MediaCapture();

            var settings = new Windows.Media.Capture.MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = Windows.Media.Capture.StreamingCaptureMode.Audio;
            settings.MediaCategory = Windows.Media.Capture.MediaCategory.Other;
            settings.AudioProcessing = Windows.Media.AudioProcessing.Default; // Use only Default

            await mediaCaptureAudioPrimery.InitializeAsync(settings);
            recordProfile = MediaEncodingProfile.CreateWav(Windows.Media.MediaProperties.AudioEncodingQuality.Low);


            while (blnStart)  // Repeate untile stop ...
            {
                try
                {
                    msIRAS0 = new MemoryStream();                       
                    streamIRAS0 = msIRAS0.AsRandomAccessStream();       // New Stream ...
                    await mediaCaptureAudioPrimery.StartRecordToStreamAsync(recordProfile, streamIRAS0); // write audio in first stream ...
                    await Task.Delay(intDelay);
                    await mediaCaptureAudioPrimery.StopRecordAsync();   // Stop first stream
                    await PlayThreadMethod(streamIRAS0);                // Play from first stream

                    msIRAS1 = new MemoryStream();
                    streamIRAS1 = msIRAS0.AsRandomAccessStream();       // Second Stream ...
                    await mediaCaptureAudioPrimery.StartRecordToStreamAsync(recordProfile, streamIRAS1);  // sweetch stream ... to second stream ...
                    await Task.Delay(intDelay);
                    await mediaCaptureAudioPrimery.StopRecordAsync();
                    await PlayThreadMethod(streamIRAS1);                // Play Second Streem

                }
                catch (Exception ex)
                {
                    Stop();
                }
            }
            
        }

        private void Stop()
        {
            blnStart = false;
            btnStart.Visibility = Visibility.Visible;
            btnStop.Visibility = Visibility.Collapsed;
            textBlock.Visibility = Visibility.Collapsed;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }


        // Playback Stream ...

        private async Task PlayThreadMethod(object o)
        {
            var cts = new CancellationTokenSource();
            IRandomAccessStream IRAS = (IRandomAccessStream)o;

            var tStart = await Task.Factory.StartNew(() =>
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        playbackElementPrimery.AutoPlay = true;
                        IRAS.Seek(0);
                        playbackElementPrimery.SetSource(IRAS, "Wav");
                        playbackElementPrimery.Play();
                    }), cts.Token);
        }


    }
}
