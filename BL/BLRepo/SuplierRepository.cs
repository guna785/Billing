using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class SuplierRepository : ISuplierRepository
    {
        private readonly ISuplierRepo _repo;
        public SuplierRepository(ISuplierRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteSuplier(string id)
        {
            var clt = await _repo.GetSuplierID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteSuplier(clt.Id);
                if (res)
                {
                    return "Suplier data Deletion successfull";
                }
                else
                {
                    return "Suplier data Deletion Fails";
                }

            }
            else
            {
                return "Suplier does Not exists";
            }
        }

        public async Task<IEnumerable<suplier>> GetSuplier()
        {
            return await _repo.GetSuplier();
        }

        public async Task<suplier> GetSuplierByPhone(string phone)
        {
            return await _repo.GetSuplierByPhone(phone);
        }

        public async Task<suplier> GetSuplierID(string ID)
        {
            return await _repo.GetSuplierID(ID);
        }

        public async Task<string> InsertSuplier(suplier _suplier)
        {
            var adm = await _repo.GetSuplierByPhone(_suplier.cphone);
            if (adm == null)
            {
                var res = await _repo.InsertSuplier(_suplier);
                if (res)
                {
                    return "Suplier data insertion successfull";
                }
                else
                {
                    return "Suplier data insertion Fails";
                }

            }
            else
            {
                return "Suplier  already exists";
            }
        }

        public async Task<string> UpdateSuplier(suplier _suplier)
        {
            var adm = await _repo.GetSuplierByPhone(_suplier.cphone);
            if (adm != null)
            {
                //_Suplier.Id = adm.Id;
                var res = await _repo.UpdateSuplier(_suplier);
                if (res)
                {
                    return "Suplier data Updation successfull";
                }
                else
                {
                    return "Suplier data Updation Fails";
                }

            }
            else
            {
                return "Suplier  Not exists";
            }
        }
    }
}
