using System;
using System.Threading.Tasks;
using App.Business.BusinessLayer;
using App.Business.DataAccessLayer;

namespace App.Business.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void AddOrUpdate(Questionnaire questionnaire);
        Task<QuestionnariesDTO> GetItems(int page, int itemsPerPage);
        Task<int> SaveAsync();
    }
}