using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Dohvatanje;

namespace Procode.PolovniAutomobili.DohvatanjeTest
{
    [TestClass]
    public class GlavnaObradaTest
    {
        [TestMethod]
        public void Start_fetching_ads()
        {
            string ConnectionStringMsSql = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nemanja\Source\Repos\polovni-automobili\PolAutData\Databases\PolovniAutomobili.mdf;Integrated Security=True";
            Data.DbContext dbContext = new Data.DbContext(Data.Provider.ProviderType.MsSql, ConnectionStringMsSql);

            var obrada = new GlavnaObrada(dbContext);
            obrada.Start();

            System.Threading.Thread.Sleep(500);

            while(!obrada.CyclesFinished())
                System.Threading.Thread.Sleep(5000);
        }
    }
}
