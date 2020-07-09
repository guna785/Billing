using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IRoleRepository
    {
        Task<role> GetRoleID(string ID);
        Task<role> GetRoleByName(string name);
        Task<IEnumerable<role>> GetRole();

        Task<string> InsertRole(role _role);
        Task<string> UpdateRole(role _role);
        Task<string> DeleteRole(string id);
    }
}
