using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.BLService;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISalesRepository _sales;
        private readonly IStocksRepository _stock;
        private readonly IInvoiceRepository _invoice;
        private readonly IPymentRepositroy _pyment;
        private readonly ILogRepository _log;
        public SalesController(ISalesRepository sales, IStocksRepository stock, IInvoiceRepository invoice, IPymentRepositroy pyment, ILogRepository log)
        {
            _sales = sales;
            _stock = stock;
            _invoice = invoice;
            _pyment = pyment;
            _log = log;

        }
        [HttpPost]
        public async Task<IActionResult> CancelSales([FromBody] string id)
        {
            var s = await _sales.GetSalesID(id);
            s.status = "Canceled";
            foreach (var item in s.content)
            {
                var stk = await _stock.GetStockID(item.pid);
                var qty = Convert.ToInt32(stk.qty) + Convert.ToInt32(item.qty);
                stk.qty = qty.ToString();
                var r = await _stock.UpdateStock(stk);
            }
            var inv = await _invoice.GetInvoice();
            var py = await _pyment.GetPayments();
            var i = inv.Where(x => x.sid.Equals(s.sid)).FirstOrDefault();
            i.status = "Canceled";
            var res = await _invoice.UpdateInvoice(i);
            var pm = py.Where(x => x.sid.Equals(s.sid)).ToList();
            foreach (var p in pm)
            {
                p.status = "Canceled";
                res = await _pyment.UpdatePayments(p);
            }
            res = await _sales.UpdateSales(s);
            if (res.Contains("successfull"))
            {
                var result = new { status = "Sales is Cancelled successfully" };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Sales " + i.sid + " Canceled Sucessfully",
                    name = "Event",
                    uid = HttpContext.User.Identity.Name
                };
                res = await _log.InsertLogs(l);
                return Ok(result);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
