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
    public class AdminRepo : IAdminRepo
    {
        private readonly BillingService context;
        public AdminRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteAdmin(string id)
        {
            FilterDefinition<admin> filter = Builders<admin>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .admins
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<admin>> GetAdmin()
        {
            return await context.admins.Find(x => true).ToListAsync();
        }

        public async Task<admin> GetAdminByPan(string uname)
        {
            return await context.admins.Find<admin>(a => a.uname.Equals(uname)).FirstOrDefaultAsync();
        }

        public async Task<admin> GetAdminID(string ID)
        {
            return await context.admins.Find<admin>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InserAdmin(admin _admin)
        {
            try
            {
                await context.admins.InsertOneAsync(_admin);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAdmin(admin _admin)
        {
            try
            {
                ReplaceOneResult updateResult = await context.admins.ReplaceOneAsync(g => g.uname == _admin.uname, replacement: _admin);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
