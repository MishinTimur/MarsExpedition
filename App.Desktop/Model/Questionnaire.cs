using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Model
{
    public class Questionnaire
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
