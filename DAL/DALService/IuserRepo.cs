using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IuserRepo
    {
        Task<user> GetUserID(string ID);
        Task<user> GetUserByPan(string uname);
        Task<IEnumerable<user>> GetUser();

        Task<bool> InserUser(user _User);
        Task<bool> UpdateUser(user _emp);
        Task<bool> DeleteUser(string id);
    }
}
