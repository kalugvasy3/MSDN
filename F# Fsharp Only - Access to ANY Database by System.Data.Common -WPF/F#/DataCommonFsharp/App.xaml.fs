module App

open System
open System.Collections.Generic
open System.Collections.ObjectModel

open System.Linq
open System.Net
open System.Windows
open System.Windows.Controls
open System.Windows.Documents
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Media.Animation
open System.Windows.Shapes

open System.Windows.Threading

open System.IO
open System.Windows.Markup
open System.Xaml
open System.Reflection

open DataCommonFsharp.Utilities 


let runWPFDataCommon() = 
// // XAML file properties -> "EmbeddedResource"
    let mySr = new StreamReader(Assembly.Load("DataCommonFsharpAssembly").GetManifestResourceStream("App.xaml"))   // XAML - MUST be Embedded Resource 
    let runWindow = XamlReader.Load(mySr.BaseStream):?> Window

// OR  Change XAML file properties to "Resource" and uncomment below (comment out above)

// // XAML file properties -> Resource 
//    let resource = new Uri("/DataCommonFsharpAssembly;component/App.xaml",System.UriKind.Relative)
//    let runWindow = Application.LoadComponent(resource) :?> Window   // Cast to Window 


    runWindow.MinWidth <- 640.0
    runWindow.MinHeight <- 440.0
    runWindow


[<STAThread>] 
[<EntryPoint>]

do (new Application()).Run(runWPFDataCommon()) |> ignore

