using Procode.PolovniAutomobili.Common.Korisno;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Procode.PolovniAutomobili.Data.Provider.MySql
{
    /// <summary>
    /// <b>Acces Firebird</b> client provider.
    /// </summary>
    public class DataMySql : Data, IData
    {
        #region Private fields
        MySqlTransaction Transaction;
        #endregion

        #region Constructors
        public DataMySql()
            : base(new MySqlConnection(Properties.Settings.Default.MySqlConnectionString))
        {
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
            if (DbConnection.State != ConnectionState.Open)
                try
                {
                    DbConnection.Open();
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
            if (DbConnection.State != ConnectionState.Closed)
                try
                {
                    DbConnection.Close();
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
                Transaction = ((MySqlConnection)DbConnection).BeginTransaction();
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
                MySqlCommand cmd = new MySqlCommand(query, (MySqlConnection)DbConnection, Transaction);
                FillParams(cmd, parameters);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
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
        override public bool GetDataSet(string query, out DataSet queryResult)
        {
            return GetDataSet(query, null, out queryResult);
        }
        public override bool Execute(string query, Hashtable parameters)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(query, (MySqlConnection)DbConnection, Transaction);
                FillParams(command, parameters);
                command.CommandType = System.Data.CommandType.Text;
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Korisno.LogError("Fail to execute SQL.", ex);
                //Common.EventLogger.WriteEventError("Fail to execute SQL.", ex); // Comment this after approving class.
                m_LastException = ex;
                return false;
            }
        }
        public override bool Execute(string query)
        {
            return Execute(query, null);
        }
        #endregion

        #region Private methods
        private void FillParams(MySqlCommand command, Hashtable parametri)
        {
            if (parametri != null)
            {
                foreach (DictionaryEntry p in parametri)
                {
                    command.Parameters.Add(new MySqlParameter(p.Key.ToString(), p.Value));
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
            MySqlTransaction tran;
            DataSet ds = null;
            if (Transaction == null)
            {
                tran = ((MySqlConnection)DbConnection).BeginTransaction();
                uTransakciji = false;
            }
            else
            {
                tran = Transaction;
                uTransakciji = true;
            }
            try
            {
                MySqlCommand Command = new MySqlCommand(upit, (MySqlConnection)DbConnection, tran);
                FillParams(Command, parametri);
                MySqlDataAdapter da = new MySqlDataAdapter(Command);
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
                ((MySqlConnection)DbConnection).Close();
            }
            catch (Exception ex)
            {
                Korisno.LogError("Nisam uspeo da zatvorim konekciju.", ex);
            }
        }
    }
}
