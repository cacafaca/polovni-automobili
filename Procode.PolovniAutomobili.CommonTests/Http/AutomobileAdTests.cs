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
    }
}