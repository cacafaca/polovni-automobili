using System;
using System.Collections;
using Common.Vehicle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolAutData.Provider;
using PolAutData.Vehicle;

namespace PolAutDataTest.Vehicle
{
    /// <summary>
    ///This is a test class for AutomobileTest and is intended
    ///to contain all AutomobileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AutomobileTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Automobile Constructor
        ///</summary>
        [TestMethod()]
        public void AutomobileConstructorTest()
        {
            try
            {
                Data data = PolAutData.Provider.DataInstance.Data; // TODO: Initialize to an appropriate value
                PolAutData.Vehicle.Automobile target = new PolAutData.Vehicle.Automobile(data);
                Assert.IsInstanceOfType(target, typeof(PolAutData.Vehicle.Automobile));
            }
            catch (Exception ex)
            {
                Assert.IsFalse(false, "target is not of type Automobile. Exception: " + ex.Message);
            }
        }

        /// <summary>
        ///A test for Exists
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PolAutData.dll")]
        public void ExistsTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Automobile_Accessor target = new Automobile_Accessor(param0); // TODO: Initialize to an appropriate value
            int adNumber = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Exists(adNumber);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FillParams
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PolAutData.dll")]
        public void FillParamsTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Automobile_Accessor target = new Automobile_Accessor(param0); // TODO: Initialize to an appropriate value
            Common.Vehicle.Automobile automobil = null; // TODO: Initialize to an appropriate value
            Hashtable expected = null; // TODO: Initialize to an appropriate value
            Hashtable actual;
            actual = target.FillParams(automobil);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Insert
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PolAutData.dll")]
        public void InsertTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Automobile_Accessor target = new Automobile_Accessor(param0); // TODO: Initialize to an appropriate value
            Common.Vehicle.Automobile automobil = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Insert(automobil);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Snimi
        ///</summary>
        [TestMethod()]
        public void SnimiTest()
        {
            Data data = null; // TODO: Initialize to an appropriate value
            PolAutData.Vehicle.Automobile target = new PolAutData.Vehicle.Automobile(data); // TODO: Initialize to an appropriate value
            Common.Vehicle.Automobile automobil = null; // TODO: Initialize to an appropriate value
            target.Save(automobil);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Snimi2
        ///</summary>
        [TestMethod()]
        public void SaveNoTransactionTest()
        {
            Data data = null; // TODO: Initialize to an appropriate value
            PolAutData.Vehicle.Automobile target = new PolAutData.Vehicle.Automobile(data); // TODO: Initialize to an appropriate value
            Common.Vehicle.Automobile automobil = null; // TODO: Initialize to an appropriate value
            target.Save(automobil, false);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("PolAutData.dll")]
        public void UpdateTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Automobile_Accessor target = new Automobile_Accessor(param0); // TODO: Initialize to an appropriate value
            Common.Vehicle.Automobile automobil = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Update(automobil);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
