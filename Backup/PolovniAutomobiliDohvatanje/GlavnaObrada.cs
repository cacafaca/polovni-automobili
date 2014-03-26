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
        static Common.Http.StranaLista procitaneStraneZaglavlja;    
        static Common.Http.StranaLista procitaneStraneOglasa;    
        static Common.Http.Brojac brojacStraneZaglavlja; 

        PisacZaglavlja[] pisacZaglavlja = new PisacZaglavlja[Properties.Settings.Default.BrojPisacaZaglavlja];
        CitacZaglavlja[] citacZaglavlja = new CitacZaglavlja[Properties.Settings.Default.BrojCitacaZaglavlja];
        CitacOglasa[] citacOglasa = new CitacOglasa[Properties.Settings.Default.BrojCitacaOglasa];
        public GlavnaObrada()
        {
            Dnevnik.Pisi("Inicijalizacija glavne obrade");
            // Inicijalizacija liste strana zaglavlja
            procitaneStraneZaglavlja = new Common.Http.StranaLista(Properties.Settings.Default.BrojStranaZaglavlja);

            // Inicijalizacija liste strana oglasa
            procitaneStraneOglasa = new Common.Http.StranaLista(Properties.Settings.Default.BrojStranaOglasa);

            // Inicijalizacija brojaca strane zaglavlja
            brojacStraneZaglavlja = new Common.Http.Brojac();

            // inicijalizacija pisca zaglavlja
            for (int i = 0; i < pisacZaglavlja.Length; i++)
            {
                pisacZaglavlja[i] = new PisacZaglavlja(ref procitaneStraneZaglavlja, brojacStraneZaglavlja, i);
            }

            // inicijalizacija citaca zaglavlja
            for (int i = 0; i < citacZaglavlja.Length; i++)
            {
                citacZaglavlja[i] = new CitacZaglavlja(ref procitaneStraneZaglavlja, ref procitaneStraneOglasa, i);
            }

            // inicijalizacija citaca oglasa
            for (int i = 0; i < citacOglasa.Length; i++)
            {
                citacOglasa[i] = new CitacOglasa(ref procitaneStraneOglasa, i);
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
                //EventLogger.WriteEventInfo("Glavna obrada zaustavljena.");
                Dnevnik.PisiSaThredom("Glavna obrada zaustavljena (poslate komande za zaustavljanje.");
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Glavna obrada nije uspešno zaustavljena.", ex);
            }
        }

        public string ToString()
        {
            return string.Format("Glavna obrada pokrenuta. Pokrenuto je \n\t{0} pisaca zaglavlja,\n\t{1} citaca zaglavlja pokusaj\n\t{2} citaca oglasa.\nVerzija .Net je {3}.", pisacZaglavlja.Length, citacZaglavlja.Length, citacOglasa.Length, Environment.Version.ToString());
        }
    }
}
