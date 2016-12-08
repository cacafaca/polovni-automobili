using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Data.Vehicle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procode.PolovniAutomobili.Data.Vehicle.Tests
{
    [TestClass()]
    public class AutomobileTests
    {
        string ConnectionStringMsSql = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nemanja\Source\Repos\polovni-automobili\PolAutData\Databases\PolovniAutomobili.mdf;Integrated Security=True";

        [TestMethod()]
        public void SaveMsSqlTest()
        {
            Provider.Data data = Provider.Data.GetNewDataInstance(Provider.ProviderType.MsSql, ConnectionStringMsSql);

            DataSet max;
            data.GetDataSet("select max(brojoglasa) from automobile", out max);
            int brOglMax;
            if (max != null && max.Tables.Count == 1 && max.Tables[0].Rows.Count == 1 && max.Tables[0].Rows[0][0] != null)
                brOglMax = (int)((long)max.Tables[0].Rows[0][0]) + 1;
            else
                brOglMax = 1;

            Automobile auto = new Automobile(data);
            auto.Save(new Procode.PolovniAutomobili.Common.Model.Vehicle.Automobile(
                brojOglasa: brOglMax,
                naslov: "Test used car ad",
                cena: 1000,
                url: "http://google.com",
                vozilo: "vozilo",
                marka: "Mazda",
                model: "6",
                godinaProizvodnje: 2006,
                karoserija: "karoserija",
                gorivo: "Dizel",
                fiksnaCena: false,
                zamena: false,
                datumPostavljanja: DateTime.Now,
                kubikaza: 1998,
                snagaKW: 89,
                snagaKS: 120,
                kilometraza: 250000,
                emisionaKlasa: "Euro4",
                pogon: "prednji",
                menjac: "manuelni",
                brojVrata: "4/5",
                brojSedista: 5,
                stranaVolana: "leva",
                klima: "ima",
                boja: "siva",
                registrovanDo: DateTime.Now.AddMonths(6),
                porekloVozila: "domace",
                opis: "Test opis.",
                kontakt: "Test kontakt."
                ));
        }
    }
}