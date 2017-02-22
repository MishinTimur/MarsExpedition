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
    public class TSVToModelConverterTests
    {
        [TestMethod]
        public void Convert_result_is_not_null()
        {
            var helper = new TSVToModelConverter();
            var outhput = helper.Convert(Input);

            Assert.IsNotNull(outhput, "output is not null");
        }

        [TestMethod]
        public void Convert_is_correct()
        {
            var helper = new TSVToModelConverter();
            var outhput = helper.Convert(Input);

            Assert.AreEqual("Иванов Иван Иванович", outhput.Name, "first: Name");
            Assert.AreEqual(new DateTime(1980, 12, 23), outhput.DateOfBirth, "first: DateOfBirth");
            Assert.AreEqual("ivanov_ivan@test.com", outhput.Email, "first: Email");
            Assert.AreEqual("+79118546524", outhput.Phone, "first: Phone");
        }

        private string Input =>
            "Иванов Иван Иванович\t23.12.1980\tivanov_ivan@test.com\t+79118546524";
    }
}
