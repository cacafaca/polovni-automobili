using System.Collections;
using System.Threading;

namespace Common.Http
{
    public class ListaStrana
    {
        object lokerRadi;
        object lokerListe;

        Queue Lista;
        uint velicina;
        private bool radi = true;
        public void NeRadi(string koJeZvao)
        {
            lock (lokerRadi)
            {
                if (radi)
                {
                    radi = false;
                    lock (lokerListe)
                    {
                        Dnevnik.PisiSaImenomThreda(string.Format("Budim sve! (zaustavljanje-zaglavlja) /{0}/", koJeZvao));
                        Monitor.PulseAll(lokerListe);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velicina">Velicina liste.</param>
        public ListaStrana(uint velicina)
        {
            Lista = new Queue();
            this.velicina = velicina;
            lokerListe = new object();
            lokerRadi = new object();
        }

        public void Dodaj(Strana strana)
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
                    Dnevnik.PisiSaImenomThreda("Uspavan. Lista puna. Elemenata " + Lista.Count + ".");
                    Monitor.Wait(lokerListe);
                    Dnevnik.PisiSaImenomThreda("Probuđen. Lista je bila puna. Elemenata " + Lista.Count + ".");
                }
                Lista.Enqueue(strana);
                switch(Common.Korisno.Korisno.disciplina)
                {
                    case Common.Korisno.Korisno.Disciplina.dPulse:
                        Dnevnik.PisiSaImenomThreda("Budim slecećeg. Dodao element. Elemenata " + Lista.Count + ".");
                        Monitor.Pulse(lokerListe);
                        break;
                    case Common.Korisno.Korisno.Disciplina.dPulseAll:
                        Dnevnik.PisiSaImenomThreda("Budim sve. Dodao element. Elemenata " + Lista.Count + ".");
                        Monitor.PulseAll(lokerListe);
                        break;

                }
            }            
        }

        public Strana Uzmi()
        {
            Strana s = null;
            lock (lokerListe)
            {
                while (Lista.Count == 0)
                {
                    lock (lokerRadi)
                    {
                        if (!radi)
                            return null;
                    }
                    Dnevnik.PisiSaImenomThreda("Uspavan. Lista je prazna. Elemenata " + Lista.Count + ".");
                    Monitor.Wait(lokerListe);
                    Dnevnik.PisiSaImenomThreda("Probuđen. Lista je bila prazna. Elemenata " + Lista.Count + ".");
                }
                s = (Strana)Lista.Dequeue();
                switch (Common.Korisno.Korisno.disciplina)
                {
                    case Common.Korisno.Korisno.Disciplina.dPulse:
                        Dnevnik.PisiSaImenomThreda("Budim sledećeg. Uzeo element. Elemenata " + Lista.Count + ".");
                        Monitor.Pulse(lokerListe);
                        break;
                    case Common.Korisno.Korisno.Disciplina.dPulseAll:
                        Dnevnik.PisiSaImenomThreda("Budim sve. Uzeo element. Elemenata " + Lista.Count + ".");
                        Monitor.PulseAll(lokerListe);
                        break;
                }
                return s;
            }
        }
    }

}
