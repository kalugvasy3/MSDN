module App

open System

open System.Windows
open WCFhostByWpfFsharpOnly

type App() as this =
    inherit Window()
    let mutable wpfMouse = new WpfMouse()
    do wpfMouse.HorizontalAlignment <- HorizontalAlignment.Stretch
    do wpfMouse.VerticalAlignment <- VerticalAlignment.Stretch
    do this.Content <- wpfMouse

    // Add here any logic for Window() ...
    // Load...Start...Exit...Exception...etc..

    do this.SizeToContent <- SizeToContent.WidthAndHeight
    do this.Title <- "Service"

    // Comment out if needed below one line - JUST FOR TEST
    //do this.KeyDown.Add(fun e -> wpfMouse.UserKeyDown(e))   // for test MOUSE by keys - see also WpfMouse.xaml.fs

[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(new App()) |> ignore