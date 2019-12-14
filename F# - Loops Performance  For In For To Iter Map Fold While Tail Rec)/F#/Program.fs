open System
open System.Diagnostics

let sw = new Stopwatch()
let testArray : int64[] = Array.init 1_000_000 (fun i -> int64(i) * int64(i))  


let testForIn() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    for i in 0 .. testArray.Length - 1 do
        result <- result + testArray.[i]

    do sw.Stop()  

    do Console.WriteLine(" <ForIn> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()



let testForTo() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    for i = 0 to testArray.Length - 1 do
        result <- result + testArray.[i]

    do sw.Stop()  

    do Console.WriteLine(" <ForTo> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()

  
    
let testIter() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    testArray |> Array.iter (fun x -> result <- result + x)

    do sw.Stop()  

    do Console.WriteLine(" <Iter>  - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()


  
    
let testFold() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    let sumArray() = Array.fold (fun acc x -> acc + x) 0L  testArray 
    do result <- sumArray()

    do sw.Stop()  

    do Console.WriteLine(" <Fold>  - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()


    
let testMap() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    do testArray |> Array.map (fun x -> result <- result + x) |> ignore  

    do sw.Stop()  

    do Console.WriteLine(" <Map>  - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()


    
let testWhile() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    let len = testArray.Length
    let mutable i : int = 0
    while (i < len) do
        result <- result + testArray.[i]
        i <- i + 1

    do sw.Stop()  

    do Console.WriteLine(" <While> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()



    
let testRecTail() =
    let mutable result = 0L

    do sw.Reset()    
    do sw.Start()
    
    let len = testArray.Length
    let mutable i : int = 0

    let sumArray() =
        let rec sum (l)  =
            match l with
            | 0 -> 0L
            | _ -> result <- result + testArray.[l - 1] 
                   sum (l - 1)                   
        sum (len)           

    do sumArray() |> ignore    

    do sw.Stop()  

    do Console.WriteLine(" <Tail>  - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")" ) 
    do Console.WriteLine()



let runAllTests() =
    do testForIn()
    do testForTo()
    do testIter()
    do testFold()
    do testMap() 
    do testWhile()
    do testRecTail()

[<EntryPoint>]
let main argv = 
    do runAllTests()
    do Console.ReadLine() |> ignore
    0 // return an integer exit code
