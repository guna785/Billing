using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IProductCompanyRepository
    {
        Task<productcompany> GetProductCompanyID(string ID);
        Task<productcompany> GetProductCompanyByName(string name);
        Task<IEnumerable<productcompany>> GetProductCompany();

        Task<string> InsertProductCompany(productcompany _productcompany);
        Task<string> UpdateProductCompany(productcompany _productcompany);
        Task<string> DeleteProductCompany(string id);
    }
}
