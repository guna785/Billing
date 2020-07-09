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
    public class StocksRepo : IStocksRepo
    {
        private readonly BillingService context;
        public StocksRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteStock(string id)
        {
            FilterDefinition<stock> filter = Builders<stock>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .stocks
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<stock>> GetStock()
        {
            return await context.stocks.Find(x => true).ToListAsync();
        }

        public async Task<stock> GetStockByName(string name)
        {
            return await context.stocks.Find<stock>(a => a.name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<stock> GetStockID(string ID)
        {
            return await context.stocks.Find<stock>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertStock(stock _stock)
        {
            try
            {
                await context.stocks.InsertOneAsync(_stock);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStock(stock _stock)
        {
            try
            {
                ReplaceOneResult updateResult = await context.stocks.ReplaceOneAsync(g => g.Id == _stock.Id, replacement: _stock);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
