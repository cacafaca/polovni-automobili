﻿using System;
using System.Collections.Generic;
using System.Text;
using Common.Http;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    /// <summary>
    /// This thread reads ads.
    /// </summary>
    class AdReader: Citac
    {
        #region Private fields
        PolAutData.Vehicle.Automobile autoDB;
        #endregion

        #region Constructors
        public AdReader(ref Common.Http.ListaStrana procitaneStraneOglasa, int threadId):
            base(ref procitaneStraneOglasa, threadId, typeof(AdReader).Name, (int)Properties.Settings.Default.BrojCitacaOglasa)
        {
            autoDB = new PolAutData.Vehicle.Automobile();
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
                    Common.Vehicle.Automobile auto = null;
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
