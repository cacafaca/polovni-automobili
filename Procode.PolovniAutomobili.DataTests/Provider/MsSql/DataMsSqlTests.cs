using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Data.Provider.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procode.PolovniAutomobili.Data.Provider.MsSql.Tests
{
    [TestClass()]
    public class DataMsSqlTests
    {
        string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nemanja\Source\Repos\polovni-automobili\PolAutData\Databases\PolovniAutomobili.mdf;Integrated Security=True";

        [TestMethod()]
        public void OpenMsSqlTest()
        {
            Data data = Data.GetNewDataInstance(ProviderType.MsSql, ConnectionString);
            data.Open();
            data.Close();
        }
    }
}