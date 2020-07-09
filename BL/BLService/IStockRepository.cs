using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IStocksRepository
    {
        Task<stock> GetStockID(string ID);
        Task<stock> GetStockByName(string name);
        Task<IEnumerable<stock>> GetStock();

        Task<string> InsertStock(stock _stock);
        Task<string> UpdateStock(stock _stock);
        Task<string> DeleteStock(string id);
    }
}
