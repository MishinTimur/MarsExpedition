using System.Collections.Generic;
using System.Linq;
using App.Business.DataAccessLayer;

namespace App.Business.Infrastructure
{
    public class QuestionnariesDTO
    {
        public IEnumerable<Questionnaire> Items { get; set; } = Enumerable.Empty<Questionnaire>();

        public int TotalCount { get; set; }
    }
}
