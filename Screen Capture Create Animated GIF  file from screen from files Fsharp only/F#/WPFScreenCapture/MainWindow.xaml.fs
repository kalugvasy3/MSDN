
module MainWindow

open System 
open System.Windows 
open System.IO
open System.Windows.Markup
open Utilities
open System.Windows.Controls 
open MyUserControl


open System.Reflection
open System.Windows.Media.Imaging

// init Main Window
let mutable this : Window = new Window()
do this.ResizeMode <- ResizeMode.NoResize
do this.SizeToContent <-SizeToContent.WidthAndHeight 
do this.Title <- "Capture Screen | Create animated GIF"

let iconUri = new Uri("pack://application:,,,/AP.ico", UriKind.RelativeOrAbsolute);
do this.Icon <-  BitmapFrame.Create(iconUri)

do this.Name <- "MainWindow" 

let mutable  ucMainWindow =  MyUserControl.CaptureImage()
do this.Content <- ucMainWindow
do ucMainWindow.WindowMain <- this  // Parameter  WindowMain                 // MUST SETUP (like parameter) "this"
do this.Topmost <- true   // MainWindow will be TOP MOST (always over other windows)

let mutable captureArea = new MyUserControl.CaptureArea(false)             // Init Normal Window
let mutable captureAreaTransparency = new MyUserControl.CaptureArea(true)  // Init Transparency window

do this.Loaded.Add(fun _ -> captureAreaTransparency.MainWindow <- this   // Need to show Transparency first  (Transparency window itself imposable change size/position)
                            ucMainWindow.CaptureAreaTransparency <- captureAreaTransparency
                            captureAreaTransparency.Show() 

                            captureArea.MainWindow <- this               // Show normal window - CaptureArea - over transparency window...
                            ucMainWindow.CaptureArea <- captureArea
                            captureArea.AllowDrop <- false
                            captureArea.Show() )

// If "this" was changed set parameter "MainWindow" - it change position both below windows base on "this"
do this.LocationChanged.Add(fun _ -> captureArea.MainWindow <- this      // See Capture Area class ...
                                     captureAreaTransparency.MainWindow <- this)

// If "CaptureArea" size was changed -> "this" and "transparency" will be changed   ("this" and "Transparency" Repeat changes of "CaptureArea"
do captureArea.SizeChanged.Add(fun _ -> this.Left <- captureArea.Left 
                                        this.Top <-  captureArea.Top - this.ActualHeight - 8.0  // - 8.0 Exclude Of Shedow from Main Window .. 
                                        captureAreaTransparency.Left <- captureArea.Left
                                        captureAreaTransparency.Top <- captureArea.Top 
                                        captureAreaTransparency.Height <- captureArea.Height
                                        captureAreaTransparency.Width <- captureArea.Width)

// All three window are separate in same process - if "this" was closed - close two below also ....
do this.Unloaded.Add(fun _ -> captureArea.Close()
                              captureAreaTransparency.Close()) 


[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(this) |> ignore 




 

