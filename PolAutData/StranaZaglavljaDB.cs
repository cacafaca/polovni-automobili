using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Http;
using Procode.PolovniAutomobili.Data.Provider;

namespace Procode.PolovniAutomobili.Data
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
                Provider.Data data = Provider.Data.GetNewDataInstance();
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
