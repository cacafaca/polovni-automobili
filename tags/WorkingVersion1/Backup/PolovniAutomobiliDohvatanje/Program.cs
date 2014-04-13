using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace PolovniAutomobiliDohvatanje
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new PolAutSrv() };

            ServiceBase.Run(servicesToRun);
        }
    }    
}
