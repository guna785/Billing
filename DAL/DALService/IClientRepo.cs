using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IClientRepo
    {
        Task<client> GetClientID(string ID);
        Task<client> GetClientByPhone(string phone);
        Task<IEnumerable<client>> GetClient();

        Task<bool> InsertClient(client _client);
        Task<bool> UpdateClient(client _client);
        Task<bool> DeleteClient(string id);
    }
}
