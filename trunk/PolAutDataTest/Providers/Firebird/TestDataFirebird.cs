using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolAutData;
using System.Data;

namespace PolAutDataTest.Providers.Firebird
{
    [TestClass]
    public class TestDataFirebird
    {
        [TestMethod]
        public void OpenConnection()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            bool openResult;

            // act
            openResult = df.Open();

            // assert
            Assert.AreEqual(true, openResult, "Can't open Firebird connection.");
        }

        [TestMethod]
        public void CloseConnection()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            bool openResult, closeResult;

            // act
            openResult = df.Open();
            closeResult = df.Close();

            // assert
            Assert.AreEqual(true, openResult, "Can't open Firebird connection.");
            Assert.AreEqual(true, closeResult, "Can't open Firebird connection.");
        }
        
        [TestMethod]
        public void DatabaseVersionIs_2_5_0()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            string expectedVersion = "2.5.0";
            string actualVersion = null;

            // act
            df.Open();
            DataSet returnedDataSet = df.OpenDataSet("select RDB$GET_CONTEXT('SYSTEM', 'ENGINE_VERSION') from RDB$DATABASE;", null);
            if((returnedDataSet != null) && (returnedDataSet.Tables.Count == 1) && (returnedDataSet.Tables[0].Rows.Count == 1))
                actualVersion = returnedDataSet.Tables[0].Rows[0][0].ToString();
            df.Close();

            // assert
            Assert.AreEqual(expectedVersion, actualVersion, "Incorrect database version.");
        }
    }
}
