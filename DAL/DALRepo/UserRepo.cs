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
    public class UserRepo : IuserRepo
    {
        private readonly BillingService context;
        public UserRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteUser(string id)
        {
            FilterDefinition<user> filter = Builders<user>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .users
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<user>> GetUser()
        {
            return await context.users.Find(x => true).ToListAsync();
        }

        public async Task<user> GetUserByPan(string uname)
        {
            return await context.users.Find<user>(a => a.uname.Equals(uname)).FirstOrDefaultAsync();
        }

        public async Task<user> GetUserID(string ID)
        {
            return await context.users.Find<user>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InserUser(user _user)
        {
            try
            {
                await context.users.InsertOneAsync(_user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(user _user)
        {
            try
            {
                ReplaceOneResult updateResult = await context.users.ReplaceOneAsync(g => g.uname == _user.uname, replacement: _user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
