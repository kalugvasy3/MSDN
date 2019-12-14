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
open System.Text
open System.IO
open System.Windows.Markup
open System.Xaml
open System.Reflection

open Utilities
open DataCommonFsharp 


type DataCommonFsharp() as this =
    inherit UserControl()

 // XAML file properties -> "EmbeddedResource"

    let mySr = new StreamReader(Assembly.Load("FsharpObjXAMLLibrary").GetManifestResourceStream("DataCommonFsharp.xaml"))   // XAML - MUST be Embedded Resource 
    do this.Content <- XamlReader.Load(mySr.BaseStream):?> UserControl  // Load XAML

// OR  Change XAML file properties to "Resource" and uncomment below (comment out above)

// // XAML file properties -> Resource 
//    let resource = new Uri("/FsharpObjXAMLLibrary;component/DataCommonFsharp.xaml",System.UriKind.Relative)
//    do  <- Application.LoadComponent(resource) :?> UserControl   // Cast to UserControl 

    let mutable txtSQL : TextBox = this.Content?txtSQL
    let mutable txtConnectionString : TextBox = this.Content?txtConnectionString
    let mutable comboProvider : ComboBox = this.Content?comboProvider
    let mutable btnOleDbHelper : Button = this.Content?btnOleDbHelper
    let mutable btnRUN : Button = this.Content?btnRUN
    let mutable chkSpell : CheckBox = this.Content?chkSpell
    let mutable btnTest : Button = this.Content?btnTest
    
    let mutable scaleXY : ScaleTransform = this.Content?scaleXY
    let mutable slider : Slider = this.Content?slider
    
    let mutable sh : ShowData = new ShowData() 

    let dc = new DataCommon() 

    do btnOleDbHelper.ToolTip <- "Show OleDb data providers which installed on YOUR PC."
    do comboProvider.ToolTip <- "Show ALL data providers which installed on YOUR PC."

    // Init combo box providers
     
    do comboProvider.DisplayMemberPath <- "Name"
    do comboProvider.SelectedValuePath <- "InvariantName"
    do comboProvider.ItemsSource <- (dc.GetAllProviders()).DefaultView;

    let runDS() = dc.GetDataSet(comboProvider.SelectedValue, txtConnectionString.Text, new StringBuilder(txtSQL.Text) )

    let showData(ds:DataSet) = async {
                              do sh <- new ShowData()  
                              do sh.ChangedScale(slider.Value)
                              do sh.Owner <- Application.Current.MainWindow  // window be on top of all other windows 
                              do sh.Show() 
                              if dc.StrError = "" then do sh.InitGrid(ds)
                                                  else do sh.InitGridError(dc.StrError) 
                            }

    let comboSelected() = let s = comboProvider.SelectedValue.ToString()
                          if s.IndexOf("OleDb") >= 0 then do btnOleDbHelper.IsEnabled <- true
                                                     else do btnOleDbHelper.IsEnabled <- false

    let testConnection() = async {
                                   do sh <- new ShowData()
                                   do sh.ChangedScale(slider.Value)
                                   do sh.Owner <- Application.Current.MainWindow  // window be on top of all other windows 
                                   do sh.Show() 
                                   if dc.TestConnection(comboProvider.SelectedValue, txtConnectionString.Text) 
                                       then do sh.InitGridError("Connection String - OK")
                                       else do sh.InitGridError(dc.StrError)
                                   ignore()
                             }
    let changedScale() = do scaleXY.ScaleX <- slider.Value / 100.0
                         do scaleXY.ScaleY <- slider.Value / 100.0
                         do sh.ChangedScale(slider.Value)

    do slider.ValueChanged.Add(fun _ -> changedScale()) 

    do this.Unloaded.Add(fun _ -> dc.CloseConnection())

    do comboProvider.SelectionChanged.Add(fun _ ->  comboSelected())

    do chkSpell.Checked.Add(fun _ -> do txtSQL.SpellCheck.IsEnabled <- true )
    do chkSpell.Unchecked.Add(fun _ -> do txtSQL.SpellCheck.IsEnabled <- false )

    do btnTest.Click.Add(fun _ ->  Async.StartImmediate(testConnection())) 
    do btnRUN.Click.Add(fun _ ->  Async.StartImmediate(showData(runDS()))) 
    do btnOleDbHelper.Click.Add(fun _ -> Async.StartImmediate(showData(dc.GetAllOldbProviders())))