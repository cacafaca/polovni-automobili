using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Data;
using Common.Korisno;

namespace PolAutData
{
    //public  Connection
    public class Data : IDisposable
    {
        FbTransaction Transaction;
        FbConnection Connection;
        readonly string PodrParamZnak = "@";
        public Exception LastException { get { return izuzeci.Count > 0 ? izuzeci[izuzeci.Count - 1] : null; } }
        private List<Exception> izuzeci = new List<Exception>();
        public Data()
        {
            Connection = new FbConnection(Properties.Settings.Default.PolovniAutomobiliConnectionString);
            OtvoriPonovoKonekciju();
        }
        public void OtvoriPonovoKonekciju()
        {
            Transaction = null;
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                Korisno.LogujGresku("Nisam uspeo da zatvorim konekciju.", ex);
            }
            try
            {
                Connection.Open();
            }
            catch (Exception ex)
            {
                Korisno.LogujGresku("Nisam uspeo da otvorim konekciju.", ex);
                Connection = new FbConnection(Properties.Settings.Default.PolovniAutomobiliConnectionString);
            }
        }
        public bool BeginTran()
        {
            izuzeci.Clear();
            if (Transaction == null)
                try
                {
                    Transaction = Connection.BeginTransaction(System.Threading.Thread.CurrentThread.Name);
                    return true;
                }
                catch (Exception ex)
                {
                    izuzeci.Add(ex);
                    Korisno.LogujGresku("Nisam uspeo da otvorim transakciju.", ex);
                    return false;
                }
            else
                return false;
        }

        public bool CommitTran()
        {
            try
            {
                if (Transaction != null)
                {
                    Transaction.Commit();
                    Transaction.Dispose();
                    Transaction = null;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                izuzeci.Add(ex);
                Korisno.LogujGresku("Nisam uspeo da komitujem transakciju.", ex);
                return false;
            }
        }

        public bool RollbackTran()
        {
            try
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                    Transaction = null;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                izuzeci.Add(ex);
                Korisno.LogujGresku("Nisam uspeo da rolbekujem transakciju.", ex);
                return false;
            }
        }

        public void IzvrsiUTransakciji(string upit)
        {
            FbCommand Command = new FbCommand(upit, Connection, Transaction);
            Command.CommandType = System.Data.CommandType.Text;
            Command.ExecuteNonQuery();
        }

        public void IzvrsiUTransakciji(string upit, Hashtable parametri)
        {
            FbCommand Command = new FbCommand(upit, Connection, Transaction);
            NapuniParametre(Command, parametri);
            Command.CommandType = System.Data.CommandType.Text;
            Command.ExecuteNonQuery();
        }

        private void NapuniParametre(FbCommand command, Hashtable parametri)
        {
            if (parametri != null)
            {
                foreach (DictionaryEntry p in parametri)
                {
                    command.Parameters.Add(new FbParameter(p.Key.ToString(), p.Value));
                }
            }
        }

        private string ParamZnak()
        {
            return PodrParamZnak;
        }

        /// <summary>
        /// Glavna metoda za INSERT, UPDATE, DELETE komande.
        /// </summary>
        /// <param name="upit"></param>
        /// <param name="parametri"></param>
        /// <returns></returns>
        public bool Izvrsi(string upit, Hashtable parametri)
        {
            bool uTransakciji;
            FbTransaction tran;
            if (Transaction == null)    // ako nije u transakciji napravi novu transakciju
            {
                izuzeci.Clear();
                tran = Connection.BeginTransaction();
                uTransakciji = false;
            }
            else
            {
                tran = Transaction;
                uTransakciji = true;
            }
            try
            {
                using (FbCommand Command = new FbCommand(upit, Connection, tran))
                {
                    if (parametri != null)
                    {
                        NapuniParametre(Command, parametri);
                    }
                    Command.ExecuteNonQuery();
                }
                if (!uTransakciji)
                {
                    tran.Commit();
                    tran.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                izuzeci.Add(ex);
                Korisno.LogujGresku("Nije izvršen upit:\n" + upit, ex);
                if (!uTransakciji)// ako je u lokalnoj transakciji pokusaj da rollbackujes iz 3 puta
                {
                    for (int pokusaj = 0; pokusaj < 3; pokusaj++)
                    {
                        try
                        {
                            tran.Rollback();
                            tran.Dispose();
                        }
                        catch (Exception ex2)
                        {                            
                            izuzeci.Add(ex2);
                            Korisno.LogujGresku("Nije uspeo rollback iz pokusaja " + (pokusaj + 1).ToString(), ex2);
                        }
                        System.Threading.Thread.Sleep(1000);    // cekaj jednu sekundu pa pokusaj ponovo
                    }
                }
                return false;
            }
        }

        public bool ZatvoriKonekciju()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch(Exception ex)
            {
                izuzeci.Add(ex);
                Korisno.LogujGresku("Ne mogu da zatvorim konekciju.", ex);
                return false;
            }
        }

        public DataSet Otvori(string upit, Hashtable parametri)
        {
            bool uTransakciji;
            FbTransaction tran;
            DataSet ds = null;
            if (Transaction == null)
            {
                izuzeci.Clear();
                tran = Connection.BeginTransaction();
                uTransakciji = false;
            }
            else
            {
                tran = Transaction;
                uTransakciji = true;
            }
            try
            {
                FbCommand Command = new FbCommand(upit, Connection, tran);
                if (parametri != null)
                {
                    NapuniParametre(Command, parametri);
                }
                FbDataAdapter da = new FbDataAdapter(Command);
                ds = new DataSet();
                da.Fill(ds);
                if (!uTransakciji)
                {
                    tran.Commit();
                    tran.Dispose();
                }
            }
            catch (Exception ex)
            {
                izuzeci.Add(ex);
                if (!uTransakciji)
                {
                    tran.Rollback();
                    tran.Dispose();
                }
                else
                {
                    throw ex;
                }
            }
            return ds;
        }
        public bool TranasakcijaOtvorena()
        {
            return Transaction != null;
        }
        /// <summary>
        /// Dispose ce da zatvori konekciju.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                izuzeci.Add(ex);
                Korisno.LogujGresku("Nisam uspeo da zatvorim konekciju.", ex);
            }            
        }
        public string StackTrace()
        {
            if (izuzeci.Count > 0)
            {
                string st = string.Empty;
                for (int i = 0; i < izuzeci.Count; i++)
                {
                    Exception e = izuzeci[i];
                    st += string.Format("Exception{0}: {1}\nStackTrace{0}: {2}\n", i, e.Message, e.StackTrace);
                }
                return st;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
