namespace WCFhostByWpfFsharpOnly
module  Utilities =

    open System.Windows
    open System.Windows.Controls


    /// Use this implementation of the dynamic binding operator
    /// to bind to XAML components in code-behind, see example below
    let (?) (c:obj) (s:string) =
        match c with
        | :? ResourceDictionary as r ->  r.[s] :?> 'T
        | :? Control as c -> c.FindName(s) :?> 'T
        | _ -> failwith "dynamic lookup failed"
