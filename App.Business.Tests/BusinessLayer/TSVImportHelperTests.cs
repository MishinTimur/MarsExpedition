using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Business.Tests.BusinessLayer
{
    [TestClass]
    public class TSVImportHelperTests
    {
        [TestMethod]
        public void Import_result_is_not_null()
        {
            var helper = new TSVImportHelper(new DirtyModelParser());
            var outhput = helper.Import(Input);

            Assert.IsNotNull(outhput, "output is not null");
        }

        [TestMethod]
        public void Import_correct_count()
        {
            var helper = new TSVImportHelper(new DirtyModelParser());
            var outhput = helper.Import(Input);

            Assert.AreEqual(2, outhput.Count, "two items");
        }

        [TestMethod]
        public void Import_firstItem_IsCorrect()
        {
            var helper = new TSVImportHelper(new DirtyModelParser());
            var outhput = helper.Import(Input);

            var firstItem = outhput.First();
            
            Assert.AreEqual("Иванов Иван Иванович", firstItem.Name, "first: Name");
            Assert.AreEqual(new DateTime(1980, 12, 23), firstItem.DateOfBirth, "first: DateOfBirth");
            Assert.AreEqual("ivanov_ivan@test.com", firstItem.Email, "first: Email");
            Assert.AreEqual("+79118546524", firstItem.Phone, "first: Phone");
        }

        [TestMethod]
        public void Import_lastItem_IsCorrect()
        {
            var helper = new TSVImportHelper(new DirtyModelParser());
            var outhput = helper.Import(Input);

            var lastItem = outhput.Last();

            Assert.AreEqual("Петров Петр Петрович", lastItem.Name, "last: Name");
            Assert.AreEqual(new DateTime(1955, 07, 04), lastItem.DateOfBirth, "last: DateOfBirth");
            Assert.AreEqual("petrov_petr@test.com", lastItem.Email, "last: Email");
            Assert.AreEqual("+79095258763", lastItem.Phone, "last: Phone");

        }

        private string Input =>
            "Иванов Иван Иванович\t23.12.1980\tivanov_ivan@test.com\t+79118546524\n" +
            "Петров Петр Петрович\t4.7.1955\tpetrov_petr@test.com\t89095258763";
    }
}
