using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace FFT

{
    class Program
    {
        static void Main(string[] args)
        {
            Complex[] c = new Complex[65536];
            for (int i = 0; i < 65535; i++)
            {
                c[i] = new Complex(Math.Sin(2.0 * Math.PI / 16.0 * (float)i), 0.0);
            }

            Stopwatch sw = new Stopwatch();

            sw.Start();
            fork(c.Length,  c, true);
            sw.Stop();

            Console.WriteLine("---Classic FFT C# ---" + sw.ElapsedMilliseconds.ToString() + " ms.");
            Console.ReadLine();
        }



//  Re-written FORTRAN to C# vkalugin (see links)
//
//  www.geo.mtu.edu/~jdiehl/Potential_Fields/fork.f
//  www.geo.mtu.edu/~jdiehl/Homework4550/dft.for
//  geocities.ws/rashvand/fortran.html
//  read.pudn.com/downloads153/sourcecode/math/669570/FFT.FOR__.htm

        public static void fork(int lx, Complex[] cx, bool forward)
        {
            Complex carg, cw, ctemp;

            // reverse transform
            double signi = 1.0;
            double sc = 1.0;

            // forward transform
            if (forward)
            {
                signi = -1.0;
                sc = 1.0 / lx;
            }

            int j = 1;
            int m;

            for (int i = 1; i <= lx; i++)
            {
                if (i > j) goto l2;
                ctemp = cx[j - 1] * sc;
                cx[j - 1] = cx[i - 1] * sc;
                cx[i - 1] = ctemp;
                l2: m = lx / 2;
                l3: if (j <= m) goto l5;
                j = j - m;
                m = m / 2;
                if (m >= 1) goto l3;
                l5: j = j + m;
            }

            int l = 1;
            l6: int istep = 2 * l;
            for (m = 1; m <= l; m++)
            {
                carg = Complex.ImaginaryOne * Math.PI * signi * (m - 1) / l;
                cw = Complex.Exp(carg);
                for (int i = m; i <= lx; i += istep)
                {
                    int ipl = i + l;
                    ctemp = cw * cx[ipl - 1];
                    cx[ipl - 1] = cx[i - 1] - ctemp;
                    cx[i - 1] = cx[i - 1] + ctemp;
                }
            }
            l = istep;
            if (l < lx) goto l6;
            return;
        }


    }
}
