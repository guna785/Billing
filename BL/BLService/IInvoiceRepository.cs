using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLService
{
    public interface IInvoiceRepository
    {
        Task<invoice> GetInvoiceID(string ID);
        Task<invoice> GetInvoiceByInvoiceID(string invoiceId);
        Task<IEnumerable<invoice>> GetInvoice();

        Task<string> InsertInvoice(invoice _invoice);
        Task<string> UpdateInvoice(invoice _invoice);
        Task<string> DeleteInvoice(string id);
    }
}
