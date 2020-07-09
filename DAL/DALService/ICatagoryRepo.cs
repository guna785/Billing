using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface ICatagoryRepo
    {
        Task<category> GetCatagoryID(string ID);
        Task<category> GetCatagoryByName(string name);
        Task<IEnumerable<category>> GetCatagory();

        Task<bool> InsertCatagory(category _category);
        Task<bool> UpdateCatagory(category _category);
        Task<bool> DeleteCatagory(string id);
    }
}
