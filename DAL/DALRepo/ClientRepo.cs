using DAL.DALService;
using DAL.DbService;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALRepo
{
    public  class ClientRepo : IClientRepo
    {
        private readonly BillingService context;
        public ClientRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteClient(string id)
        {
            FilterDefinition<client> filter = Builders<client>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .clients
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<client>> GetClient()
        {
            return await context.clients.Find(x => true).ToListAsync();
        }

        public async Task<client> GetClientByPhone(string phone)
        {
            return await context.clients.Find<client>(a => a.phone.Equals(phone)).FirstOrDefaultAsync();
        }

        public async Task<client> GetClientID(string ID)
        {
            return await context.clients.Find<client>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertClient(client _client)
        {
            try
            {
                await context.clients.InsertOneAsync(_client);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateClient(client _client)
        {
            try
            {
                ReplaceOneResult updateResult = await context.clients.ReplaceOneAsync(g => g.phone == _client.phone, replacement: _client);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
