using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly IuserRepo _repo;
        public UserRepository(IuserRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteUser(string id)
        {
            var clt = await _repo.GetUserID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteUser(clt.Id);
                if (res)
                {
                    return "User data Deletion successfull";
                }
                else
                {
                    return "User data Deletion Fails";
                }

            }
            else
            {
                return "User does Not exists";
            }
        }

        public async Task<IEnumerable<user>> GetUser()
        {
            return await _repo.GetUser();
        }

        public async Task<user> GetUserByName(string uname)
        {
            return await _repo.GetUserByPan(uname);
        }

        public async Task<user> GetUserID(string ID)
        {
            return await _repo.GetUserID(ID);
        }

        public async Task<string> InsertUser(user _user)
        {
            var adm = await _repo.GetUserByPan(_user.uname);
            if (adm == null)
            {
                var res = await _repo.InsertUser(_user);
                if (res)
                {
                    return "User data insertion successfull";
                }
                else
                {
                    return "User data insertion Fails";
                }

            }
            else
            {
                return "User User name already exists";
            }
        }

        public async Task<string> UpdateUser(user _user)
        {
            var adm = await _repo.GetUserByPan(_user.uname);
            if (adm != null)
            {
                //_User.Id = adm.Id;
                var res = await _repo.UpdateUser(_user);
                if (res)
                {
                    return "User data Updation successfull";
                }
                else
                {
                    return "User data Updation Fails";
                }

            }
            else
            {
                return "User User name Not exists";
            }
        }
    }
}
