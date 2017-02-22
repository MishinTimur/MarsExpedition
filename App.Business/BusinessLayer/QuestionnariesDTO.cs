using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.DataAccessLayer;

namespace App.Business.BusinessLayer
{
    public class QuestionnariesDTO
    {
        public IEnumerable<Questionnaire> Items { get; set; } = Enumerable.Empty<Questionnaire>();

        public int TotalCount { get; set; }
    }
}
