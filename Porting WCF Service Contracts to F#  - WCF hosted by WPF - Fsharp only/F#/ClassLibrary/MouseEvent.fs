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
    extern void mouse_event(uint32 dwFlags, uint32 dx, uint32 dy, uint32 dwData, UIntPtr dwExtraInfo)

// Interfaces (F#)
// https://msdn.microsoft.com/en-us/library/dd233207.aspx
// http://www.fssnip.net/c5

 [<DataContract>]
 type MouseData = {
                    [<DataMember>] mutable DwFlags : uint32
                    [<DataMember>] mutable Dx : uint32 
                    [<DataMember>] mutable Dy : uint32 
                    [<DataMember>] mutable DwData : uint32 
                    [<DataMember>] mutable DwExtraInfo : UIntPtr
                  }

[<ServiceContract>]
type IMouseEvent = 
    [<OperationContract(IsOneWay=true)>]
    abstract member MoveEvent : value : MouseData -> unit
    [<OperationContract>]
    abstract member ServiceOK : unit -> string


//To make the remote access possible, the object must inherit from the MarshalByRefObject type
type MouseEvent() =
    interface IMouseEvent with
        member x.MoveEvent(md: MouseData) = Imported.mouse_event(md.DwFlags, md.Dx, md.Dy, md.DwData, md.DwExtraInfo)
        member x.ServiceOK() = "OK" 
    







   