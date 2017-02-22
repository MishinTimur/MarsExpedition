using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.DataAccessLayer;
using App.Business.Infrastructure;

namespace App.Business.BusinessLayer
{
    public class TSVToModelConverter : ITSVToModelConverter
    {
        private readonly IDirtyModelParser mParser;

        public TSVToModelConverter(IDirtyModelParser parser = null)
        {
            mParser = parser ?? new DirtyModelParser();
        }

        public Questionnaire Convert(string row)
        {
            var cells = row.Split('\t');

            string dirtyName = cells[0];
            string dirtyDateOfBirth = cells[1];
            string dirtyEmail = cells[2];
            string dirtyPhone = cells[3];

            var item = new Questionnaire()
            {
                Name = mParser.ParseName(dirtyName),
                DateOfBirth = mParser.ParseDateOfBirth(dirtyDateOfBirth),
                Email = mParser.ParseEmail(dirtyEmail),
                Phone = mParser.ParsePhone(dirtyPhone)
            };

            return item;
        } 
    }
}
