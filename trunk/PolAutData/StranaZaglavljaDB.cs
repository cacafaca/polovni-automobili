using System;
using System.Collections.Generic;
using System.Text;
using Common.Http;

namespace PolAutData
{
    class StranaZaglavljaDB: StranaZaglavlja
    {
        public StranaZaglavljaDB(string adresa)
            : base(adresa)
        {
        }

        public bool Snimi()
        {
            if (Sadrzaj != string.Empty)
            {
                System.Collections.Hashtable parametri = new System.Collections.Hashtable();
                Data d = Data.GetDataInstance();
                d.Izvrsi("", parametri);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
