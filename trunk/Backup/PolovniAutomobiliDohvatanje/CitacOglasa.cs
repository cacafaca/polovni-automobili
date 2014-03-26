using System;
using System.Collections.Generic;
using System.Text;
using Common.Http;
using Common;
using PolAutData;

namespace PolovniAutomobiliDohvatanje
{
    class CitacOglasa: Citac 
    {
        PolAutData.AutomobilDB autoDB;

        public CitacOglasa(ref Common.Http.StranaLista procitaneStraneOglasa, int threadId): 
            base (ref procitaneStraneOglasa, threadId, typeof(CitacOglasa).Name)
        {
            autoDB = new PolAutData.AutomobilDB();
        }

        #region RadiObradu
        protected override void RadiObradu()
        {
            AutomobiliDBQueue red = new AutomobiliDBQueue();
            while (radi)
            {
                Strana strana = procitaneStrane.Uzmi(typeof(StranaOglasa).Name);
                if (strana != null)
                {
                    if (strana is StranaOglasa)
                    {
                        try
                        {
                            if (strana.Procitaj())
                            {
                                Common.Vozilo.Automobil auto = ((StranaOglasa)strana).Automobil;
                                if (auto != null)
                                {
                                    autoDB.Snimi(auto); // upis u bazu
                                    //autoDB.Snimi2(auto); // upis u bazu
                                    //red.Dodaj(auto);
                                    Dnevnik.PisiSaThredom("Dodat oglas u bazu: " + auto.ToString());
                                }
                            }
                            else
                            {
                                Dnevnik.PisiSaThredomGreska("Nisam uspeo da pročitam stranu. URL: " + strana.Adresa);
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Korisno.Korisno.LogujGresku("Nisam uspeo da dodam automobil u bazu. URL: " + strana.Adresa, ex);
                        }
                    }
                }
            }
            red.Snimi();
        }
        #endregion
    }
}
