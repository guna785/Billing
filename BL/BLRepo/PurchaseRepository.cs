using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IpurchaseRepo _repo;
        public PurchaseRepository(IpurchaseRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeletePurchase(string id)
        {
            var clt = await _repo.GetPurchaseID(id);
            if (clt != null)
            {
                var res = await _repo.DeletePurchase(clt.Id);
                if (res)
                {
                    return "Purchase data Deletion successfull";
                }
                else
                {
                    return "Purchase data Deletion Fails";
                }

            }
            else
            {
                return "Purchase does Not exists";
            }
        }

        public async Task<IEnumerable<purchase>> GetPurchase()
        {
            return await _repo.GetPurchase();
        }

        public async Task<purchase> GetPurchaseByInvID(string invid)
        {
            return await _repo.GetPurchaseByInvID(invid);
        }

        public async Task<purchase> GetPurchaseID(string ID)
        {
            return await _repo.GetPurchaseID(ID);
        }

        public async Task<string> InsertPurchase(purchase _purchase)
        {
            var adm = await _repo.GetPurchaseByInvID(_purchase.invid);
            if (adm == null)
            {
                var res = await _repo.InsertPurchase(_purchase);
                if (res)
                {
                    return "Purchase data insertion successfull";
                }
                else
                {
                    return "Purchase data insertion Fails";
                }

            }
            else
            {
                return "Purchase Purchase name already exists";
            }
        }

        public async Task<string> UpdatePurchase(purchase _purchase)
        {
            var adm = await _repo.GetPurchaseID(_purchase.Id);
            if (adm != null)
            {
                //_Purchase.Id = adm.Id;
                var res = await _repo.UpdatePurchase(_purchase);
                if (res)
                {
                    return "Purchase data Updation successfull";
                }
                else
                {
                    return "Purchase data Updation Fails";
                }

            }
            else
            {
                return "Purchase Purchase name Not exists";
            }
        }
    }
}
