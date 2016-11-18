using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Vehicle;
using Procode.PolovniAutomobili.Data;
using System.Collections;
using System.Threading;
using Procode.PolovniAutomobili.Data.Provider;

namespace Procode.PolovniAutomobili.Data.Vehicle
{
    public class Automobile : Vehicle
    {
        #region Private fields
        Procode.PolovniAutomobili.Data.Provider.Data Data;
        #endregion

        #region Constructors
        public Automobile(Procode.PolovniAutomobili.Data.Provider.Data data)
        {
            Data = data;
        }

        /// <summary>
        /// If no args are specified instance has it onw connection.
        /// </summary>
        public Automobile()
        {
            Data = Procode.PolovniAutomobili.Data.Provider.Data.GetNewDataInstance();
        }
        #endregion

        private Hashtable FillParams(Common.Vehicle.Automobile automobil)
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

        private bool Exists(int adNumber)
        {
            bool found = false;
            try
            {
                Hashtable par = new Hashtable();
                par.Add("@brojOglasa", adNumber);
                System.Data.DataSet exists = null;
                if (Data.GetDataSet("select null from AUTOMOBIL where brojOglasa = @brojOglasa", par, out exists))
                    found = exists != null && exists.Tables[0].Rows.Count == 1;
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Neuspela provera oglasa.", ex);
            }
            return found;
        }

        /// <summary>
        /// Inserts object Automobile in DB.
        /// </summary>
        /// <param name="automobil"></param>
        private bool Insert(Common.Vehicle.Automobile automobil)
        {
            Hashtable parameters;
            parameters = FillParams(automobil);
            return Data.Execute(
                "insert into AUTOMOBIL (brojoglasa, naslov, cena, url, vozilo, marka, model, godinaproizvodnje, karoserija, " +
                    " gorivo, fiksnacena, zamena, datumpostavljanja, kubikaza, snaga_KW, snaga_KS, Kilometraza, EmisionaKlasa, " +
                    " Pogon, Menjac, brojvrata, brojsedista, stranavolana, klima, boja, registrovando, POREKLOVOZILA, " +
                    " opis, kontakt, thread)" +
                    " values (@brojoglasa, @naslov, @cena, @url, @vozilo, @marka, @model, @godinaproizvodnje, @karoserija, " +
                    " @gorivo, @fiksnacena, @zamena, @datumpostavljanja, @kubikaza, @snaga_KW, @snaga_KS, @Kilometraza, @EmisionaKlasa, " +
                    " @Pogon, @Menjac, @brojvrata, @brojsedista, @stranavolana, @klima, @boja, @registrovando, @POREKLOVOZILA, " +
                    " @opis, @kontakt, @thread)"
                    , parameters);
        }
        /// <summary>
        /// Update u tabelu automobili.
        /// </summary>
        /// <param name="automobile"></param>
        private bool Update(Common.Vehicle.Automobile automobile)
        {
            Hashtable parameters;
            parameters = FillParams(automobile);

            StringBuilder updateCommand = new StringBuilder();
            updateCommand.AppendLine("UPDATE AUTOMOBIL");
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
            return Data.Execute(updateCommand.ToString(), parameters);
        }

