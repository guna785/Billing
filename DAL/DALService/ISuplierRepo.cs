using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface ISuplierRepo
    {
        Task<suplier> GetSuplierID(string ID);
        Task<suplier> GetSuplierByPhone(string phone);
        Task<IEnumerable<suplier>> GetSuplier();

        Task<bool> InsertSuplier(suplier _Suplier);
        Task<bool> UpdateSuplier(suplier _Suplier);
        Task<bool> DeleteSuplier(string id);
    }
}
