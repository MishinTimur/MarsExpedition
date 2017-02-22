using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.BusinessLayer;
using App.Business.DataAccessLayer;
using App.Business.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Business.Tests.BusinessLayer
{
    [TestClass]
    public class ImportHelperTests
    {
        [TestMethod]
        public void Import_correct_count()
        {
            var res = GetResultFromImport();
            Assert.AreEqual(2, res.TotalCount);
        }

        [TestMethod]
        public void Convert_firstItem_IsCorrect()
        {
            var res = GetResultFromImport();

            var firstItem = res.Items.First();

            Assert.AreEqual("Иванов Иван Иванович", firstItem.Name, "first: Name");
            Assert.AreEqual(new DateTime(1980, 12, 23), firstItem.DateOfBirth, "first: DateOfBirth");
            Assert.AreEqual("ivanov_ivan@test.com", firstItem.Email, "first: Email");
            Assert.AreEqual("+79118546524", firstItem.Phone, "first: Phone");
        }

        [TestMethod]
        public void Convert_lastItem_IsCorrect()
        {
            var res = GetResultFromImport();

            var lastItem = res.Items.Last();

            Assert.AreEqual("Петров Петр Петрович", lastItem.Name, "last: Name");
            Assert.AreEqual(new DateTime(1955, 07, 04), lastItem.DateOfBirth, "last: DateOfBirth");
            Assert.AreEqual("petrov_petr@test.com", lastItem.Email, "last: Email");
            Assert.AreEqual("+79095258763", lastItem.Phone, "last: Phone");

        }


        public QuestionnariesDTO GetResultFromImport()
        {
            var factory = new UnitOfWorkFactoryMock();
            var importHelper = new ImportHelper(unitOfWorkFactory: factory);
            var stream = GetTsvStream();
            importHelper.Import(stream).Wait();
            return factory.CreateUnitOfWork().GetItems(1, 10).Result;

        }

        private Stream GetTsvStream()
        {
            string s = 
            "Иванов Иван Иванович\t23.12.1980\tivanov_ivan@test.com\t+79118546524\n" +
            "Петров Петр Петрович\t4.7.1955\tpetrov_petr@test.com\t89095258763";

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        #region Mocks

        private class UnitOfWorkFactoryMock : IUnitOfWorkFactory
        {
            private IUnitOfWork mUnitOfWork;

            public IUnitOfWork CreateUnitOfWork()
            {
                return mUnitOfWork ?? (mUnitOfWork = new UnitOfWorkMock());
            }
        }

        private class UnitOfWorkMock : IUnitOfWork
        {
            private readonly List<Questionnaire> mItems = new List<Questionnaire>();

            public void AddOrUpdate(Questionnaire questionnaire)
            {
                mItems.Add(questionnaire);
            }

            public Task<int> SaveAsync()
            {
                return Task.FromResult(mItems.Count);
            }

            public Task<QuestionnariesDTO> GetItems(int page, int itemsPerPage)
            {
                return Task.FromResult(new QuestionnariesDTO()
                {
                    Items = mItems.ToList(),
                    TotalCount = mItems.Count
                });
            }

            public void Dispose()
            {
            }
        }

        #endregion

    }
}
