using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IPurchaseRepository
    {
        Task<purchase> GetPurchaseID(string ID);
        Task<purchase> GetPurchaseByInvID(string invid);
        Task<IEnumerable<purchase>> GetPurchase();

        Task<string> InsertPurchase(purchase _purchase);
        Task<string> UpdatePurchase(purchase _purchase);
        Task<string> DeletePurchase(string id);
    }
}
