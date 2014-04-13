using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Http;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    public abstract class Citac
    {
        Thread Radnik;
        protected Common.Http.StranaLista procitaneStrane;    // Zajednicki objekat/resurs
        protected int threadId;
        public Citac(ref Common.Http.StranaLista procitaneStrane, int threadId, string threadName)
        {
            this.procitaneStrane = procitaneStrane;
            Radnik = new Thread(new ThreadStart(Obrada));
            this.threadId = threadId;
            if (this is CitacZaglavlja)
            {
                Radnik.Name = threadName + Common.Korisno.Korisno.IntUStrSaNulama(threadId, (int)Properties.Settings.Default.BrojCitacaZaglavlja);
            }
            else if (this is CitacOglasa)
            {
                Radnik.Name = threadName + Common.Korisno.Korisno.IntUStrSaNulama(threadId, (int)Properties.Settings.Default.BrojCitacaOglasa);
            }
            else
                Radnik.Name = threadName + threadId.ToString();
        }        
        protected abstract void RadiObradu();
        public bool radi = true;
        private void Obrada()
        {
            Dnevnik.PisiSaThredom("Obrada pokrenuta.");
            try
            {
                RadiObradu();
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Greska u RadiObradu().", ex);
            }
            Dnevnik.PisiSaThredom("Obrada završena.", true);
        }

        public void Pokreni()
        {
            Radnik.Start();
        }
        public virtual void Zaustavi()
        {
            radi = false;
            procitaneStrane.NeRadi();
        }
    }
}