        private bool SaveOnce(Common.Vehicle.Automobile automobile)
        {
            bool saveSucceed = false;
            if (automobile.BrojOglasa > 0)
            {
                if (Data.Open())
                {
                    if (Data.BeginTran())
                    {
                        try
                        {
                            if (!Exists(automobile.BrojOglasa))   // select
                                if (Insert(automobile))
                                    Common.Dnevnik.PisiSaImenomThreda("Uspešno dodat u bazu oglas " + automobile);
                                else
                                    Common.Dnevnik.PisiSaImenomThreda("Nije dodat u bazu oglas " + automobile);
                            else
                                if (Update(automobile))
                                    Common.Dnevnik.PisiSaImenomThreda("Uspešno izmenjen u bazi oglas " + automobile);
                                else
                                    Common.Dnevnik.PisiSaImenomThreda("Nije izmenjen u bazi oglas " + automobile);

                            if (Data.CommitTran())
                            {
                                saveSucceed = true;
                                Common.Dnevnik.PisiSaImenomThreda("Uspešno dodat u bazu oglas " + automobile);
                            }
                            else
                            {
                                //Common.Korisno.Korisno.LogError("Can't commit transaction. Automobile: " + automobile);
                                if (!Data.RollbackTran())
                                {
                                    Common.Korisno.Korisno.LogError(string.Format("Can't rollback transaction. Connection: {0}. Automobile: {1}", Data, automobile));
                                    if (!Data.Close())
                                    {
                                        Common.Korisno.Korisno.LogError("Can't close connection. Automobile: " + automobile);
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Korisno.Korisno.LogError(string.Format("Neuspelo azuriranje oglasa broj {0}.", automobile.BrojOglasa), ex);
                            if (!Data.RollbackTran())
                            {
                                Data.Open();
                                throw new Exception("Nisam uspeo da rollbekujem transakciju.");
                            }
                            throw ex;
                        }
                    }
                    else
                    {
                        Common.Korisno.Korisno.LogError("Can't begin transaction. Automobile: " + automobile);
                    }
                }
            }
            return saveSucceed;
        }

        /// <summary>
        /// Saves autmobile in DB.
        /// </summary>
        /// <param name="automobile">Automobile to save.</param>
        public bool Save(Common.Vehicle.Automobile automobile)
        {
            for (int i = 0; i < Properties.Settings.Default.NumberOfSaveAttempts; i++)
                if (SaveOnce(automobile))
                    return true;
                else
                {
                    Common.Korisno.Korisno.LogError(string.Format("SaveOnce didn't succeded. Attempt #{0}. Sleeping {1} ms.", i, Properties.Settings.Default.SleepTimeAfterFailedSave));
                    Thread.Sleep(Properties.Settings.Default.SleepTimeAfterFailedSave);
                }
            return false;
        }

        /// <summary>
        /// Saves automobile in DB. 
        /// </summary>
        /// <param name="automobil">Automobile to save.</param>
        /// <param name="withTransaction">If true saves automobile in transaction.</param>
        public void Save(Common.Vehicle.Automobile automobil, Boolean withTransaction)
        {
            if (withTransaction)
            {
                Save(automobil);
            }
            else
            {
                if (!Exists(automobil.BrojOglasa))
                {
                    Insert(automobil);
                }
                else
                {
                    Update(automobil);
                }
            }
        }

        public System.Data.DataSet GetAllAsDataSet(int count = 0)
        {
            string firstN = string.Empty;
            if (count > 0)
                firstN = string.Format(" first {0} ", count);
            System.Data.DataSet allAutomobiles = null;
            Data.GetDataSet(string.Format(
                @" select {0} BROJOGLASA, NASLOV, CENA, URL, VOZILO, MARKA, MODEL, GODINAPROIZVODNJE, KAROSERIJA, GORIVO, FIKSNACENA, ZAMENA, 
                       DATUMPOSTAVLJANJA, KUBIKAZA, SNAGA_KW, SNAGA_KS, KILOMETRAZA, EMISIONAKLASA, POGON, MENJAC, BROJVRATA,
                       BROJSEDISTA, STRANAVOLANA, KLIMA, BOJA, REGISTROVANDO, POREKLOVOZILA, OPIS, KONTAKT
                 from AUTOMOBIL A ", firstN), out allAutomobiles);
            return allAutomobiles;
        }

        private Common.Vehicle.Automobile DataRowToAutomobile(System.Data.DataRow dr)
        {
            Common.Vehicle.Automobile a = null;
            if (dr != null)
            {
                try
                {
                    int brojOglasa = int.Parse(dr["BrojOglasa"].ToString());
                    string naslov = dr["naslov"].ToString();
                    float cena = 0;
                    float.TryParse(dr["cena"].ToString(), out cena);
                    string url = dr["url"].ToString();
                    string vozilo = dr["vozilo"].ToString();
                    string marka = dr["marka"].ToString();
                    string model = dr["model"].ToString();
                    int godinaProizvodnje = 0;
                    int.TryParse(dr["godinaProizvodnje"].ToString(), out godinaProizvodnje);
                    string karoserija = dr["karoserija"].ToString();
                    string gorivo = dr["gorivo"].ToString();
                    bool fiksnaCena = dr["fiksnaCena"].ToString() == "1";
                    bool zamena = dr["zamena"].ToString() == "1";
                    DateTime datumPostavljanja = DateTime.MinValue;
                    DateTime.TryParse(dr["datumPostavljanja"].ToString(), out datumPostavljanja);
                    int kubikaza = 0;
                    int.TryParse(dr["kubikaza"].ToString(), out kubikaza);
                    int snagaKW = 0;
                    int.TryParse(dr["snaga_KW"].ToString(), out snagaKW);
                    int snagaKS = 0;
                    int.TryParse(dr["snaga_KS"].ToString(), out snagaKS);
                    int kilometraza = 0;
                    int.TryParse(dr["kilometraza"].ToString(), out kilometraza);
                    string emisionaKlasa = dr["emisionaKlasa"].ToString();
                    string pogon = dr["pogon"].ToString();
                    string menjac = dr["menjac"].ToString();
                    string brojVrata = dr["brojVrata"].ToString();
                    byte brojSedista = 0;
                    byte.TryParse(dr["brojSedista"].ToString(), out brojSedista);
                    string stranaVolana = dr["stranaVolana"].ToString();
                    string klima = dr["klima"].ToString();
                    string boja = dr["boja"].ToString();
                    DateTime registrovanDo = DateTime.MinValue;
                    DateTime.TryParse(dr["registrovanDo"].ToString(), out registrovanDo);
                    string porekloVozila = dr["porekloVozila"].ToString();
                    string opis = dr["opis"].ToString();
                    string kontakt = dr["kontakt"].ToString();

                    a = new Common.Vehicle.Automobile(brojOglasa, naslov, cena, url, vozilo, marka, model, godinaProizvodnje, karoserija,
                            gorivo, fiksnaCena, zamena, datumPostavljanja, kubikaza, snagaKW, snagaKS, kilometraza, emisionaKlasa, pogon, menjac,
                            brojVrata, brojSedista, stranaVolana, klima, boja, registrovanDo, porekloVozila, opis, kontakt);
                }
                catch (Exception ex)
                {
                    Common.Korisno.Korisno.LogError("Can't read automobile from database into object.", ex);
                }
            }
            return a;
        }

        public List<Common.Vehicle.Automobile> GetAllAsList()
        {
            System.Data.DataSet allAutomobiles;
            List<Common.Vehicle.Automobile> automobileList = null;
            if (Data.GetDataSet(
                @" select BROJOGLASA, NASLOV, CENA, URL, VOZILO, MARKA, MODEL, GODINAPROIZVODNJE, KAROSERIJA, GORIVO, FIKSNACENA, ZAMENA, 
                       DATUMPOSTAVLJANJA, KUBIKAZA, SNAGA_KW, SNAGA_KS, KILOMETRAZA, EMISIONAKLASA, POGON, MENJAC, BROJVRATA,
                       BROJSEDISTA, STRANAVOLANA, KLIMA, BOJA, REGISTROVANDO, POREKLOVOZILA, OPIS, KONTAKT
                 from AUTOMOBIL A ", out allAutomobiles))
            {
                if (allAutomobiles != null && allAutomobiles.Tables.Count > 0 && allAutomobiles.Tables[0].Rows.Count > 0)
                {
                    automobileList = new List<Common.Vehicle.Automobile>();
                    foreach (System.Data.DataRow autoDb in allAutomobiles.Tables[0].Rows)
                    {
                        Common.Vehicle.Automobile auto = DataRowToAutomobile(autoDb);
                        if (auto != null)
                            automobileList.Add(auto);
                    }
                }
            }
            return automobileList;
        }

        /// <summary>
        /// Returns two dimensional array with column header at position 0.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public object[,] GetAllAsArray(int count = 0)
        {
            System.Data.DataSet allAutomobiles = GetAllAsDataSet(count);
            object[,] automobileArray = null;
            if (allAutomobiles != null && allAutomobiles.Tables.Count > 0 && allAutomobiles.Tables[0].Rows.Count > 0)
            {
                automobileArray = new object[allAutomobiles.Tables[0].Rows.Count + 1, allAutomobiles.Tables[0].Columns.Count];
                for (int col = 0; col < allAutomobiles.Tables[0].Columns.Count; col++)
                    automobileArray[0, col] = allAutomobiles.Tables[0].Columns[col].ColumnName;
                for (int row = 1; row < allAutomobiles.Tables[0].Rows.Count + 1; row++)
                    for (int col = 0; col < allAutomobiles.Tables[0].Columns.Count; col++)
                        automobileArray[row, col] = allAutomobiles.Tables[0].Rows[row-1][col];
            }
            return automobileArray;
        }
    }
}
