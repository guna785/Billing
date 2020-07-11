using BL.BLService;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.BLRepo
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IInvoiceRepo _repo;
        public InvoiceRepository(IInvoiceRepo repo)
        {
            _repo = repo;
        }
        public async Task<string> DeleteInvoice(string id)
        {
            var clt = await _repo.GetInvoiceID(id);
            if (clt != null)
            {
                var res = await _repo.DeleteInvoice(clt.Id);
                if (res)
                {
                    return "Invoice data Deletion successfull";
                }
                else
                {
                    return "Invoice data Deletion Fails";
                }

            }
            else
            {
                return "Invoice does Not exists";
            }
        }

        public async Task<IEnumerable<invoice>> GetInvoice()
        {
            return await _repo.GetInvoice();
        }

        public async Task<invoice> GetInvoiceByInvoiceID(string invoiceId, bool isTaxed)
        {
            return await _repo.GetInvoiceByInvoiceID(invoiceId,isTaxed);
        }

        public async Task<invoice> GetInvoiceID(string ID)
        {
            return await _repo.GetInvoiceID(ID);
        }

        public async  Task<string> InsertInvoice(invoice _invoice,bool isTaxed)
        {
            var adm = await _repo.GetInvoiceByInvoiceID(_invoice.invid, isTaxed);
            if (adm == null)
            {
                var res = await _repo.InsertInvoice(_invoice);
                if (res)
                {
                    return "Invoice data insertion successfull";
                }
                else
                {
                    return "Invoice data insertion Fails";
                }

            }
            else
            {
                return "Invoice Invoice name already exists";
            }
        }

        public async Task<string> UpdateInvoice(invoice _invoice, bool isTaxed)
        {
            var adm = await _repo.GetInvoiceByInvoiceID(_invoice.invid,isTaxed);
            if (adm != null)
            {
                //_Invoice.Id = adm.Id;
                var res = await _repo.UpdateInvoice(_invoice);
                if (res)
                {
                    return "Invoice data Updation successfull";
                }
                else
                {
                    return "Invoice data Updation Fails";
                }

            }
            else
            {
                return "Invoice Invoice name Not exists";
            }
        }
    }
}
