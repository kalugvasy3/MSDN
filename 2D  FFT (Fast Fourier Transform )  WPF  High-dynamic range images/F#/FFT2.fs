// Fast Fourier Transforms 2 Dimensional (FFT2) 

// Base on original code, see http://www.fssnip.net/dC  (for List)  
// Thanks Kaspar              http://www.fssnip.net/authors/Kaspar
// Compare with C#            http://www.lomont.org/Software/Misc/FFT/LomontFFT.html 

// Theory of 2D FFT here      http://paulbourke.net/miscellaneous/dft/

namespace Image

module FFT2 =
    open System
    open System.Numerics
    open System.Diagnostics
    open System.Threading.Tasks

    
    let fftF (input : Complex[]) = FFT1.forkF(input) |>  Array.iteri(fun i c -> do input.[i] <- c)   // using false for compatible with above
                                   input   

    let fftR (input : Complex[]) = FFT1.forkR(input) |>  Array.iteri(fun i c -> do input.[i] <- c)   // using false for compatible with above
                                   input  

    let mutable fft2Time = 0L
    let mutable forward : Complex [,] = null   // Save Forwarded Complex Array (will be used for backward FFT)

    let fft2F  (input : Complex [,] ) = 
        let sW = new Stopwatch()
        sW.Start()
               
        let d0 = input.GetLength(0) 
        let d1 = input.GetLength(1) 
        
        let parallelRow() = Parallel.For(0, d0 , (fun k -> input.[k , *] |> fftF |> Array.Parallel.iteri(fun i c -> input.[k , i ] <- c ) )) 
        let parallelColumn() = Parallel.For(0, d1 , (fun l -> input.[* , l] |> fftF |> Array.Parallel.iteri(fun i c -> input.[i , l ] <- c ) )) 
        
        do Task.Factory.StartNew(fun () -> parallelRow()).Wait() 
        do Task.Factory.StartNew(fun () -> parallelColumn()).Wait() 
        
        
        sW.Stop()
        fft2Time <- sW.ElapsedMilliseconds
        forward <-  input.Clone() :?> Complex[,]
        input   
               
    let fft2R  (input : Complex [,] ) = 
        let sW = new Stopwatch()
        sW.Start()
        
        let d0 = input.GetLength(0) 
        let d1 = input.GetLength(1) 
        
        let parallelRow() = Parallel.For(0, d0 , (fun k -> input.[k , *] |> fftR |> Array.Parallel.iteri(fun i c -> input.[k , i ] <- c ) )) 
        let parallelColumn() = Parallel.For(0, d1 , (fun l -> input.[* , l] |> fftR |> Array.Parallel.iteri(fun i c -> input.[i , l ] <- c ) )) 
        
        do Task.Factory.StartNew(fun () -> parallelRow()).Wait() 
        do Task.Factory.StartNew(fun () -> parallelColumn()).Wait()       
        
        sW.Stop()
        fft2Time <- sW.ElapsedMilliseconds
        input                                                

 
