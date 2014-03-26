using System;
using System.Collections.Generic;
using System.Text;
using Common.Vozilo;
using PolAutData;
using System.Collections;
using System.Threading;

namespace PolAutData.Vehicle
{
    public class Automobile: Vehicle
    {
        #region Private fields
        Data data;
        #endregion

        #region Constructors
        public Automobile()
        {
            data = Data.GetDataInstance();
        }
        #endregion

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
        private bool Dodaj(Automobil automobil)
        {
            Hashtable parametri;
            parametri = NapuniParametre(automobil);
            return data.Izvrsi(
                "insert into automobil (brojoglasa, naslov, cena, url, vozilo, marka, model, godinaproizvodnje, karoserija, " +
                    " gorivo, fiksnacena, zamena, datumpostavljanja, kubikaza, snaga_KW, snaga_KS, Kilometraza, EmisionaKlasa, " +
                    " Pogon, Menjac, brojvrata, brojsedista, stranavolana, klima, boja, registrovando, POREKLOVOZILA, " +
                    " opis, kontakt, thread)" +
                    " values (@brojoglasa, @naslov, @cena, @url, @vozilo, @marka, @model, @godinaproizvodnje, @karoserija, " +
                    " @gorivo, @fiksnacena, @zamena, @datumpostavljanja, @kubikaza, @snaga_KW, @snaga_KS, @Kilometraza, @EmisionaKlasa, " +
                    " @Pogon, @Menjac, @brojvrata, @brojsedista, @stranavolana, @klima, @boja, @registrovando, @POREKLOVOZILA, " +
                    " @opis, @kontakt, @thread)"
                    , parametri);
        }
        /// <summary>
        /// Update u tabelu automobili.
        /// </summary>
        /// <param name="automobil"></param>
        private bool Izmeni(Automobil automobil)
        {
            Hashtable parametri;
            parametri = NapuniParametre(automobil);

            StringBuilder updateCommand = new StringBuilder();
            updateCommand.AppendLine("UPDATE automobil");
            updateCommand.AppendLine("SET    naslov = @naslov");
            updateCommand.AppendLine("       ,cena = @cena");
            updateCommand.AppendLine("       ,url = @url");
            updateCommand.AppendLine("       ,vozilo = @vozilo");
            updateCommand.AppendLine("       ,marka = @marka");
            updateCommand.AppendLine("       ,model = @model");
            updateCommand.AppendLine("       ,godinaproizvodnje = @godinaproizvodnje");
            updateCommand.AppendLine("       ,karoserija = @karoserija");
            updateCommand.AppendLine("       ,gorivo = @gorivo");
            updateCommand.AppendLine("       ,fiksnacena = @fiksnacena");
            updateCommand.AppendLine("       ,zamena = @zamena");
            updateCommand.AppendLine("       ,datumpostavljanja = @datumpostavljanja");
            updateCommand.AppendLine("       ,kubikaza = @kubikaza");
            updateCommand.AppendLine("       ,snaga_kw = @snaga_kw");
            updateCommand.AppendLine("       ,snaga_ks = @snaga_ks");
            updateCommand.AppendLine("       ,kilometraza = @kilometraza");
            updateCommand.AppendLine("       ,emisionaklasa = @emisionaklasa");
            updateCommand.AppendLine("       ,pogon = @pogon");
            updateCommand.AppendLine("       ,menjac = @menjac");
            updateCommand.AppendLine("       ,brojvrata = @brojvrata");
            updateCommand.AppendLine("       ,brojsedista = @brojsedista");
            updateCommand.AppendLine("       ,stranavolana = @stranavolana");
            updateCommand.AppendLine("       ,klima = @klima");
            updateCommand.AppendLine("       ,boja = @boja");
            updateCommand.AppendLine("       ,registrovando = @registrovando");
            updateCommand.AppendLine("       ,poreklovozila = @poreklovozila");
            updateCommand.AppendLine("       ,opis = @opis");
            updateCommand.AppendLine("       ,kontakt = @kontakt");
            updateCommand.AppendLine("       ,thread = @thread");
            updateCommand.AppendLine("WHERE  brojoglasa = @brojoglasa ");
            return data.Izvrsi(updateCommand.ToString(), parametri);
        }

        /// <summary>
        /// Provera i dodavanje/izmena se obavlja u jednoj transakciji.
        /// </summary>
        /// <param name="automobil">Objekat koji se snima u bazu.</param>
        public void Snimi(Automobil automobil)
        {
            if (data.TranasakcijaOtvorena()) // ako je otvorena zatvori. ne smem da zateknem ovde otvorenu transakciju
            {
                Common.Dnevnik.PisiSaThredomUpozorenje("Transakcija je bila otvorena, a nije trebala da bude. Rollback-ujem");
                try
                {
                    data.RollbackTran();
                }
                catch (Exception ex)
                {
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
            }
            try
            {
                if (data.BeginTran())
                {
                    try
                    {
                        if (!PostojiOglas(automobil))   // select
                            if(Dodaj(automobil))
                                Common.Dnevnik.PisiSaThredom("Uspešno dodat u bazu oglas " + automobil);
                            else
                                Common.Dnevnik.PisiSaThredom("Nije dodat u bazu oglas " + automobil);
                        else
                            if (Izmeni(automobil))
                                Common.Dnevnik.PisiSaThredom("Uspešno izmenjen u bazi oglas " + automobil);
                            else
                                Common.Dnevnik.PisiSaThredom("Nije izmenjen u bazi oglas " + automobil);
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
                        Common.Korisno.Korisno.LogujGresku(string.Format("Neuspelo azuriranje oglasa broj {0}.", automobil.BrojOglasa), ex);
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
        /// Provera i dodavanje, ili provera i izmena se obavljaju u razlicitim transakcijama.
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
