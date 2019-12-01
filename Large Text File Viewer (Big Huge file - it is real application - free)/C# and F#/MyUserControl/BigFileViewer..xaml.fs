namespace MyUserControl

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.IO
open System.Drawing
open System.Windows.Markup
open System.Windows.Input
open System.Reflection
open System.Threading.Tasks
open System.Threading

open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Input
open CommonClassLibrary

open FsharpXAMLLibrary

open Utilities 
open System.Windows.Threading


type BigFileViewer() as this  = 
    inherit UserControl() 
    let mySr = new StreamReader(Assembly.Load("MyUserControl").GetManifestResourceStream("BigFileViewer.xaml"))   // XAML - MUST be Embedded Resource  
    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML
    
    // FIND ALL OBJECTS IN THIS.CONTENT  :StatusBarSystem x:Name="statusBar"

    let mutable myTextBox : MyTextBox = (this.Content)?myTextBox 
    let mutable statusBar : StatusBarSystem = (this.Content)?statusBar 

    let mutable allScale : ScaleTransform = (this.Content)?allScale
    let mutable bigGrid : Grid  = (this.Content)?bigGrid
    let mutable winHolder : Window = new Window()


    let eventScaleUpdate(e : float) = allScale.ScaleX <- e
                                      allScale.ScaleY <- e
    
    do myTextBox.StatusBar <- ref statusBar
    do myTextBox.EventSysInfoUpdate.Add(fun e ->  eventScaleUpdate(e))



    let deltaAdjVert =  4.0 * (float)System.Windows.SystemParameters.Border
                      + 4.0 * (float)System.Windows.SystemParameters.FixedFrameHorizontalBorderHeight 
                      + System.Windows.SystemParameters.WindowCaptionHeight + 2.0


    let deltaAdjHoriz = 4.0 * (float)System.Windows.SystemParameters.Border
                      + 4.0 * (float)System.Windows.SystemParameters.FixedFrameVerticalBorderWidth + 2.0



        // Synchronized UserControl size with Window
    member x.WinHolder  with set(v) = ( do winHolder <- v 
                                        do this.Width <- v.ActualWidth -   deltaAdjHoriz 
                                        do this.Height <- v.ActualHeight - deltaAdjVert 
                                     )





 
                                





