﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using Common.Korisno;

namespace PolovniAutomobiliDohvatanje
{
    class BarijeraZaPisce
    {
        public BarijeraZaPisce(int brojPisaca)
        {
            this.brojPisaca = brojPisaca;
            brojZavrsenihPisaca = 0;
        }

        private int brojPisaca;
        private int brojZavrsenihPisaca;
        protected static readonly Object lokerBarijere = new Object();

        /// <summary>
        /// Uvecavanje broja pisaca koji su zavrsili. Proces se uspavljuje dok ne zavrse svi ostali pisci.
        /// </summary>
        public int PisacZaglavljaZavrsio()
        {
            int povratniBroj;
            lock(lokerBarijere)
            {
                if (brojZavrsenihPisaca < brojPisaca-1) // poslednji pisac ne treba da udje u blok i da se uspava
                {
                    brojZavrsenihPisaca++;
                    Monitor.Wait(lokerBarijere);
                }
                if (brojZavrsenihPisaca >= brojPisaca-1)
                {
                    brojZavrsenihPisaca = 0;
                    switch (Common.Korisno.Korisno.disciplina)
                    {
                        case Common.Korisno.Korisno.Disciplina.dPulse:
                            Monitor.Pulse(lokerBarijere);
                            break;
                        case Common.Korisno.Korisno.Disciplina.dPulseAll:
                            Monitor.PulseAll(lokerBarijere);
                            break;
                    }
                }
                povratniBroj = brojZavrsenihPisaca;
            }
            return povratniBroj;
        }

    }
}
