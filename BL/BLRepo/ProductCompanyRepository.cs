using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class ProductCompanyRepository : IProductCompanyRepository
    {
        private readonly IProductCompanyRepo _repo;
        public ProductCompanyRepository(IProductCompanyRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteProductCompany(string id)
        {
            var clt = await _repo.GetProductCompanyID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteProductCompany(clt.Id);
                if (res)
                {
                    return "ProductCompany data Deletion successfull";
                }
                else
                {
                    return "ProductCompany data Deletion Fails";
                }

            }
            else
            {
                return "ProductCompany does Not exists";
            }
        }

        public async Task<IEnumerable<productcompany>> GetProductCompany()
        {
            return await _repo.GetProductCompany();
        }

        public async Task<productcompany> GetProductCompanyByName(string name)
        {
            return await _repo.GetProductCompanyByName(name);
        }

        public async Task<productcompany> GetProductCompanyID(string ID)
        {
            return await _repo.GetProductCompanyID(ID);
        }

        public async Task<string> InsertProductCompany(productcompany _productcompany)
        {
            var adm = await _repo.GetProductCompanyByName(_productcompany.name);
            if (adm == null)
            {
                var res = await _repo.InsertProductCompany(_productcompany);
                if (res)
                {
                    return "ProductCompany data insertion successfull";
                }
                else
                {
                    return "ProductCompany data insertion Fails";
                }

            }
            else
            {
                return "ProductCompany ProductCompany name already exists";
            }
        }

        public async Task<string> UpdateProductCompany(productcompany _productcompany)
        {
            var adm = await _repo.GetProductCompanyID(_productcompany.Id);
            if (adm != null)
            {
                //_ProductCompany.Id = adm.Id;
                var res = await _repo.UpdateProductCompany(_productcompany);
                if (res)
                {
                    return "ProductCompany data Updation successfull";
                }
                else
                {
                    return "ProductCompany data Updation Fails";
                }

            }
            else
            {
                return "ProductCompany  name Not exists";
            }
        }
    }
}
