using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Model.Vehicle;
using System.Linq;

namespace Procode.PolovniAutomobili.Common.Http
{
    public class StranaOglasa : Strana
    {
        public StranaOglasa(string adresa)
            : base(adresa)
        {
        }

        private Automobile automobil;
        public Automobile Automobil { get { return automobil; } }

        private enum PodaciOAutomobilu { OpsteInformacije, DodatneInformacije, Sigurnost, Oprema, StanjeVozila, Opis, Kontakt }

        private string DajPodatakIzGrupeDodatneInformacije(HtmlAgilityPack.HtmlNode nodeDodatno, string nazivPodatka, StringBuilder greske)
        {
            string podatak = string.Empty;
            try
            {
                var nodePodatak = nodeDodatno.SelectNodes("ul/li")
                    .Where(listItem => listItem.ChildNodes.Any(elem => elem.InnerHtml.Equals(nazivPodatka)));
                if (nodePodatak != null)
                    podatak = nodePodatak.First().SelectNodes("span[2]")[0].InnerHtml.Trim();
            }
            catch (Exception ex)
            {
                greske.AppendLine("Nisam mogao da pročitam polje '" + nazivPodatka + "': " + ex.Message);
            }

            return podatak;
        }

        public override bool Procitaj()
        {
            bool rezultat = base.Procitaj();
            if (Sadrzaj != null)
                automobil = Http.AutomobileAd.ParseAutomobileAd(Sadrzaj, adresa);
            return rezultat && Sadrzaj != null && automobil != null;
        }

        private int SnagaKWUKS(float snagaKW)
        {
            try
            {
                return (int)(snagaKW * 1.359621617);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEventError("Greška u konverziji KW u KS.", ex);
                return 0;
            }
        }

        private string PronadjiOsnovneInformacije(HtmlAgilityPack.HtmlDocument dok, string kljuc)
        {
            int i = 1;
            HtmlAgilityPack.HtmlNodeCollection nodeColl;
            string rezultat = string.Empty;
            nodeColl = dok.DocumentNode.SelectNodes("//*[@id=\"basic_info\"]/ul/li[" + (i++).ToString() + "]");
            while (nodeColl != null)
            {
                if (nodeColl[0].InnerHtml.ToLower().Contains(kljuc.ToLower()))
                {
                    rezultat = nodeColl[0].InnerHtml.Trim();
                    break;
                }
                nodeColl = dok.DocumentNode.SelectNodes("//*[@id=\"basic_info\"]/ul/li[" + (i++).ToString() + "]");
            }
            return rezultat;
        }

    }

    public class StranaNijeProcitanaException : Exception
    {
        public StranaNijeProcitanaException(string message) : base(message) { }
        public StranaNijeProcitanaException() : base("Strana nije procitana.") { }
    }

}
