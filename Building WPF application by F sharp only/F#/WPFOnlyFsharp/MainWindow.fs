open System
open System.Windows
open System.Windows.Controls
open WPFOnlyFsharp.Utilities

// Very simple WPF ...
//let a = Application()
//let w = Window()
//
//[<STAThread>]
//do a.Run(w) |> ignore


let runWPFOnlyFsharp() =
    let resource = new Uri("/WPFOnlyFsharp;component/MainWindow.fs.xaml",System.UriKind.Relative)
    let runWindow = Application.LoadComponent(resource) :?> Window   // Cast to Window 
    let txtHello : TextBlock = runWindow?txtHello   // Find txtHello name in runWindow and Cast to TextBlock
    
    let btnHello : Button = runWindow?btnHello      // Find btnHelp name in runWindow and cast to Button
    do btnHello.Click.Add(fun _ -> do if txtHello.Text = "***" then txtHello.Text <- "Hello!" else txtHello.Text <- "***" )

    runWindow   // Where "runWindow" is return value for function "runWPFOnlyFsharp()"

[<STAThread>]
(new Application()).Run(runWPFOnlyFsharp()) |> ignore



