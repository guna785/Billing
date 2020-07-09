using DAL.DALService;
using DAL.DbService;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALRepo
{
    public class CatagoryRepo : ICatagoryRepo
    {
        private readonly BillingService context;
        public CatagoryRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteCatagory(string id)
        {
            FilterDefinition<category> filter = Builders<category>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .categorys
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<category>> GetCatagory()
        {
            return await context.categorys.Find(x => true).ToListAsync();
        }

        public async Task<category> GetCatagoryByName(string name)
        {
            return await context.categorys.Find<category>(a => a.name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<category> GetCatagoryID(string ID)
        {
            return await context.categorys.Find<category>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertCatagory(category _category)
        {
            try
            {
                await context.categorys.InsertOneAsync(_category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCatagory(category _category)
        {
            try
            {
                ReplaceOneResult updateResult = await context.categorys.ReplaceOneAsync(g => g.Id == _category.Id, replacement: _category);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
