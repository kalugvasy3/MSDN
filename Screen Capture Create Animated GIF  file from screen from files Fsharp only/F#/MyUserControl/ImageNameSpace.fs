namespace ImageNameSpace

open System.Windows.Media.Imaging
open System.Drawing
open System
open System.IO
open System.Drawing.Imaging
open System.Windows
open System.Windows.Media
open System.Windows.Threading
open System.Windows.Interop
open System.Runtime.InteropServices
open System.Windows.Forms


/// Base on msdn project  https://code.msdn.microsoft.com/windowsdesktop/F-WPF-image-cropper-app-6febcce3

type ImgWrapper = { Bitmap : Bitmap ; BitmapImage : BitmapImage }

type Img ()  =       
    
    let mutable image : ImgWrapper = {Bitmap = null ; BitmapImage = null }

    let load (path) = 
        image <- { Bitmap = Bitmap.FromFile(path) :?> Bitmap;  BitmapImage = BitmapImage(Uri(path))}
        image

    // Save image like "image format" 
    let saveImageToFormat(path : string, format : string) = 
        match format.ToUpper() with
        | "PNG" -> image.Bitmap.Save(path, ImageFormat.Png)
        | "BMP" -> image.Bitmap.Save(path, ImageFormat.Bmp)
        | "GIF" -> image.Bitmap.Save(path, ImageFormat.Gif)
        | "JPEG" -> image.Bitmap.Save(path, ImageFormat.Jpeg)
        | "TIFF" -> image.Bitmap.Save(path, ImageFormat.Tiff)
        | "WMF" -> image.Bitmap.Save(path, ImageFormat.Wmf)
        | "RAW" -> image.Bitmap.Save(path, image.Bitmap.RawFormat)
        | "" -> image.Bitmap.Save(path, ImageFormat.Bmp)
        | _ -> ignore()

    // Crops "Bitmap" and rebuild "BitMapImage" image 
    // DoesNotChange original Image
    let croppedByOriginImg x y width height  = 
        let newBmp = image.Bitmap.Clone(Rectangle(x, y, width, height), image.Bitmap.PixelFormat)
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
        { Bitmap = newBmp
          BitmapImage = newBmpImg }

    // Create "BitmapImage" base on "Bitmap" 
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
        image <- { Bitmap = newBmp; BitmapImage = newBmpImg }
        image  

    let loadFromStream(stream : IO.Stream) =  
        imgWrapperBitmap(Bitmap.FromStream(stream) :?> Bitmap)
 
    let mutable posP = new System.Drawing.Point(0,0)

    // "BitmapImage" was changed - synchronize "Bitmap" 
    let imgWrapperBitmapImage (bitmapImage : BitmapImage) = 
        let newBitmapImage =  new Bitmap(bitmapImage.StreamSource)  
        image <- { Bitmap = newBitmapImage; BitmapImage = bitmapImage }   
        image 

    let cursorBmp() = 
        let cursorSize = Cursor.Current.Size.Height
        let cursor = Cursor.Current
        let img = new Bitmap(cursorSize, cursorSize)
        use g = Graphics.FromImage(img)
        do cursor.Draw(g, new Rectangle(0,0, cursorSize, cursorSize))
        img

    let deltaAdjW = (float)System.Windows.SystemParameters.Border  + (float)System.Windows.SystemParameters.FixedFrameVerticalBorderWidth 
    let deltaAdjH = (float)System.Windows.SystemParameters.Border  + (float)System.Windows.SystemParameters.FixedFrameHorizontalBorderHeight 

    // Capture "Screen" base on absolute position 
    let croppedByCurrentSelected (this : Window, blnCursor : bool)  =  
        let leftAbsolute = this.Left  + 2.0 * deltaAdjW
        let topAbsolute = this.Top + 2.0 * deltaAdjH  
        let width = this.Width     - 4.0 * deltaAdjW
        let height = this.Height   - 4.0 * deltaAdjH            
        let mutable bmp = new Bitmap((int)width, (int)height, Imaging.PixelFormat.Format32bppArgb)
        use mutable  g = Graphics.FromImage(bmp)
        do g.CopyFromScreen((int)leftAbsolute,(int)topAbsolute,0,0, new System.Drawing.Size((int)width, (int)height), CopyPixelOperation.SourceCopy)              
 
        if blnCursor then
            do posP <- Control.MousePosition
            let x = posP.X
            let y = posP.Y
            if x > (int)leftAbsolute  && x < (int)leftAbsolute + (int)width then
                if y > (int)topAbsolute && y < (int)topAbsolute + (int)height then
                   do this.Dispatcher.Invoke(fun _ -> g.DrawImage(cursorBmp(), x - (int)leftAbsolute, y - (int)topAbsolute))
 
        let newBmpImg = 
            use stream = new MemoryStream()
            bmp.Save(stream, ImageFormat.Png)
            stream.Position <- 0L
            let result = BitmapImage()
            result.BeginInit()
            result.StreamSource <- stream
            result.CacheOption <- BitmapCacheOption.OnLoad
            result.EndInit()
            result
        image <- { Bitmap = bmp ; BitmapImage = newBmpImg }
        image

    
    let imgSpecificSize(w : int , h : int, blnFill : bool) =       
        let newBmpFill = new Bitmap(image.Bitmap, w ,h)
        let newBmpUniform = let kw = (float)image.Bitmap.Width / (float)w  // koeff  w
                            let hr = (float)image.Bitmap.Height / (float)h // koeff  h
                            let wr = (kw / hr)
                            let bmpTmp = match wr >= 1.0 with
                                         | true -> (new Bitmap(image.Bitmap, int ((float)w * wr) , h)).Clone(Rectangle((int ((float)w * wr) - w ) / 2, 0, w, h), image.Bitmap.PixelFormat)
                                         | false  -> (new Bitmap(image.Bitmap, w , int ((float) h / wr))).Clone(Rectangle(0, (int ((float) h / wr) - h) / 2 , w, h), image.Bitmap.PixelFormat)
                            bmpTmp
        match blnFill with
        | true -> newBmpFill
        | false -> newBmpUniform



    let convertToSource () =
        let bitmap = image.Bitmap
        let mutable bitmapData : BitmapData = null
        do bitmapData <- bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat)
        let mutable bitmapSource : BitmapSource = null
        do bitmapSource <- BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96.0, 96.0, PixelFormats.Bgr24, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
        do bitmap.UnlockBits(bitmapData)
        bitmapSource

    member z.Load (path) = load path

    member z.SaveImageToFormat(fileName : string, format : string) = saveImageToFormat(fileName, format)
    member z.Image with get() = image and set(v) = image <- v   

    member z.ImgBitmap with get() = image.Bitmap and set(v) = image <- imgWrapperBitmap(v)
   
    member z.ImgSpecificSize(w ,h , blnFill)  = imgSpecificSize(w, h, blnFill)  // Scale existing Bitmap image to given size...  (does not change original "image")

    member z.ImgBitmapImage with get() = image.BitmapImage and set(v) = image <- imgWrapperBitmapImage (v)

    member z.CroppedByOriginImg (x,y,width,height) =  croppedByOriginImg x y width height  
    member z.CroppedByCurrentSelected (this, blnCursor) = croppedByCurrentSelected (this, blnCursor )                                    
                                          
    member z.LoadFromStream(stream : IO.Stream) = loadFromStream(stream : IO.Stream)
    member z.ConvertToSource () = convertToSource ()  
    
    member z.PositionP with get() = posP
  