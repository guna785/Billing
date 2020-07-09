using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class LogRepository : ILogRepository
    {
        private readonly ILogRepo _repo;
        public LogRepository(ILogRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteLogs(string id)
        {
            var clt = await _repo.GetLogsID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteLogs(clt.Id);
                if (res)
                {
                    return "Logs data Deletion successfull";
                }
                else
                {
                    return "Logs data Deletion Fails";
                }

            }
            else
            {
                return "Logs does Not exists";
            }
        }

        public async Task<IEnumerable<log>> GetLogs()
        {
            return await _repo.GetLogs();
        }

        public async Task<log> GetLogsByName(string evtName)
        {
            return await _repo.GetLogsByName(evtName);
        }

        public async Task<log> GetLogsID(string ID)
        {
            return await _repo.GetLogsID(ID);
        }

        public async Task<string> InsertLogs(log _log)
        {
          
                var res = await _repo.InsertLogs(_log);
                if (res)
                {
                    return "Logs data insertion successfull";
                }
                else
                {
                    return "Logs data insertion Fails";
                }

          
        }

        public async Task<string> UpdateLogs(log _log)
        {
            var adm = await _repo.GetLogsByName(_log.name);
            if (adm != null)
            {
                //_Logs.Id = adm.Id;
                var res = await _repo.UpdateLogs(_log);
                if (res)
                {
                    return "Logs data Updation successfull";
                }
                else
                {
                    return "Logs data Updation Fails";
                }

            }
            else
            {
                return "Logs Logs name Not exists";
            }
        }
    }
}
