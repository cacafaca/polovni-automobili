using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Data;
using Common.Korisno;

namespace PolAutData.Provider.Firebird
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

        #region Interface methods
        /// <summary>
        /// Otvara konekciju ka Firebird bazi.
        /// </summary>
        /// <returns>Vraca true ako je otvaranje uspesno.</returns>
        override public bool Open()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch { return false; }
        }
        override public bool Close()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch { return false; }
        }
        public override bool BeginTran()
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
        public override bool CommitTran()
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
        override public bool RollbackTran()
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
        /// <summary>
        /// Check if transaction is open.
        /// </summary>
        /// <returns>Returns true if it is in transaction. Otherwise false.</returns>
        public override bool InTransaction()
        {
            return Transaction != null;
        }
        override public bool GetDataSet(string query, Hashtable parameters, DataSet queryResult)
        { 
            return false;
        }
        public override bool Execute(string query, Hashtable parameters)
        {
            try
            {
                FbCommand Command = new FbCommand(query, Connection, Transaction);
                FillParams(Command, parameters);
                Command.CommandType = System.Data.CommandType.Text;
                Command.ExecuteNonQuery();
                return true;
            }
            catch 
            {
                // todo logovanje
                return false;
            }
        }
        public override bool Execute(string query)
        {
            return Execute(query, null);
        }
        #endregion

        #region Private methods
        private void FillParams(FbCommand command, Hashtable parametri)
        {
            if (parametri != null)
            {
                foreach (DictionaryEntry p in parametri)
                {
                    command.Parameters.Add(new FbParameter(p.Key.ToString(), p.Value));
                }
            }
        }
        #endregion

        public string ParameterPrefix
        {
            get;
            set;
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
                FillParams(Command, parametri);
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
