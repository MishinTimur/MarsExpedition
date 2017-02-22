using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.DataAccessLayer;
using App.Business.Infrastructure;

namespace App.Business.BusinessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Model mContext = new Model();

        public void AddOrUpdate(Questionnaire questionnaire)
        {
            if (questionnaire.ID == Guid.Empty)
            {
                questionnaire.ID = Guid.NewGuid();
                mContext.Questionnaires.Add(questionnaire);
            }
            else
                mContext.Entry(questionnaire).State = EntityState.Modified;
        }

        public async Task<QuestionnariesDTO> GetItems(int page, int itemsPerPage)
        {
            int count = await mContext.Questionnaires.CountAsync();
            var items = await mContext.Questionnaires.Skip(page*itemsPerPage).Take(itemsPerPage).ToListAsync();

            return new QuestionnariesDTO()
            {
                Items = items,
                TotalCount = count
            };
        } 

        public async Task<int> SaveAsync()
        {
            return await mContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            mContext.Dispose();
        }
    }
}
