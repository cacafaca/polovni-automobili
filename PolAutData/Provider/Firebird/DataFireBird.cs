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
            if (Connection.State != ConnectionState.Open)
                try
                {
                    Connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    Korisno.LogError("Can't open connection.", ex);
                    return false;
                }
            else
                return true;
        }
        override public bool Close()
        {
            if (Connection.State != ConnectionState.Closed)
                try
                {
                    Connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Korisno.LogError("Can't close connection.", ex);
                    return false;
                }
            else
                return true;
        }
        public override bool BeginTran()
        {
            try
            {
                Transaction = Connection.BeginTransaction(System.Threading.Thread.CurrentThread.Name);
                return true;
            }
            catch (Exception ex)
            {
                Korisno.LogError("Can't begin transaction.", ex);
                return false;
            }
        }
        public override bool CommitTran()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                    Transaction.Dispose();
                    Transaction = null;
                    return true;
                }
                catch (Exception ex)
                {
                    Korisno.LogError("Can't commit transaction.", ex);
                    return false;
                }
            }
            else
                return false;
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
                Korisno.LogError("Nisam uspeo da rolbekujem transakciju.", ex);
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
        override public bool GetDataSet(string query, Hashtable parameters, out DataSet queryResult)
        {
            try
            {
                FbCommand cmd = new FbCommand(query, Connection, Transaction);
                FillParams(cmd, parameters);
                FbDataAdapter da = new FbDataAdapter(cmd);
                queryResult = new DataSet();
                da.Fill(queryResult);
                return (queryResult != null) && (queryResult.Tables.Count > 0) && (queryResult.Tables[0].Rows.Count > 0);
            }
            catch (Exception ex)
            {
                queryResult = null;
                Common.EventLogger.WriteEventError("Can't get dataset.", ex);
                return false;
            }
        }
        public override bool Execute(string query, Hashtable parameters)
        {
            try
            {
                FbCommand Command = new FbCommand(query, Connection, Transaction);
                FillParams(Command, parameters);
                Command.CommandType = System.Data.CommandType.Text;
                return Command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Common.EventLogger.WriteEventError("Fail to execute SQL.", ex);
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
                Korisno.LogError("Nisam uspeo da zatvorim konekciju.", ex);
            }
        }
    }
}
