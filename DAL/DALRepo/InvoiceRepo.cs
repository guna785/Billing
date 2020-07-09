using DAL.DALService;
using DAL.DbService;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DALRepo
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly BillingService context;
        public InvoiceRepo()
        {
            context = new BillingService();
        }
        public async Task<bool> DeleteInvoice(string id)
        {
            FilterDefinition<invoice> filter = Builders<invoice>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .invoicess
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<invoice>> GetInvoice()
        {
            return await context.invoicess.Find(x => true).ToListAsync();
        }

        public async Task<invoice> GetInvoiceByInvoiceID(string invoiceId)
        {
            return await context.invoicess.Find<invoice>(a => a.invid.Equals(invoiceId)).FirstOrDefaultAsync();
        }

        public async Task<invoice> GetInvoiceID(string ID)
        {
            return await context.invoicess.Find<invoice>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertInvoice(invoice _invoice)
        {
            try
            {
                await context.invoicess.InsertOneAsync(_invoice);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateInvoice(invoice _invoice)
        {
            try
            {
                ReplaceOneResult updateResult = await context.invoicess.ReplaceOneAsync(g => g.Id == _invoice.Id, replacement: _invoice);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
