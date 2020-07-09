using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IBranchRepo
    {
        Task<branch> GetBranchID(string ID);
        Task<branch> GetBranchByName(string name);
        Task<IEnumerable<branch>> GetBranch();
        Task<bool> InsertBranch(branch _branch);
        Task<bool> UpdateBranch(branch _branch);
        Task<bool> DeleteBranch(string id);
    }
}
