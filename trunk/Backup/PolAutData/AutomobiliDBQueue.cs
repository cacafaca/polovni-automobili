using System;
using System.Collections.Generic;
using System.Text;
using Common.Vozilo;

namespace PolAutData
{
    public class AutomobiliDBQueue
    {
        private Queue<Automobil> red;
        private uint maxZaSnimanje;
        public AutomobiliDBQueue(uint velicina)
        {
            red = new Queue<Automobil>();
            maxZaSnimanje = velicina  > 0 ? velicina : 1;
        }
        public AutomobiliDBQueue() : this(20)
        {            
        }
        public void Dodaj(Automobil a)
        {            
            red.Enqueue(a);
            if (red.Count == maxZaSnimanje)
            {
                Snimi();
            }
        }
        public void Snimi()
        {
            while (red.Count > 0)
            {
                Automobil a = red.Dequeue();
                if (a != null)
                {
                    AutomobilDB autoDB = new AutomobilDB();
                    autoDB.Snimi2(a);
                }
            }
        }
    }
}
