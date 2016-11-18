using System;
using System.Collections.Generic;
using System.Text;

namespace Procode.PolovniAutomobili.Common.Http
{
    public class Brojac
    {
        static readonly object lokerBrojac = new object();
        private uint brojac;

        /// <summary>
        /// Odakle da počne. Od 0 ili od 1.
        /// </summary>
        private uint minBrojac = 0;
        
        public Brojac()
        {
            brojac = minBrojac;
        }
        
        public uint Sledeci()
        {
            lock (lokerBrojac)
            {                
                return ++brojac;
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
