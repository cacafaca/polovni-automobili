using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Http
{
    public class Brojac
    {
        static readonly object lokerBrojac = new object();
        private uint brojac;
        public uint Vrednost
        {
            get
            {
                lock (lokerBrojac)
                {
                    return brojac;
                }
            }
            set
            {
                lock (lokerBrojac)
                {
                    brojac = value;
                }
            }
        }
        private uint minBrojac = 0;
        public Brojac()
        {
            brojac = minBrojac;
        }
        public uint Sledeci()
        {
            lock (lokerBrojac)
            {
                brojac = brojac + 1;
                return brojac;
            }
        }
        public void Ponisti()
        {
            lock (lokerBrojac)
            {
                brojac = minBrojac;
            }
        }
    }
}
