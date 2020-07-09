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
    public class ProductCompanyRepo : IProductCompanyRepo
    {
        private readonly BillingService context;
        public ProductCompanyRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteProductCompany(string id)
        {
            FilterDefinition<productcompany> filter = Builders<productcompany>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .productcompanys
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<productcompany>> GetProductCompany()
        {
            return await context.productcompanys.Find(x => true).ToListAsync();
        }

        public async Task<productcompany> GetProductCompanyByName(string name)
        {
            return await context.productcompanys.Find<productcompany>(a => a.name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<productcompany> GetProductCompanyID(string ID)
        {
            return await context.productcompanys.Find<productcompany>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertProductCompany(productcompany _productcompany)
        {
            try
            {
                await context.productcompanys.InsertOneAsync(_productcompany);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductCompany(productcompany _productcompany)
        {
            try
            {
                ReplaceOneResult updateResult = await context.productcompanys.ReplaceOneAsync(g => g.Id == _productcompany.Id, replacement: _productcompany);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
