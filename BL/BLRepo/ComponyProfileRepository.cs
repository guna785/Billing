using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class ComponyProfileRepository : ICompanyProfileRepository
    {
        private readonly ICompanyProfileRepo _repo;
        public ComponyProfileRepository(ICompanyProfileRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteCompanyProfile(string id)
        {
            var clt = await _repo.GetCompanyProfileID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteCompanyProfile(clt.Id);
                if (res)
                {
                    return "CompanyProfile data Deletion successfull";
                }
                else
                {
                    return "CompanyProfile data Deletion Fails";
                }

            }
            else
            {
                return "CompanyProfile does Not exists";
            }
        }

        public async Task<IEnumerable<companyprofile>> GetCompanyProfile()
        {
            return await _repo.GetCompanyProfile();
        }

        public async Task<companyprofile> GetCompanyProfileByPan(string pan)
        {
            return await _repo.GetCompanyProfileByPan(pan);
        }

        public async Task<companyprofile> GetCompanyProfileID(string ID)
        {
            return await _repo.GetCompanyProfileID(ID);
        }

        public async Task<string> InsertCompanyProfile(companyprofile _CompanyProfile)
        {
            var adm = await _repo.GetCompanyProfileByPan(_CompanyProfile.pan);
            if (adm == null)
            {
                var res = await _repo.InsertCompanyProfile(_CompanyProfile);
                if (res)
                {
                    return "CompanyProfile data insertion successfull";
                }
                else
                {
                    return "CompanyProfile data insertion Fails";
                }

            }
            else
            {
                return "CompanyProfile pan already exists";
            }
        }

        public async Task<string> UpdateCompanyProfile(companyprofile _CompanyProfile)
        {
            var adm = await _repo.GetCompanyProfileID(_CompanyProfile.Id);
            if (adm != null)
            {
                _CompanyProfile.Id = adm.Id;
                var res = await _repo.UpdateCompanyProfile(_CompanyProfile);
                if (res)
                {
                    return "CompanyProfile data Updation successfull";
                }
                else
                {
                    return "CompanyProfile data Updation Fails";
                }

            }
            else
            {
                return "CompanyProfile pan Not exists";
            }
        }
    }
}
