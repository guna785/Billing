using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface ISalesRepo
    {
        Task<sales> GetSalesID(string ID);
        Task<sales> GetSalesBySalesID(string salesId, bool isTaxed);
        Task<IEnumerable<sales>> GetSales();

        Task<bool> InsertSales(sales _sales);
        Task<bool> UpdateSales(sales _sales);
        Task<bool> DeleteSales(string id);
    }
}
