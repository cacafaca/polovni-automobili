using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace PolAutData.Provider.MsSql
{
    public class DataMsSql : Data, IData
    {
        #region Private fields
        SqlConnection Connection;
        SqlTransaction Transaction;        
        #endregion

        #region Constructors
        public DataMsSql()
        {
            //ConnectionString = Properties.Settings.Default.MsSqlConnectionString;
            m_DbConnection = Connection;
        }
        #endregion

        #region Interface methods
        public override bool Open()
        {
            if (Connection != null)
                Connection.Close();
            Connection = new SqlConnection("");
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
            throw new System.NotImplementedException();
        }
        public override bool CommitTran()
        {
            throw new System.NotImplementedException();
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
            queryResult = null;
            return false;
        }
        public override bool GetDataSet(string query, out DataSet queryResult)
        {
            return GetDataSet(query, null, out queryResult);
        }
        public override bool Execute(string query, Hashtable parameters)
        {
            try
            {
                SqlCommand command = new SqlCommand(query, Connection, Transaction);
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
