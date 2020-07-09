using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IStocksRepo
    {
        Task<stock> GetStockID(string ID);
        Task<stock> GetStockByName(string name);
        Task<IEnumerable<stock>> GetStock();

        Task<bool> InsertStock(stock _stock);
        Task<bool> UpdateStock(stock _stock);
        Task<bool> DeleteStock(string id);
    }
}
