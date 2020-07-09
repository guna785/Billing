using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICatagoryRepo _repo;
        public CategoryRepository(ICatagoryRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteCatagory(string id)
        {
            var clt = await _repo.GetCatagoryID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteCatagory(clt.Id);
                if (res)
                {
                    return "Catagory data Deletion successfull";
                }
                else
                {
                    return "Catagory data Deletion Fails";
                }

            }
            else
            {
                return "Catagory does Not exists";
            }
        }

        public async Task<IEnumerable<category>> GetCatagory()
        {
            return await _repo.GetCatagory();
        }

        public async Task<category> GetCatagoryByName(string name)
        {
            return await _repo.GetCatagoryByName(name);
        }

        public async Task<category> GetCatagoryID(string ID)
        {
            return await _repo.GetCatagoryID(ID);
        }

        public async Task<string> InsertCatagory(category _category)
        {
            var adm = await _repo.GetCatagoryByName(_category.name);
            if (adm == null)
            {
                var res = await _repo.InsertCatagory(_category);
                if (res)
                {
                    return "Catagory data insertion successfull";
                }
                else
                {
                    return "Catagory data insertion Fails";
                }

            }
            else
            {
                return "CatagoryCatagory name already exists";
            }
        }

        public async Task<string> UpdateCatagory(category _category)
        {
            var adm = await _repo.GetCatagoryID(_category.Id);
            if (adm != null)
            {
                //_Catagory.Id = adm.Id;
                var res = await _repo.UpdateCatagory(_category);
                if (res)
                {
                    return "Catagory data Updation successfull";
                }
                else
                {
                    return "Catagory data Updation Fails";
                }

            }
            else
            {
                return "CatagoryCatagory name Not exists";
            }
        }
    }
}
