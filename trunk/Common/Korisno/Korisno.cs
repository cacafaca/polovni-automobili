using System.Text;
using System;

namespace Common.Korisno
{
    public static class Korisno
    {
        public static string TekstIzmedju(string tekst, string pocetak, string kraj)
        {
            tekst = tekst.Substring(tekst.IndexOf(pocetak));
            tekst = tekst.Substring(0, tekst.IndexOf(kraj));
            return tekst;
        }

        public static void WriteConsole(string tekst)
        {
            System.Console.WriteLine(tekst);
        }

        public static string DecodeFromUtf8(string utf8String)
        {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //Debug.Assert( 0 <= utf8String[brojPokusaja] && utf8String[brojPokusaja] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
        }

        public static string TrimMultiline(string multiLine)
        {
            StringBuilder rez = new StringBuilder();
            string s = multiLine.Trim();
            using(System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(s), 0, s.Length))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ms, Encoding.UTF8))
                {
                    string l;
                    while((l = sr.ReadLine()) != null)
                    {
                        if(!l.Trim().Equals(string.Empty))
                            rez.AppendLine(l.Trim());
                    }
                }
            }
            return rez.ToString();
        }

        /// <summary>
        /// Ova funkcija postoji ToString("0000000");
        /// </summary>
        /// <param name="vrednost"></param>
        /// <param name="maxVrednost"></param>
        /// <returns></returns>
        public static string IntUStrSaNulama(int vrednost, int maxVrednost)
        {
            int duz = (int)Math.Log10(maxVrednost) + 1;
            return vrednost.ToString("0".PadLeft(duz, '0'));
        }

        public static void LogujGresku(string tekst, Exception ex)
        {
            try
            {
                Common.EventLogger.WriteEventError(string.Format("Thred {0}> " + tekst, System.Threading.Thread.CurrentThread.Name), ex);
                Common.Dnevnik.PisiSaThredomGreska(tekst, ex);
            }
            catch
            {
            }
        }
        public enum Disciplina { dPulse, dPulseAll };
        public static Disciplina disciplina = Disciplina.dPulseAll;
    }
}
