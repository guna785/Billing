using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class AdminRepository : IAdminRepository
    {
        IAdminRepo _repo;
        public AdminRepository(IAdminRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteAdmin(string id)
        {
            var clt = await _repo.GetAdminID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteAdmin(clt.Id);
                if (res)
                {
                    return "Admin data Deletion successfull";
                }
                else
                {
                    return "Admin data Deletion Fails";
                }

            }
            else
            {
                return "Admin does Not exists";
            }
        }

        public async Task<IEnumerable<admin>> GetAdmin()
        {
            return await _repo.GetAdmin();
        }

        public async Task<admin> GetAdminByPan(string uname)
        {
            return await _repo.GetAdminByPan(uname);
        }

        public async Task<admin> GetAdminID(string ID)
        {
            return await _repo.GetAdminID(ID);
        }

        public async Task<string> InsertAdmin(admin _admin)
        {
            var adm = await _repo.GetAdminID(_admin.Id);
            if (adm == null)
            {
                var res = await _repo.InsertAdmin(_admin);
                if (res)
                {
                    return "Admin data insertion successfull";
                }
                else
                {
                    return "Admin data insertion Fails";
                }

            }
            else
            {
                return "Admin  name already exists";
            }
        }

        public async Task<string> UpdateAdmin(admin _admin)
        {
            var adm = await _repo.GetAdminID(_admin.Id);
            if (adm != null)
            {
                //_stock.Id = adm.Id;
                var res = await _repo.UpdateAdmin(_admin);
                if (res)
                {
                    return "Admin data Updation successfull";
                }
                else
                {
                    return "Admin data Updation Fails";
                }

            }
            else
            {
                return "Admin  name Not exists";
            }
        }
    }
}
