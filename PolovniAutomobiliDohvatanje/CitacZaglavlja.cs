using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Procode.PolovniAutomobili.Common.Http;
using Procode.PolovniAutomobili.Common;

namespace Procode.PolovniAutomobili.Dohvatanje
{
    class CitacZaglavlja: Citac
    {
        Common.Http.ListaStrana procitaneStraneOglasa;  // U ovu listu ce da upisuje procitane oglase
        Common.Http.Brojac brojacStranaZaglavlja;

        public CitacZaglavlja(ref Common.Http.ListaStrana procitaneStraneZaglavlja, ref Common.Http.ListaStrana procitaneStraneOglasa, int threadId,
            Common.Http.Brojac brojac) :
            base(ref procitaneStraneZaglavlja, threadId, typeof(CitacZaglavlja).Name, (int)Properties.Settings.Default.BrojCitacaZaglavlja)
        {
            this.procitaneStraneOglasa = procitaneStraneOglasa;
            brojacStranaZaglavlja = brojac;
        }

        protected override void RadiObradu()
        {
            while (radi)
            {
                Strana strana = procitaneStrane.Uzmi(); // base property, strane zaglavlja.
                //Console.WriteLine(strana.Adresa); //
                if (strana != null)
                {
                    List<string> adreseOglasa = strana.DajAdreseOglasa();
                    if (adreseOglasa != null)
                    {
                        if (adreseOglasa.Count != 0)
                        {
                            foreach (string adresa in adreseOglasa)
                            {
                                Strana stranaOglasa = new StranaOglasa(adresa);
                                procitaneStraneOglasa.Dodaj(stranaOglasa);
                                if (!radi)
                                    return;
                            }
                            Dnevnik.PisiSaImenomThreda("Obrađeno je zaglavlje: " + strana.Adresa);
                        }
                        else
                        {
                            brojacStranaZaglavlja.Ponisti();
                        }
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
            //procitaneStraneOglasa.NeRadi("ČitačZaglavlja");
        }
    }
}
