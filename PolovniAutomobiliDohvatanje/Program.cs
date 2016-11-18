using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace Procode.PolovniAutomobili.Dohvatanje
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase servicesToRun = new PolAutSrv();

            ServiceBase.Run(servicesToRun);
        }
    }    
}
