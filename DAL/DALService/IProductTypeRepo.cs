using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IProductTypeRepo
    {
        Task<producttype> GetProductTypeID(string ID);
        Task<producttype> GetProductTypeByName(string name);
        Task<IEnumerable<producttype>> GetProductType();

        Task<bool> InsertProductType(producttype _producttype);
        Task<bool> UpdateProductType(producttype _producttype);
        Task<bool> DeleteProductType(string id);
    }
}
