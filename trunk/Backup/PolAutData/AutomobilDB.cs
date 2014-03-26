using System;
using System.Collections.Generic;
using System.Text;
using Common.Vozilo;
using PolAutData;
using System.Collections;
using System.Threading;

namespace PolAutData
{
    public class AutomobilDB
    {
        Data data;
        public AutomobilDB()
        {
            data = new Data();
        }
        private Hashtable NapuniParametre(Automobil automobil)
        {
            Hashtable par = new Hashtable();
            par.Add("@brojoglasa", automobil.BrojOglasa);
            par.Add("@naslov", automobil.Naslov);
            par.Add("@cena", automobil.Cena);
            par.Add("@url", automobil.URL);
            par.Add("@vozilo", automobil.Vozilo);
            par.Add("@marka", automobil.Marka);
            par.Add("@model", automobil.Model);
            par.Add("@godinaproizvodnje", automobil.GodinaProizvodnje);
            par.Add("@karoserija", automobil.Karoserija);
            par.Add("@gorivo", automobil.Gorivo);
            par.Add("@fiksnacena", automobil.FiksnaCena);
            par.Add("@zamena", automobil.Zamena);
            par.Add("@datumpostavljanja", automobil.DatumPostavljanja);
            //dodatne informacije
            par.Add("@kubikaza", automobil.Kubikaza);
            par.Add("@snaga_KW", automobil.SnagaKW);
            par.Add("@snaga_KS", automobil.SnagaKS);
            par.Add("@Kilometraza", automobil.Kilometraza);
            par.Add("@emisionaklasa", automobil.EmisionaKlasa);
            par.Add("@pogon", automobil.Pogon);
            par.Add("@menjac", automobil.Menjac);
            par.Add("@brojvrata", automobil.BrojVrata);
            par.Add("@brojsedista", automobil.BrojSedista);
            par.Add("@stranavolana", automobil.StranaVolana);
            par.Add("@klima", automobil.Klima);
            par.Add("@boja", automobil.Boja);
            if (automobil.RegistrovanDo != DateTime.MinValue)
            {
                par.Add("@registrovando", automobil.RegistrovanDo);
            }
            else
            {
                par.Add("@registrovando", null);
            }
            par.Add("@POREKLOVOZILA", automobil.PorekloVozila);
            //sigurnost
            //oprema
            //stanje vozila
            //opis
            par.Add("@Opis", automobil.Opis);
            //kontakt
            par.Add("@Kontakt", automobil.Kontakt);
            //slike mozda

            par.Add("@thread", Thread.CurrentThread.Name);
            return par;
        }
        private bool PostojiOglas(Automobil automobil)
        {
            bool postoji = false;
            try
            {
                Hashtable par = new Hashtable();
                par.Add("@brojOglasa", automobil.BrojOglasa);
                System.Data.DataSet ds = data.Otvori("select null from automobil where brojOglasa = @brojOglasa", par);
                postoji = ds != null && ds.Tables[0].Rows.Count == 1;
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Neuspela provera oglasa.", ex);
            }
            return postoji;
        }
        /// <summary>
        /// Insert u tabelu AUTOMOBILI
        /// </summary>
        /// <param name="automobil"></param>
        private void Dodaj(Automobil automobil)
        {
            Hashtable par;
            par = NapuniParametre(automobil);
            if (!data.Izvrsi(
                "insert into automobil (brojoglasa, naslov, cena, url, vozilo, marka, model, godinaproizvodnje, karoserija, " +
                    " gorivo, fiksnacena, zamena, datumpostavljanja, kubikaza, snaga_KW, snaga_KS, Kilometraza, EmisionaKlasa, " +
                    " Pogon, Menjac, brojvrata, brojsedista, stranavolana, klima, boja, registrovando, POREKLOVOZILA, " +
                    " opis, kontakt, thread)" +
                    " values (@brojoglasa, @naslov, @cena, @url, @vozilo, @marka, @model, @godinaproizvodnje, @karoserija, " +
                    " @gorivo, @fiksnacena, @zamena, @datumpostavljanja, @kubikaza, @snaga_KW, @snaga_KS, @Kilometraza, @EmisionaKlasa, " +
                    " @Pogon, @Menjac, @brojvrata, @brojsedista, @stranavolana, @klima, @boja, @registrovando, @POREKLOVOZILA, " +
                    " @opis, @kontakt, @thread)"
                    , par))
            {
                throw new Exception("Neuspelo dodavanje.");
            }
        }
        /// <summary>
        /// Update u tabelu automobili.
        /// </summary>
        /// <param name="automobil"></param>
        private void Izmeni(Automobil automobil)
        {
            Hashtable par;
            par = NapuniParametre(automobil);

            StringBuilder var1 = new StringBuilder();
            var1.AppendLine("UPDATE automobil");
            var1.AppendLine("SET    naslov = @naslov");
            var1.AppendLine("       ,cena = @cena");
            var1.AppendLine("       ,url = @url");
            var1.AppendLine("       ,vozilo = @vozilo");
            var1.AppendLine("       ,marka = @marka");
            var1.AppendLine("       ,model = @model");
            var1.AppendLine("       ,godinaproizvodnje = @godinaproizvodnje");
            var1.AppendLine("       ,karoserija = @karoserija");
            var1.AppendLine("       ,gorivo = @gorivo");
            var1.AppendLine("       ,fiksnacena = @fiksnacena");
            var1.AppendLine("       ,zamena = @zamena");
            var1.AppendLine("       ,datumpostavljanja = @datumpostavljanja");
            var1.AppendLine("       ,kubikaza = @kubikaza");
            var1.AppendLine("       ,snaga_kw = @snaga_kw");
            var1.AppendLine("       ,snaga_ks = @snaga_ks");
            var1.AppendLine("       ,kilometraza = @kilometraza");
            var1.AppendLine("       ,emisionaklasa = @emisionaklasa");
            var1.AppendLine("       ,pogon = @pogon");
            var1.AppendLine("       ,menjac = @menjac");
            var1.AppendLine("       ,brojvrata = @brojvrata");
            var1.AppendLine("       ,brojsedista = @brojsedista");
            var1.AppendLine("       ,stranavolana = @stranavolana");
            var1.AppendLine("       ,klima = @klima");
            var1.AppendLine("       ,boja = @boja");
            var1.AppendLine("       ,registrovando = @registrovando");
            var1.AppendLine("       ,poreklovozila = @poreklovozila");
            var1.AppendLine("       ,opis = @opis");
            var1.AppendLine("       ,kontakt = @kontakt");
            var1.AppendLine("       ,thread = @thread");
            var1.AppendLine("WHERE  brojoglasa = @brojoglasa ");
            if (!data.Izvrsi(var1.ToString(), par))
            {
                throw new Exception("Nisam uspeo da izmenim zapis.");
            }
        }
        /// <summary>
        /// Provera i dodavanje/izmena se obavlja u jednoj transakciji.
        /// </summary>
        /// <param name="automobil"></param>
        public void Snimi(Automobil automobil)
        {
            try
            {
                if (data.TranasakcijaOtvorena()) // ako je otvorena zatvori. ne smem da zateknem ovde otvorenu transakciju
                {
                    Common.Dnevnik.PisiSaThredomUpozorenje("Transakcija je bila otvorena, a nije trebala da bude.");
                    data.RollbackTran();
                }
            }
            catch (Exception ex)
            {
                //Transakcija je trebala da bude otvorena ali nije mogao da rolbackuje.
                Common.Dnevnik.PisiSaThredomGreska("Nisam mogao da rollbackujem transakciju.", ex);
                try
                {
                    data.OtvoriPonovoKonekciju();
                }
                catch (Exception ex1)
                {
                    // Ako ovo ne uspe onda ne znam sta da mu radim!? Samo cu da logujem.
                    Common.Dnevnik.PisiSaThredomGreska("Nisam uspeo da ponovo otvorim konekciju.", ex1);
                    throw ex1;
                }
            }
            try
            {
                if (data.BeginTran())
                {
                    try
                    {
                        if (!PostojiOglas(automobil))   // select
                        {
                            Dodaj(automobil);   // insert
                        }
                        else
                        {
                            Izmeni(automobil);  // update
                        }
                        if (!data.CommitTran())
                        {
                            // posto nije uspeo da komituje, pokusacu da rollbekujem
                            if (!data.RollbackTran())
                            {
                                data.OtvoriPonovoKonekciju();                                
                            }
                            throw new Exception("Nisam uspeo da komitujem transakciju.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Korisno.Korisno.LogujGresku("Neuspelo azuriranje oglasa.", ex);
                        if (!data.RollbackTran())
                        {
                            data.OtvoriPonovoKonekciju();
                            throw new Exception("Nisam uspeo da rollbekujem transakciju.");
                        }
                        throw ex;
                    }
                }
                else
                {
                    throw new Exception("Ne mogu da otvorim transakciju.");
                }
            }
            catch (Exception ex)
            {
                Common.Korisno.Korisno.LogujGresku("Snimanje nije uspelo.", ex);
            }
        }
        /// <summary>
        /// Provera i dodavanje ili izmena se obavljaju u jednoj razlicitim transakcijama.
        /// </summary>
        /// <param name="automobil"></param>
        public void Snimi2(Automobil automobil)
        {
            if (!PostojiOglas(automobil))
            {
                Dodaj(automobil);
            }
            else
            {
                Izmeni(automobil);
            }
        }
    }
}
