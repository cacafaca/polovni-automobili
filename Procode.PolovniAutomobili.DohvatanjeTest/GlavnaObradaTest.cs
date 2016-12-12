using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Procode.PolovniAutomobili.Dohvatanje;

namespace Procode.PlovniAutomobili.DohvatanjeTest
{
    [TestClass]
    public class GlavnaObradaTest
    {
        [TestMethod]
        public void Start_fetching_ads()
        {
            //pozovi thread
            var obrada = new GlavnaObrada();
            obrada.Pokreni();
        }
    }
}
