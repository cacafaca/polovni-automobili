using System.Collections;
using System.Data;

namespace PolAutData
{
    /// <summary>
    /// Database access.
    /// </summary>
    public abstract class Data: IData
    {
        #region Private fields
        #endregion
        
        #region Protected fields
        protected static DatabaseProvider DataBaseProvider;
        #endregion

        #region
        static Data()
        {
            DataBaseProvider = (DatabaseProvider)Enum.Parse(typeof(DatabaseProvider), Properties.Settings.Default.DatabaseProvider, true);
        }
        #endregion

        public static Data GetDataInstance()
        {
            switch (DataBaseProvider)
            {
                case DatabaseProvider.Firebird:
                default:
                    return new DataFirebird();
                case DatabaseProvider.MsSql:
                    return new DataMsSql();
            }
        }

        public abstract bool Open();
        public abstract bool Close();
        public abstract bool BeginTran();
        public abstract bool CommitTran();
        public abstract bool RollbackTran();
        public abstract bool GetDataSet(string query, Hashtable parameters, DataSet queryResult);
        public abstract bool Execute(string query, Hashtable parameters);
    }    
}
