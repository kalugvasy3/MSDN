
namespace Image

module FFT1 =

//  Re-written FORTRAN to F# Vasily Kalugin 

//  www.geo.mtu.edu/~jdiehl/Potential_Fields/fork.f
//  www.geo.mtu.edu/~jdiehl/Homework4550/dft.for
//  geocities.ws/rashvand/fortran.html
//  read.pudn.com/downloads153/sourcecode/math/669570/FFT.FOR__.htm    

    open System
    open System.Numerics

    let forkR (cx : Complex[] ) =
       
        let mutable carg = Complex.Zero
        let mutable cw = Complex.Zero
        let mutable ctemp = Complex.Zero
       
        let mutable lx = cx.GetLength(0)
        let mutable signi : float = 1.0  // revers transform  ... -1 forward
        let mutable sc : float = 1.0 
         
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


    let forkF (cx : Complex[] ) =
       
        let mutable carg = Complex.Zero
        let mutable cw = Complex.Zero
        let mutable ctemp = Complex.Zero
       
        let mutable lx = cx.GetLength(0)
        let mutable signi : float = -1.0 // forward
        let mutable sc : float = 1.0 / float lx   

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

