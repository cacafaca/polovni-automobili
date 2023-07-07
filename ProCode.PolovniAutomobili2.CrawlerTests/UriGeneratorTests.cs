using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProCode.PolovniAutomobili2.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCode.PolovniAutomobili2.Crawler.Tests
{
    [TestClass()]
    public class UriGeneratorTests
    {
        [TestMethod()]
        public void Get_First_And_Second_Page_Uri()
        {
            UriGenerator uriGenerator = new UriGenerator();
            Assert.AreEqual("https://www.polovniautomobili.com/auto-oglasi/pretraga?page=1", uriGenerator.GetNext().ToString());
            Assert.AreEqual("https://www.polovniautomobili.com/auto-oglasi/pretraga?page=2", uriGenerator.GetNext().ToString());
        }
    }
}