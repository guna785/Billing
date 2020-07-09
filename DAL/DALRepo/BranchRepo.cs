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
    public class BranchRepo : IBranchRepo
    {
        private readonly BillingService context;
        public BranchRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteBranch(string id)
        {
            FilterDefinition<branch> filter = Builders<branch>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .branchs
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<branch>> GetBranch()
        {
            return await context.branchs.Find(x => true).ToListAsync();
        }

        public async Task<branch> GetBranchByName(string name)
        {
            return await context.branchs.Find<branch>(a => a.name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<branch> GetBranchID(string ID)
        {
            return await context.branchs.Find<branch>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertBranch(branch _branch)
        {
            try
            {
                await context.branchs.InsertOneAsync(_branch);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBranch(branch _branch)
        {
            try
            {
                ReplaceOneResult updateResult = await context.branchs.ReplaceOneAsync(g => g.name == _branch.name, replacement: _branch);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
