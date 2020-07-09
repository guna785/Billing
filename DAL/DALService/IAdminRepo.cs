using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IAdminRepo
    {
        Task<admin> GetAdminID(string ID);
        Task<admin> GetAdminByPan(string uname);
        Task<IEnumerable<admin>> GetAdmin();

        Task<bool> InsertAdmin(admin _admin);
        Task<bool> UpdateAdmin(admin _admin);
        Task<bool> DeleteAdmin(string id);
    }
}
