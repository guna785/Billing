using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface ISuplierRepository
    {
        Task<suplier> GetSuplierID(string ID);
        Task<suplier> GetSuplierByPhone(string phone);
        Task<IEnumerable<suplier>> GetSuplier();

        Task<string> InsertSuplier(suplier _suplier);
        Task<string> UpdateSuplier(suplier _suplier);
        Task<string> DeleteSuplier(string id);
    }
}
