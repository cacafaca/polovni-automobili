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
        //PolAutData.AutomobilDB autoDB;
        PolAutData.AutomobilCSV autoCSV;

        public CitacOglasa(ref Common.Http.ListaStrana procitaneStraneOglasa, int threadId):
            base(ref procitaneStraneOglasa, threadId, typeof(CitacOglasa).Name, (int)Properties.Settings.Default.BrojCitacaOglasa)
        {
            //autoDB = new PolAutData.AutomobilDB();
            autoCSV = new AutomobilCSV();
        }

        #region RadiObradu
        protected override void RadiObradu()
        {
            while (radi)
            {
                Strana strana = procitaneStrane.Uzmi();
                if (strana != null)
                {
                    Common.Vehicle.Automobil auto = null;
                    try
                    {
                        if (strana.Procitaj())
                        {
                            auto = ((StranaOglasa)strana).Automobil;
                            if (auto != null)
                            {
                                //autoDB.Snimi(auto); // upis u bazu
                                //autoDB.Snimi2(auto); // upis u bazu
                                //red.Dodaj(auto);
                                //autoCSV.DodajMem(auto);
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
                        EventLogger.WriteEventError(string.Format("Nisam uspeo da dodam automobil (br.ogl.{0}) u bazu.\nURL: {1}", auto.BrojOglasa, strana.Adresa), ex);
                    }
                }
                else
                {
                    EventLogger.WriteEventWarning("Dobijena null vrednost za stranu iz liste procitanih strana. Proveri zasto.");
                }
            }
        }
        #endregion
    }
}
