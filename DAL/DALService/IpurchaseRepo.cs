using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IpurchaseRepo
    {
        Task<purchase> GetPurchaseID(string ID);
        Task<purchase> GetPurchaseByInvID(string invid);
        Task<IEnumerable<purchase>> GetPurchase();

        Task<bool> InsertPurchase(purchase _purchase);
        Task<bool> UpdatePurchase(purchase _purchase);
        Task<bool> DeletePurchase(string id);
    }
}
