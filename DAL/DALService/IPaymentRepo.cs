using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IPaymentRepo
    {
        Task<payments> GetPaymentsID(string ID);
        Task<payments> GetPaymentsByPaymentsID(string PaymentsId);
        Task<IEnumerable<payments>> GetPayments();

        Task<bool> InsertPayments(payments _payments);
        Task<bool> UpdatePayments(payments _payments);
        Task<bool> DeletePayments(string id);
    }
}
