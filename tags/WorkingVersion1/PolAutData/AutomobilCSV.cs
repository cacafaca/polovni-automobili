using System;
using System.Collections.Generic;
using System.Text;
using Common.Vehicle;
using System.IO;
using Common;

namespace PolAutData
{
    /// <summary>
    /// Ovu klasu više ne koristim. Služila je za upisivanje u CSV file dok nisam implementirao 
    /// upis u bazu.
    /// </summary>
    public class AutomobilCSV
    {
        List<Automobile> Lista;
        protected static readonly object lokerListeAutomobila = new object();
        protected static DateTime vremeSnimanja;

        int limitSnimanjeRedovi = 1000;
        int limitSnimanjeMinuti = 10;

        public AutomobilCSV()
        {
            Lista = new List<Automobile>();
            vremeSnimanja = DateTime.Now;
        }

        public void DodajMem(Automobile automobil, string threadName)
        {
            lock(lokerListeAutomobila)
            {
                Lista.Add(automobil);
                TimeSpan ts = DateTime.Now.Subtract(vremeSnimanja);
                if(ts.Minutes >= limitSnimanjeMinuti && Lista.Count >= limitSnimanjeRedovi)
                {
                    SnimiCSV("c:\\temp\\" + threadName + "_" + DateTime.Now.ToString().Replace(":","_")+ ".csv");
                    vremeSnimanja = DateTime.Now;                    
                    Dnevnik.PisiSaThredom("Snimljen CSV.");
                }
            }            
        }
        public void DodajMem(Automobile automobil)
        {
            DodajMem(automobil, System.Threading.Thread.CurrentThread.Name);
        }

        public void DodajDB(Automobile automobil, string threadName)
        {
            lock (lokerListeAutomobila)
            {
                TimeSpan ts = DateTime.Now.Subtract(vremeSnimanja);
                if (ts.Minutes > 5 && Lista.Count > 0)
                {
                    SnimiCSV("c:\\temp\\" + threadName + "_" + DateTime.Now.ToString().Replace(":", "_") + ".csv");
                    vremeSnimanja = DateTime.Now;
                    Dnevnik.Pisi("Snimljen CSV." + threadName);
                }
                Lista.Add(automobil);
            }
        }

        public List<Automobile> DajListu()
        {
            lock (lokerListeAutomobila)
            {
                return Lista;
            }            
        }

        public int Broj()
        {
            lock (lokerListeAutomobila)
            {
                return Lista.Count;
            }            
        }

        public void SnimiCSV(string datoteka)
        {
            using (TextWriter tw = File.CreateText(datoteka))
            {
                try
                {
                    tw.WriteLine(Automobile.CSVZaglavlje());
                    foreach (Automobile a in Lista)
                    {
                        if (a != null)
                        {
                            string s = a.CSV().Trim();
                            if (s != null && !s.Equals(string.Empty))
                                tw.WriteLine(s);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.WriteEventError("Greška pri generisanju CSV fajla.", ex);
                }
                finally
                {
                    tw.Close();
                }
            }   
        }

        private void WriteLine(string tekst, string threadName)
        {
            System.Console.WriteLine(string.Format("{0}: {1}", threadName, tekst));
        }
    }
}
