using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IAdminRepository
    {
        Task<admin> GetAdminID(string ID);
        Task<admin> GetAdminByPan(string uname);
        Task<IEnumerable<admin>> GetAdmin();

        Task<string> InsertAdmin(admin _admin);
        Task<string> UpdateAdmin(admin _admin);
        Task<string> DeleteAdmin(string id);
    }
}
