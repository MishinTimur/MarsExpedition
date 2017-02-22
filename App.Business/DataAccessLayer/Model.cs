using System.Data.Entity;

namespace App.Business.DataAccessLayer
{
    public class Model : DbContext
    {
        public Model()
            : base("DefaultConnection")
        {
        }

        public DbSet<Questionnaire> Questionnaires { get; set; } 
    }

}