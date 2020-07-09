using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IRoleRepo
    {
        Task<role> GetRoleID(string ID);
        Task<role> GetRoleByName(string name);
        Task<IEnumerable<role>> GetRole();

        Task<bool> InsertRole(role _role);
        Task<bool> UpdateRole(role _role);
        Task<bool> DeleteRole(string id);
    }
}
