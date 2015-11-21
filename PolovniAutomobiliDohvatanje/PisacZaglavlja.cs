using System;
using System.Threading;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    class PisacZaglavlja
    {
        Thread Pisac;
        Common.Http.ListaStrana procitaneStrane;    // Zajednicki objekat/resurs        
        BarijeraZaPisce barijera;
        int threadId;
        Common.Http.Brojac brojacStranaZaglavlja;
        private bool radi = true;   // uslov da se thread vrti
        public PisacZaglavlja(ref Common.Http.ListaStrana straneZaglavlja, Common.Http.Brojac brojac, int threadId, ref BarijeraZaPisce barijera)
        {
            this.procitaneStrane = straneZaglavlja;
            Pisac = new Thread(new ThreadStart(Obrada));
            Pisac.Name = "PisacZaglavlja" + threadId.ToString(new String('0', Properties.Settings.Default.BrojPisacaZaglavlja.ToString().Length));
            
            this.threadId = threadId;
            this.brojacStranaZaglavlja = brojac;
            this.barijera = barijera;
        }

        private void Obrada()
        {
            Dnevnik.PisiSaImenomThreda("Obrada pokrenuta.");
            try
            {
                while (radi)
                {
                    ProcitajZaglavlja();

                    // malo logovanja
                    string porukaOZavrsetku = String.Format("Thread {0} je završio sa dohvatanjem zaglavlja. Čekam da završe ostali pisci zaglavlja.", Pisac.Name);
                    EventLogger.WriteEventInfo(porukaOZavrsetku);
                    Dnevnik.PisiSaImenomThreda(porukaOZavrsetku);

                    // sinhronizacija na barijeri
                    if (barijera.PisacZaglavljaZavrsio() == 0)
                    {
                        brojacStranaZaglavlja.Ponisti();
                    }

                    /*if (Common.BrojacPisacaZaglavlja.BrojAktivnihPisacaZaglavlja == 0)
                    {
                        brojacStranaZaglavlja.Ponisti();
                    }*/
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Greska pri pokretanju obrade zaglavlja", ex);
            }
            Dnevnik.PisiSaImenomThreda("Obrada završena.");
            Dnevnik.Isprazni();
        }

        private void ProcitajZaglavlja()
        {
            Common.BrojacPisacaZaglavlja.UvecajBrojAktivnihPisacaZaglavlja();
            uint i;
            string adresa;
            i = brojacStranaZaglavlja.Sledeci();
            try
            {
                adresa = DajAdresuZaglavlja(i);
                Common.Http.Strana strana = new Common.Http.StranaZaglavlja(adresa);
                while (strana.Procitaj())
                {
                    procitaneStrane.Dodaj(strana);  // dodaje u deljenu listu u memoriju
                    if (!radi)
                        return;
                    i = brojacStranaZaglavlja.Sledeci();
                    adresa = DajAdresuZaglavlja(i);
                    strana = new Common.Http.StranaZaglavlja(adresa);
                }
                Dnevnik.PisiSaImenomThreda("Više nema zaglavlja za čitanje.");
                //brojacStranaZaglavlja.Ponisti();   // ponistavam brojac da krene iz pocetka ???????
            }
            catch (Exception ex)
            {
                string porukaGreske = "Citac zaglavlja nije uspesno zavrsio.";
                EventLogger.WriteEventError(porukaGreske, ex);
                Dnevnik.PisiSaImenomThreda(porukaGreske);
            }
            finally
            {
                Common.BrojacPisacaZaglavlja.SmanjiBrojAktivnihPisacaZaglavlja();
            }
        }

        private string DajAdresuZaglavlja(uint brojStrane)
        {
            return @"http://www.polovniautomobili.com/putnicka-vozila/pretraga?page=" + (brojStrane - 1).ToString() +
                @"&sort=renewDate_desc&model=&city_distance=0&showOldNew=all&without_price=1";
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
            procitaneStrane.NeRadi("PisacZaglavlja");
        }
    }
}