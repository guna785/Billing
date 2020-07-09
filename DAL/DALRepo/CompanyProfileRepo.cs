using DAL.DALService;
using DAL.DbService;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALRepo
{
    public class CompanyProfileRepo : ICompanyProfileRepo
    {
        private readonly BillingService context;
        public CompanyProfileRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteCompanyProfile(string id)
        {
            FilterDefinition<companyprofile> filter = Builders<companyprofile>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .companyprofiles
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<companyprofile>> GetCompanyProfile()
        {
            return await context.companyprofiles.Find(x => true).ToListAsync();
        }

        public async Task<companyprofile> GetCompanyProfileByPan(string pan)
        {
            return await context.companyprofiles.Find<companyprofile>(a => a.pan.Equals(pan)).FirstOrDefaultAsync();
        }

        public async Task<companyprofile> GetCompanyProfileID(string ID)
        {
            return await context.companyprofiles.Find<companyprofile>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertCompanyProfile(companyprofile _CompanyProfile)
        {
            try
            {
                await context.companyprofiles.InsertOneAsync(_CompanyProfile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCompanyProfile(companyprofile _CompanyProfile)
        {
            try
            {
                ReplaceOneResult updateResult = await context.companyprofiles.ReplaceOneAsync(g => g.Id == _CompanyProfile.Id, replacement: _CompanyProfile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
