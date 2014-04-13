using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common;

namespace PolovniAutomobiliDohvatanje
{
    public class GlavnaObrada 
    {
        // zajednicki resursi
        static Common.Http.ListaStrana procitaneStraneZaglavlja;    
        static Common.Http.ListaStrana procitaneStraneOglasa;    
        static Common.Http.Brojac brojacStraneZaglavlja;
        static BarijeraZaPisce barijera; // za singronizaciju procesa pisacaZaglavlja i citacaOglasa

        PisacZaglavlja[] pisacZaglavlja = new PisacZaglavlja[Properties.Settings.Default.BrojPisacaZaglavlja];
        CitacZaglavlja[] citacZaglavlja = new CitacZaglavlja[Properties.Settings.Default.BrojCitacaZaglavlja];
        AdReader[] citacOglasa = new AdReader[Properties.Settings.Default.BrojCitacaOglasa];
        public GlavnaObrada()
        {
            Dnevnik.Pisi("Inicijalizacija glavne obrade");
            // Inicijalizacija liste strana zaglavlja
            procitaneStraneZaglavlja = new Common.Http.ListaStrana(Properties.Settings.Default.BrojStranaZaglavlja);

            // Inicijalizacija liste strana oglasa
            procitaneStraneOglasa = new Common.Http.ListaStrana(Properties.Settings.Default.BrojStranaOglasa);

            // Inicijalizacija brojaca strane zaglavlja
            brojacStraneZaglavlja = new Common.Http.Brojac();

            // BarijeraZaPisce
            barijera = new BarijeraZaPisce(pisacZaglavlja.Length);

            // inicijalizacija pisca zaglavlja
            for (int i = 0; i < pisacZaglavlja.Length; i++)
            {
                pisacZaglavlja[i] = new PisacZaglavlja(ref procitaneStraneZaglavlja, brojacStraneZaglavlja, i, ref barijera);
            }

            // inicijalizacija citaca zaglavlja
            for (int i = 0; i < citacZaglavlja.Length; i++)
            {
                citacZaglavlja[i] = new CitacZaglavlja(ref procitaneStraneZaglavlja, ref procitaneStraneOglasa, i, brojacStraneZaglavlja);
            }

            // inicijalizacija citaca oglasa
            for (int i = 0; i < citacOglasa.Length; i++)
            {
                citacOglasa[i] = new AdReader(ref procitaneStraneOglasa, i);
            }

            //EventLogger.WriteEventInfo("Glavna obrada inicijalizovana.");
        }

        public void Pokreni()
        {
            Dnevnik.Pisi("Pokretanje threadova.");
            try
            {
                for (int i = 0; i < pisacZaglavlja.Length; i++)
                {
                    pisacZaglavlja[i].Pokreni();
                }
                for (int i = 0; i < citacZaglavlja.Length; i++)
                {
                    citacZaglavlja[i].Pokreni();
                }
                for (int i = 0; i < citacOglasa.Length; i++)
                {
                    citacOglasa[i].Pokreni();
                }
                //EventLogger.WriteEventInfo(string.Format("Glavna obrada pokrenuta. Pokrenuto je \n\t{0} pisaca zaglavlja,\n\t{1} citaca zaglavlja pokusaj\n\t{2} citaca oglasa.\nVerzija .Net je {3}.", pisacZaglavlja.Length, citacZaglavlja.Length, citacOglasa.Length, Environment.Version.ToString()));
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Glavna obrada nije uspešno pokrenuta.", ex);
            }
        }

        public void Zaustavi()
        {
            Dnevnik.PisiSaThredom("Zaustavljanje threadova.");
            try
            {
                for (int i = 0; i < pisacZaglavlja.Length; i++)
                {
                    pisacZaglavlja[i].Zaustavi();
                }
                for (int i = 0; i < citacZaglavlja.Length; i++)
                {
                    citacZaglavlja[i].Zaustavi();
                }
                for (int i = 0; i < citacOglasa.Length; i++)
                {
                    citacOglasa[i].Zaustavi();
                }
                PolAutData.Provider.DataInstance.Data.Close();
                Dnevnik.PisiSaThredom("Glavna obrada zaustavljena. Poslate komande za zaustavljanje.");
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Glavna obrada nije uspešno zaustavljena.", ex);
            }
        }

        public override string ToString()
        {
            return string.Format("Glavna obrada pokrenuta. Pokrenuto je \n\t{0} pisaca zaglavlja,\n\t{1} citaca zaglavlja pokusaj\n\t{2} citaca oglasa.\nVerzija .Net je {3}.", pisacZaglavlja.Length, citacZaglavlja.Length, citacOglasa.Length, Environment.Version.ToString());
        }
    }
}
