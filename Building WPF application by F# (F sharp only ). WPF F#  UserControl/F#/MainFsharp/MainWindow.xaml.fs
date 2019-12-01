// This sample was built 05/03/2015 by Vasily Kalugin 
// This example was built base on my different example - https://code.msdn.microsoft.com/windowsapps/Building-WPF-application-09b8fb8c 
// License - MIT 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


module MainWindow

open System 
open System.Windows 
open System.Windows.Input
open System.Windows.Controls
open System.IO
open System.Xml 
open System.Xaml
open System.Windows.Markup
open System.Text
open System.Reflection
open Utilities
open MyUserControl

open System.Windows.Media
open System.Windows.Shapes


type MainWindow() as this  = 
     inherit UserControl() 

     do this.Content <- contentAsXamlObject("MainWindow.xaml") :?> UserControl  // Load XAML

    // Find Control on MainWindow     
     let uc = this.Content :?> UserControl

     let txtHello : TextBlock = uc?txtHello  // Find txtHello name in runWindow and Cast to TextBlock     
     let btnHello : Button = uc?btnHello       // Find btnHelp name in runWindow and cast to Button   
     let gridAll  : Grid   = uc?gridAll  

     do btnHello.Click.Add(fun _ -> do if txtHello.Text = "***" then txtHello.Text <- "Hello!" else txtHello.Text <- "***" ) 

     // Example for adding behavior for UserControl ... External UserControl from Library added to "MainWindow" user control (see MainWindow.XAML file) ....
     
     let myRec : Rect  = uc?recRed           
     do myRec.recBrush <- new SolidColorBrush(Colors.Blue)   // Change Red color (see type) to Blue ...just example ...
 

     //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
     //Uncomment below - logic for drag rectangle ...

     let mutable tr = new TranslateTransform() 
     
     do tr.X <- 100.0
     do tr.Y <- 100.0

     do myRec.RenderTransform <- tr   // added for drag object to  myRec ...    
    
     do myRec.recLblX <- "X = " + tr.X.ToString()
     do myRec.recLblY <- "Y = " + tr.Y.ToString()
    
     let mutable blnOn : bool = false

     do myRec.MouseLeftButtonDown.Add(fun _  -> do blnOn <- true)  //if press left button and move -> drag object MyUserControl.RectModul.Rect
     do myRec.MouseLeftButtonUp.Add(fun _ -> do blnOn <- false)

     let mouseMove(e : MouseEventArgs) =
        
         let mousePos = e.GetPosition(gridAll) 
         let mutable rectPos = myRec.RenderTransform.Transform

         let sender = e.OriginalSource
         match sender with
         | :? Rectangle ->  
                        try                 
                            if myRec.IsMouseOver && blnOn then

                                  do tr.X  <-  mousePos.X - gridAll.ActualWidth  / 2.0 
                                  do tr.Y  <-  mousePos.Y - gridAll.ActualHeight / 2.0 
                                  do myRec.recLblX <- "X -> " + tr.X.ToString()
                                  do myRec.recLblY <- "Y -> " + tr.Y.ToString()

                        with ex -> () 
                                |> ignore
   
         | _ -> do blnOn <- false 
   
     do myRec.recMoveEvent.Add(fun  e -> mouseMove(e))  
     // End ----- Example for moving User Control By Mouse


     


type AAA() as this =
     inherit Window()
     do this.Content <- new MainWindow()    // add User Control "MainWindow" to Window()

     // Add here any logic for Window() ...
     // Load...Start...Exit...Exception...etc..

     do this.Width <- 600.0
     do this.Height <- 400.0


[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(new AAA()) |> ignore


 

