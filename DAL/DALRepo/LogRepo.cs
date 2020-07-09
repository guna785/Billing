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
    public class LogRepo : ILogRepo
    {
        private readonly BillingService context;
        public LogRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteLogs(string id)
        {
            FilterDefinition<log> filter = Builders<log>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .logs
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<log>> GetLogs()
        {
            return await context.logs.Find(x => true).ToListAsync();
        }

        public async Task<log> GetLogsByName(string evtName)
        {
            return await context.logs.Find<log>(a => a.name.Equals(evtName)).FirstOrDefaultAsync();
        }

        public async Task<log> GetLogsID(string ID)
        {
            return await context.logs.Find<log>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertLogs(log _log)
        {
            try
            {
                await context.logs.InsertOneAsync(_log);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLogs(log _log)
        {
            try
            {
                ReplaceOneResult updateResult = await context.logs.ReplaceOneAsync(g => g.Id == _log.Id, replacement: _log);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
