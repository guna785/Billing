using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IClientRepostory
    {
        Task<client> GetClientID(string ID);
        Task<client> GetClientByPhone(string phone);
        Task<IEnumerable<client>> GetClient();

        Task<string> InsertClient(client _client);
        Task<string> UpdateClient(client _client);
        Task<string> DeleteClient(string id);
    }
}
