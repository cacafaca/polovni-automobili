using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace PolAutData.Provider
{
    /// <summary>
    /// Database access.
    /// </summary>
    public abstract class Data : IData
    {
        #region Private fields
        static Data DataInstance;
        private System.Data.Common.DbConnection m_DbConnection;
        #endregion

        #region Protected fields
        protected static ProviderType DataBaseProvider = (ProviderType)Enum.Parse(typeof(ProviderType), Properties.Settings.Default.DatabaseProvider, true);
        protected Exception m_LastException;
        #endregion

        #region Public fields
        Exception LastException { get { return m_LastException; } }
        public DbConnection DbConnection { get { return m_DbConnection; } }
        #endregion

        #region Constructors
        public Data(DbConnection dbConnection)
        {
            m_DbConnection = dbConnection;
            DataInstance = null;
        }

        public Data()
            :this(null)
        {
        }
        #endregion

        public static Data GetDataInstance()
        {
            switch (DataBaseProvider)
            {
                case ProviderType.Firebird:
                default:
                    if (DataInstance == null)
                        DataInstance = new Provider.Firebird.DataFirebird();
                    break;
                case ProviderType.MsSql:
                    if (DataInstance == null)
                        DataInstance = new Provider.MsSql.DataMsSql();
                    break;
            }
            return DataInstance;
        }

        /// <summary>
        /// Get new data instance. Make new connection.
        /// </summary>
        /// <returns></returns>
        public static Data GetNewDataInstance()
        {
            Data dataInstance = null;
            switch (DataBaseProvider)
            {
                case ProviderType.Firebird:
                default:
                    dataInstance = new Provider.Firebird.DataFirebird();
                    break;
                case ProviderType.MsSql:
                    dataInstance = new Provider.MsSql.DataMsSql();
                    break;
                case ProviderType.MySql:
                    dataInstance = new Provider.MySql.DataMySql();
                    break;
            }
            return dataInstance;
        }

        #region Interface methods
        public abstract bool Open();
        public abstract bool Close();
        public abstract bool BeginTran();
        public abstract bool CommitTran();
        public abstract bool RollbackTran();
        public abstract bool InTransaction();
        public abstract bool GetDataSet(string query, Hashtable parameters, out DataSet queryResult);
        public abstract bool GetDataSet(string query, out DataSet queryResult);
        public abstract bool Execute(string query, Hashtable parameters);
        public abstract bool Execute(string query);
        #endregion

        public override string ToString()
        {
            if (m_DbConnection != null)
                return m_DbConnection.State.ToString();
            else
                return string.Empty;
        }
    }
}
