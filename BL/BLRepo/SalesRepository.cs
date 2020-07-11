using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ISalesRepo _repo;
        public SalesRepository(ISalesRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteSales(string id)
        {
            var clt = await _repo.GetSalesID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteSales(clt.Id);
                if (res)
                {
                    return "Sales data Deletion successfull";
                }
                else
                {
                    return "Sales data Deletion Fails";
                }

            }
            else
            {
                return "Sales does Not exists";
            }
        }

        public async Task<IEnumerable<sales>> GetSales()
        {
            return await _repo.GetSales();
        }

        public async Task<sales> GetSalesBySalesID(string salesId, bool isTaxed)
        {
            return await _repo.GetSalesBySalesID(salesId,true);
        }

        public async Task<sales> GetSalesID(string ID)
        {
            return await _repo.GetSalesID(ID);
        }

        public async Task<string> InsertSales(sales _sales, bool isTaxed)
        {
            var adm = await _repo.GetSalesBySalesID(_sales.sid,isTaxed);
            if (adm == null)
            {
                var res = await _repo.InsertSales(_sales);
                if (res)
                {
                    return "Sales data insertion successfull";
                }
                else
                {
                    return "Sales data insertion Fails";
                }

            }
            else
            {
                return "Sales Sales name already exists";
            }
        }

        public async Task<string> UpdateSales(sales _sales, bool isTaxed)
        {
            var adm = await _repo.GetSalesBySalesID(_sales.sid,isTaxed);
            if (adm != null)
            {
                //_Sales.Id = adm.Id;
                var res = await _repo.UpdateSales(_sales);
                if (res)
                {
                    return "Sales data Updation successfull";
                }
                else
                {
                    return "Sales data Updation Fails";
                }

            }
            else
            {
                return "Sales Sales name Not exists";
            }
        }
    }
}
