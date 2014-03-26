using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    class PisacZaglavlja
    {
        Thread Pisac;
        Common.Http.StranaLista procitaneStrane;    // Zajednicki objekat/resurs
        int threadId;
        Common.Http.Brojac brojac;
        private bool radi = true;   // uslov da se thread vrti
        public PisacZaglavlja(ref Common.Http.StranaLista straneZaglavlja, Common.Http.Brojac brojac, int threadId)
        {
            this.procitaneStrane = straneZaglavlja;
            Pisac = new Thread(new ThreadStart(Obrada));
            Pisac.Name = "PisacZaglavlja" + Common.Korisno.Korisno.IntUStrSaNulama(threadId, (int)Properties.Settings.Default.BrojPisacaZaglavlja);
            this.threadId = threadId;
            this.brojac = brojac;
        }

        private void Obrada()
        {
            Dnevnik.PisiSaThredom("Obrada pokrenuta.");
            try
            {
                while (radi)
                {
                    ProcitajZaglavlja();
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Greska pri pokretanju obrade zaglavlja", ex);
            }
            Dnevnik.PisiSaThredom("Obrada završena.");
            Dnevnik.Isprazni();
        }

        private void ProcitajZaglavlja()
        {
            Common.BrojacPisacaZaglavlja.UvecajBrojAktivnihPisacaZaglavlja();
            uint i;
            string adresa;
            i = brojac.Sledeci();
            try
            {
                adresa = Zaglavlje(i);
                Common.Http.Strana strana = new Common.Http.StranaZaglavlja(adresa);
                while (strana.Procitaj())
                {
                    procitaneStrane.Dodaj(strana);  // dodaje u deljenu listu u memoriju
                    if (!radi)
                        return;
                    i = brojac.Sledeci();
                    adresa = Zaglavlje(i);
                    strana = new Common.Http.StranaZaglavlja(adresa);
                }
                Dnevnik.PisiSaThredom("Više nema zaglavlja za čitanje.");
                brojac.Ponisti();   // ponistavam brojac da krene iz pocetka
            }
            catch (Exception ex)
            {
                string porukaGreske = "Citac zaglavlja nije uspesno zavrsio.";
                EventLogger.WriteEventError(porukaGreske, ex);
                Dnevnik.PisiSaThredom(porukaGreske);
            }
            finally
            {
                Common.BrojacPisacaZaglavlja.SmanjiBrojAktivnihPisacaZaglavlja();
            }
        }

        private string Zaglavlje(uint brojStrane)
        {
            return @"http://www.polovniautomobili.com/putnicka-vozila-26/" +
                (brojStrane - 1).ToString() +
                @"/?tags=&showold_pt=false&shownew_pt=false&brand=0&price_to=&tag_218_from=0&tag_218_to=0&selectedRegion=0&showoldnew=all";
        }

        public void Pokreni()
        {
            Pisac.Start();
        }

        private void WriteLine(string tekst)
        {
            System.Console.WriteLine(string.Format("{0}: {1}", Pisac.Name, tekst));
            //System.Console.WriteLine(string.Format("Pisac[{0}]: {1}", threadId, tekst));
        }
        public void Zaustavi()
        {
            radi = false;
            procitaneStrane.NeRadi();
        }
    }
}