using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface ICompanyProfileRepo
    {
        Task<companyprofile> GetCompanyProfileID(string ID);
        Task<companyprofile> GetCompanyProfileByPan(string pan);
        Task<IEnumerable<companyprofile>> GetCompanyProfile();

        Task<bool> InsertCompanyProfile(companyprofile _CompanyProfile);
        Task<bool> UpdateCompanyProfile(companyprofile _CompanyProfile);
        Task<bool> DeleteCompanyProfile(string id);
    }
}
