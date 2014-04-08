using System;
using System.Collections;
using System.Data;

namespace PolAutData.Provider
{
    /// <summary>
    /// Database access.
    /// </summary>
    public abstract class Data : IData
    {
        #region Private fields
        static Data DataInstance;
        #endregion

        #region Protected fields
        protected static ProviderType DataBaseProvider;
        #endregion

        #region Constructors
        public Data()
        {
            DataInstance = null;
            DataBaseProvider = (ProviderType)Enum.Parse(typeof(ProviderType), Properties.Settings.Default.DatabaseProvider, true);
        }
        #endregion

        public static Data GetDataInstance()
        {
            switch (DataBaseProvider)
            {
                case ProviderType.Firebird:
                default:
                    if(DataInstance == null)
                        DataInstance = new Provider.Firebird.DataFirebird();
                    break;
                case ProviderType.MsSql:
                    if(DataInstance == null)
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
        public abstract bool Execute(string query, Hashtable parameters);
        public abstract bool Execute(string query);
        #endregion
    }
}
