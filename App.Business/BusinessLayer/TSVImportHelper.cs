using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.DataAccessLayer;

namespace App.Business.BusinessLayer
{
    public class TSVImportHelper
    {
        private readonly DirtyModelParser mParser;

        public TSVImportHelper(DirtyModelParser parser)
        {
            mParser = parser;
        }

        

        public List<Questionnaire> Import(string input)
        {
            var res = new List<Questionnaire>();

            var rows = input.Split('\n');

            foreach (var row in rows)
            {

                var cells = row.Split('\t');

                string dirtyName = cells[0];
                string dirtyDateOfBirth = cells[1];
                string dirtyEmail = cells[2];
                string dirtyPhone = cells[3];

                try
                {
                    var item = new Questionnaire()
                    {
                        Name = mParser.ParseName(dirtyName),
                        DateOfBirth = mParser.ParseDateOfBirth(dirtyDateOfBirth),
                        Email = mParser.ParseEmail(dirtyEmail),
                        Phone = mParser.ParsePhone(dirtyPhone)
                    };
                    res.Add(item);
                }
                catch
                {
                }

                
            }

            return res;
        } 
    }
}
