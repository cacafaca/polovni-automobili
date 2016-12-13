using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procode.PolovniAutomobili.Data
{
    public class DbContext
    {
        public DbContext(Procode.PolovniAutomobili.Data.Provider.ProviderType dataBaseProviderType, string connectionString)
        {
            myConnectionString = connectionString;
            myDatabaseProviderType = dataBaseProviderType;
        }

        private string myConnectionString;

        public string ConnectionString
        {
            get { return myConnectionString; }
        }

        private Provider.ProviderType myDatabaseProviderType;

        public Provider.ProviderType DatabaseProviderType
        {
            get { return myDatabaseProviderType; }
            set { myDatabaseProviderType = value; }
        }


    }
}
