using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface ILogRepository
    {
        Task<log> GetLogsID(string ID);
        Task<log> GetLogsByName(string evtName);
        Task<IEnumerable<log>> GetLogs();

        Task<string> InsertLogs(log _log);
        Task<string> UpdateLogs(log _log);
        Task<string> DeleteLogs(string id);
    }
}
