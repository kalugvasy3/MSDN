namespace MyUserControl

open System.Windows.Input
open System.IO
open System.Xaml
open System.Windows.Markup
open System.Reflection
open Utilities 

open System.Windows.Media
open System.Windows.Shapes
open System.Windows.Controls

open System.Windows
open System

type Rect() as this  = 
         inherit UserControl() 
     
         do this.Content <- contentAsXamlObject("rect.xaml") :?> UserControl  // Load XAML   
         let myrect : Rectangle = (this.Content)?myRect       // You MUST find Control inside "this.Content"
         do myrect.Fill <- new SolidColorBrush(Colors.Red)

// Some extra logic ... For example ... 
         
         let lblX : Label = (this.Content)?lblX
         let lblY : Label = (this.Content)?lblY 

         let moveMouseRectangle = new Event<MouseEventArgs>()             //creates event 
         do myrect.MouseMove.Add(fun e -> moveMouseRectangle.Trigger(e))  //invokes event handler 

         member this.recBrush with get() = myrect.Fill and
                                   set(v) = myrect.Fill <- v
         member this.recMoveEvent  =  moveMouseRectangle.Publish     // exposed event handler 
                                                                      // to allow listeners to hook onto our event  
         member this.recLblX  with get() = lblX.Content and
                                   set(v) = lblX.Content <- v                                                                                      
        
         member this.recLblY  with get() = lblY.Content and
                                   set(v) = lblY.Content <- v  

// Examples's of path - also work ... XAMAL - Must be MUST be Embedded Resource ... but we need to copy it to bin folder ...  
//     let mySr = new StreamReader(@"C:\""FULL PATH TO"" \rect.xaml") 
//     let mySr = new StreamReader(Application.GetRemoteStream(new Uri("pack://siteoforigin:,,,/rect.xaml", UriKind.Absolute)).Stream)  // Copy To Bin 
