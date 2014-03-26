using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Common.Http
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

        public List<string> DajAdreseOglasa()
        {
            HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            d.LoadHtml(Sadrzaj);
            List<string> adrese = new List<string>();
            foreach (HtmlNode n in d.DocumentNode.SelectNodes("//*[@id=\"searchlist-items\"]")["div"].ChildNodes["ul"].ChildNodes)
            {
                if (n.Name.ToLower().Equals("li") && (n.Attributes.Count == 1))
                {
                    adrese.Add("http://www.polovniautomobili.com" + n.ChildNodes[1].ChildNodes[1].ChildNodes[0].Attributes[0].Value);
                }
            }
            return adrese;
        }
    }
}
