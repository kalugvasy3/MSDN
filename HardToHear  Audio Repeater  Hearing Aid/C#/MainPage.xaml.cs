
using System;
using System.IO;
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Windows.System.Display;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Phone.BackgroundAudio;

namespace HardToHear
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Microphone microphone = Microphone.Default;     // Object representing the physical microphone on the device
        private byte[] buffer;                                  // Dynamic buffer to retrieve audio data from the microphone
        private byte[] bufferPlay;
       // private MemoryStream stream = new MemoryStream();       // Stores the audio data for later playback
        private SoundEffectInstance soundInstance;              // Used to play back audio
        private bool soundIsStarted = false;                    // Flag to monitor the state of sound playback
        private int intBufferDuration;                          // keep BufferDuration length.
        private DisplayRequest drDisplayRequest;

        /// <summary>
        /// Constructor 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Timer to simulate the XNA Framework game loop (Microphone is from the XNA Framework)..

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33); // 1/0.033  - at 30fps, which is the standard rendering rate.
            dt.Tick += new EventHandler(XNA_Dispetcher);
            dt.Start();

            // Event handler for getting audio data when the buffer is full
            microphone.BufferReady += new EventHandler<EventArgs>(microphone_BufferReady);
        }

        void XNA_Dispetcher(object sender, EventArgs e)
        {
                FrameworkDispatcher.Update(); // Updates the XNA FrameworkDispatcher ...
        }


        void microphone_BufferReady(object sender, EventArgs e) // The Microphone.BufferReady event handler.Gets the audio data from the microphone and stores it in a buffer,then writes that buffer to a stream for later playback.
        {
            // Retrieve audio data
            microphone.GetData(buffer);

            if (chk2x.IsChecked == true) 
            { 
            for (int i = 0; i < intBufferDuration; i++) { bufferPlay[i] = (byte)(buffer[i] << 1); }
            }
            else 
            { 
            bufferPlay = (byte[])buffer.Clone();
            }

            Thread soundThread = new Thread(new ThreadStart(playSound));
            soundThread.Start();

        }

        private void playSound()
        {
            // Play audio using SoundEffectInstance 

            SoundEffect sound = new SoundEffect(bufferPlay, microphone.SampleRate, AudioChannels.Mono);

            soundInstance = sound.CreateInstance();
            soundInstance.Play();
        }

        private void btnStartStop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (microphone.State == MicrophoneState.Started)
            {
                microphone.Stop();
            }


            if (soundIsStarted == false)
            {
                //prevent go to sleep
                drDisplayRequest = new DisplayRequest();
                drDisplayRequest.RequestActive();

                soundIsStarted = true;

                btnStartStop.Content = "STOP";
                btnStartStop.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)255, (byte)0, (byte)0));

                // Get audio data in 200ms chunks - optimum
                microphone.BufferDuration = TimeSpan.FromMilliseconds(200);

                // Allocate memory to hold the audio data
                intBufferDuration = microphone.GetSampleSizeInBytes(microphone.BufferDuration);
                buffer = new byte[intBufferDuration];
                bufferPlay = new byte[intBufferDuration];

                microphone.Start();// Start recording

            }
            else
            {
                stopSound();
            };
        }


        private void stopSound()
        {
            btnStartStop.Content = "START";
            btnStartStop.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)255, (byte)0, (byte)255, (byte)0));

            soundIsStarted = false;
            drDisplayRequest.RequestRelease();

            microphone.Stop();
            soundInstance.Stop();
        }


        private void mainPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try { microphone.Stop(); }
            catch { };

            try { soundInstance.Stop(); }
            catch { };

            try { drDisplayRequest.RequestRelease(); }
            catch { };

        }


    }
}
