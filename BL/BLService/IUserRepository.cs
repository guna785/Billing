using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IUserRepository
    {
        Task<user> GetUserID(string ID);
        Task<user> GetUserByName(string uname);
        Task<IEnumerable<user>> GetUser();

        Task<string> InsertUser(user _user);
        Task<string> UpdateUser(user _user);
        Task<string> DeleteUser(string id);
    }
}
