using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class StockRepository : IStocksRepository
    {
        private readonly IStocksRepo _repo;
        public StockRepository(IStocksRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteStock(string id)
        {
            var clt = await _repo.GetStockID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteStock(clt.Id);
                if (res)
                {
                    return "Stock data Deletion successfull";
                }
                else
                {
                    return "Stock data Deletion Fails";
                }

            }
            else
            {
                return "Stock does Not exists";
            }
        }

        public async Task<IEnumerable<stock>> GetStock()
        {
            return await _repo.GetStock();
        }

        public async Task<stock> GetStockByName(string name)
        {
            return await _repo.GetStockByName(name);
        }

        public async Task<stock> GetStockID(string ID)
        {
            return await _repo.GetStockID(ID);
        }

        public async Task<string> InsertStock(stock _stock)
        {
            var adm = await _repo.GetStockID(_stock.Id);
            if (adm == null)
            {
                var res = await _repo.InsertStock(_stock);
                if (res)
                {
                    return "Stock data insertion successfull";
                }
                else
                {
                    return "Stock data insertion Fails";
                }

            }
            else
            {
                return "Stock  name already exists";
            }
        }

        public async Task<string> UpdateStock(stock _stock)
        {
            var adm = await _repo.GetStockID(_stock.Id);
            if (adm != null)
            {
                //_stock.Id = adm.Id;
                if (_stock.photo == null)
                {
                    _stock.photo = adm.photo;
                }
                var res = await _repo.UpdateStock(_stock);
                if (res)
                {
                    return "Stock data Updation successfull";
                }
                else
                {
                    return "Stock data Updation Fails";
                }

            }
            else
            {
                return "Stock  name Not exists";
            }
        }
    }
}
