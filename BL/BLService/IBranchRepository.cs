using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IBranchRepository
    {
        Task<branch> GetBranchID(string ID);
        Task<branch> GetBranchByName(string name);
        Task<IEnumerable<branch>> GetBranch();
        Task<string> InsertBranch(branch _branch);
        Task<string> UpdateBranch(branch _branch);
        Task<string> DeleteBranch(string id);
    }
}
