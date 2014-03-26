using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace PolovniAutomobiliZaglavlje
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new PolAutSrvZag() };

            ServiceBase.Run(servicesToRun);
        }
    }
}
