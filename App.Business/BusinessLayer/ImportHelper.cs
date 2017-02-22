using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.DataAccessLayer;
using App.Business.Infrastructure;

namespace App.Business.BusinessLayer
{
    public class ImportHelper
    {
        private readonly ITSVToModelConverter mConverter;
        private readonly IUnitOfWorkFactory mUnitOfWorkFactory;

        public ImportHelper(ITSVToModelConverter converter = null, IUnitOfWorkFactory unitOfWorkFactory = null)
        {
            mConverter = converter ?? new TSVToModelConverter();
            mUnitOfWorkFactory = unitOfWorkFactory ?? new UnitOfWorkFactory();
        }

        public async Task Import(Stream fileStream)
        {
            using (var uof = mUnitOfWorkFactory.CreateUnitOfWork())
            using (var sr = new StreamReader(fileStream))
            {
                string row;
                while ((row = sr.ReadLine()) != null)
                {
                    try
                    {
                        var item = mConverter.Convert(row);
                        uof.AddOrUpdate(item);
                    }
                    catch { }
                }
                await uof.SaveAsync();
            }
        }
    }
}
