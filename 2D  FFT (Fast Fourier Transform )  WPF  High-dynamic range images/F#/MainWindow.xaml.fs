
module MainWindow

open System 
open System.Windows 
open System.IO
open System.Windows.Markup
open System.Windows.Controls 
open System.Windows.Media

open Utilities

open Image

open System.Reflection
open System.Diagnostics
open FFT2

let mySr = new StreamReader(Assembly.Load("FFT2").GetManifestResourceStream("MainWindow.xaml"))   // XAML - MUST be Embedded Resource 
let mutable this : Window = XamlReader.Load(mySr.BaseStream):?> Window  // Load XAML

let mutable gridAll : Grid = this?gridAll 
let mutable btnGray : Button = this?btnGray 
let mutable btnFFTf : Button = this?btnFFTf 
let mutable btnFFTb : Button = this?btnFFTb 

let mutable btnSwap : Button = this?btnSwap
let mutable btnShift : Button = this?btnShift

let mutable lblNote : Label = this?lblNote 

let mutable blnForward = false
let img = new Img()
do img.LoadPath(AppDomain.CurrentDomain.BaseDirectory + "Square.png") |> ignore // Load image 512x512   Square.png
do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)


let gray () =
    let sW = new Stopwatch()
    sW.Start()
    do img.ToGrayScaleImage() |> ignore
    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)
    sW.Stop()
    lblNote.Content <- sW.ElapsedMilliseconds.ToString() + " ms"
    ignore()

let fft2f ()= 
    let sW = new Stopwatch()
    sW.Start()
    do img.ToFFT2forward() |> ignore
    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)
    sW.Stop()
    lblNote.Content <- "Total Time = " + sW.ElapsedMilliseconds.ToString() + " ms. Forward FFT2 = " + FFT2.fft2Time.ToString() + " ms." 
    btnFFTf.IsEnabled <- false
    btnFFTb.IsEnabled <- true
    btnSwap.IsEnabled <- true
    btnShift.IsEnabled <- true
    blnForward <- true
    ignore()

let fft2b ()= 
    let sW = new Stopwatch()
    sW.Start()
    do img.ToFFT2backward() |> ignore
    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)
    sW.Stop()
    lblNote.Content <- "Total Time = " + sW.ElapsedMilliseconds.ToString() + " ms. Backward FFT2 = " + FFT2.fft2Time.ToString() + " ms." 
    btnFFTf.IsEnabled <- true
    btnFFTb.IsEnabled <- false
    btnSwap.IsEnabled <- false
    btnShift.IsEnabled <- false
    blnForward <- false
    ignore()

let SwapImage() =  
    do img.SwapImage() |> ignore
    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)                
    if btnShift.IsEnabled  then btnShift.IsEnabled <- false
                           else btnShift.IsEnabled <- true 

let ScaleLog(e : Input.MouseWheelEventArgs) = 
    if blnForward then
        match e.Delta < 0 with
        | false -> let mutable x = img.ScaleLog / 2.0 
                   img.ScaleLog <- x 
        | true  -> let mutable x = img.ScaleLog * 2.0
                   if x <= 1.0 then img.ScaleLog <- x 

        btnSwap.IsEnabled <- true
        btnShift.IsEnabled <- true


    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage) 

let ShiftImage() = 
    do img.ShiftImage() |> ignore
    do gridAll.Background <- new ImageBrush(img.ImgWrapperBitmapImage)
    if btnSwap.IsEnabled  then btnSwap.IsEnabled <- false
                          else btnSwap.IsEnabled <- true 

do btnGray.Click.Add(fun _ -> gray() 
                              btnGray.IsEnabled <- false
                              btnFFTf.IsEnabled <- true)

do btnFFTf.Click.Add(fun _ -> fft2f()
                              btnGray.IsEnabled <- false)

do btnFFTb.Click.Add(fun _ -> fft2b())

do btnSwap.Click.Add(fun _ -> SwapImage())
do btnShift.Click.Add(fun _ -> ShiftImage())

do gridAll.MouseWheel.Add(fun e -> ScaleLog(e))

[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(this) |> ignore


 

