﻿using System;
using System.Collections.Generic;
using System.Text;
using Common.Vozilo;

namespace Common.Http
{
    public class StranaOglasa : Strana
    {
        public StranaOglasa(string adresa)
            : base(adresa)
        {
        }

        private Automobil automobil;
        public Automobil Automobil { get { return automobil; } }

        private enum PodaciOAutomobilu { OpsteInformacije, DodatneInformacije, Sigurnost, Oprema, StanjeVozila, Opis, Kontakt }

        public void Obradi()
        {
            if (Sadrzaj != null && !Sadrzaj.Equals(string.Empty))
            {
                HtmlAgilityPack.HtmlDocument dok = new HtmlAgilityPack.HtmlDocument();
                try
                {
                    dok.LoadHtml(Sadrzaj);
                    StringBuilder greske = new StringBuilder();
                    HtmlAgilityPack.HtmlNodeCollection nodeCol;

                    // provera da li je strana pronadjena
                    nodeCol = dok.DocumentNode.SelectNodes("//*[@id=\"frame\"]/div[2]/div/div[5]/h1");
                    if (nodeCol != null && nodeCol[0].InnerHtml.Trim().ToUpper().Equals("Stranica nije pronađena".ToUpper()))
                    {
                        automobil = null;
                        return;
                    }

                    //Zaglavlje 
                    string naslov = string.Empty;
                    try
                    {
                        nodeCol = dok.DocumentNode.SelectNodes("//*[@id=\"listing\"]/div[1]/h1/div[1]");
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
                        string cenaStr = dok.DocumentNode.SelectNodes("//*[@id=\"listing\"]/div[1]/h1/div[2]")[0].InnerHtml.Trim();
                        if (cenaStr.ToLower().Equals("na upit"))
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

                    //Opste
                    string vozilo = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "vozilo");
                    string marka = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "marka");
                    string model = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "model");
                    int godinaProizvodnje = 0;
                    try
                    {
                        godinaProizvodnje = int.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "Godina proizvodnje", "0"));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Godina proizvodnje': " + ex.Message);
                    }
                    string karoserija = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "karoserija");
                    string gorivo = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "gorivo");
                    bool fiksnaCena = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "Fiksna cena").ToLower() == "da";
                    bool zamena = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "zamena").ToLower() == "da";
                    DateTime datumPostavljanja = DateTime.MinValue; ;
                    try
                    {
                        datumPostavljanja = DateTime.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "Datum postavljanja", DateTime.MinValue.ToShortDateString()).TrimEnd('.'));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Datum postavljanja': " + ex.Message);
                    }
                    int brojOglasa = 0;
                    try
                    {
                        brojOglasa = int.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.OpsteInformacije, "Broj oglasa"));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Broj oglasa': " + ex.Message);
                    }

                    // Dodatne informacije
                    int kubikaza = 0;
                    try
                    {
                        kubikaza = int.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, new string[] {"Kubikaža (cm3)", "Kubika"}, "0"));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Kubikaža': " + ex.Message);
                    }
                    int snagaKW = 0;
                    int snagaKS = 0;
                    try
                    {
                        snagaKW = int.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Snaga", "0").Replace(".", "")) / 100;
                        snagaKS = SnagaKWUKS(snagaKW);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Snaga': " + ex.Message);
                    }
                    int kilometraza = 0;
                    try
                    {
                        kilometraza = int.Parse(DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, new string[] {"Kilometraža", "Kilometra"}));
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Kilometraža': " + ex.Message);
                    }
                    string emisionaKlasa = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Emisiona klasa motora");
                    string pogon = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Pogon");
                    string menjac = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Menjač");
                    string brojVrata = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Broj vrata").Replace(" vrata", "");
                    string brojSedistaStr = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Broj sedišta", "0");
                    byte brojSedista = 0;
                    try
                    {
                        if (brojSedistaStr.IndexOf(' ') > 0)
                            brojSedista = byte.Parse(brojSedistaStr.Remove(brojSedistaStr.IndexOf(' ')));
                        else
                            if (!brojSedistaStr.Equals(string.Empty))
                                brojSedista = byte.Parse(brojSedistaStr);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Broj sedišta': " + ex.Message);
                    }
                    string stranaVolana = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Strana volana");
                    string klima = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Klima");
                    string boja = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Boja");
                    DateTime registrovanDo = DateTime.MinValue;
                    try
                    {
                        string regDoStr = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Registrovan do", DateTime.MinValue.ToShortDateString()).TrimEnd('.');
                        DateTime.TryParse(regDoStr, out registrovanDo);
                    }
                    catch (Exception ex)
                    {
                        greske.AppendLine("Nisam mogao da pročitam polje 'Registrovan do': " + ex.Message);
                    }
                    string porekloVozila = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.DodatneInformacije, "Poreklo vozila");

                    // Sigurnost

                    // Oprema

                    // Opis
                    string opis = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.Opis);

                    //kontakt
                    string kontakt = DajPodatakIzDokumenta(ref dok, PodaciOAutomobilu.Kontakt).Replace("(Pogledajte mapu)\r\n", "");

                    automobil = new Automobil(brojOglasa, naslov, cena, adresa, vozilo, marka, model, godinaProizvodnje, karoserija,
                        gorivo, fiksnaCena, zamena, datumPostavljanja, kubikaza, snagaKW, snagaKS, kilometraza, emisionaKlasa, pogon,
                        menjac, brojVrata, brojSedista, stranaVolana, klima, boja, registrovanDo, porekloVozila, opis, kontakt);

                    if (greske.Length > 0)
                    {
                        string upozorenje = string.Format("Greške pri parsiranju oglasa {0}, URL: {1}\n", automobil.BrojOglasa, automobil.URL) + greske;
                        Dnevnik.PisiSaThredomUpozorenje(upozorenje);
                        Dnevnik.PisiSaThredomUpozorenje("Sadržaj strane: " + Sadrzaj);
                        //EventLogger.WriteEventWarning(upozorenje);
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
                Dnevnik.PisiSaThredomGreska("Greska pri citanju HTML cvora na putanji " + putanja);
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
                        Dnevnik.PisiSaThredom("Nema cele tabele.");
                    }
                    return podrVred;
                    break;
                case PodaciOAutomobilu.DodatneInformacije:
                    if (nodeColl != null && nodeColl[1].ChildNodes != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode node in nodeColl[1].ChildNodes)
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
                        Dnevnik.PisiSaThredom("Nema cele tabele.");
                    }
                    return podrVred;
                    break;
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
            return DajPodatakIzDokumenta(ref dok, tipPodatka, new string[] {nazivPodatka});
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
            if(Sadrzaj != null)
                Obradi();
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

    }

    public class StranaNijeProcitanaException : Exception
    {
        public StranaNijeProcitanaException(string message) : base(message) { }
        public StranaNijeProcitanaException() : base("Strana nije procitana.") { }
    }

}
