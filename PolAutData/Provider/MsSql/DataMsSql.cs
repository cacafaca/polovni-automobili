using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System;

namespace Procode.PolovniAutomobili.Data.Provider.MsSql
{
    public class DataMsSql : Data, IData
    {
        #region Private fields
        SqlTransaction Transaction;
        #endregion

        #region Constructors
        public DataMsSql(string connectionString)
            : base(connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }
        #endregion

        #region Interface methods
        public override bool Open()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                    return true;
                try
                {
                    Connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        public override bool Close()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool BeginTran()
        {
            if (Transaction != null)
                throw new Exception("Transaction is not null.");
            var con = (SqlConnection)Connection;
            Transaction = con.BeginTransaction(System.Threading.Thread.CurrentThread.Name);
            return true;
        }

        public override bool CommitTran()
        {            
            Transaction.Commit();
            Transaction = null;
            return true;
        }

        public override bool RollbackTran()
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
            catch
            {
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

        public override bool GetDataSet(string query, Hashtable parameters, out DataSet queryResult)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, (SqlConnection)Connection, Transaction);
                FillParams(cmd, parameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public override bool GetDataSet(string query, out DataSet queryResult)
        {
            return GetDataSet(query, null, out queryResult);
        }

        public override bool Execute(string query, Hashtable parameters)
        {
            try
            {
                SqlCommand command = new SqlCommand(query, (SqlConnection)Connection, Transaction);
                FillParams(command, parameters);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Execute(string query)
        {
            return Execute(query, null);
        }
        #endregion

        #region Private methods
        private void FillParams(SqlCommand command, Hashtable parametri)
        {
            if (parametri != null)
            {
                foreach (DictionaryEntry p in parametri)
                {
                    command.Parameters.Add(new SqlParameter(p.Key.ToString(), p.Value));
                }
            }
        }
        #endregion
    }
}
