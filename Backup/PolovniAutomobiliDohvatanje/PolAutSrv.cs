using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    class PolAutSrv : ServiceBase
    {
        GlavnaObrada obrada;

        public PolAutSrv()
        {
            InitializeComponent();
            ServiceName = Common.Properties.Settings.Default.NazivServisa;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args); // Da li je ovo potrebno?
            //pozovi thread
            obrada = new GlavnaObrada();
            try
            {
                string poruka = "Pokrecem servis.";
                //EventLogger.WriteEventInfo(poruka);
                Dnevnik.PisiSaThredom(poruka);
                
                obrada.Pokreni();

                poruka = "Servis je pokrenut.\n"+obrada.ToString(); 
                EventLogger.WriteEventInfo(poruka);
                Dnevnik.PisiSaThredom(poruka);
            }
            catch (Exception ex)
            {
                string poruka = "Nisam uspeo da pokrenem servis.";
                EventLogger.WriteEventError(poruka, ex);
                Dnevnik.PisiSaThredomGreska(poruka);
            }
        }

        protected override void OnStop()
        {
            obrada.Zaustavi();
            obrada = null;

            string poruka = "Servis je zaustavljen.";
            EventLogger.WriteEventInfo(poruka);
            Dnevnik.PisiSaThredom(poruka);

            Dnevnik.Isprazni();
            base.OnStop();
        }

        private void InitializeComponent()
        {
            // 
            // PolAutSrv
            // 
            this.AutoLog = false;
            this.ServiceName = "PolovniAutomobili";
        }

    }
}
