using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface ICompanyProfileRepository
    {
        Task<companyprofile> GetCompanyProfileID(string ID);
        Task<companyprofile> GetCompanyProfileByPan(string pan);
        Task<IEnumerable<companyprofile>> GetCompanyProfile();

        Task<string> InsertCompanyProfile(companyprofile _CompanyProfile);
        Task<string> UpdateCompanyProfile(companyprofile _CompanyProfile);
        Task<string> DeleteCompanyProfile(string id);
    }
}
