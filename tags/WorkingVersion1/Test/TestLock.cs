using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Test
{
    class TestLock
    {
        static object o = new object();
        static Thread t1 = new Thread(new ThreadStart(proc1));
        static Thread t2 = new Thread(new ThreadStart(proc1));
        static int i = 0;

        static void Main()
        {
            t1.Name = "T1";
            t2.Name = "T2";
            t1.Start();
            t2.Start();
            System.Console.ReadLine();
        }
        static void proc1()
        {
            /*lock (o)
            {*/
                i = i+1;
                System.Console.WriteLine(Thread.CurrentThread.Name + ": " + i.ToString());
            //}
        }
    }
}
