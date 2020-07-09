using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IPymentRepositroy
    {
        Task<payments> GetPaymentsID(string ID);
        Task<payments> GetPaymentsByPaymentsID(string PaymentsId);
        Task<IEnumerable<payments>> GetPayments();

        Task<string> InsertPayments(payments _payments);
        Task<string> UpdatePayments(payments _payments);
        Task<string> DeletePayments(string id);
    }
}
