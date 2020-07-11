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
    public class SalesRepo : ISalesRepo
    {
        private readonly BillingService context;
        public SalesRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteSales(string id)
        {
            FilterDefinition<sales> filter = Builders<sales>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .saless
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<sales>> GetSales()
        {
            return await context.saless.Find(x => true).ToListAsync();
        }

        public async Task<sales> GetSalesBySalesID(string salesId, bool isTaxed)
        {
            return await context.saless.Find<sales>(a => a.sid.Equals(salesId) && a.isTaxed==isTaxed).FirstOrDefaultAsync();
        }

        public async Task<sales> GetSalesID(string ID)
        {
            return await context.saless.Find<sales>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertSales(sales _sales)
        {
            try
            {
                await context.saless.InsertOneAsync(_sales);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSales(sales _sales)
        {
            try
            {
                ReplaceOneResult updateResult = await context.saless.ReplaceOneAsync(g => g.Id == _sales.Id, replacement: _sales);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
