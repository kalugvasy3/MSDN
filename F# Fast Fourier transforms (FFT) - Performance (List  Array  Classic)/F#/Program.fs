// Fast Fourier transforms (FFT) 

// base on original code, see http://www.fssnip.net/dC  (for List) 
// Compare with C#  http://www.lomont.org/Software/Misc/FFT/LomontFFT.html 

open System
open System.Numerics
open System.Diagnostics

// ARRAY
let rec fftA = function
  | [||]  -> [||]
  | [|x|] -> [|x|] 
  | x ->
    x
    |> Array.mapi (fun i c -> (i % 2 = 0, c))                                                           
    |> Array.partition fst                                                                              
    |> fun (even, odd) -> fftA (Array.map snd even), fftA (Array.map snd odd)                             
    ||> Array.mapi2 (fun i even odd -> 
        let btf = odd * Complex.FromPolarCoordinates(1., -2. * Math.PI * (float i / float x.Length ))
        even + btf, even - btf)
    |> Array.unzip
    ||> Array.append 
  
// LIST  
let rec fftL = function
  | []  -> []
  | [x] -> [x] 
  | x ->
    x
    |> List.mapi (fun i c -> i % 2 = 0, c)
    |> List.partition fst
    |> fun (even, odd) -> fftL (List.map snd even), fftL (List.map snd odd)
    ||> List.mapi2 (fun i even odd -> 
        let btf = odd * Complex.FromPolarCoordinates(1., -2. * Math.PI * (float i / float x.Length ))
        even + btf, even - btf)
    |> List.unzip
    ||> List.append


//  Re-written FORTRAN to F# vkalugin (see links)
//
//  www.geo.mtu.edu/~jdiehl/Potential_Fields/fork.f
//  www.geo.mtu.edu/~jdiehl/Homework4550/dft.for
//  geocities.ws/rashvand/fortran.html
//  read.pudn.com/downloads153/sourcecode/math/669570/FFT.FOR__.htm

let fork (cx : Complex[]  , forward : bool) =
       
    let mutable carg = Complex.Zero
    let mutable cw = Complex.Zero
    let mutable ctemp = Complex.Zero
       
    let mutable lx = cx.GetLength(0)
    let mutable signi : float = 1.0  // revers transform
    let mutable sc : float = 1.0 
        
    if forward then
        signi <- -1.0 
        sc <- 1.0 / (float lx)
        
    let mutable j = 1
    let mutable m = 0

    for i = 1 to lx do
            if (i <= j) then 
                ctemp <- cx.[j - 1] * Complex(sc, 0.0)
                cx.[j - 1] <- cx.[i - 1] * Complex(sc, 0.0)
                cx.[i - 1] <- ctemp
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
                ctemp <- cw * cx.[ipl - 1]
                cx.[ipl - 1] <- cx.[i - 1] - ctemp
                cx.[i - 1] <- cx.[i - 1] + ctemp
        l <- istep   
    cx



let  inputA = [|for x in 0. .. 65535.  -> Complex(sin(2.0 * Math.PI / 16.0 * x) , 0.0) |]   // Array
let  inputL =  [for x in 0. .. 65535. -> Complex(sin(2.0 * Math.PI / 16.0 * x) , 0.0) ]     // List
    


[<EntryPoint>]
let main argv = 

    Console.WriteLine( "---Wait About 9 (for slowest process) sec --- " ) 

    let sW = new Stopwatch()

    sW.Start()
    inputL |> fftL |> ignore // List.iter(fun z -> printfn "%A" z.Magnitude) 
    sW.Stop()
    
    Console.WriteLine( "---List--- " + sW.ElapsedMilliseconds.ToString() + " ms.")

    sW.Reset()

    sW.Start()
    inputA |> fftA |> ignore //Array.iter(fun z -> printfn "%A" z.Magnitude) 
    sW.Stop()

    Console.WriteLine( "---Array---" + sW.ElapsedMilliseconds.ToString() + " ms.")

    sW.Reset()

    sW.Start()
    fork(inputA , false) |>  ignore // Array.iter(fun z -> printfn "%A" z.Magnitude) 
    sW.Stop()

    Console.WriteLine( "---Classic FFT F# ---" + sW.ElapsedMilliseconds.ToString() + " ms.")
    Console.ReadLine() |> ignore

    0 // return an integer exit code
