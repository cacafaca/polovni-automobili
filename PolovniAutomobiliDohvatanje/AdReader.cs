using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Http;
using Procode.PolovniAutomobili.Common;

namespace Procode.PolovniAutomobili.Dohvatanje
{
    /// <summary>
    /// This thread reads ads.
    /// </summary>
    class AdReader: Citac
    {
        #region Private fields
        Procode.PolovniAutomobili.Data.Vehicle.Automobile autoDB;
        #endregion

        
        #region Constructors

        public AdReader(Data.DbContext dbContext, ref Common.Http.ListaStrana procitaneStraneOglasa, int threadId):
            base(ref procitaneStraneOglasa, threadId, typeof(AdReader).Name, (int)Properties.Settings.Default.BrojCitacaOglasa)
        {
            autoDB = new Procode.PolovniAutomobili.Data.Vehicle.Automobile(Data.Provider.Data.GetNewDataInstance(dbContext));
        }
        
        #endregion


        #region RadiObradu
        protected override void RadiObradu()
        {
            while (radi)
            {
                StranaOglasa stranaOglasa = procitaneStrane.Uzmi() as StranaOglasa;
                if (stranaOglasa != null)
                {
                    Common.Model.Vehicle.Automobile auto = null;
                    try
                    {
                        if (stranaOglasa.Procitaj())
                        {
                            auto = stranaOglasa.Automobil;
                            if (auto != null)
                            {
                                autoDB.Save(auto); 
                                Dnevnik.PisiSaImenomThreda("Dodat oglas u bazu: " + auto.ToString());
                            }
                        }
                        else
                        {
                            Dnevnik.PisiSaThredomGreska("Nisam uspeo da pročitam stranu. URL: " + stranaOglasa.Adresa);
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.WriteEventError(string.Format("Nisam uspeo da dodam automobil (br.ogl.{0}) u bazu.\nURL: {1}", auto.BrojOglasa, stranaOglasa.Adresa), ex);
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
