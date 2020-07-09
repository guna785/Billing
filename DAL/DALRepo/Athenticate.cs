
using DAL.DALService;
using DAL.DbService;
using DAL.Helper;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALRepo
{
    public class Athenticate : IAthenticate
    {
        private readonly BillingService context;
        public Athenticate()
        {
            context = new BillingService();
        }

        public async Task<AunthenticatedModel> authenticateuser(string uname, string password)
        {
            var adm =await context.admins.Find<admin>(x => x.uname.Equals(uname) && x.password.Equals(password)).FirstOrDefaultAsync();
            if (adm != null)
            {
                return new AunthenticatedModel() { role = Role.Admin, uname = adm.uname };

            }
            else
            {
                var usr= await context.users.Find<user>(x => x.uname.Equals(uname) && x.password.Equals(password)).FirstOrDefaultAsync();
                if (usr != null)
                {
                    return new AunthenticatedModel() { role = Role.User, uname = adm.uname };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
