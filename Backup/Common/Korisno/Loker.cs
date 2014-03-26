using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class BrojacPisacaZaglavlja
    {
        public static readonly object lokerPisciZaglavlja = new object();
        private static int brojAktivnihPisacaZaglavlja = 0;
        public static int BrojAktivnihPisacaZaglavlja { 
            get {
                lock (lokerPisciZaglavlja)
                {
                    return brojAktivnihPisacaZaglavlja;
                }
            } 
        }
        public static void UvecajBrojAktivnihPisacaZaglavlja()
        {
            lock (lokerPisciZaglavlja)
            {
                brojAktivnihPisacaZaglavlja++;
            }            
        }
        public static void SmanjiBrojAktivnihPisacaZaglavlja()
        {
            lock (lokerPisciZaglavlja)
            {
                brojAktivnihPisacaZaglavlja--;
            }
        }

    }
}
