using System;
using System.Collections.Generic;
using System.Text;
using Procode.PolovniAutomobili.Common.Model.Vehicle;
using Procode.PolovniAutomobili.Data;
using System.Collections;
using System.Threading;
using Procode.PolovniAutomobili.Data.Provider;
using System.Data;

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


        private Hashtable FillParams(Common.Model.Vehicle.Automobile automobil)
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

            par.Add("@thread", Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name : string.Empty);
            return par;
        }

        private bool Exists(int adNumber)
        {
            CheckOrCreateTable();

            bool found = false;
            try
            {
                Hashtable par = new Hashtable();
                par.Add("@brojOglasa", adNumber);
                System.Data.DataSet exists = null;
                if (Data.GetDataSet("select null from AUTOMOBILe where brojOglasa = @brojOglasa", par, out exists))
                    found = exists != null && exists.Tables[0].Rows.Count == 1;
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Neuspela provera oglasa.", ex);
            }
            return found;
        }

        /// <summary>
        /// Check if table exists.
        /// </summary>
        /// <returns></returns>
        private bool TableExists()
        {
            DataSet numberOfTables;
            Data.GetDataSet(
@"IF EXISTS (SELECT 1 
           FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_TYPE='BASE TABLE' 
           AND TABLE_NAME='automobile') 
   SELECT 1 AS res ELSE SELECT 0 AS res;", null, out numberOfTables);
            int num = 0;
            num = (int)numberOfTables?.Tables[0].Rows[0].ItemArray[0];
            return num == 1;
        }

        private void CreateTable()
        {
            Data.Execute(Properties.Resources.AutomobileMsSql);
        }

        /// <summary>
        /// Check if table exists. If not, create the table.
        /// </summary>
        private void CheckOrCreateTable()
        {
            if (!TableExists())
                CreateTable();
        }

        /// <summary>
        /// Inserts object Automobile in DB.
        /// </summary>
        /// <param name="automobil"></param>
        private bool Insert(Common.Model.Vehicle.Automobile automobil)
        {
            Hashtable parameters;
            parameters = FillParams(automobil);
            return Data.Execute(
                "insert into AUTOMOBILe (brojoglasa, naslov, cena, url, vozilo, marka, model, godinaproizvodnje, karoserija, " +
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
        private bool Update(Common.Model.Vehicle.Automobile automobile)
        {
            Hashtable parameters;
            parameters = FillParams(automobile);

            StringBuilder updateCommand = new StringBuilder();
            updateCommand.AppendLine("UPDATE AUTOMOBILe");
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

        private bool SaveOnce(Common.Model.Vehicle.Automobile automobile)
        {
            bool saveSucceed = false;
            bool errorOccurred = false;

            if (automobile.BrojOglasa > 0)
            {
                if (Data.Open())
                {
                    if (Data.BeginTran())
                    {
                        try
                        {
                            if (!Exists(automobile.BrojOglasa))   // select
                            {
                                if (Insert(automobile))
                                { 
                                    Common.Dnevnik.PisiSaImenomThreda("Uspešno dodat u bazu oglas " + automobile);
                                }
                                else
                                {
                                    errorOccurred = true;
                                    Common.Dnevnik.PisiSaImenomThreda("Nije dodat u bazu oglas " + automobile);
                                }
                            }
                            else
                            {
                                if (Update(automobile))
                                { 
                                    Common.Dnevnik.PisiSaImenomThreda("Uspešno izmenjen u bazi oglas " + automobile);
                                }
                                else
                                {
                                    errorOccurred = true;
                                    Common.Dnevnik.PisiSaImenomThreda("Nije izmenjen u bazi oglas " + automobile);
                                }
                            }

                            if (!errorOccurred)
                            {
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
                            else
                                Data.RollbackTran();
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
        /// Saves automobile in DB.
        /// </summary>
        /// <param name="automobile">Automobile to save.</param>
        public bool Save(Common.Model.Vehicle.Automobile automobile)
        {
            for (int i = 0; i < Properties.Settings.Default.NumberOfSaveAttempts; i++)
                if (SaveOnce(automobile))
                    return true;
                else
                {
                    Common.Korisno.Korisno.LogError(string.Format("SaveOnce didn't succeeded. Attempt #{0}. Sleeping {1} ms.", i, Properties.Settings.Default.SleepTimeAfterFailedSave));
                    Thread.Sleep(Properties.Settings.Default.SleepTimeAfterFailedSave);
                }
            return false;
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

        private Common.Model.Vehicle.Automobile DataRowToAutomobile(System.Data.DataRow dr)
        {
            Common.Model.Vehicle.Automobile a = null;
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

                    a = new Common.Model.Vehicle.Automobile(brojOglasa, naslov, cena, url, vozilo, marka, model, godinaProizvodnje, karoserija,
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

        public List<Common.Model.Vehicle.Automobile> GetAllAsList()
        {
            System.Data.DataSet allAutomobiles;
            List<Common.Model.Vehicle.Automobile> automobileList = null;
            if (Data.GetDataSet(
                @" select BROJOGLASA, NASLOV, CENA, URL, VOZILO, MARKA, MODEL, GODINAPROIZVODNJE, KAROSERIJA, GORIVO, FIKSNACENA, ZAMENA, 
                       DATUMPOSTAVLJANJA, KUBIKAZA, SNAGA_KW, SNAGA_KS, KILOMETRAZA, EMISIONAKLASA, POGON, MENJAC, BROJVRATA,
                       BROJSEDISTA, STRANAVOLANA, KLIMA, BOJA, REGISTROVANDO, POREKLOVOZILA, OPIS, KONTAKT
                 from AUTOMOBIL A ", out allAutomobiles))
            {
                if (allAutomobiles != null && allAutomobiles.Tables.Count > 0 && allAutomobiles.Tables[0].Rows.Count > 0)
                {
                    automobileList = new List<Common.Model.Vehicle.Automobile>();
                    foreach (System.Data.DataRow autoDb in allAutomobiles.Tables[0].Rows)
                    {
                        Common.Model.Vehicle.Automobile auto = DataRowToAutomobile(autoDb);
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
                        automobileArray[row, col] = allAutomobiles.Tables[0].Rows[row - 1][col];
            }
            return automobileArray;
        }
    }
}
