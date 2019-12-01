

namespace ImageNameSpace

open System.Windows.Media.Imaging
open System.Drawing
open System
open System.Text
open System.IO
open System.Drawing.Imaging
open System.Windows
open System.Windows.Media
open System.Windows.Interop
open System.Runtime.InteropServices

// Base on solution - https://social.msdn.microsoft.com/Forums/office/en-US/d208a9c9-9299-4912-b35e-1c36d6c7ba84/animated-gif-encoder-net?forum=vbgeneral
// https://en.wikipedia.org/wiki/GIF

type AnimatedGIFencoder (this : Window)  =       
    let gifEncoder = new System.Windows.Media.Imaging.GifBitmapEncoder()       
    let encoderVersion = "GIF89a"                    // format encoder GIF89a
    let mutable repeat = true                        // true - default
    let mutable metadataString : List<String> = []   // metadataString - nothing - default
    let mutable frameDelay : int = 200               // 200 milliseconds - default
    let mutable packedFields : byte = 0x08uy;        // 3 bits - Reserved , 3 bits - Disposal Method, 1 - bit User Input Flag , 1 - Transparent Color Flag  
    let mutable transparenColorIndex : byte = 0x00uy;    

    let mutable actualCurrentFrames = 0
    // Add fame to Encoder
    let addFrame(frame : Bitmap) = 
        actualCurrentFrames <- actualCurrentFrames  + 1
        match isNull(frame) with
        | false -> this.Dispatcher.Invoke(fun _ ->
                   let bmpSource = Imaging.CreateBitmapSourceFromHBitmap(frame.GetHbitmap(), IntPtr.Zero, Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
                   let mutable bmf = BitmapFrame.Create(bmpSource)
                   
                   do this.Dispatcher.Invoke(fun _ -> gifEncoder.Frames.Add(bmf) ))
                   
        | true -> ignore()


    let arrayInsert (index : int , input : byte[], insert : byte[]) =
       let array1 = [| for i : int in 0 .. index - 1 -> input.[i] |] 
       let array2 = [| for i : int in index .. input.Length - 1  -> input.[i] |]
       Array.concat [| array1 ;  insert; array2 |] 

//    let arrayReplace (index : int , input : byte[], replace : byte[]) =
//       let array1 = [| for i in 0 .. index - 1 -> input.[i] |] 
//       let array2 = [| for i in index + replace.Length .. input.Length - 1  -> input.[i] |]
//       Array.concat [| array1 ;  replace; array2 |] 

    let arrayReplace (index : int , input : byte[], replace : byte[]) =
      do for i in index  .. index + replace.Length - 1 do input.[i] <- replace.[i - index] 
      input

    let arraySearch (startIndex : int , input : byte[], search : byte[]) =
       let mutable stopIndex = input.Length-1
       let mutable blnDone = false
       for i = startIndex to stopIndex do
           let mutable j = 0
           while input.[i + j] = search.[j] && not blnDone do
              if j < search.Length - 1 then j <- j + 1 
              else stopIndex <- i              
                   blnDone <- true
       if stopIndex = input.Length-1 then -1 else stopIndex
         

    // The stream where the binary is to be output.
    let save(stream : IO.FileStream) = 
            if gifEncoder.Frames.Count = 0 
            then  ignore()
            else  let mutable gifData : byte[] = [||]  
                  let mStream = new IO.MemoryStream()
                  do gifEncoder.Save(mStream)
                  do gifData <- mStream.ToArray()
                  // Locate the right location where to insert the meta-data in the binary
                  // This will be just before the first label &H0021F9 (Graphic Control Extension)          
                  let searchPTR = [|0x21uy; 0xF9uy|]   // Extension Block  (21 F9) - Graphic Control Extension for Frame # 1,2,3,...N 
              
                  let mutable metadataPTR = arraySearch(0, gifData ,searchPTR)   
                            
                  // SET METADATA Repeat
                  // This add an Application Extension Netscape2.0
                  // {&H21, &HFF, &HB, &H4E, &H45, &H54, &H53, &H43, &H41, &H50, &H45, &H32, &H2E, &H30, &H3, &H1, &H0, &H0, &H0}
                  if repeat then
                      let applicationExtension : Byte[] = [|0x21uy; 0xFFuy; 0xBuy; 0x4Euy; 0x45uy; 0x54uy; 0x53uy; 0x43uy; 0x41uy; 0x50uy; 0x45uy; 0x32uy; 0x2Euy; 0x30uy; 0x3uy; 0x1uy; 0xFFuy; 0xFFuy; 0x0uy|]    
                      gifData <- arrayInsert(metadataPTR, gifData , applicationExtension)              
            
                  // gifEncoder.Metadata.Comment
                  // SET METADATA Comments
                  // This add a Comment Extension for each string

                  if metadataString.Length > 0 then
                      for comm in metadataString do
                          let mutable byteCom : byte[] = [||]
                          if comm.Length > 254 then byteCom <- System.Text.UTF7Encoding.UTF7.GetBytes(comm.Substring(0,254))
                                               else  byteCom <- System.Text.UTF7Encoding.UTF7.GetBytes(comm)
                          let dataComments =Array.concat [| [|0x21uy; 0xFEuy|]; byteCom; [|0x00uy|] |] 
                          gifData <- arrayInsert(metadataPTR, gifData , dataComments)              
                
                  // SET METADATA frameRate
                  // Sets each Graphic Control Extension (5 bytes from each label 0x0021F9)
              
                  let bte : Byte[] = BitConverter.GetBytes(frameDelay / 10)

                  let search : Byte[] = [|0x21uy;0xF9uy;0x04uy|]  // 21 F9 Graphic Extension ... 
              
                  let replaceSetOfParameters = [|0x21uy; 0xF9uy; 0x04uy; packedFields ; bte.[0]; bte.[1] ; transparenColorIndex; 0x00uy |]   // 0x00uy - block termination
             
                  // function for searching position from special place            
                  let currentRatePosition (from : int) = arraySearch(from, gifData ,search)

                  let mutable current = currentRatePosition (0)
                  while current >= 0 do
                      gifData <- arrayReplace (current, gifData, replaceSetOfParameters )
                      current <- currentRatePosition (current + 7)
              
                  do stream.Write(gifData, 0, gifData.Length)
                  do stream.Flush()           

    //Return the GIF specification version. This always returns "GIF89a"
    // https://en.wikipedia.org/wiki/GIF

    member x.EncoderVersion with get() = encoderVersion 
    
    //Get or set a value that indicate if the GIF will repeat the animation after the last frame is shown
    member x.Repeat with get() = repeat and set(v) = repeat <- v
    
    // Get or set a collection of meta-data string to be embedded in the GIF file. Each string has a max length of 254 
    // characters (Any character above this limit will be truncated). The string will be encoded UTF-7. 
    member x.MetadataString with get() = metadataString and set(v) = metadataString <- v

    // 3 bits - Reserved , 3 bits - Disposal Method, 1 - bit User Input Flag , 1 - Transparent Color Flag  
    member x.PackedFields with get() = packedFields and set(v) =  packedFields <- v

    member x.TransparenColorIndex with get() = transparenColorIndex and set(v) = transparenColorIndex <- v 
    
    // Get or set the amount of time each frame will be shown (in milliseconds). The default value is 200ms
    member x.FrameDelay with get() = frameDelay and set(v) = frameDelay <- v

    // Add a frame to the encoder frame collection
    member x.AddFrame(frame : Bitmap) = addFrame(frame)

    // Writes the animated GIF binary to a specified IO.Stream
    // The stream where the binary is to be output. Can be any object type that derives from IO.Stream
    member x.Save(stream) = save(stream)


    // Animated GIF Image
    // https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k(System.Windows.Media.Imaging.GifBitmapEncoder.%23ctor);k(DevLang-fsharp);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.6)&rd=true
    // http://stackoverflow.com/questions/210922/how-do-i-get-an-animated-gif-to-work-in-wpf
    // http://stackoverflow.com/questions/30727343/fast-converting-bitmap-to-bitmapsource-wpf

