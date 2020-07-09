using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class PaymentRepository : IPymentRepositroy
    {
        private readonly IPaymentRepo _repo;
        public PaymentRepository(IPaymentRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeletePayments(string id)
        {
            var clt = await _repo.GetPaymentsID(id);
            if (clt != null)
            {
                var res = await _repo.DeletePayments(clt.Id);
                if (res)
                {
                    return "Payments data Deletion successfull";
                }
                else
                {
                    return "Payments data Deletion Fails";
                }

            }
            else
            {
                return "Payments does Not exists";
            }
        }

        public async Task<IEnumerable<payments>> GetPayments()
        {
            return await _repo.GetPayments();
        }

        public async Task<payments> GetPaymentsByPaymentsID(string PaymentsId)
        {
            return await _repo.GetPaymentsByPaymentsID(PaymentsId);
        }

        public async Task<payments> GetPaymentsID(string ID)
        {
            return await _repo.GetPaymentsID(ID);
        }

        public async Task<string> InsertPayments(payments _payments)
        {
            var adm = await _repo.GetPaymentsByPaymentsID(_payments.pid);
            if (adm == null)
            {
                var res = await _repo.InsertPayments(_payments);
                if (res)
                {
                    return "Payments data insertion successfull";
                }
                else
                {
                    return "Payments data insertion Fails";
                }

            }
            else
            {
                return "Payments  already exists";
            }
        }

        public async Task<string> UpdatePayments(payments _payments)
        {
            var adm = await _repo.GetPaymentsByPaymentsID(_payments.pid);
            if (adm != null)
            {
                //_Payments.Id = adm.Id;
                var res = await _repo.UpdatePayments(_payments);
                if (res)
                {
                    return "Payments data Updation successfull";
                }
                else
                {
                    return "Payments data Updation Fails";
                }

            }
            else
            {
                return "Payments  Not exists";
            }
        }
    }
}
