using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Vehicle;
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

        public void ExtractAutomobile()
        {
            if (!string.IsNullOrWhiteSpace(Sadrzaj))
            {
                HtmlAgilityPack.HtmlDocument dok = new HtmlAgilityPack.HtmlDocument();
                try
                {
                    dok.LoadHtml(Sadrzaj);
                    StringBuilder greske = new StringBuilder();
                    HtmlAgilityPack.HtmlNodeCollection nodeCol;

                    // provera da li je strana pronadjena
                    nodeCol = dok.DocumentNode.SelectNodes("/html/body/div[2]/div[1]/p[1]");
                    if (nodeCol != null && nodeCol[0].InnerHtml.Trim().ToUpper().Equals("404".ToUpper()))
                    {
                        automobil = null;
                        return;
                    }

                    #region Zaglavlje
                    string naslov = string.Empty;
                    try
                    {
                        nodeCol = dok.DocumentNode.SelectNodes("id('main_content')/div[8]/h1");
                        if (nodeCol != null)
                            naslov = nodeCol[0].InnerHtml.Trim();
                        else
                            naslov = "Bez naslova";
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje naslov: " + ex.Message);
                    }

                    float cena = 0;
                    try
                    {
                        string cenaStr = dok.DocumentNode.SelectNodes("id('main_content')/div[9]/div[1]/div[1]/span")[0].InnerHtml.Trim();
                        if (cenaStr.ToLower().Equals("na upit") || cenaStr.ToLower().Equals("po dogovoru"))
                        {
                            cena = 0;
                        }
                        else
                        {
                            cena = float.Parse(cenaStr.Substring(0, cenaStr.IndexOf(" ")).Replace(".", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje cena: " + ex.Message);
                    }
                    #endregion Zaglavlje

                    #region Opste

                    var nodeOpste = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                            div.Attributes["class"].Value.Equals("basic-info")).First();
                    int listItemIndex = 1;

                    string vozilo = string.Empty;
                    try
                    {
                        vozilo = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje vozilo: " + ex.Message);
                    }

                    string marka = string.Empty;
                    try
                    {
                        marka = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje marka: " + ex.Message);
                    }

                    string model = string.Empty;
                    try
                    {
                        model = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje model: " + ex.Message);
                    }

                    int godinaProizvodnje = 0;
                    try
                    {
                        godinaProizvodnje = int.Parse(nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim().Substring(0, 4));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Godina proizvodnje': " + ex.Message);
                    }

                    int kilometraza = 0;
                    try
                    {
                        string kmStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                        kilometraza = int.Parse(kmStr.Replace(" km", string.Empty).Replace(".", string.Empty));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Kilometraža': " + ex.Message);
                    }

                    string karoserija = string.Empty; ;
                    try
                    {
                        karoserija = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje karoserija: " + ex.Message);
                    }

                    string gorivo = string.Empty;
                    try
                    {
                        gorivo = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje gorivo: " + ex.Message);
                    }

                    int kubikaza = 0;
                    try
                    {
                        string kubikazaStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim().Replace(" cm3", string.Empty);
                        if (kubikazaStr.Contains("Atestiran do:"))
                            kubikazaStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim().Replace(" cm3", string.Empty);
                        kubikaza = int.Parse(kubikazaStr);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje kubikaža: " + ex.Message);
                    }

                    int snagaKW = 0;
                    int snagaKS = 0;
                    try
                    {
                        string snaga = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim().Replace(" (kW/KS)", string.Empty);
                        snagaKW = int.Parse(snaga.Remove(snaga.IndexOf('/')));
                        snagaKS = int.Parse(snaga.Remove(0, snaga.IndexOf('/') + 1)); ;
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Snaga': " + ex.Message);
                    }

                    bool fiksnaCena = false;
                    try
                    {
                        string fiksnaCenaStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                        fiksnaCena = fiksnaCenaStr.ToLower() != "Cena nije fiksna".ToLower();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje fiksnaCena: " + ex.Message);
                    }

                    bool zamena = false;
                    try
                    {
                        string zamenaStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim();
                        zamena = zamenaStr.ToLower() != "Zamena: NE".ToLower();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje fiksnaCena: " + ex.Message);
                    }

                    DateTime datumPostavljanja = DateTime.MinValue;
                    listItemIndex++;
                    try
                    {
                        string datumPostavljanjaStr = nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]/time")[0].Attributes["datetime"].Value.Trim(); ;
                        datumPostavljanja = DateTime.SpecifyKind(DateTime.Parse(datumPostavljanjaStr), DateTimeKind.Utc);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Datum postavljanja': " + ex.Message);
                    }

                    int brojOglasa = 0;
                    try
                    {
                        brojOglasa = int.Parse(nodeOpste.SelectNodes("ul/li[" + listItemIndex++ + "]")[0].InnerHtml.Trim().Replace("Broj oglasa: ", string.Empty));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Broj oglasa': " + ex.Message);
                    }
                    #endregion Opste

                    #region Dodatne informacije

                    var nodeDodatno = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                        div.Attributes["class"].Value.Equals("detailed-info")).First();

                    string emisionaKlasa = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Emisiona klasa motora", greske);

                    string pogon = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Pogon", greske);

                    string menjac = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Menjač", greske);

                    string brojVrata = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Broj vrata", greske).Replace(" vrata", string.Empty);

                    string brojSedistaStr = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Broj sedišta", greske).Replace(" sedišta", string.Empty);
                    byte brojSedista = 0;
                    byte.TryParse(brojSedistaStr, out brojSedista);

                    string stranaVolana = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Strana volana", greske);

                    string klima = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Klima", greske);

                    string bojaKaroserije = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Boja", greske);

                    string materijalEnterijera = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Materijal enterijera", greske);

                    string bojaEnterijera = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Boja enterijera", greske);

                    string registrovanDoStr = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Registrovan do", greske);
                    DateTime registrovanDo = DateTime.MinValue;
                    if (!registrovanDoStr.Equals("Nije registrovan"))
                    {
                        registrovanDoStr = "01." + registrovanDoStr.Remove(registrovanDoStr.Length - 1); ;
                        DateTime.TryParse(registrovanDoStr, out registrovanDo);
                    }

                    string porekloVozila = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Poreklo vozila", greske);

                    string vlastnistvo = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Vlasništvo", greske);

                    string ostecenje = DajPodatakIzGrupeDodatneInformacije(nodeDodatno, "Oštećenje", greske);

                    #endregion Dodatne informacije

                    #region Sigurnost
                    #endregion Sigurnost

                    #region Oprema
                    #endregion Oprema

                    #region Opis
                    var nodeOpis = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                            div.Attributes["class"].Value.Equals("description")).First();

                    string opis = string.Empty;
                    try
                    {
                        foreach (var element in nodeOpis.ChildNodes)
                        {
                            if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Text)
                                opis += element.InnerHtml.Trim();
                            else if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Element && element.Name == "br")
                                opis += Environment.NewLine;
                        }
                        opis = opis.Trim();
                        opis = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Opis': " + ex.Message);
                    }

                    #endregion Opis

                    #region Kontakt

                    var nodeKontakt = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                            div.Attributes["class"].Value.Equals("advertiser-info")).First();

                    string kontakt = string.Empty;
                    try
                    {
                        foreach(var element in nodeKontakt.ChildNodes.Where(node => node.NodeType == HtmlAgilityPack.HtmlNodeType.Text))
                            kontakt += element.InnerHtml.Trim();

                        if (kontakt.Length > 2000)
                            kontakt.Remove(2000);
                        kontakt = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Kontakt': " + ex.Message);
                    }

                    #endregion Kontakt

                    automobil = new Automobile(brojOglasa, naslov, cena, adresa, vozilo, marka, model, godinaProizvodnje, karoserija,
                        gorivo, fiksnaCena, zamena, datumPostavljanja, kubikaza, snagaKW, snagaKS, kilometraza, emisionaKlasa, pogon,
                        menjac, brojVrata, brojSedista, stranaVolana, klima, bojaKaroserije, registrovanDo, porekloVozila, opis, kontakt);

                    if (greske.Length > 0)
                    {
                        string upozorenje = string.Format("Greške pri parsiranju oglasa {0}, URL: {1}\nGreške: {2}", automobil.BrojOglasa, automobil.URL, greske);
                        Dnevnik.PisiSaThredomUpozorenje(upozorenje);
                    }
                }
                catch (Exception ex)
                {
                    Dnevnik.PisiSaThredomGreska("Ne mogu da parsiram html sa adrese: " + adresa, ex);
                }
            }
            else
            {
                throw new StranaNijeProcitanaException();
            }
        }

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
                ExtractAutomobile();
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
