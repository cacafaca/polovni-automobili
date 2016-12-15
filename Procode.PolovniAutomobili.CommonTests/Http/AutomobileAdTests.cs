using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procode.PolovniAutomobili.Common.Http.Tests
{
    [TestClass()]
    public class AutomobileAdTests
    {
        [TestMethod()]
        public void Parse_Ad_Property_Vlasnistvo_When_There_Is_No_One()
        {
            string adAddress = "http://www.polovniautomobili.com/putnicka-vozila/9290644/mitsubishi-carisma-19-d-nemacka";
            string adContent = Http.HttpComm.GetPage(adAddress).ToString();

            Common.Model.Vehicle.Automobile automobile = Common.Http.AutomobileAd.ParseAutomobileAd(adContent, adAddress);
        }

        [TestMethod()]
        public void Parse_Ad_Property_Materijal_Enterijera_When_There_Is_No_One()
        {
            string adAddress = "http://www.polovniautomobili.com/putnicka-vozila/9250987/ford-fusion-14tdci-2003";
            string adContent = Http.HttpComm.GetPage(adAddress).ToString();

            Common.Model.Vehicle.Automobile automobile = Common.Http.AutomobileAd.ParseAutomobileAd(adContent, adAddress);
        }

        [TestMethod]
        public void Parse_when_Opis_is_missing()
        {
            string adAddress = "http://www.polovniautomobili.com/putnicka-vozila/9299208/bmw-x5-x5-40d-306ksfacelift";
            string adContent = Http.HttpComm.GetPage(adAddress).ToString();

            Common.Model.Vehicle.Automobile automobile = Common.Http.AutomobileAd.ParseAutomobileAd(adContent, adAddress);
            Assert.IsNotNull(automobile);
            Assert.IsTrue(string.IsNullOrEmpty(automobile.Opis));
        }

        [TestMethod]
        public void Parse_when_there_is_no_field_Kilometraza() // For new vehicles there is no km set. 
        {
            string adAddress = "http://www.polovniautomobili.com/putnicka-vozila/9297521/mercedes-benz-c-200-d-akcija";
            string adContent = Http.HttpComm.GetPage(adAddress).ToString();

            Common.Model.Vehicle.Automobile automobile = Common.Http.AutomobileAd.ParseAutomobileAd(adContent, adAddress);
            Assert.IsNotNull(automobile);
            Assert.AreEqual(automobile.Kilometraza, 0);
        }

        [TestMethod]
        public void Parse_when_Datum_postavljanja_is_within_few_hours() // Datum postalvjanja is always present.
        {
            string adAddress = "http://www.polovniautomobili.com/putnicka-vozila/9300182/skoda-octavia-16-tdi";
            string adContent = Http.HttpComm.GetPage(adAddress).ToString();

            Common.Model.Vehicle.Automobile automobile = Common.Http.AutomobileAd.ParseAutomobileAd(adContent, adAddress);
            Assert.IsNotNull(automobile);
            Assert.AreNotEqual(automobile.DatumPostavljanja, DateTime.MinValue);
        }
    }
}