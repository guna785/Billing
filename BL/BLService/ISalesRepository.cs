using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface ISalesRepository
    {
        Task<sales> GetSalesID(string ID);
        Task<sales> GetSalesBySalesID(string salesId, bool isTaxed);
        Task<IEnumerable<sales>> GetSales();

        Task<string> InsertSales(sales _sales, bool isTaxed);
        Task<string> UpdateSales(sales _sales, bool isTaxed);
        Task<string> DeleteSales(string id);
    }
}
