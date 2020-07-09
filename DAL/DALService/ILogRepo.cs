using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface ILogRepo
    {
        Task<log> GetLogsID(string ID);
        Task<log> GetLogsByName(string evtName);
        Task<IEnumerable<log>> GetLogs();

        Task<bool> InsertLogs(log _log);
        Task<bool> UpdateLogs(log _log);
        Task<bool> DeleteLogs(string id);
    }
}
