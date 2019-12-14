module App

open System

open System.Windows
open WCFhostByWpfFsharpOnly

type App() as this =
    inherit Window()
    let mutable wpfMouse = new WpfMouse()
    do this.Content <- wpfMouse

    // Add here any logic for Window() ...
    // Load...Start...Exit...Exception...etc..

    do this.Width <- 330.0
    do this.Height <- 330.0
    do this.Title <- "WPF Mouse Service !"

   // do this.KeyDown.Add(fun e -> wpfMouse.UserKeyDown(e))   // for test MOUSE by keys - uncomment (see also WpfMouse.xaml.fs)

[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(new App()) |> ignore