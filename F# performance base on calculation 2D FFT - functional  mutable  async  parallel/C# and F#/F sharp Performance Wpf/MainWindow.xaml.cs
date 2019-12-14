using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Diagnostics;


using FsharpLibrary;
using System.Threading.Tasks;

namespace F_sharp_Performance_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String path = AppDomain.CurrentDomain.BaseDirectory + "Square.png"; //   Capture512x512.png
        private Bitmap bmp;
        private FFTfsharp fs = new FFTfsharp();
        public MainWindow()
        {
            InitializeComponent();
            LoadSquare();
        }

        private void LoadSquare()
        {
            bmp = (Bitmap)Bitmap.FromFile(path);
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(bmp));
        }

        private void btnOriginalImage_Click(object sender, RoutedEventArgs e)
        {
            LoadSquare();
        }

        private void btnGirlImage_Click(object sender, RoutedEventArgs e)
        {
            bmp = (Bitmap)Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Capture512x512.png");
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(bmp));
        }
        private void btnFFTfsharp_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2FuncAsync(bmp)));
            sw.Stop();
            lblFFTfsharpAsync.Content = sw.ElapsedMilliseconds.ToString() + " ms.     FFT functional Async -> " + fs.FFTtime.ToString() + " ms." ;
        }

        private void btnFFTfsharpParalllel_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2FuncParallel(bmp)));
            sw.Stop();
            lblFFTfsParallel.Content = sw.ElapsedMilliseconds.ToString() + " ms.     FFT functional Parallel -> " + fs.FFTtime.ToString() + " ms.";
        }

        private void btnFFTfsharp_async_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2Async(bmp)));
            sw.Stop();
            lblFFTfsharp_mutable.Content = sw.ElapsedMilliseconds.ToString() + " ms.     FFT async.parallel -> " + fs.FFTtime.ToString() + " ms." ;
        }

        private void btnFFTfsharp_parallel_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2Parallel(bmp)));
            sw.Stop();
            lblFFTfParallel.Content = sw.ElapsedMilliseconds.ToString() + " ms.     FFT parallel.for -> " + fs.FFTtime.ToString() + " ms.";
        }

        private void btnFFTmathParalllel_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2MathParallel(bmp)));
            sw.Stop();
            lblFFTmathParallel.Content = sw.ElapsedMilliseconds.ToString() + " ms.    FFT Math Parallel -> " + fs.FFTtime.ToString() + " ms.";
        }

        private void btnFFTmathAsync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            grdAll.Background = new ImageBrush(fs.ImgBitmapImage(fs.ToFFT2MathAsync(bmp)));
            sw.Stop();
            lblFFTmathasync.Content = sw.ElapsedMilliseconds.ToString() + " ms.    FFT Math Async -> " + fs.FFTtime.ToString() + " ms.";
        }


    }
}
