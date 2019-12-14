namespace FsharpLibrary

open MathNet.Numerics
open System
open System.Numerics
open System.Drawing
open System.Drawing.Imaging
open System.IO
open System.Windows.Media.Imaging
open System.Threading.Tasks

open MutableFFT2

type FFTfsharp() =

    let mutable lx = 1
    let mutable signi : float = -1.0 // forward
    let mutable sc : float = 1.0 

    let rec fft = function
      | [||]  -> [||]
      | [|x|] -> [|x|] 
      | x -> x |> Array.mapi (fun i c -> (i % 2 = 0, c))                                                           
               |> Array.partition fst                                                                              
               |> fun (even, odd) -> fft (Array.map snd even), fft (Array.map snd odd)                             
               ||> Array.mapi2 (fun i even odd -> 
                   let btf = odd * Complex.FromPolarCoordinates(1., -2. * signi * Math.PI * (float i / float x.Length ))
                   even + btf, even - btf)
               |> Array.unzip
               ||> Array.append 

    let fft1 (input : Complex[]) = 
        do lx <- input.GetLength(0)
        do sc <- 1.0 / float lx    // scale for Forward
        input |> Array.map(fun z -> z * Complex(sc, 0.0) ) |> fft |>  Array.iteri(fun i c -> do input.[i] <- c)   
        input   


    let fft2FuncAsync (input : Complex [,] ) =              

        MutableFFT2.sw.Reset()
        MutableFFT2.sw.Start() 
        
        let d0 = input.GetLength(0) 
        let d1 = input.GetLength(1) 
 
 
        Async.Parallel [for k=0 to d0 - 1 do input.[k , *] |> fft1 |> Array.iteri(fun i c -> input.[k , i ] <- c ) ] |> Async.RunSynchronously |> ignore    // For each ROW
        Async.Parallel [for l=0 to d1 - 1 do input.[* , l] |> fft1 |> Array.iteri(fun i c -> input.[i , l ] <- c ) ] |> Async.RunSynchronously |> ignore    // For each COLUMN    

        
        MutableFFT2.sw.Stop()
        MutableFFT2.fftTime <- MutableFFT2.sw.ElapsedMilliseconds

        input  



    let fft2FuncParallel (input : Complex [,] ) =              

        MutableFFT2.sw.Reset()
        MutableFFT2.sw.Start() 
        
        let d0 = input.GetLength(0) 
        let d1 = input.GetLength(1) 
 
        let parallelRow() = Parallel.For(0, d0 , (fun k -> input.[k , *] |> fft1 |> Array.iteri(fun i c -> input.[k , i ] <- c ) )) 
        let parallelColumn() = Parallel.For(0, d1 , (fun l -> input.[* , l] |> fft1 |> Array.iteri(fun i c -> input.[i , l ] <- c ) )) 

        do Task.Factory.StartNew(fun () -> parallelRow()).Wait() 
        do Task.Factory.StartNew(fun () -> parallelColumn()).Wait()   

        
        MutableFFT2.sw.Stop()
        MutableFFT2.fftTime <- MutableFFT2.sw.ElapsedMilliseconds

        input  



    let mutable scaleLog = 0.001   // usualy here 1.0 but for see extremly small value use this   LOG10(scaleLog + X)
    let mutable deltaLog = 0.0 
    let mutable delta = 0.0  
    let mutable min = + infinity
    let mutable max = - infinity

    let colorToGrayComplex (c : Color)  = 
        let gscale = (float c.R * 0.3) + (float c.G * 0.59) + (float c.B * 0.11)
        Complex(gscale, 0.0)
  
    // converts an image to Complex [,] - (gray) 
    let toComplexGrayArray (image : Bitmap) =
        let w = image.Width
        let h = image.Height
        Array2D.init w h (fun i j -> colorToGrayComplex(image.GetPixel( i, j))) 


    let mutable delta = 0.0
  
    let minMax (cx : Complex[,]) =  
        let w = cx.GetLength(0)
        let h = cx.GetLength(1)       
        Parallel.For(0, w , ( fun i ->       
                                 for j = 0 to h - 1 do 
                                    let z = cx.[i, j]
                                    if min > z.Magnitude then min <- z.Magnitude 
                                    if max < z.Magnitude then max <- z.Magnitude ))  |> ignore                                             
        deltaLog <- (Math.Log10(scaleLog + max) - Math.Log10(scaleLog + min)) / 255.0                         
        delta <-(max - min) / 255.0

    // Gray Log10 Color after FFT  ---- we must use LOG - if we want to see image - not just one dot - extra large dynamic diapason of data ... 
    let logColor (c: Complex ) = 
        let mag = c.Magnitude
        let mutable g = 0
        g <- int  ((Math.Log10(scaleLog + mag) - Math.Log10(scaleLog + min)) / deltaLog)  // (mag / delta) //
        Color.FromArgb(255, g, g, g)


    // Use for image after FFT2
    let toGrayLogBitmap (c : Complex[,] ) =
        let w = (c).GetLength(0)
        let h = (c).GetLength(1)
        let bitMap = new Bitmap(w, h)
        minMax(c) 
        for i = 0 to w - 1 do
            for j = 0 to h - 1 do bitMap.SetPixel(i, j, logColor((c).[i, j])) 
        bitMap  

    let shiftImage(bitMapGray : Bitmap) =   
        let w = bitMapGray.Width
        let h = bitMapGray.Height
        let bitMap = new Bitmap(w, h)
        Async.Parallel [ for i = 0 to (w - 1) do
                            for j = 0 to (h - 1)  do
                                        match (i < w / 2), (j < h / 2) with
                                        | true , true ->  bitMap.SetPixel(i , j, bitMapGray.GetPixel( w / 2 + i , h / 2 + j ))  //Shift image a fourth  1 to 4
                                        | false, true ->  bitMap.SetPixel(i , j, bitMapGray.GetPixel( i - w / 2 , j + h / 2 ))  //Shift image a fourth  2 to 3
                                        | true , false -> bitMap.SetPixel(i , j, bitMapGray.GetPixel( i + w / 2 , j - h / 2 ))  //Shift image a fourth  3 to 2
                                        | false, false -> bitMap.SetPixel(i , j, bitMapGray.GetPixel( i - w / 2 , j - h / 2 ))  //Shift image a fourth  4 to 1
                        ] |> Async.RunSynchronously |> ignore
        bitMap


    let imgBitmapImage (newBmp : Bitmap) = 
        let newBmpImg = 
            use stream = new MemoryStream()
            newBmp.Save(stream, ImageFormat.Png)
            stream.Position <- 0L
            let result = System.Windows.Media.Imaging.BitmapImage()
            result.BeginInit()
            result.StreamSource <- stream
            result.CacheOption <- BitmapCacheOption.OnLoad
            result.EndInit()
            result
        newBmpImg   
        
        
    let mathFFT2Parallel(input : Complex[,]) = MutableFFT2.sw.Reset()
                                               MutableFFT2.sw.Start()  
                                               let mutable mfft2 : Complex[,] = null
                                               do mfft2 <-input |> MutableFFT2.fft2Parallel 
                                               MutableFFT2.sw.Stop()
                                               MutableFFT2.fftTime <- MutableFFT2.sw.ElapsedMilliseconds
                                               mfft2    

    let mathFFT2Async(input : Complex[,]) = MutableFFT2.sw.Reset()
                                            MutableFFT2.sw.Start()  
                                            let mutable mfft2 : Complex[,] = null
                                            do mfft2 <-input |> MutableFFT2.fft2Async 
                                            MutableFFT2.sw.Stop()
                                            MutableFFT2.fftTime <- MutableFFT2.sw.ElapsedMilliseconds
                                            mfft2  


    member x.FFTtime = MutableFFT2.fftTime
    
    member x.ToFFT2FuncAsync(image : Bitmap) = image |> toComplexGrayArray |>  fft2FuncAsync |> toGrayLogBitmap |> shiftImage 
    member x.ToFFT2FuncParallel(image : Bitmap) = image |> toComplexGrayArray |>  fft2FuncParallel |> toGrayLogBitmap |> shiftImage 


    member x.ToFFT2Async(image : Bitmap) = image |>  toComplexGrayArray |> MutableFFT2.fft2Async |>  toGrayLogBitmap |> shiftImage  
    member x.ToFFT2Parallel(image : Bitmap) = image |>  toComplexGrayArray |> MutableFFT2.fft2Parallel |>  toGrayLogBitmap |> shiftImage
    
    member x.ToFFT2MathParallel(image : Bitmap) = image |>  toComplexGrayArray |> mathFFT2Parallel |>  toGrayLogBitmap |> shiftImage
    member x.ToFFT2MathAsync(image : Bitmap) = image |>  toComplexGrayArray |> mathFFT2Async |>  toGrayLogBitmap |> shiftImage
       
    member x.ImgBitmapImage(newBmp) = imgBitmapImage (newBmp) 