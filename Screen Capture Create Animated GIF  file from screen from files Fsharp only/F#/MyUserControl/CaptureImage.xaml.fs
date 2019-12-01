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
open Microsoft.Win32
open System.Windows.Threading
open System.Drawing

open Utilities 
open ImageNameSpace.Common

type CaptureImage() as this  = 
    inherit UserControl() 
//    let mySr = new StreamReader(Assembly.Load("MyUserControl").GetManifestResourceStream("CaptureImage.xaml"))   // XAML - MUST be Embedded Resource  
//    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML

    do this.Content <- Utilities.contentAsXamlObject("CaptureImage.xaml"):?> UserControl // Load XAML

    
    // FIND ALL OBJECTS IN THIS.CONTENT 
    let mutable newJob : string = ""

    let mutable sliderDelay : Slider = (this.Content)?sliderDelay  
    let mutable txtDelay : TextBox = (this.Content)?txtDelay  

    let mutable txtFrames : TextBox = (this.Content)?txtFrames 
    
    let mutable btnNewJobs : Button = (this.Content)?btnNewJob  
    let mutable btnConvert : Button = (this.Content)?btnConvert 
    let mutable btnRun : Button = (this.Content)?btnRun 
    let mutable btnNext : Button = (this.Content)?btnNext

    let mutable chkTransparence : CheckBox = (this.Content)?chkTransparence 
    let mutable btnSave : Button = (this.Content)?btnSave

    let mutable chkCursor : CheckBox = (this.Content)?chkCursor 
    let mutable chkFill : CheckBox = (this.Content)?chkFill 

    let mutable prgBar : ProgressBar = (this.Content)?prgBar  
    let mutable statusText : TextBlock  = (this.Content)?statusText 
    let mutable statusInfo : TextBlock  = (this.Content)?statusInfo
    // FIND ALL OBJECTS IN THIS.CONTENT 

    let mutable windowMain : Window  = null
    let mutable captureArea : Window = null
    let mutable captureAreaTransparency : Window = null

    let mutable maxFramesPerJob = 100.0
    let mutable blnDropGeneral = false

    do prgBar.Maximum <- maxFramesPerJob

    let testNull (x : _ Nullable) = if x.HasValue then x.Value else false

    let setTransparence() =                                                 
        match testNull(chkTransparence.IsChecked) with
        | true  ->  (captureArea :?> CaptureArea).Hide()
                    (captureAreaTransparency :?> CaptureArea).SetEventOnTimedEvent()
        | false ->  (captureArea :?> CaptureArea).Show()                                             
                    (captureArea :?> CaptureArea).SetEventOnTimedEvent()

    let setCursor() =                                                 
        match testNull(chkCursor.IsChecked) with
        | true  ->  (captureArea :?> CaptureArea).BlnCursor <- true
                    (captureAreaTransparency :?> CaptureArea).BlnCursor <- true
        | false ->  (captureArea :?> CaptureArea).BlnCursor <- false                                             
                    (captureAreaTransparency :?> CaptureArea).BlnCursor <- false
    let setFill () =                                                 
        match testNull(chkFill.IsChecked) with
        | true  ->  (captureArea :?> CaptureArea).BlnFill <- true
                    (captureAreaTransparency :?> CaptureArea).BlnFill <- true

        | false ->  (captureArea :?> CaptureArea).BlnFill <- false                                             
                    (captureAreaTransparency :?> CaptureArea).BlnFill <- false
    
        do (captureArea :?> CaptureArea).FillShow()
 
    let setNewJob() = 
        do (captureAreaTransparency :?> CaptureArea).Reset(0)
        do (captureAreaTransparency :?> CaptureArea).Reset((int)maxFramesPerJob)
        do (captureArea :?> CaptureArea).Reset(0)
        do (captureArea :?> CaptureArea).AllowDrop <- true 
        do (captureArea :?> CaptureArea).Reset((int)maxFramesPerJob) 
        
        do chkTransparence.IsEnabled <- true 
        do chkFill.IsEnabled <- true
        do chkCursor.IsEnabled <- true
        
        do sliderDelay.IsEnabled <- true
        do sliderDelay.Value <- 100.0

        do prgBar.Value <- 0.0 
        do txtFrames.Text <- "00" 
        do statusText.Text <- newJob   
        do btnNewJobs.IsEnabled <- true
        do btnRun.IsEnabled <- true
        do btnNext.IsEnabled <- true
        do btnSave.IsEnabled <- true
        do btnConvert.IsEnabled <- true      
        do statusText.Background <- System.Windows.Media.Brushes.Transparent  
        do btnRun.Content <- "Run|Continue"                               
    
    let setDelay() = 
        txtDelay.Text <- ((int)sliderDelay.Value).ToString()
        (captureArea :?> CaptureArea).Delay <- sliderDelay.Value
        (captureAreaTransparency :?> CaptureArea).Delay <- sliderDelay.Value

    let setWheelDelay(e : Input.MouseWheelEventArgs) =  
        match e.Delta < 0 with
        | true ->  sliderDelay.Value <- sliderDelay.Value - 1.0 
        | false -> sliderDelay.Value <- sliderDelay.Value + 1.0 
                   
    
    let continueGif() = 
        match testNull(chkTransparence.IsChecked) with
            | true  ->  do (captureAreaTransparency :?> CaptureArea).Run()                                  
            | false ->  do (captureArea :?> CaptureArea).Run()   
                                  
    
    let pauseGif() = 
        statusText.Background <- System.Windows.Media.Brushes.AliceBlue
        this.Dispatcher.Invoke(fun _ ->
             match testNull(chkTransparence.IsChecked) with
             | true  ->  do (captureAreaTransparency :?> CaptureArea).Pause()       
             | false ->  do (captureArea :?> CaptureArea).Pause()  )


    let nextGif() = 
        statusText.Background <- System.Windows.Media.Brushes.Transparent
        this.Dispatcher.Invoke(fun _ ->
             match testNull(chkTransparence.IsChecked) with
             | true  ->  do (captureAreaTransparency :?> CaptureArea).NextCapture()       
             | false ->  do (captureArea :?> CaptureArea).NextCapture()  )
         
   
    let dialog = new System.Windows.Forms.FolderBrowserDialog() 
   
    let run() =  
        if newJob= "" || newJob = Environment.SpecialFolder.MyPictures.ToString() 
            then 
                 do dialog.RootFolder <- Environment.SpecialFolder.UserProfile    // UserProfile is StartFolder ....You can change it ... 
                 let result = dialog.ShowDialog()              
                 if result = System.Windows.Forms.DialogResult.OK
                     then newJob <- dialog.SelectedPath 
                          setNewJob()
                     else ignore() // keep all previous setting ...     
            else
                setDelay()
                let strName =  btnRun.Content.ToString() 
                match strName  with
                | "Pause|Stop" ->  
                    btnRun.Content <- "Run|Continue"
                    pauseGif()
                            
                | "Run|Continue" -> 
                    btnRun.Content <- "Pause|Stop"
                    continueGif()

                | _ ->  ignore()
    

    let prgBarChanged( p : System.Drawing.Point) = 
        this.Dispatcher.Invoke(fun _ -> if newJob <> "" then 
                                            prgBar.Value <- (float)listImg.Length  
                                            statusInfo.Text <- p.ToString()
                                            txtFrames.Text <- listImg.Length.ToString() 
                                                     
                                            match listImg.Length >= (int)maxFramesPerJob with
                                            | true -> 
                                                       btnRun.Content <- "Run|Continue"
                                                       btnRun.IsEnabled <-false
                                                       maxFramesPerJob <- (float) listImg.Length
                                            | false -> ignore())  
                                                                                                                       

    let convert() =  
        match testNull(chkTransparence.IsChecked) with
        | true  -> (captureAreaTransparency :?> CaptureArea).Convert(newJob)
        | false -> (captureArea :?> CaptureArea).Convert(newJob)
        
        statusText.Background <- System.Windows.Media.Brushes.Aqua  
                                         


    let save() = 
        match testNull(chkTransparence.IsChecked) with
         | true  ->  (captureAreaTransparency :?> CaptureArea).Save(newJob)                  
         | false ->  (captureArea :?> CaptureArea).Save(newJob)                                              
        statusText.Background <- System.Windows.Media.Brushes.Aquamarine


    do btnConvert.Click.Add(fun _ -> convert())

    do btnSave.Click.Add(fun e -> save())
     
    do btnNext.Click.Add(fun e -> nextGif())  

    do btnNewJobs.Click.Add(fun e -> setNewJob())
    do btnRun.Click.Add(fun e -> run())

    do chkTransparence.Click.Add(fun e -> setTransparence()) 
    do chkCursor.Click.Add(fun e -> setCursor())
    do chkFill.Click.Add(fun e -> setFill())  
     
    do sliderDelay.ValueChanged.Add(fun e -> setDelay())
    do sliderDelay.MouseWheel.Add(fun e -> setWheelDelay(e))
    
                                
    member x.WindowMain with set(v) = windowMain <- v  
    member x.CaptureArea with  set(v) = captureArea <- v 
                                        do (captureArea :?> CaptureArea).EventOnTimedEvent.Add(fun (p) -> prgBarChanged(p))

    member x.CaptureAreaTransparency with set(v) = captureAreaTransparency <- v 
                                                   do (captureAreaTransparency :?> CaptureArea).EventOnTimedEvent.Add(fun (p) -> prgBarChanged(p))

    member x.DelayFrame  with  get() = (int)sliderDelay.Value and
                               set(v) = sliderDelay.Value <- float(v)

                  




