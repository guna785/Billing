using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class BranchRepository : IBranchRepository
    {
        private readonly IBranchRepo _repo;
        public BranchRepository(IBranchRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteBranch(string id)
        {
            var clt = await _repo.GetBranchID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteBranch(clt.Id);
                if (res)
                {
                    return "Branch data Deletion successfull";
                }
                else
                {
                    return "Branch data Deletion Fails";
                }

            }
            else
            {
                return "Branch does Not exists";
            }
        }

        public async Task<IEnumerable<branch>> GetBranch()
        {
            return await _repo.GetBranch();
        }

        public async Task<branch> GetBranchByName(string name)
        {
            return await _repo.GetBranchByName(name);
        }

        public async Task<branch> GetBranchID(string ID)
        {
            return await _repo.GetBranchID(ID);
        }

        public async Task<string> InsertBranch(branch _branch)
        {
            var adm = await _repo.GetBranchByName(_branch.name);
            if (adm == null)
            {
                var res = await _repo.InsertBranch(_branch);
                if (res)
                {
                    return "Branch data insertion successfull";
                }
                else
                {
                    return "Branch data insertion Fails";
                }

            }
            else
            {
                return "BranchBranch name already exists";
            }
        }

        public async Task<string> UpdateBranch(branch _branch)
        {
            var adm = await _repo.GetBranchByName(_branch.name);
            if (adm != null)
            {
                //_Branch.Id = adm.Id;
                var res = await _repo.UpdateBranch(_branch);
                if (res)
                {
                    return "Branch data Updation successfull";
                }
                else
                {
                    return "Branch data Updation Fails";
                }

            }
            else
            {
                return "Branch name Not exists";
            }
        }
    }
}
