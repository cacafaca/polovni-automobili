using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Data.Provider.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Procode.PolovniAutomobili.Data.Provider;

namespace Procode.PolovniAutomobili.DataTests.Provider.MsSql.Tests
{
    [TestClass()]
    public class DataMsSqlTests
    {
        string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nemanja\Source\Repos\polovni-automobili\PolAutData\Databases\PolovniAutomobili.mdf;Integrated Security=True";

        [TestMethod()]
        public void OpenMsSqlTest()
        {
            Data.Provider.Data data = Data.Provider.Data.GetNewDataInstance(new Data.DbContext(ProviderType.MsSql, ConnectionString));
            data.Open();
            data.Close();
        }
    }
}