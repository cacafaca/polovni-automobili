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
    /// <summary>
    /// <b>Acces Firebird</b> client provider.
    /// </summary>
    public class DataFirebird : Data, IData
    {
        #region Private fields
        FbTransaction Transaction;
        FbConnection Connection;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor sets:
        ///     <list type="bullet">
        ///         <listheader>
        ///             <term>term</term>
        ///             <description>description</description>
        ///         </listheader>
        ///         <item>default connection string</item>
        ///         <item>parameter prefix (eg. "@")</item>       
        ///     </list>
        /// </summary>
        public DataFirebird()
        {
            Connection = new FbConnection(Properties.Settings.Default.FBConnectionString);
            ParameterPrefix = Properties.Settings.Default.PodrazumevaniPrefixParametra;
        }
        #endregion

        #region Database access
        /// <summary>
        /// Otvara konekciju ka Firebird bazi.
        /// </summary>
        /// <returns>Vraca true ako je otvaranje uspesno.</returns>
        public bool Open()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch { return false; }
        }

        public bool Close()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch { return false; }
        }
        #endregion

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
                Korisno.LogujGresku("Nisam uspeo da rolbekujem transakciju.", ex);
                return false;
            }
        }

        public bool BeginTran()
        {
            if (Transaction == null)
                try
                {
                    Transaction = Connection.BeginTransaction(System.Threading.Thread.CurrentThread.Name);
                    return true;
                }
                catch (Exception ex)
                {
                    Korisno.LogujGresku("Nisam uspeo da otvorim transakciju.", ex);
                    return false;
                }
            else
                return false;
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

        public string ParameterPrefix
        {
            get;
            set;
        }

        /// <summary>
        /// Glavna metoda za INSERT, UPDATE, DELETE komande.
        /// </summary>
        /// <param name="upit"></param>
        /// <param name="parametri"></param>
        /// <returns></returns>
        public bool Izvrsi(string upit, Hashtable parametri)
        {
            int affectedRows = 0;
            bool uTransakciji;
            FbTransaction tran;
            if (Transaction == null)    // ako nije u transakciji napravi novu transakciju
            {
                tran = Connection.BeginTransaction();
                uTransakciji = false;
            }
            else
            {
                tran = Transaction;
                uTransakciji = true;
            }
            using (FbCommand Command = new FbCommand(upit, Connection, tran))
            {
                NapuniParametre(Command, parametri);
                try
                {
                    affectedRows = Command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Common.Dnevnik.PisiSaThredomGreska("Upit nije izvršen:\r\n" + upit, ex);
                }
            }
            if (!uTransakciji)
            {
                tran.Commit();
                tran.Dispose();
            }
            return affectedRows > 0;
        }

        public DataSet OpenDataSet(string upit, Hashtable parametri)
        {
            bool uTransakciji;
            FbTransaction tran;
            DataSet ds = null;
            if (Transaction == null)
            {
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
                NapuniParametre(Command, parametri);
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

        /// <summary>
        /// Proverava da li je transakcija otvorena.
        /// </summary>
        /// <returns>true ako je otvorena transakcija.</returns>
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
                Korisno.LogujGresku("Nisam uspeo da zatvorim konekciju.", ex);
            }
        }
    }
}
