using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.BLService;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchase;
        private readonly IStocksRepository _stock;
        private readonly ILogRepository _log;
        public PurchaseController(IPurchaseRepository purchase, IStocksRepository stock, ILogRepository log)
        {
            _purchase = purchase;
            _stock = stock;
            _log = log;
        }
        public async Task<IActionResult> CancelPurchase([FromBody] string id)
        {
            var p =await _purchase.GetPurchaseID(id);
            p.status = "Canceled";
            foreach (var item in p.content)
            {
                var stk = await _stock.GetStockID(item.pid);
                var qty = Convert.ToInt32(stk.qty) - Convert.ToInt32(item.qty);
                stk.qty = qty.ToString();
                var r =await _stock.UpdateStock(stk);
            }
            var res = await _purchase.UpdatePurchase(p);
            if (res.Contains("successfull"))
            {               
                var result = new { status = "Purchase is Cancelled successfully" };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Purchase " + p.invid + " Canceled Sucessfully",
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
