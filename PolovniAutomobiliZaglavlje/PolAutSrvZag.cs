using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace PolovniAutomobiliZaglavlje
{
    class PolAutSrvZag : ServiceBase
    {
        Obrada obrada;
        public PolAutSrvZag()
        {
            ServiceName = Properties.Settings.Default.NazivServisa;
            obrada = new Obrada();
        }
        protected override void OnStart(string[] args)
        {
            // prvo nesto moje
            base.OnStart(args);
            obrada.Start();
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
        protected override void OnPause()
        {
            base.OnPause();
        }
    }
}
