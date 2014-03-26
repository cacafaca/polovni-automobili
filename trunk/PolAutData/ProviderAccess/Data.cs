using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolAutData
{
    /// <summary>
    /// Database access.
    /// </summary>
    public abstract class Data
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

        public bool Open()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool BeginTran()
        {
            throw new NotImplementedException();
        }

        public bool CommitTran()
        {
            throw new NotImplementedException();
        }

        public bool RollbackTran()
        {
            throw new NotImplementedException();
        }

        public bool Izvrsi(string upit, System.Collections.Hashtable parametri)
        {
            throw new NotImplementedException();
        }
    }    
}
