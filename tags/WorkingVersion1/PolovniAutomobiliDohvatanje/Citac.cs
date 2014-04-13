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
        protected Common.Http.ListaStrana procitaneStrane;    // Zajednicki objekat/resurs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procitaneStrane"></param>
        /// <param name="threadId"></param>
        /// <param name="threadName"></param>
        /// <param name="brojCitaca">brojCitaca sluzi da se odredi koliko ce vodecih nula da ispise.</param>
        public Citac(ref Common.Http.ListaStrana procitaneStrane, int threadId, string threadName, int brojCitaca)
        {
            this.procitaneStrane = procitaneStrane;
            Radnik = new Thread(new ThreadStart(Obrada));
            Radnik.Name = threadName + threadId.ToString(new String('0', brojCitaca.ToString().Length));
        }

        protected abstract void RadiObradu();

        public bool radi = true;

        /// <summary>
        /// Starter procedura za Thread. Ona poziva proceduru RadiObradu u kojoj se izvrsava petlja obrade.
        /// </summary>
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
            procitaneStrane.NeRadi("Čitač");
        }
    }
}
