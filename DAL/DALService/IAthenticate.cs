using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IAthenticate
    {
         Task<AunthenticatedModel> authenticateuser(string uname, string password);
    }
}
