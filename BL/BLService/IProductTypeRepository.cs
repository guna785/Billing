using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IProductTypeRepository
    {
        Task<producttype> GetProductTypeID(string ID);
        Task<producttype> GetProductTypeByName(string name);
        Task<IEnumerable<producttype>> GetProductType();

        Task<string> InsertProductType(producttype _producttype);
        Task<string> UpdateProductType(producttype _producttype);
        Task<string> DeleteProductType(string id);
    }
}
