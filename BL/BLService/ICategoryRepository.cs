using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface ICategoryRepository
    {
        Task<category> GetCatagoryID(string ID);
        Task<category> GetCatagoryByName(string name);
        Task<IEnumerable<category>> GetCatagory();

        Task<string> InsertCatagory(category _category);
        Task<string> UpdateCatagory(category _category);
        Task<string> DeleteCatagory(string id);
    }
}
