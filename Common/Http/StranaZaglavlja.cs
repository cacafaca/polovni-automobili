using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Procode.PolovniAutomobili.Common.Http
{
    public class StranaZaglavlja: Strana 
    {
        public StranaZaglavlja(string adresa): base(adresa)
        {
        }

        public override bool Procitaj()
        {   
            if (base.Procitaj())
                //return Sadrzaj.IndexOf("Trenutno nema rezultata koji odgovaraju Vašem kriterijumu pretraživanja") == -1;
                return Sadrzaj.IndexOf("Trenutno nema rezultata koji odgovaraju Va") == -1; // zbog enkodinga
            else
                return true; // ako nije dobro procitao neka vrati true, pa neka cita dalje.
        }
    }
}
