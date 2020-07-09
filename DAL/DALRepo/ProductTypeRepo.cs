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
    public class ProductTypeRepo:IProductTypeRepo
    {
        private readonly BillingService context;
        public ProductTypeRepo()
        {
            context = new BillingService();
        }

        public async Task<bool> DeleteProductType(string id)
        {
            FilterDefinition<producttype> filter = Builders<producttype>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .producttypes
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<producttype>> GetProductType()
        {
            return await context.producttypes.Find(x => true).ToListAsync();
        }

        public async Task<producttype> GetProductTypeByName(string name)
        {
            return await context.producttypes.Find<producttype>(a => a.name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<producttype> GetProductTypeID(string ID)
        {
            return await context.producttypes.Find<producttype>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertProductType(producttype _producttype)
        {
            try
            {
                await context.producttypes.InsertOneAsync(_producttype);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductType(producttype _producttype)
        {
            try
            {
                ReplaceOneResult updateResult = await context.producttypes.ReplaceOneAsync(g => g.Id == _producttype.Id, replacement: _producttype);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
