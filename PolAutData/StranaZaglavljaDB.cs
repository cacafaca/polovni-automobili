using System;
using System.Collections.Generic;
using System.Text;
using Common.Http;
using PolAutData.Provider;

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
                Data data = Data.GetDataInstance();
                data.Execute("", parametri);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
