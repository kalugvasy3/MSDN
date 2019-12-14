namespace WCFhostByWpfFsharpOnly

open System
open System.ServiceModel
open System.Runtime.Serialization
open System.Runtime.InteropServices 


//Wrapping a DLL import in a module in F#
//http://stackoverflow.com/questions/25192002/wrapping-a-dll-import-in-a-module-in-f

//mouse_event function
//https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx

module Imported = 
    [<DllImport( "user32.dll", CallingConvention = CallingConvention.StdCall )>]
    extern void mouse_event(int32 dwFlags, int32 dx, int32 dy, int32 dwData, int32 dwExtraInfo)

// Interfaces (F#)
// https://msdn.microsoft.com/en-us/library/dd233207.aspx
// http://www.fssnip.net/c5    


[<ServiceContract(Namespace = "http://www.anotherpart.com")>]
type IMouseEvent = 

//HTTP binding ->  USE parameters as string - resolve a serialize/deserialize  issue ...  
    [<OperationContract(IsOneWay=true)>] 
    abstract member MouseMoveTuple : DwFlag : string * Dx : string * Dy : string * DwData : string * DwExtraInfo : string  -> unit    

//To make the remote access possible, the object must inherit from the MarshalByRefObject type
type MouseEvent() =
    interface IMouseEvent with
       
        member x.MouseMoveTuple(dwFlag : string, dx : string, dy : string, dwData : string, dwExtraInfo : string) = 
            Imported.mouse_event((int32)dwFlag ,(int32)dx, (int32)dy, (int32)dwData, (int32)dwExtraInfo)                                           



    







   