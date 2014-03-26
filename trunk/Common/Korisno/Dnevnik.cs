using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Common
{
    public static class Dnevnik
    {
        public static readonly bool PisiUDnevnik = Properties.Settings.Default.PisiUDnevnik;

        private static int kapacitet = Properties.Settings.Default.ValicinaBaferaDnevnika;
        private static StringBuilder bafer = new StringBuilder(kapacitet);
        private static string nazivDatoteke = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Dnevnik.txt";
        private static string nazivDnevnikDir = "ArhivaDnevnika";
        private static readonly object loker = new object();
        private static bool arhivirao = false;

        public static void Pisi(string tekst)
        {
            Pisi(tekst, false);
        }

        public static void Pisi(string tekst, bool isprazni)
        {
            if (!PisiUDnevnik)
                return;

            lock (loker)
            {
                isprazni = true; // za potrebe testiranja
                if (bafer.Length + tekst.Length > kapacitet || isprazni)
                {
                    Snimi();
                }
                bafer.AppendLine(DateTime.Now.ToString() + "> " + tekst);
            }
        }

        public static void PisiSaThredom(string tekst)
        {
            if (tekst != null)
            {
                string threadName = Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name.PadLeft(20) : "bezimeni".PadLeft(20);
                Pisi(threadName + ": " + tekst);
            }
        }
        public static void PisiSaThredom(string tekst, bool isprazni)
        {
            PisiSaThredom(tekst);
            if(isprazni)
                Isprazni();
        }
        public static void PisiSaThredomGreska(string tekst)
        {
            PisiSaThredom("Greška: " + tekst);
        }
        public static void PisiSaThredomGreska(string tekst, Exception ex)
        {
            PisiSaThredomGreska(tekst + "; Exception: " + ex.Message + "; StackTrace: " + ex.StackTrace);
        }

        public static void PisiSaThredomUpozorenje(string tekst)
        {
            PisiSaThredom("Upozorenje: " + tekst);
        }
        public static bool Arhiviraj()
        {
            string dirPath = Path.GetDirectoryName(nazivDatoteke)+"\\" + nazivDnevnikDir;
            if(!Directory.Exists(dirPath))
            {
                try
                {
                    Directory.CreateDirectory(dirPath);
                }
                catch (Exception ex)
                {
                    EventLogger.WriteEventError("Ne mogu da napravim folder " + dirPath, ex);
                    return false;
                }
            }
            string novoIme = dirPath + "\\" + Path.GetFileNameWithoutExtension(nazivDatoteke) + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss") + Path.GetExtension(nazivDatoteke);
            if (File.Exists(nazivDatoteke) && !File.Exists(novoIme))
            {
                try
                {
                    File.Move(nazivDatoteke, novoIme);
                }
                catch (Exception ex)
                {
                    EventLogger.WriteEventError(string.Format("Nisam uspeo da preimenujem fajl '{0}' u '{1}'.", nazivDatoteke, novoIme), ex);
                    return false; 
                }
            }
            return true;
        }
        private static void Snimi()
        {
            if (!PisiUDnevnik)
                return;

            if (!arhivirao)
            {
                arhivirao = Arhiviraj();
            }
            using (TextWriter tw = File.AppendText(nazivDatoteke))
            {
                try
                {
                    tw.Write(bafer);
                }
                catch (Exception ex)
                {
                    Common.EventLogger.WriteEventError("Greška pri upisu u dnevnik.", ex);
                }
                finally
                {
                    tw.Close();
                }
            }
            bafer.Length = 0;
        }

        public static void Isprazni()
        {
            lock (loker)
            {
                Snimi();
            }
        }
    }
}
