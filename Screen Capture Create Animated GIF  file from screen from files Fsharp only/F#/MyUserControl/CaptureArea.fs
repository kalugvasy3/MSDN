namespace MyUserControl

open System
open System.Windows
open System.Windows.Controls
open System.IO
open System.Drawing
open System.Windows.Markup
open System.Reflection
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Controls.Primitives
open System.Windows.Threading
open System.Threading
open System.Drawing



open ImageNameSpace
open Utilities 
open Common

type CaptureArea(blnAllowTrans : bool) as area  = 
     inherit Window() 

     let mutable blnFill = false

     // Default Setting 
     do area.Opacity <- if blnAllowTrans then 0.05 else 1.0
     do area.WindowStyle <- WindowStyle.None
     do area.AllowsTransparency <- blnAllowTrans
     do area.AllowDrop <- false
     do area.Background <- System.Windows.Media.Brushes.White
     do area.ResizeMode <- ResizeMode.CanResize

     do if blnAllowTrans then 
           do area.ResizeMode <- ResizeMode.NoResize
           do area.BorderThickness <- Thickness(1.0, 1.0, 1.0, 1.0) 
           do area.BorderBrush <- System.Windows.Media.Brushes.Black                
          
     let mutable mainWindow : Window = null  
     
     let eventOnTimedEvent = new Event<_>()   // Event when Img has been added ...
     let eventWasDropped = new Event<_>()   // Event when Img has been added ...
     let mutable intNumCurrentImage = 0

     let mutable aTimer = new System.Timers.Timer() // timer for build sequences of images ....   
    
     let mutable floatDelay = 0.0 
     let mutable setMaxFrames = 0
     let mutable mainFolder = "" 
     let mutable blnCursor = false
   

     let mutable img  = new Img()   
     let mutable posP = new System.Drawing.Point()

     let mutable blnEnable = false // value uses in timer 

     // Set Size base on Parent Window - only once
     let setSizeInit (w : Window) = 
         do area.Left   <- w.RestoreBounds.Left 
         do area.Top    <- w.RestoreBounds.Bottom + 8.0  //   8.0 Exclude Of Shedow from Main Window .. 
         do area.Width  <- w.RestoreBounds.Width 
         do area.Height <- w.RestoreBounds.Height 

     let setSizeAfter (w : Window) =                  // control Left/Top "area" base on MainWindow - allow move object "area" base on moving "MainWindow"
         do area.Left   <- w.Left 
         do area.Top    <- w.RestoreBounds.Bottom  + 8.0  //    8.0 Exclude Of Shedow from Main Window .. 

     let reset(setFrame : int) =
         if not blnAllowTrans then do area.Background <- System.Windows.Media.Brushes.White
         setMaxFrames <- setFrame 
         listImg  <- [] 
         img  <- new Img()
         aTimer.Enabled <- false
         eventOnTimedEvent.Trigger(posP)
 
      
     let nextCapture() = 
         img <- new Img() 
         area.Dispatcher.Invoke(fun e -> do img.CroppedByCurrentSelected(area, blnCursor) |> ignore )
         listImg <- List.append listImg [img] 
         do posP <- img.PositionP  
         eventOnTimedEvent.Trigger( posP)

     let save() =      
         do listImg |> List.iteri(fun i v ->  
             let strTicks = DateTime.Now.Ticks.ToString()
             let path = mainFolder + "\\" + i.ToString() + "_" + strTicks + ".PNG"
             v.SaveImageToFormat(path,"PNG")) 

     let convert() =
         let animatedGif = new  ImageNameSpace.AnimatedGIFencoder(area)
         do listImg |> List.iteri(fun i v ->  animatedGif.AddFrame(v.ImgSpecificSize((int)area.Width, (int)area.Height , blnFill)))
         do animatedGif.Repeat <- true
         do animatedGif.FrameDelay <- (int)floatDelay 
         let strTicks = DateTime.Now.Ticks.ToString() // File Name
         use  stream : FileStream = File.Create(mainFolder + "\\GIF_" + strTicks + ".gif" )
         do  animatedGif.Save(stream)

     let run() =     // numberOfFrames - is TOTAL value
         blnEnable <- true
         aTimer.Enabled <- true;

     let onTimedEvent(e) = 
        if listImg.Length <= setMaxFrames then  nextCapture()
                                          else blnEnable <- false  
        aTimer.Enabled <- blnEnable 
        eventOnTimedEvent.Trigger(posP)            

     do  aTimer.Elapsed.Add(fun e -> onTimedEvent(e))  

            
     let initTimer(v) = 
         floatDelay <- v 
         aTimer.Interval <- v
         aTimer.AutoReset <- false;  //MUST BE  FALSE 
         aTimer.Enabled <- false; // create, do not Run


     let openFiles(e : System.Windows.DragEventArgs) = 
                                        let data = e.Data
                                                                       
                                        let files  = data.GetData(DataFormats.FileDrop) :?> string[] 
                                        match files.GetLength(0) with
                                        | 1 ->  try let img = new Img()
                                                    do img.Load(files.[0]) |> ignore
                                                    let mutable br = new ImageBrush(img.Image.BitmapImage) 
                                                    if blnFill then br.Stretch <- Stretch.Fill else br.Stretch <- Stretch.UniformToFill  
                                                    area.Background  <- br
                                                    listImg <- List.append listImg [img] 
                                                    eventOnTimedEvent.Trigger(posP)
                                                    intNumCurrentImage <- listImg.Length - 1

                                                 with | _ ->  MessageBox.Show("IT IS NOT AN IMAGE") |> ignore   

                                        | _ -> MessageBox.Show("DROP ONE FILE ... FILE BY FILE PLEASE ...") |> ignore   
     
     do area.Drop.Add(fun e -> openFiles(e))
     
     do area.Unloaded.Add(fun e -> mainWindow.Close() )  // if windows was closed - we MUST close MainWindow also ...

     let fillShow() = 
        if listImg.Length <> 0 
            then
                let mutable br = new ImageBrush(listImg.[intNumCurrentImage].Image.BitmapImage) 
                if blnFill then br.Stretch <- Stretch.Fill else br.Stretch <- Stretch.UniformToFill 
                area.Background  <- br
            else area.Background <- Media.Brushes.White
                 intNumCurrentImage <- -1                         

     let wheelChangeImg(e : Input.MouseWheelEventArgs) = 
        let numMaxImages = listImg.Length - 1
        match e.Delta < 0 with
        | true -> if intNumCurrentImage > 0 then intNumCurrentImage <- intNumCurrentImage - 1
        | false -> if intNumCurrentImage < numMaxImages then intNumCurrentImage <- intNumCurrentImage + 1
        fillShow()

     let deleteYesNo(e) = 
         let numMaxImages = listImg.Length - 1
         if (MessageBox.Show("Delete Image", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.No) 
             then ignore()
             else listImg <- match intNumCurrentImage with
                             | _ when intNumCurrentImage = 0 -> listImg.[intNumCurrentImage + 1..numMaxImages]                                    
                             | _ when intNumCurrentImage = numMaxImages -> listImg.[0..numMaxImages - 1]
                             | _ -> List.append listImg.[0..intNumCurrentImage - 1] listImg.[intNumCurrentImage + 1..numMaxImages]
         if intNumCurrentImage = listImg.Length then intNumCurrentImage <- listImg.Length - 1  
         eventOnTimedEvent.Trigger(posP)
         do fillShow()


     let eventAddIfDrop() = 
         match blnAllowTrans with
         | true -> ignore()
         | false -> do area.MouseWheel.Add(fun e -> wheelChangeImg(e))
                    do area.MouseRightButtonUp.Add( fun e -> deleteYesNo(e)) 

     do eventAddIfDrop()

     member x.MainWindow with set(v) = if isNull mainWindow then setSizeInit (v)
                                                            else setSizeAfter(v)
                                       mainWindow  <- v

     [<CLIEvent>]
     member x.EventOnTimedEvent = eventOnTimedEvent.Publish   // In this case we can not use properties - we need to use properties from class which located below ...
     [<CLIEvent>]
     member x.EventWasDropped = eventWasDropped.Publish       // If file was dropped ... 

     
     member x.SetEventOnTimedEvent() = eventOnTimedEvent.Trigger(posP)  // Trigger Event from External ...
     member x.SetEventWasDroppedt() = eventWasDropped.Trigger(listImg)


     member x.Run() = 
         run()
      
     member x.Pause() = 
         blnEnable <- false
    
     member x.NextCapture() = 
         blnEnable <- false; 
         nextCapture() 
    
     member x.Reset(setFrames)  = 
         reset(setFrames)      

     member x.Save(folder)  = 
         mainFolder <-folder; 
         save()  

     member x.Convert(folder) = 
         mainFolder <-folder; 
         convert()                                     
      
     member x.Delay 
         with set(v) = floatDelay <- v
                       initTimer(v)

     member x.BlnCursor 
         with set(v) = blnCursor <- v

     member x.BlnFill 
         with set(v) = blnFill  <- v  
         
     member x.FillShow() = fillShow()    
