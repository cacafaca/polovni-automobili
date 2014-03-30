using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace PolAutData
{
    public class DataMsSql : Data, IData
    {
        #region Private fields
        SqlConnection Connection;
        SqlTransaction Transaction;
        SqlCommand Command;
        #endregion

        public DataMsSql()
        {
            //ConnectionString = Properties.Settings.Default.MsSqlConnectionString;
        }

        new public bool Open()
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
        new public bool Close()
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
        new public bool BeginTran()
        {
            throw new System.NotImplementedException();
        }
        new public bool CommitTran()
        {
            throw new System.NotImplementedException();
        }
        new public bool RollbackTran()
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
        new public DataSet GetDataSet(string query, Hashtable parameters)
        {
            return null;
        }
        new public bool Execute(string query, Hashtable parameters)
        {
            throw new System.NotImplementedException();
        }
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
    }
}
