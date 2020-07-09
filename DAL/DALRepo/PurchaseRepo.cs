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
    public class PurchaseRepo : IpurchaseRepo
    {
        private readonly BillingService context;
        public PurchaseRepo()
        {
            context = new BillingService();
        }

        public async Task<bool> DeletePurchase(string id)
        {
            FilterDefinition<purchase> filter = Builders<purchase>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .purchases
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<purchase>> GetPurchase()
        {
            return await context.purchases.Find(x => true).ToListAsync();
        }

        public async Task<purchase> GetPurchaseByInvID(string invid)
        {
            return await context.purchases.Find<purchase>(a => a.invid.Equals(invid)).FirstOrDefaultAsync();
        }

        public async Task<purchase> GetPurchaseID(string ID)
        {
            return await context.purchases.Find<purchase>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }


        public async Task<bool> InsertPurchase(purchase _purchase)
        {
            try
            {
                await context.purchases.InsertOneAsync(_purchase);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePurchase(purchase _purchase)
        {
            try
            {
                ReplaceOneResult updateResult = await context.purchases.ReplaceOneAsync(g => g.Id == _purchase.Id, replacement: _purchase);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
