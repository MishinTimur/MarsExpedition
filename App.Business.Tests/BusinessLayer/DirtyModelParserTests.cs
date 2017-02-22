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
    public class DirtyModelParserTests
    {
        [TestMethod]
        public void ParseName_CapitalLetters()
        {
            string input = "иванов иван иванович";
            var parser = new DirtyModelParser();

            Assert.AreEqual("Иванов Иван Иванович", parser.ParseName(input));
        }

        [TestMethod]
        public void ParseName_Spaces()
        {
            string input = " Иванов  Иван  Иванович ";
            var parser = new DirtyModelParser();

            Assert.AreEqual("Иванов Иван Иванович", parser.ParseName(input));
        }

        [TestMethod]
        public void ParseDateOfBirth_separator_dots()
        {
            string input = "24.05.16";
            var parser = new DirtyModelParser();

            Assert.AreEqual(new DateTime(2016, 5, 24), parser.ParseDateOfBirth(input));
        }

        [TestMethod]
        public void ParseDateOfBirth_separator_slash()
        {
            string input = "24/05/16";
            var parser = new DirtyModelParser();

            Assert.AreEqual(new DateTime(2016, 5, 24), parser.ParseDateOfBirth(input));
        }

        [TestMethod]
        public void ParseDateOfBirth_without_lead_zero()
        {
            string input = @"4.5.16";
            var parser = new DirtyModelParser();

            Assert.AreEqual(new DateTime(2016, 5, 4), parser.ParseDateOfBirth(input));
        }

        [TestMethod]
        public void ParseEmail_if_is_valid()
        {
            string input = "test@test.com";
            var parser = new DirtyModelParser();

            Assert.AreEqual(input, parser.ParseEmail(input));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ParseEmail_if_is_invalid()
        {
            string input = "test@test";
            var parser = new DirtyModelParser();

            parser.ParseEmail(input);
        }

        [TestMethod]
        public void ParsePhoneNumber_when_start_with_8()
        {
            string input = "89095258763";

            var parser = new DirtyModelParser();


            Assert.AreEqual("+79095258763", parser.ParsePhone(input));
        }

        [TestMethod]
        public void ParsePhoneNumber_when_start_with_7()
        {
            string input = "79095258763";

            var parser = new DirtyModelParser();

            Assert.AreEqual("+79095258763", parser.ParsePhone(input));
        }

        [TestMethod]
        public void ParsePhoneNumber_when_start_with_plus_7()
        {
            string input = "+79095258763";

            var parser = new DirtyModelParser();

            Assert.AreEqual("+79095258763", parser.ParsePhone(input));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ParsePhoneNumber_InvalidNumber()
        {
            string input = "8990909090099099090909";

            var parser = new DirtyModelParser();
            parser.ParsePhone(input);
        }
    }
}
