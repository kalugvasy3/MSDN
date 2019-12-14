namespace DataCommonFsharp

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

open System.Data

open System.IO
open System.Windows.Markup
open System.Xaml
open System.Reflection

open DataCommonFsharp.Utilities 


type ShowData() as this =
    inherit Window()

    let mySr = new StreamReader(Assembly.Load("FsharpObjXAMLLibrary").GetManifestResourceStream("ShowData.xaml"))   // XAML - MUST be Embedded Resource 
    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML
    do this.Title <- "Data Grid - Show"

    do this.Width <- 400.0
    do this.Height <- 300.0
    
    let mutable gridResult : Grid = this.Content?gridResult   // Find txtHello name in runWindow and Cast to TextBlock
    let mutable scaleXY : ScaleTransform = this.Content?scaleXY
    
    let initGrid(ds: DataSet) =    let tabMain = new TabControl()
                                   do gridResult.Children.Add(tabMain) |> ignore

                                   for dt in ds.Tables do
                                      let mutable ti = new TabItem()
                                      do ti.Header <- dt.TableName
                                      let mutable dg = new DataGrid()
                                      do dg.AutoGenerateColumns <- true
                                      do dg.ItemsSource <- dt.DefaultView
                                      do ti.Content <- dg
                                      do tabMain.Items.Add(ti) |> ignore

    let initGridError(strErr: String) =
                                    let tabMain = new TabControl()
                                    do gridResult.Children.Add(tabMain) |> ignore
                                    let mutable ti = new TabItem()
                                    do ti.Header <- "ERROR(S)"
                                    let mutable tb = new TextBox()
                                    do tb.Text <- strErr
                                    do tb.TextWrapping <- TextWrapping.Wrap
                                    do ti.Content <- tb
                                    do tabMain.Items.Add(ti) |> ignore
  
    let changedScale(v) = do scaleXY.ScaleX <- v / 100.0
                          do scaleXY.ScaleY <- v / 100.0
    
    member x.InitGrid(ds : DataSet) =  initGrid(ds)
    member x.InitGridError(strError : String) =  initGridError(strError)
    member x.ChangedScale(v) = changedScale(v)
