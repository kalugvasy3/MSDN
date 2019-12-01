namespace Image

open System.Windows.Media.Imaging
open System.Drawing
open System
open System.IO
open System.Drawing.Imaging
open System.Numerics
open System.Threading.Tasks


// Base on msdn project  https://code.msdn.microsoft.com/windowsdesktop/F-WPF-image-cropper-app-6febcce3

type ImgWrapper = { Bitmap : Bitmap ; BitmapImage : BitmapImage }

type Img ()  =       
    
    let mutable image : ImgWrapper = {Bitmap = null ; BitmapImage = null }

    let loadPath (path) = 
        do image <- { Bitmap = Bitmap.FromFile(path) :?> Bitmap
                      BitmapImage = BitmapImage(Uri(path)) }
        image

    let loadUri (uri : Uri) =
        let bitmapImage = BitmapImage(uri)
        let newBitmap =  new Bitmap(bitmapImage.StreamSource)   
        do image <- { Bitmap = newBitmap
                      BitmapImage = bitmapImage }
        image    

    let imgWrapperBitmapImage (bitmapImage : BitmapImage) = 
        let newBitmap =  new Bitmap(bitmapImage.StreamSource)  
        do image <- { Bitmap = newBitmap
                      BitmapImage = bitmapImage } 
        image
          
    let imgWrapperBitmap ( newBmp : Bitmap) = 
        let newBmpImg = 
            use stream = new MemoryStream()
            newBmp.Save(stream, ImageFormat.Png)
            stream.Position <- 0L
            let result = BitmapImage()
            result.BeginInit()
            result.StreamSource <- stream
            result.CacheOption <- BitmapCacheOption.OnLoad
            result.EndInit()
            result          
        do image <- { Bitmap = newBmp
                      BitmapImage = newBmpImg }                     
        image


    let colorToGrayComplex (c : Color)  = 
        let gscale = (float c.R * 0.3) + (float c.G * 0.59) + (float c.B * 0.11)
        Complex(gscale, 0.0)


    // converts an image to Complex [,] - (gray) 
    let toComplexGrayArray (bmp : Bitmap) =
        let w = bmp.Width
        let h = bmp.Height
        let result = Array2D.init w h (fun i j -> colorToGrayComplex(bmp.GetPixel( i, j))) 
        result


    let mutable scaleLog = 1.0
    let mutable deltaLog = 0.0 
    let mutable delta = 0.0  
    let mutable min = + infinity
    let mutable max = - infinity

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


    let reverseColor (c: Complex ) = 
        let g = int ((c.Magnitude - min) / delta)  // - min => Contrast
        Color.FromArgb(255, g, g, g)

 
    let toGrayBitmap (c : Complex[,]) =
        let w = (c).GetLength(0)
        let h = (c).GetLength(1)
        let bitMap = new Bitmap(w, h)
        minMax(c)        
        for i = 0 to w - 1 do
            for j = 0 to h - 1 do bitMap.SetPixel(i, j, reverseColor((c).[i, j]))
        bitMap     
  

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


    let toGrayScaleImage () = image.Bitmap |> toComplexGrayArray |> toGrayBitmap |> imgWrapperBitmap |> ignore                                

    let toFFT2forward() = image.Bitmap |> toComplexGrayArray|> FFT2.fft2F |> toGrayLogBitmap |> imgWrapperBitmap |> ignore                           
    let toFFT2backward() = FFT2.forward |> FFT2.fft2R |> toGrayBitmap |> imgWrapperBitmap |> ignore  


    // Image as an image of quarters -----------------------------------------
    //    1 2
    //    3 4
   
    let swapImage() = 
        let w = (image.Bitmap).Width
        let h = (image.Bitmap).Height
        let bitMap = new Bitmap(w, h)
        for i = 0 to (w - 1) do
           for j = 0 to (h - 1)  do
               match (i < w / 2), (j < h / 2) with  
               | true , true ->  bitMap.SetPixel(i , j, image.Bitmap.GetPixel( (h / 2 - 1) - j , (w / 2 - 1) - i ))           // Swap 1   "/" symmetrical axis
               | false, true ->  bitMap.SetPixel(i , j, image.Bitmap.GetPixel(  h / 2 + j      ,  i - w / 2      ))           // Swap 2   "\"
               | true , false -> bitMap.SetPixel(i , j, image.Bitmap.GetPixel(  j - h / 2      ,  w / 2  + i     ))           // Swap 3   "\"
               | false, false -> bitMap.SetPixel(i , j, image.Bitmap.GetPixel(  h - (j - h / 2) - 1 ,  w - (i - w / 2) - 1 )) // Swap 4   "/"
        imgWrapperBitmap(bitMap) 


    let shiftImage() =   
        let w = (image.Bitmap).Width
        let h = (image.Bitmap).Height
        let bitMap = new Bitmap(w, h)
        for i = 0 to (w - 1) do
           for j = 0 to (h - 1)  do
               match (i < w / 2), (j < h / 2) with
               | true , true ->  bitMap.SetPixel(i , j, image.Bitmap.GetPixel( w / 2 + i , h / 2 + j ))  //Shift image a fourth  1 to 4
               | false, true ->  bitMap.SetPixel(i , j, image.Bitmap.GetPixel( i - w / 2 , j + h / 2 ))  //Shift image a fourth  2 to 3
               | true , false -> bitMap.SetPixel(i , j, image.Bitmap.GetPixel( i + w / 2 , j - h / 2 ))  //Shift image a fourth  3 to 2
               | false, false -> bitMap.SetPixel(i , j, image.Bitmap.GetPixel( i - w / 2 , j - h / 2 ))  //Shift image a fourth  4 to 1
        imgWrapperBitmap(bitMap) 

    // ------------------------------------------------------------------------


    member z.LoadPath (path) = loadPath path
    member z.LoadUri (uri) = loadUri uri

    member z.Image with get() = image and set(v) = image <- v   

    member z.ImgWrapperBitmapImage with get() = image.BitmapImage and 
                                        set(v) = imgWrapperBitmapImage (v) |> ignore

    member z.ImgWrapperBitmap with get() = image.Bitmap and
                                   set(v) = imgWrapperBitmap(v) |> ignore

    member z.ToGrayScaleImage() = toGrayScaleImage () 
    
    member z.ToFFT2forward()  = toFFT2forward() 
    member z.ToFFT2backward() = toFFT2backward() 
     
    member z.SwapImage() = swapImage() 
    member z.ShiftImage() = shiftImage()

    member z.ScaleLog with get() = scaleLog 
                       and set(v)= scaleLog <- v
                                   FFT2.forward |> toGrayLogBitmap |> imgWrapperBitmap |> ignore

    
