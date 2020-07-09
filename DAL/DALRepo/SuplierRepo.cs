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
    public class SuplierRepo : ISuplierRepo
    {
        private readonly BillingService context;
        public SuplierRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteSuplier(string id)
        {
            FilterDefinition<suplier> filter = Builders<suplier>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .supliers
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<suplier>> GetSuplier()
        {
            return await context.supliers.Find(x => true).ToListAsync();
        }

        public async Task<suplier> GetSuplierByPhone(string phone)
        {
            return await context.supliers.Find<suplier>(a => a.cphone.Equals(phone)).FirstOrDefaultAsync();
        }

        public async Task<suplier> GetSuplierID(string ID)
        {
            return await context.supliers.Find<suplier>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertSuplier(suplier _Suplier)
        {
            try
            {
                await context.supliers.InsertOneAsync(_Suplier);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSuplier(suplier _Suplier)
        {
            try
            {
                ReplaceOneResult updateResult = await context.supliers.ReplaceOneAsync(g => g.cphone == _Suplier.cphone, replacement: _Suplier);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
