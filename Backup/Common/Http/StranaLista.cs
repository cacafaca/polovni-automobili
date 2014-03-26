using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Common.Http
{
    public class StranaLista
    {
        protected static readonly object lokerListeStranaZaglavlja = new object();
        protected static readonly object lokerListeStranaOglasa = new object();
        protected static readonly object lokerRadi = new object();
        object lokerListe;
        private enum Disciplina { dPulse, dPulseAll };
        private Disciplina disciplina = Disciplina.dPulseAll;

        Queue Lista;
        uint velicina;
        private bool radi = true;
        public void NeRadi()
        {
            lock (lokerRadi)
            {
                if (radi)
                {
                    radi = false;
                    lock (lokerListeStranaZaglavlja)
                    {
                        Dnevnik.PisiSaThredom("Budim sve! (zaustavljanje-zaglavlja)");
                        Monitor.PulseAll(lokerListeStranaZaglavlja);
                    }
                    lock (lokerListeStranaOglasa)
                    {
                        Dnevnik.PisiSaThredom("Budim sve! (zaustavljanje-oglasi)");
                        Monitor.PulseAll(lokerListeStranaOglasa);
                    }
                }
            }
        }

        public StranaLista(uint velicina)
        {
            Lista = new Queue();
            this.velicina = velicina;
            lokerListe = new object();
        }

        public void Dodaj(Strana strana)
        {
            if (strana is StranaZaglavlja) // ako je strana zaglavlja
            {
                lock (lokerListe)
                {
                    while (Lista.Count == velicina) // provera da li je lista puna
                    {
                        lock (lokerRadi)
                        {
                            if (!radi)
                                return;                            
                        }
                        Dnevnik.PisiSaThredom("Uspavan (dodavanje). Br. el. " + Lista.Count);
                        Monitor.Wait(lokerListe);
                        Dnevnik.PisiSaThredom("Probuđen (dodavanje). Br. el. " + Lista.Count);
                    }
                    Lista.Enqueue(strana);
                    switch(disciplina)
                    {
                        case Disciplina.dPulse:
                            Dnevnik.PisiSaThredom("Budim slecećeg (dodavanje)! Br. el. " + Lista.Count);
                            Monitor.Pulse(lokerListe);
                            break;
                        case Disciplina.dPulseAll:
                            Dnevnik.PisiSaThredom("Budim sve (dodavanje)! Br. el. " + Lista.Count);
                            Monitor.PulseAll(lokerListe);
                            break;

                    }
                }
            }
            else if (strana is StranaOglasa)// ako je Strana oglasa
            {
                lock (lokerListeStranaOglasa)
                {
                    while (Lista.Count == velicina)
                    {
                        lock (lokerRadi)
                        {
                            if (!radi)
                                return;
                        }
                        Dnevnik.PisiSaThredom("Uspavan (dodavanje). Br. el. " + Lista.Count);
                        Monitor.Wait(lokerListeStranaOglasa);
                        Dnevnik.PisiSaThredom("Probuđen (dodavanje). Br. el. " + Lista.Count);
                    }
                    Lista.Enqueue(strana);
                    switch(disciplina)
                    {
                        case Disciplina.dPulse:
                            Dnevnik.PisiSaThredom("Budim sledećeg (dodavanje)! Br. el. " + Lista.Count);
                            Monitor.Pulse(lokerListeStranaOglasa);
                            break;
                        case Disciplina.dPulseAll:
                            Dnevnik.PisiSaThredom("Budim sve (dodavanje)! Br. el. " + Lista.Count);
                            Monitor.PulseAll(lokerListeStranaOglasa);
                            break;
                    }
                }
            }
            else // ako je nesto drugo baci exception
            {
                throw new Exception("Poslata strana nije ni strana oglasa ni strana zaglavlja.");
            }
        }

        public Strana Uzmi(string klasaKojaUzima)
        {
            Strana s = null;
            if (klasaKojaUzima == typeof(StranaOglasa).Name)
            {
                lock (lokerListeStranaOglasa)
                {
                    while (Lista.Count == 0)
                    {
                        lock (lokerRadi)
                        {
                            if (!radi)
                                return null;
                        }
                        Dnevnik.PisiSaThredom("Uspavan (uzimanje). Br. el. " + Lista.Count);
                        Monitor.Wait(lokerListeStranaOglasa);
                        Dnevnik.PisiSaThredom("Probuđen (uzimanje). Br. el. " + Lista.Count);
                    }
                    s = (Strana)Lista.Dequeue();
                    switch(disciplina)
                    {
                        case Disciplina.dPulse:
                            Dnevnik.PisiSaThredom("Budim sledećeg (uzimanje)! Br. el. " + Lista.Count);
                            Monitor.Pulse(lokerListeStranaOglasa);
                            break;
                        case Disciplina.dPulseAll:
                            Dnevnik.PisiSaThredom("Budim sve (uzimanje)! Br. el. " + Lista.Count);
                            Monitor.PulseAll(lokerListeStranaOglasa);
                            break;
                    }
                }
                return s;
            }
            else if (klasaKojaUzima == typeof(StranaZaglavlja).Name)
            {
                lock (lokerListe)
                {
                    while (Lista.Count == 0)
                    {
                        lock (lokerRadi)
                        {
                            if (!radi)
                                return null;
                        }
                        Dnevnik.PisiSaThredom("Uspavan (uzimanje). Br. el. " + Lista.Count);
                        Monitor.Wait(lokerListe);
                        Dnevnik.PisiSaThredom("Probuđen (uzimanje). Br. el. " + Lista.Count);
                    }
                    s = (Strana)Lista.Dequeue();
                    switch (disciplina)
                    {
                        case Disciplina.dPulse:
                            Dnevnik.PisiSaThredom("Budim sledećeg (uzimanje)! Br. el. " + Lista.Count);
                            Monitor.Pulse(lokerListe);
                            break;
                        case Disciplina.dPulseAll:
                            Dnevnik.PisiSaThredom("Budim sve (uzimanje)! Br. el. " + Lista.Count);
                            Monitor.PulseAll(lokerListe);
                            break;
                    }
                }
                return s;
            }
            else
            {
                throw new Exception("Nije prosledjen naziv klase koja uzima stranu.");
            }
        }
    }

}
