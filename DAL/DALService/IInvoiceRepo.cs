using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALService
{
    public interface IInvoiceRepo
    {
        Task<invoice> GetInvoiceID(string ID);
        Task<invoice> GetInvoiceByInvoiceID(string invoiceId);
        Task<IEnumerable<invoice>> GetInvoice();

        Task<bool> InsertInvoice(invoice _invoice);
        Task<bool> UpdateInvoice(invoice _invoice);
        Task<bool> DeleteInvoice(string id);
    }
}
