using App.Business.DataAccessLayer;

namespace App.Business.Infrastructure
{
    public interface ITSVToModelConverter
    {
        Questionnaire Convert(string row);
    }
}