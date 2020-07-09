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
    public class PaymentRepo:IPaymentRepo
    {
        private readonly BillingService context;
        public PaymentRepo()
        {
            context = new BillingService();
        }

        public async Task<bool> DeletePayments(string id)
        {
            FilterDefinition<payments> filter = Builders<payments>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                                                .paymentss
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<payments>> GetPayments()
        {
            return await context.paymentss.Find(x => true).ToListAsync();
        }

        public async Task<payments> GetPaymentsByPaymentsID(string PaymentsId)
        {
            return await context.paymentss.Find<payments>(a => a.pid.Equals(PaymentsId)).FirstOrDefaultAsync();
        }

        public async Task<payments> GetPaymentsID(string ID)
        {
            return await context.paymentss.Find<payments>(a => a.Id.Equals(ID)).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertPayments(payments _payments)
        {
            try
            {
                await context.paymentss.InsertOneAsync(_payments);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePayments(payments _payments)
        {
            try
            {
                ReplaceOneResult updateResult = await context.paymentss.ReplaceOneAsync(g => g.Id == _payments.Id, replacement: _payments);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
