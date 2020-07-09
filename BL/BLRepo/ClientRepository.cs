using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class ClientRepository : IClientRepostory
    {
        private readonly IClientRepo _repo;
        public ClientRepository(IClientRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteClient(string id)
        {
            var clt = await _repo.GetClientID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteClient(clt.Id);
                if (res)
                {
                    return "Client data Deletion successfull";
                }
                else
                {
                    return "Client data Deletion Fails";
                }

            }
            else
            {
                return "Client does Not exists";
            }
        }

        public async Task<IEnumerable<client>> GetClient()
        {
            return await _repo.GetClient();
        }

        public async Task<client> GetClientByPhone(string phone)
        {
            return await _repo.GetClientByPhone(phone);
        }

        public async Task<client> GetClientID(string ID)
        {
            return await _repo.GetClientID(ID);
        }

        public async Task<string> InsertClient(client _client)
        {
            var adm = await _repo.GetClientByPhone(_client.phone);
            if (adm == null)
            {
                var res = await _repo.InsertClient(_client);
                if (res)
                {
                    return "Client data insertion successfull";
                }
                else
                {
                    return "Client data insertion Fails";
                }

            }
            else
            {
                return "Client Phone No already exists";
            }
        }

        public async Task<string> UpdateClient(client _client)
        {
            var adm = await _repo.GetClientID(_client.Id);
            if (adm != null)
            {
                _client.Id = adm.Id;
                var res = await _repo.UpdateClient(_client);
                if (res)
                {
                    return "Client data Updation successfull";
                }
                else
                {
                    return "Client data Updation Fails";
                }

            }
            else
            {
                return "Client Phone No Not exists";
            }
        }
    }
}
