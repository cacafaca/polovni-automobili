using System;
using System.Collections.Generic;
using System.Text;
using Common.Vehicle;
using System.Linq;

namespace Common.Http
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

                    string vozilo = string.Empty;
                    try
                    {
                        vozilo = nodeOpste.SelectNodes("ul/li[1]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje vozilo: " + ex.Message);
                    }

                    string marka = string.Empty;
                    try
                    {
                        marka = nodeOpste.SelectNodes("ul/li[2]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje marka: " + ex.Message);
                    }

                    string model = string.Empty;
                    try
                    {
                        model = nodeOpste.SelectNodes("ul/li[3]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje model: " + ex.Message);
                    }

                    int godinaProizvodnje = 0;
                    try
                    {
                        godinaProizvodnje = int.Parse(nodeOpste.SelectNodes("ul/li[4]")[0].InnerHtml.Trim().Substring(0, 4));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Godina proizvodnje': " + ex.Message);
                    }

                    int kilometraza = 0;
                    try
                    {
                        string kmStr = nodeOpste.SelectNodes("ul/li[5]")[0].InnerHtml.Trim();
                        kilometraza = int.Parse(kmStr.Replace(" km", string.Empty).Replace(".", string.Empty));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Kilometraža': " + ex.Message);
                    }

                    string karoserija = string.Empty; ;
                    try
                    {
                        karoserija = nodeOpste.SelectNodes("ul/li[6]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje karoserija: " + ex.Message);
                    }

                    string gorivo = string.Empty;
                    try
                    {
                        gorivo = nodeOpste.SelectNodes("ul/li[7]")[0].InnerHtml.Trim();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje gorivo: " + ex.Message);
                    }

                    int kubikaza = 0;
                    try
                    {
                        string kubikazaStr = nodeOpste.SelectNodes("ul/li[8]")[0].InnerHtml.Trim().Replace(" cm3", string.Empty);
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
                        string snaga = nodeOpste.SelectNodes("ul/li[9]")[0].InnerHtml.Trim().Replace(" (kW/KS)", string.Empty);
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
                        string fiksnaCenaStr = nodeOpste.SelectNodes("ul/li[10]")[0].InnerHtml.Trim();
                        fiksnaCena = fiksnaCenaStr.ToLower() != "Cena nije fiksna".ToLower();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje fiksnaCena: " + ex.Message);
                    }

                    bool zamena = false;
                    try
                    {
                        string zamenaStr = nodeOpste.SelectNodes("ul/li[11]")[0].InnerHtml.Trim();
                        zamena = zamenaStr.ToLower() != "Zamena: NE".ToLower();
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje fiksnaCena: " + ex.Message);
                    }

                    DateTime datumPostavljanja = DateTime.MinValue;
                    try
                    {
                        string datumPostavljanjaStr = nodeOpste.SelectNodes("ul/li[13]/time")[0].Attributes["datetime"].Value.Trim(); ;
                        datumPostavljanja = DateTime.SpecifyKind(DateTime.Parse(datumPostavljanjaStr), DateTimeKind.Utc);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Datum postavljanja': " + ex.Message);
                    }

                    int brojOglasa = 0;
                    try
                    {
                        brojOglasa = int.Parse(nodeOpste.SelectNodes("ul/li[14]")[0].InnerHtml.Trim().Replace("Broj oglasa: ", string.Empty));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Broj oglasa': " + ex.Message);
                    }
                    #endregion Opste

                    #region Dodatne informacije

                    var nodeDodatno = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                        div.Attributes["class"].Value.Equals("detailed-info")).First();

                    string emisionaKlasa = nodeDodatno.SelectNodes("ul/li[1]/span[2]")[0].InnerHtml.Trim();

                    string pogon = nodeDodatno.SelectNodes("ul/li[2]/span[2]")[0].InnerHtml.Trim();

                    string menjac = nodeDodatno.SelectNodes("ul/li[3]/span[2]")[0].InnerHtml.Trim();

                    string brojVrata = nodeDodatno.SelectNodes("ul/li[4]/span[2]")[0].InnerHtml.Trim().Replace(" vrata", string.Empty);

                    byte brojSedista = byte.Parse(nodeDodatno.SelectNodes("ul/li[5]/span[2]")[0].InnerHtml.Trim().Replace(" sedišta", string.Empty));

                    string stranaVolana = nodeDodatno.SelectNodes("ul/li[6]/span[2]")[0].InnerHtml.Trim();

                    string klima = nodeDodatno.SelectNodes("ul/li[7]/span[2]")[0].InnerHtml.Trim();

                    string bojaKaroserije = nodeDodatno.SelectNodes("ul/li[8]/span[2]")[0].InnerHtml.Trim();

                    string materijalEnterijera = nodeDodatno.SelectNodes("ul/li[9]/span[2]")[0].InnerHtml.Trim();

                    string bojaEnterijera = nodeDodatno.SelectNodes("ul/li[10]/span[2]")[0].InnerHtml.Trim();

                    DateTime registrovanDo = DateTime.MinValue;
                    try
                    {
                        string regDoStr = "01." + nodeDodatno.SelectNodes("ul/li[11]/span[2]")[0].InnerHtml.Trim();
                        regDoStr = regDoStr.Remove(regDoStr.Length - 1);
                        DateTime.TryParse(regDoStr, out registrovanDo);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Registrovan do': " + ex.Message);
                    }

                    string porekloVozila = nodeDodatno.SelectNodes("ul/li[12]/span[2]")[0].InnerHtml.Trim();

                    string vlastnistvo = nodeDodatno.SelectNodes("ul/li[13]/span[2]")[0].InnerHtml.Trim();

                    string ostecenje = nodeDodatno.SelectNodes("ul/li[14]/span[2]")[0].InnerHtml.Trim();

                    #endregion Dodatne informacije

                    #region Sigurnost
                    #endregion Sigurnost

                    #region Oprema
                    #endregion Oprema

                    #region Opis
                    var nodeOpis = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                            div.Attributes["class"].Value.Equals("description")).First();

                    string opis = string.Empty;
                    foreach (var element in nodeOpis.ChildNodes)
                    {
                        if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Text)
                            opis += element.InnerHtml.Trim();
                        else if (element.NodeType == HtmlAgilityPack.HtmlNodeType.Element && element.Name == "br")
                            opis += Environment.NewLine;
                    }
                    opis = opis.Trim();

                    #endregion Opis

                    #region Kontakt

                    var nodeKontakt = dok.DocumentNode.Descendants("div").Where(div => div.Attributes.Contains("class") &&
                            div.Attributes["class"].Value.Equals("advertiser-info")).First();
                    string kontakt = nodeKontakt.InnerHtml.Trim();

                    #endregion Kontakt

                    automobil = new Automobile(brojOglasa, naslov, cena, adresa, vozilo, marka, model, godinaProizvodnje, karoserija,
                        gorivo, fiksnaCena, zamena, datumPostavljanja, kubikaza, snagaKW, snagaKS, kilometraza, emisionaKlasa, pogon,
                        menjac, brojVrata, brojSedista, stranaVolana, klima, bojaKaroserije, registrovanDo, porekloVozila, opis, kontakt);

                    if (greske.Length > 0)
                    {
                        string upozorenje = string.Format("Greške pri parsiranju oglasa {0}, URL: {1}\nGreške: {2}", automobil.BrojOglasa, automobil.URL, greske);
                        Dnevnik.PisiSaThredomUpozorenje(upozorenje);
                        //Dnevnik.PisiSaThredomUpozorenje("Sadržaj strane: " + Sadrzaj);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.WriteEventError("Greška pri parsiranju strane oglasa: " + adresa, ex);
                }
            }
            else
            {
                throw new StranaNijeProcitanaException();
            }
        }

        private string DajPodatakIzDokumenta(ref HtmlAgilityPack.HtmlDocument dok, PodaciOAutomobilu tipPodatka, string[] nazivPodatka, string podrVred)
        {
            // odredjivanje stringa za pretragu
            string putanja = string.Empty;
            switch (tipPodatka)
            {
                case PodaciOAutomobilu.OpsteInformacije:
                case PodaciOAutomobilu.DodatneInformacije:
                    putanja = "//*[@id=\"tbl-details\"]";
                    break;
                case PodaciOAutomobilu.Sigurnost:
                case PodaciOAutomobilu.Oprema:
                    putanja = string.Empty;
                    break;
                case PodaciOAutomobilu.Opis:
                    putanja = "//*[@id=\"tab_bg\"]/div[1]";
                    break;
                case PodaciOAutomobilu.Kontakt:
                    putanja = "//*[@id=\"details-agency\"]/div[1]";
                    break;
            }
            if (putanja.Equals(string.Empty))
                return string.Empty;

            // citanje cvora iz putanje
            HtmlAgilityPack.HtmlNodeCollection nodeColl;
            try
            {
                nodeColl = dok.DocumentNode.SelectNodes(putanja);
            }
            catch (Exception ex)
            {
                Dnevnik.PisiSaThredomGreska("Gršeska pri čitanju HTML čvora na putanji " + putanja + ". Greška: " + ex.Message);
                return string.Empty;
            }

            // odredjivanje podatka
            switch (tipPodatka)
            {
                case PodaciOAutomobilu.OpsteInformacije:
                    if (nodeColl != null && nodeColl[0].ChildNodes != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode node in nodeColl[0].ChildNodes)
                        {
                            if (node.ChildNodes.Count > 0)
                            {
                                foreach (string nazPod in nazivPodatka)
                                {
                                    if (node.ChildNodes[1].InnerHtml.Trim().ToLower().Contains(nazPod.ToLower()))
                                    {
                                        return node.ChildNodes[3].InnerHtml.Trim();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Dnevnik.PisiSaImenomThreda("Nema cele tabele.");
                    }
                    return podrVred;
                case PodaciOAutomobilu.DodatneInformacije:
                    if (nodeColl != null && nodeColl[0].ChildNodes != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode node in nodeColl[0].ChildNodes)
                        {
                            if (node.ChildNodes.Count > 0)
                            {
                                foreach (string nazPod in nazivPodatka)
                                {
                                    if (node.ChildNodes[1].InnerHtml.Trim().ToLower().Contains(nazPod.ToLower()))
                                    {
                                        return node.ChildNodes[3].InnerHtml.Trim();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Dnevnik.PisiSaImenomThreda("Nema cele tabele.");
                    }
                    return podrVred;
                case PodaciOAutomobilu.Sigurnost:
                case PodaciOAutomobilu.Oprema:
                    break;
                case PodaciOAutomobilu.Opis:
                    if (nodeColl != null && nodeColl.Count > 0)
                    {
                        foreach (HtmlAgilityPack.HtmlNode node in nodeColl[0].ChildNodes)
                        {
                            if (node.ChildNodes != null && node.ChildNodes.Count > 1 &&
                                node.ChildNodes[1].InnerHtml.Trim().ToLower().Equals("opis"))
                            {
                                if (node.ChildNodes.Count >= 3)
                                    return node.ChildNodes[3].InnerHtml;
                            }
                        }
                    }
                    return podrVred;
                case PodaciOAutomobilu.Kontakt:
                    if (nodeColl != null && nodeColl.Count > 0)
                    {
                        return Common.Korisno.Korisno.TrimMultiline(nodeColl[0].InnerText);
                    }
                    break;
            }
            return String.Empty;
        }

        private string DajPodatakIzDokumenta(ref HtmlAgilityPack.HtmlDocument dok, PodaciOAutomobilu tipPodatka, string[] nazivPodatka)
        {
            return DajPodatakIzDokumenta(ref dok, tipPodatka, nazivPodatka, string.Empty);
        }

        private string DajPodatakIzDokumenta(ref HtmlAgilityPack.HtmlDocument dok, PodaciOAutomobilu tipPodatka, string nazivPodatka)
        {
            return DajPodatakIzDokumenta(ref dok, tipPodatka, new string[] { nazivPodatka });
        }

        private string DajPodatakIzDokumenta(ref HtmlAgilityPack.HtmlDocument dok, PodaciOAutomobilu tipPodatka, string nazivPodatka, string podrVred)
        {
            return DajPodatakIzDokumenta(ref dok, tipPodatka, new string[] { nazivPodatka }, podrVred);
        }

        private string DajPodatakIzDokumenta(ref HtmlAgilityPack.HtmlDocument dok, PodaciOAutomobilu tipPodatka)
        {
            return DajPodatakIzDokumenta(ref dok, tipPodatka, string.Empty, string.Empty);
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
