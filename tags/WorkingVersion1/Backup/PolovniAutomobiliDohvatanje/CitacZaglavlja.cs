using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Http;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    class CitacZaglavlja: Citac
    {
        Common.Http.StranaLista procitaneStraneOglasa;  // U ovu listu ce da upisuje procitane oglase

        public CitacZaglavlja(ref Common.Http.StranaLista procitaneStraneZaglavlja, ref Common.Http.StranaLista procitaneStraneOglasa, int threadId) :
            base(ref procitaneStraneZaglavlja, threadId, typeof(CitacZaglavlja).Name)
        {
            this.procitaneStraneOglasa = procitaneStraneOglasa;
        }

        protected override void RadiObradu()
        {
            while (radi)
            {
                Strana strana = procitaneStrane.Uzmi(typeof(StranaZaglavlja).Name);
                if (strana != null)
                {
                    List<string> adreseOglasa = strana.DajAdreseOglasa();
                    if (adreseOglasa != null)
                    {
                        foreach (string adresa in adreseOglasa)
                        {
                            Strana stranaOglasa = new StranaOglasa(adresa);
                            procitaneStraneOglasa.Dodaj(stranaOglasa);
                            if (!radi)
                                return;
                        }
                        Dnevnik.PisiSaThredom("Obrađeno je zaglavlje: " + strana.Adresa);
                    }
                    else
                    {
                        Dnevnik.PisiSaThredomGreska("Nije obrađena strana zaglavlja: " + strana.Adresa);
                    }
                }
            }
        }

        public override void Zaustavi()
        {
            base.Zaustavi();
            procitaneStraneOglasa.NeRadi();
        }
    }
}
