using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PerformanceOfLoopsCsharp
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();
        static long[] testArray = new long[1_000_000];
        static long result = 0L;


        static void testForIn()
        {
            result = 0L;

            sw.Reset();
            sw.Start();

            foreach (long l in testArray)
            {
                result = result + l;
            }

            sw.Stop();

            Console.WriteLine(" <ForIn> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }




        static void testForTo()
        {
            result = 0L;

            sw.Reset();
            sw.Start();

            for (int i = 0; i < testArray.Length; i++)
            {
                result = result + testArray[i];
            }

            sw.Stop();

            Console.WriteLine(" <ForTo> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }




        static void testDo()
        {
            result = 0L;

            sw.Reset();
            sw.Start();

            int len = testArray.Length;
            int i = 0;
            do
            {
                result = result + testArray[i];
                i = i + 1;
            } while (i < len);
            sw.Stop();

            Console.WriteLine(" <Do> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }



        static void testWhile()
        {
            result = 0L;

            sw.Reset();
            sw.Start();

            int len = testArray.Length;
            int i = 0;
            while (i < len)
            {
                result = result + testArray[i];
                i = i + 1;
            }
            sw.Stop();

            Console.WriteLine(" <While> - Adding Time - " + sw.ElapsedTicks.ToString("0,000,000") + " ticks. (result=" + result.ToString("0,0") + ")");
            Console.WriteLine();
        }



        static void runAllTests()
        {
            testForIn();
            testForTo();

            testDo();
            testWhile();

        }



        static void Main(string[] args)
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                testArray[i] = (long)i * (long)i;
            }

            runAllTests();
            Console.ReadLine();

        }
    }
}
