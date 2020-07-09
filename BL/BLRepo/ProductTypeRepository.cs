using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly IProductTypeRepo _repo;
        public ProductTypeRepository(IProductTypeRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteProductType(string id)
        {
            var clt = await _repo.GetProductTypeID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteProductType(clt.Id);
                if (res)
                {
                    return "ProductType data Deletion successfull";
                }
                else
                {
                    return "ProductType data Deletion Fails";
                }

            }
            else
            {
                return "ProductType does Not exists";
            }
        }

        public async Task<IEnumerable<producttype>> GetProductType()
        {
            return await _repo.GetProductType();
        }

        public async Task<producttype> GetProductTypeByName(string name)
        {
            return await _repo.GetProductTypeByName(name);
        }

        public async Task<producttype> GetProductTypeID(string ID)
        {
            return await _repo.GetProductTypeID(ID);
        }

        public async Task<string> InsertProductType(producttype _producttype)
        {
            var adm = await _repo.GetProductTypeByName(_producttype.name);
            if (adm == null)
            {
                var res = await _repo.InsertProductType(_producttype);
                if (res)
                {
                    return "ProductType data insertion successfull";
                }
                else
                {
                    return "ProductType data insertion Fails";
                }

            }
            else
            {
                return "ProductType ProductType name already exists";
            }
        }

        public async Task<string> UpdateProductType(producttype _producttype)
        {
            var adm = await _repo.GetProductTypeID(_producttype.Id);
            if (adm != null)
            {
                //_ProductType.Id = adm.Id;
                var res = await _repo.UpdateProductType(_producttype);
                if (res)
                {
                    return "ProductType data Updation successfull";
                }
                else
                {
                    return "ProductType data Updation Fails";
                }

            }
            else
            {
                return "ProductType  name Not exists";
            }
        }
    }
}
