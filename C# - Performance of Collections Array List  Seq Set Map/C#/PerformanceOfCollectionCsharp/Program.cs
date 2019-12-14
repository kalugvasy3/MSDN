using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;



namespace PerformanceOfCollectionCsharp
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();

        static void testArr()
        {
            long result = 0L;
            sw.Reset();
            sw.Start();
            long[] testArray = new long[1_000_000];

            long startBytes = System.GC.GetTotalMemory(true);
            for (int i = 0; i <= testArray.Length - 1; i++)
            {
                testArray[i] = (long) i * (long) i;
            }

            long stopBytes = System.GC.GetTotalMemory(true);
            GC.KeepAlive(testArray);  // This ensure a reference to object keeps object in memory
            sw.Stop();

            Task.Delay(200);

            string timeInitArray = sw.ElapsedTicks.ToString("0,000,000") + " ticks.   Memory Used - " + ((stopBytes - startBytes)).ToString("0,0");
            Console.WriteLine(" <Array> - Init Time - " + timeInitArray);

            sw.Reset();
            sw.Start();

            for (int i = 0; i <= testArray.Length - 1; i++)
            {
                result = result + testArray[i];
            };

            sw.Stop();

            Console.WriteLine(" <Array>-Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }

        static void testList()
        {
            long result = 0L;
            sw.Reset();
            sw.Start();
            List<long> testList = new  List<long> ();

            long startBytes = System.GC.GetTotalMemory(true);
            for (int i = 0; i < 1_000_000 ; i++)
            {
                testList.Add((long)i * (long)i);
            }

            long stopBytes = System.GC.GetTotalMemory(true);
            GC.KeepAlive(testList);  // This ensure a reference to object keeps object in memory

            sw.Stop();

            Task.Delay(200);

            string timeInitList = sw.ElapsedTicks.ToString("0,000,000") + " ticks.   Memory Used - " + ((stopBytes - startBytes)).ToString("0,0");
            Console.WriteLine(" <List> - Init Time - " + timeInitList);

            sw.Reset();
            sw.Start();

            for (int i = 0; i <= testList.Count - 1; i++)
            {
                result = result + testList[i];
            };

            sw.Stop();

            Console.WriteLine(" <List>-Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }


        static void testSeq()
        {
            long result = 0L;
            sw.Reset();
            sw.Start();
            //Sequence testSeq = new Sequence();

            long startBytes = System.GC.GetTotalMemory(true);

            IEnumerable<long> testSeq = Enumerable.Range(1, 1_000_000).Select(x => (long)x * (long)x);

            long stopBytes = System.GC.GetTotalMemory(true);
            GC.KeepAlive(testSeq);  // This ensure a reference to object keeps object in memory

            sw.Stop();

            Task.Delay(200);

            string timeInitSeq = sw.ElapsedTicks.ToString("0,000,000") + " ticks.   Memory Used - " + ((stopBytes - startBytes)).ToString("0,0");
            Console.WriteLine(" <Seq> - Init Time - " + timeInitSeq);

            sw.Reset();
            sw.Start();

            result = testSeq.Aggregate(0L, (total, next) => total + next); 

            sw.Stop();

            Console.WriteLine(" <Seq>-Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }

        static void testSet()
        {
            long result = 0L;
            sw.Reset();
            sw.Start();
            //Sequence testSeq = new Sequence();

            long startBytes = System.GC.GetTotalMemory(true);
            HashSet<long> testSet  = new HashSet<long>();
            for (int i = 0; i < 1_000_000; i++)
            {
                testSet.Add((long)i * (long)i);
            }

            long stopBytes = System.GC.GetTotalMemory(true);
            GC.KeepAlive(testSet);  // This ensure a reference to object keeps object in memory

            sw.Stop();

            Task.Delay(200);

            string timeInitSeq = sw.ElapsedTicks.ToString("0,000,000") + " ticks.   Memory Used - " + ((stopBytes - startBytes)).ToString("0,0");
            Console.WriteLine(" <Set> - Init Time - " + timeInitSeq);

            sw.Reset();
            sw.Start();

            foreach (long x in testSet)

            {
                result = result + x;
            }

            sw.Stop();

            Console.WriteLine(" <Set>-Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }

        static void testDictionary()
        {
            long result = 0L;
            sw.Reset();
            sw.Start();
            //Sequence testSeq = new Sequence();

            long startBytes = System.GC.GetTotalMemory(true);
            Dictionary<int, long> testDic = new Dictionary<int, long>();
            for (int i = 0; i < 1_000_000; i++)
            {
                testDic.Add(i, (long)i * (long)i);
              }

            long stopBytes = System.GC.GetTotalMemory(true);
            GC.KeepAlive(testDic);  // This ensure a reference to object keeps object in memory

            sw.Stop();

            Task.Delay(200);

            string timeInitSeq = sw.ElapsedTicks.ToString("0,000,000") + " ticks.   Memory Used - " + ((stopBytes - startBytes)).ToString("0,0");
            Console.WriteLine(" <Dic> - Init Time - " + timeInitSeq);

            sw.Reset();
            sw.Start();

            foreach (KeyValuePair<int, long> x in testDic)
            {
                result = result + x.Value;
            }

            sw.Stop();

            Console.WriteLine(" <Dic>-Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }


        static void runAllTests()
        {
            testArr();
            testList();
            testSeq();
            testSet();
            testDictionary();
        }


        static void Main(string[] args)
        {
            runAllTests();
        }
    }
}



