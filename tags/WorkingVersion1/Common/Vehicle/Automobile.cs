using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common.Vehicle
{
    public class Automobile : Vehicle
    {
        // Zaglavlje
        string url;
        string naslov;
        float cena;

        public string Naslov { get { return naslov; } }
        public float Cena { get { return cena; } }
        public string URL { get { return url; } }

        // Opste informacije
        string vozilo;
        string marka;
        string model;
        int godinaProizvodnje;
        string karoserija;
        string gorivo;
        bool fiksnaCena;
        bool zamena;
        DateTime datumPostavljanja;
        int brojOglasa;

        public string Vozilo { get { return vozilo; } }
        public string Marka { get { return marka; } }
        public string Model { get { return model; } }
        public int GodinaProizvodnje { get { return godinaProizvodnje; } }
        public string Karoserija { get { return karoserija; } }
        public string Gorivo { get { return gorivo; } }
        public bool FiksnaCena { get { return fiksnaCena; } }
        public bool Zamena { get { return zamena; } }
        public DateTime DatumPostavljanja { get { return datumPostavljanja; } }
        public int BrojOglasa { get { return brojOglasa; } }

        // Dodatne informacije
        int kubikaza;
        int snagaKW; // kilovati
        int snagaKS; // konjska snaga
        int kilometraza;
        string emisionaKlasa;
        string pogon;
        string menjac;
        string brojVrata;
        byte brojSedista;
        string stranaVolana;
        string klima;
        string boja;
        DateTime registrovanDo;
        string porekloVozila;

        public int Kubikaza { get { return kubikaza; } }
        public int SnagaKW { get { return snagaKW; } }
        public int SnagaKS { get { return snagaKS; } }
        public int Kilometraza { get { return kilometraza; } }
        public string EmisionaKlasa { get { return emisionaKlasa; } }
        public string Pogon { get { return pogon; } }
        public string Menjac { get { return menjac; } }
        public string BrojVrata { get { return brojVrata; } }
        public byte BrojSedista { get { return brojSedista; } }
        public string StranaVolana { get { return stranaVolana; } }
        public string Klima { get { return klima; } }
        public string Boja { get { return boja; } }
        public DateTime RegistrovanDo { get { return registrovanDo; } }
        public string PorekloVozila { get { return porekloVozila; } }

        // Sigurnost

        // Oprema

        // Stanje volila

        // Opis
        string opis;
        public string Opis { get { return opis; } }

        // Kontakt
        string kontakt;
        public string Kontakt { get { return kontakt; } }

        // Slike

        public string CSV()
        {
            return
                //Zaglavlje
                naslov + ";" +
                cena + ";" +

                //Opste informacije
                vozilo + ";" +
                marka + ";" +
                model + ";" +
                godinaProizvodnje + ";" +
                karoserija + ";" +
                gorivo + ";" +
                DaNe(fiksnaCena) + ";" +
                DaNe(zamena) + ";" +
                datumPostavljanja + ";" +
                brojOglasa + ";" +

                //Dodatne informacije
                kubikaza + ";" +
                snagaKS + ";" +
                snagaKW + ";" +
                kilometraza + ";" +
                emisionaKlasa + ";" +
                pogon + ";" +
                Menjac + ";" +
                brojVrata + ";" +
                BrojSedista + ";" +
                StranaVolana + ";" +
                Klima + ";" +
                Boja + ";" +
                RegistrovanDo + ";" +
                PorekloVozila;
        }

        public static string CSVZaglavlje()
        {
            return
                //Zaglavlje
                "Naslov" + ";" +
                "Cena" + ";" +

                //Opste informacije
                "Vozilo" + ";" +
                "Marka" + ";" +
                "Model" + ";" +
                "GodinaProizvodnje" + ";" +
                "Karoserija" + ";" +
                "Gorivo" + ";" +
                "FiksnaCena" + ";" +
                "Zamena" + ";" +
                "DatumPostavljanja" + ";" +
                "BrojOglasa" + ";" +

                //Dodatne informacije
                "Kubikaza" + ";" +
                "Snaga" + ";" +
                "Kilometraza" + ";" +
                "EmisionaKlasa" + ";" +
                "Pogon" + ";" +
                "Menjac" + ";" +
                "BrojVrata" + ";" +
                "BrojSedista" + ";" +
                "StranaVolana" + ";" +
                "Klima" + ";" +
                "Boja" + ";" +
                "RegistrovanDo" + ";" +
                "PorekloVozila";
        }
        public static string CSVZaglavlje2()
        {
            Type type1 = typeof(Automobile);
            FieldInfo[] fi = type1.GetFields(System.Reflection.BindingFlags.GetField);
            string s = string.Empty;
            foreach (FieldInfo i in fi)
            {
                s += i.Name + ";";
            }
            return s;
        }
        public string CSV2()
        {
            Type type1 = typeof(Automobile);
            FieldInfo[] fi = type1.GetFields(System.Reflection.BindingFlags.Instance);
            string s = string.Empty;
            foreach (FieldInfo i in fi)
            {
                if (i != null && i.GetValue(this) != null)
                    s += "\"" + i.GetValue(this).ToString() + "\"";
                s += ";";
            }
            return s;
        }

        public Automobile(
            //zaglavlje
            int brojOglasa, string naslov, float cena, string url, 
            //opste informacije
            string vozilo, string marka, string model,
            int godinaProizvodnje, string karoserija, string gorivo, bool fiksnaCena, bool zamena, DateTime datumPostavljanja,
            //dodatne informacije            
            int kubikaza, int snagaKW, int snagaKS, int kilometraza, string emisionaKlasa, string pogon, string menjac,
            string brojVrata, byte brojSedista, string stranaVolana, string klima, string boja, DateTime registrovanDo,
            string porekloVozila,
            // Sigurnost
            // Oprema
            // Stanje volila
            // Opis
            string opis,
            //kontakt 
            string kontakt
            //slike
            )
        {
            // ovde bi mozda trebale neke provere na nekim poljima da se urade
            this.brojOglasa = brojOglasa;
            this.naslov = naslov;
            this.cena = cena;
            this.url = url;
            this.vozilo = vozilo;
            this.marka = marka;
            this.model = model;
            this.godinaProizvodnje = godinaProizvodnje;
            this.karoserija = karoserija;
            this.gorivo = gorivo;
            this.fiksnaCena = fiksnaCena;
            this.zamena = zamena;
            this.datumPostavljanja = datumPostavljanja;
            this.kubikaza = kubikaza;
            this.snagaKW = snagaKW;
            this.snagaKS = snagaKS;
            this.kilometraza = kilometraza;
            this.emisionaKlasa = emisionaKlasa;
            this.pogon = pogon;
            this.menjac = menjac;
            this.brojVrata = brojVrata;
            this.brojSedista = brojSedista;
            this.stranaVolana = stranaVolana;
            this.klima = klima;
            this.boja = boja;
            this.registrovanDo = registrovanDo;
            this.porekloVozila = porekloVozila;
            this.opis = opis.Length <= maxDescriptionLength ? opis : opis.Remove(maxDescriptionLength);
            this.kontakt = kontakt.Length <= maxContactLength ? kontakt : kontakt.Remove(maxContactLength);
        }

        string DaNe(bool b)
        {
            return b ? "Da" : "Ne";
        }

        public override string ToString() 
        {
            return string.Format("Broj oglasa: {0}; Marka: {1}; Model: {2}; Godište: {3}; Cena: {4}; URL: {5};", brojOglasa, marka, model, godinaProizvodnje, cena, url);
        }
    }
}
