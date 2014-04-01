using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolAutData;
using System.Data;
using PolAutData.Provider.Firebird;

namespace PolAutDataTest.Provider.Firebird
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

        [TestMethod]
        public void CreateTestTable()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            string createTableScript = "create table test_table (col1 int, col2 varchar(10))";
            bool expectedResult = true;
            bool actualResult = false;

            // act
            df.Open();
            actualResult = df.Execute(createTableScript);
            df.Close();

            // assert
            Assert.AreEqual(expectedResult, actualResult, "Can't create test table.");
        }

        [TestMethod]
        public void InsertRowInTestTable()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            string insertRowScript = "insert into test_table (col1, col2) values (10, 'ten')";
            bool expectedResult = true;
            bool actualResult = false;

            // act
            df.Open();
            actualResult = df.Execute(insertRowScript);
            df.Close();

            // assert
            Assert.AreEqual(expectedResult, actualResult, "Can't insert row in test table.");
        }
        
        [TestMethod]
        public void DropTestTable()
        {
            // arrange
            DataFirebird df = new DataFirebird();
            string dropTableScript = "drop table test_table";
            bool expectedResult = true;
            bool actualResult = false;
            string exceptionMessage = string.Empty;

            // act
            try
            {
                df.Open();
                actualResult = df.Execute(dropTableScript);
                df.Close();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // assert
            string assertFailMessage = "Can't drop test table.";
            if (!exceptionMessage.Equals(string.Empty))
                assertFailMessage += " Exception message: " + exceptionMessage;
            Assert.AreEqual(expectedResult, actualResult, assertFailMessage);
        }
    }
}
