﻿using System;
using System.Collections.Generic;
using System.Text;
using Common.Vozilo;
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
        List<Automobil> Lista;
        protected static readonly object lokerListeAutomobila = new object();
        protected static DateTime vremeSnimanja;

        public AutomobilCSV()
        {
            Lista = new List<Automobil>();
            vremeSnimanja = DateTime.Now;
        }

        public void DodajMem(Automobil automobil, string threadName)
        {
            lock(lokerListeAutomobila)
            {
                TimeSpan ts = DateTime.Now.Subtract(vremeSnimanja);
                if(ts.Minutes > 5 && Lista.Count > 0)
                {
                    SnimiCSV("c:\\temp\\" + threadName + "_" + DateTime.Now.ToString().Replace(":","_")+ ".csv");
                    vremeSnimanja = DateTime.Now;                    
                    Dnevnik.Pisi("Snimljen CSV." + threadName);
                }
                Lista.Add(automobil);
            }            
        }

        public void DodajDB(Automobil automobil, string threadName)
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

        public List<Automobil> DajListu()
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
                    tw.WriteLine(Automobil.CSVZaglavlje2());
                    foreach (Automobil a in Lista)
                    {
                        if (a != null)
                        {
                            string s = a.CSV2().Trim();
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
