using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IProductCompanyRepo
    {
        Task<productcompany> GetProductCompanyID(string ID);
        Task<productcompany> GetProductCompanyByName(string name);
        Task<IEnumerable<productcompany>> GetProductCompany();

        Task<bool> InsertProductCompany(productcompany _productcompany);
        Task<bool> UpdateProductCompany(productcompany _productcompany);
        Task<bool> DeleteProductCompany(string id);
    }
}
