namespace FsharpLibrary

open System
open System.Numerics
open System.Drawing
open System.Drawing.Imaging
open System.IO
open System.Windows.Media.Imaging
open System.Diagnostics
open System.Threading
open System.Threading.Tasks

module MutableFFT2 =

    let mutable fftTime  : int64 = 0L
    let mutable sw = new Stopwatch()

    let fft2Async (cx : Complex[,]) =
        
        sw.Reset()
        sw.Start()

        let mutable carg = Complex.Zero
        let mutable cw = Complex.Zero
       
        let lx = cx.GetLength(0)
        let mutable signi : float = -1.0  // forward transform
        let mutable sc : float = 1.0 / float lx
        
        let scComplex = Complex(sc, 0.0)

        // All Columns 

        
        Async.Parallel  [for col = 0 to lx - 1 do
            
                            let mutable ctemp = Complex.Zero      
                            let mutable j = 1
                            let mutable m = 0

                            for i = 1 to lx do
                                    if (i <= j) then 
                                        ctemp <- cx.[j - 1, col] * scComplex
                                        cx.[j - 1, col] <- cx.[i - 1, col] * scComplex
                                        cx.[i - 1, col] <- ctemp
                                    m <- lx / 2
                
                                    let rec l3() =
                                        if (j > m) then
                                            j <- j - m
                                            m <- m / 2
                                            if (m >= 1 ) then l3() 
                                    l3()
                                    j <- j + m 

                            let mutable l = 1
                            let mutable istep = 2 * l

                            while l < lx do
                                istep <- 2 * l
                                for m = 1 to l do
                                    carg <- Complex.ImaginaryOne * Complex(Math.PI * signi * (float m - 1.0) / float l , 0.0) 
                                    cw <- Complex.Exp(carg)
                                    for i in m..istep..lx do
                                        let ipl = i + l
                                        ctemp <- cw * cx.[ipl - 1,col]
                                        cx.[ipl - 1,col] <- cx.[i - 1,col] - ctemp
                                        cx.[i - 1,col] <- cx.[i - 1,col] + ctemp
                                l <- istep 

                        ]   |> Async.RunSynchronously
                            |> ignore                        
               
// All Rows 

        Async.Parallel  [for row = 0 to lx - 1 do
                           
                            let mutable ctemp = Complex.Zero      
                            let mutable j = 1
                            let mutable m = 0

                            for i = 1 to lx do
                                    if (i <= j) then 
                                        ctemp <- cx.[row, j - 1] * scComplex
                                        cx.[row, j - 1 ] <- cx.[row,i - 1 ] * scComplex
                                        cx.[row,i - 1 ] <- ctemp
                                    m <- lx / 2
                
                                    let rec l3() =
                                        if (j > m) then
                                            j <- j - m
                                            m <- m / 2
                                            if (m >= 1 ) then l3() 
                                    l3()
                                    j <- j + m 

                            let mutable l = 1
                            let mutable istep = 2 * l

                            while l < lx do
                                istep <- 2 * l
                                for m = 1 to l do
                                    carg <- Complex.ImaginaryOne * Complex(Math.PI * signi * (float m - 1.0) / float l , 0.0) 
                                    cw <- Complex.Exp(carg)
                                    for i in m..istep..lx do
                                        let ipl = i + l
                                        ctemp <- cw * cx.[row,ipl - 1 ]
                                        cx.[row,ipl - 1 ] <- cx.[row,i - 1 ] - ctemp
                                        cx.[row,i - 1 ] <- cx.[row,i - 1 ] + ctemp
                                l <- istep   

                        ]    |> Async.RunSynchronously
                             |> ignore  

        sw.Stop()
        fftTime <- sw.ElapsedMilliseconds

        cx
 
//----------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------


    let fft2Parallel (cx : Complex[,]) =
       
        let lx = cx.GetLength(0)
        let mutable signi : float = -1.0  // forward transform
        let mutable sc : float = 1.0 / float lx  

        let scComplex = Complex(sc, 0.0)
        
        // FFT for one Column
        let oneColumn(col : int) =
            let mutable carg = Complex.Zero
            let mutable cw = Complex.Zero           

            let mutable ctemp = Complex.Zero      
            let mutable j = 1
            let mutable m = 0

            for i = 1 to lx do
                    if (i <= j) then 
                        ctemp <- cx.[j - 1, col] * scComplex
                        cx.[j - 1, col] <- cx.[i - 1, col] * scComplex
                        cx.[i - 1, col] <- ctemp
                    m <- lx / 2
                
                    let rec l3() =
                        if (j > m) then
                            j <- j - m
                            m <- m / 2
                            if (m >= 1 ) then l3() 
                    l3()
                    j <- j + m 

            let mutable l = 1
            let mutable istep = 2 * l

            while l < lx do
                istep <- 2 * l
                for m = 1 to l do
                    carg <- Complex.ImaginaryOne * Complex(Math.PI * signi * (float m - 1.0) / float l , 0.0) 
                    cw <- Complex.Exp(carg)
                    for i in m..istep..lx do
                        let ipl = i + l
                        ctemp <- cw * cx.[ipl - 1, col]
                        cx.[ipl - 1, col] <- cx.[i - 1, col] - ctemp
                        cx.[i - 1, col] <- cx.[i - 1, col] + ctemp
                l <- istep 
 
         // FFT for one Row                                     
        let oneRow(row : int) =
            let mutable carg = Complex.Zero
            let mutable cw = Complex.Zero           

            let mutable ctemp = Complex.Zero      
            let mutable j = 1
            let mutable m = 0

            for i = 1 to lx do
                    if (i <= j) then 
                        ctemp <- cx.[row, j - 1] * scComplex
                        cx.[row, j - 1] <- cx.[row, i - 1] * scComplex
                        cx.[row, i - 1] <- ctemp
                    m <- lx / 2
                
                    let rec l3() =
                        if (j > m) then
                            j <- j - m
                            m <- m / 2
                            if (m >= 1 ) then l3() 
                    l3()
                    j <- j + m 

            let mutable l = 1
            let mutable istep = 2 * l

            while l < lx do
                istep <- 2 * l
                for m = 1 to l do
                    carg <- Complex.ImaginaryOne * Complex(Math.PI * signi * (float m - 1.0) / float l , 0.0) 
                    cw <- Complex.Exp(carg)
                    for i in m..istep..lx do
                        let ipl = i + l
                        ctemp <- cw * cx.[row, ipl - 1]
                        cx.[row, ipl - 1] <- cx.[row, i - 1] - ctemp
                        cx.[row, i - 1] <- cx.[row, i - 1] + ctemp
                l <- istep 
 
 
        let parallelColumn() = Parallel.For(0, lx , (fun col -> oneColumn(col) ))                                
        let parallelRow() = Parallel.For(0, lx , (fun row -> oneRow(row) ))     

        sw.Reset()
        sw.Start()

        do Task.Factory.StartNew(fun () -> parallelColumn()).Wait() 
        do Task.Factory.StartNew(fun () -> parallelRow()).Wait()            

        sw.Stop()

        fftTime <- sw.ElapsedMilliseconds
        cx