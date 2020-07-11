using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Billing.Models;
using BL.BLService;
using BL.Helper;
using BL.SchemaModel;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using MongoDB.Driver.Linq;

namespace Billing.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class StockController : Controller
    {
        private readonly IStocksRepository _repo;
        private readonly IPurchaseRepository _purchase;
        private readonly ISalesRepository _sales;
        private readonly IInvoiceRepository _invoice;
        private readonly IPymentRepositroy _pyment;
        private readonly ILogRepository _log;
        public StockController(IStocksRepository repo, IPurchaseRepository purchase, ISalesRepository sales,
                                IInvoiceRepository invoice, IPymentRepositroy pyment, ILogRepository log)
        {
            _repo = repo;
            _purchase = purchase;
            _sales = sales;
            _invoice = invoice;
            _pyment = pyment;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> StockPost([FromBody] AddStock value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new stock()
            {
                actprice = value.actprice,
                cdate = DateTime.Now,
                cmname = value.cmname,
                company = value.company,
                discount = value.discount,
                lmdate = DateTime.Now,
                minalert = value.minalert,
                mrcode = MrcodeGenerate(value.actprice),
                mrp = value.mrp,
                name = value.name,
                photo = string.IsNullOrEmpty(value.photo) ? null : Convert.FromBase64String(value.photo),
                pid = Guid.NewGuid().ToString(),
                qty = "0",
                nonTaxQty=value.qty,
                remarks = "none",
                status = "active",
                tax = value.tax,
                warranty = value.warranty
            };
            var res = await _repo.InsertStock(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Stock " + value.name + " Inserted Sucessfully",
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
        private string MrcodeGenerate(string code)
        {
            var newVal = code.Replace('1', 'P');
            newVal = newVal.Replace('2', 'M');
            newVal = newVal.Replace('3', 'Y');
            newVal = newVal.Replace('4', 'C');
            newVal = newVal.Replace('5', 'H');
            newVal = newVal.Replace('6', 'A');
            newVal = newVal.Replace('7', 'K');
            newVal = newVal.Replace('8', 'B');
            newVal = newVal.Replace('9', 'N');
            newVal = newVal.Replace('0', 'S');
            return newVal;
        }
        [HttpPost]
        public async Task<IActionResult> StockEditPost([FromBody] EditStock value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var sk = await _repo.GetStockID(value.Id);
            var stk = new stock()
            {
                Id = value.Id,
                actprice = value.actprice,
                cdate = DateTime.Now,
                cmname = value.cmname,
                company = value.company,
                discount = value.discount,
                lmdate = DateTime.Now,
                minalert = value.minalert,
                mrcode = MrcodeGenerate(value.actprice),
                mrp = value.mrp,
                name = value.name,
                pid = Guid.NewGuid().ToString(),
                qty = sk.qty,
                nonTaxQty=(Convert.ToDouble(sk.nonTaxQty)+Convert.ToDouble(value.qty)).ToString(),
                remarks = "none",
                status = "active",
                tax = value.tax,
                warranty = value.warranty
            };
            if (!string.IsNullOrEmpty(value.photo))
            {
                stk.photo = Convert.FromBase64String(value.photo);
            }
            else
            {
                stk.photo = sk.photo;
            }
            var res = await _repo.UpdateStock(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Stock " + value.name + " Updated Sucessfully",
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
        [HttpPost]
        public async Task<IActionResult> DeleteStock([FromBody] string id)
        {
            var res = await _repo.DeleteStock(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Stock " + id + " Deleted Sucessfully",
                    name = "Event",
                    uid = HttpContext.User.Identity.Name
                };
                res = await _log.InsertLogs(l);
                return Ok(result);
            }
            else
            {
                return BadRequest("Request Not Completed");
            }

        }
        public async Task<IActionResult> purchaseAutocomplete(string term)
        {
            var stk = await _repo.GetStock();
            var temp = new List<PurchaseAutoComplete>();
            foreach (var item in stk)
            {
                if (item.name.ToLower().Contains(term.ToLower()))
                {
                    var t = new PurchaseAutoComplete();
                    t.Id = item.Id;
                    t.value = item.name;
                    t.label = "<img width='50' height='50' src='data: image/png;base64," + Convert.ToBase64String(item.photo) + "' class='img - circle elevation - 2'> &nbsp;&nbsp;&nbsp;" + item.name + "&nbsp;&nbsp;&nbsp;" + item.cmname + "&nbsp;&nbsp;&nbsp; Rs. " + item.actprice;
                    temp.Add(t);
                }

            }
            return Ok(temp);
        }
        public async Task<IActionResult> getStockByID(string Id)
        {
            return Ok(await _repo.GetStockID(Id));
        }
        [HttpPost]
        public IActionResult DeleteStockSession([FromBody] string id)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                var s = stk.Where(x => x.Id == id).FirstOrDefault();
                stk.Remove(s);
                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
                var result = new { status = "Stock Session Removed Successfully" };
           
                return Ok(result);
            }
            else
            {
                return BadRequest("Request Not Completed");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SetStockSession([FromBody] stockSPpost value)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                if (stk.Any(x => x.Id == value.Id))
                {

                    foreach (var s in stk)
                    {
                        if (s.Id == value.Id)
                        {
                            s.qty = s.qty + value.qty;
                            s.amount = (Convert.ToInt32(s.actprice) * s.qty).ToString();
                        }
                    }

                }
                else
                {

                    var p = new PurchaseModel()
                    {
                        Id = sk.Id,
                        actprice = sk.actprice,
                        cmname = sk.cmname,
                        name = sk.name,
                        discount = sk.discount,
                        minalert = sk.minalert,
                        mrp = sk.mrp,
                        photo = sk.photo,
                        qty = value.qty,
                        tax = sk.tax,
                        amount = (Convert.ToInt32(sk.actprice) * value.qty).ToString(),
                        company = sk.company,
                        warranty = sk.warranty
                    };
                    stk.Add(p);
                }

                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            else
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = new List<PurchaseModel>();
                var p = new PurchaseModel()
                {
                    Id = sk.Id,
                    actprice = sk.actprice,
                    cmname = sk.cmname,
                    name = sk.name,
                    discount = sk.discount,
                    minalert = sk.minalert,
                    mrp = sk.mrp,
                    photo = sk.photo,
                    qty = value.qty,
                    tax = sk.tax,
                    amount = (Convert.ToInt32(sk.actprice) * value.qty).ToString(),
                    company = sk.company,
                    warranty = sk.warranty
                };
                stk.Add(p);
                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            var result = new { status = "Stock Session Added Successfully" };
            return Ok(result);
        }

        public IActionResult getSessionPTamount()
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                double tax = 0, amt = 0;
                foreach (var item in stk)
                {
                    var wtamt = (Convert.ToDouble(item.amount) * 100) / (100 + Convert.ToDouble(item.tax));

                    tax += (wtamt * Convert.ToDouble(item.tax)) / 100;
                    amt += Convert.ToDouble(item.amount);
                }
                tax = Math.Round(tax, 2);
                var result = new { tax = tax.ToString(), amt = amt.ToString() };
                return Ok(result);
            }
            else
            {
                var result = new { tax = "0", amt = "0" };
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MekePuchaseWithBill([FromBody] PurchaseMake value)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));

                var p = new purchase();
                p.cdate = DateTime.Now;
                p.invdate = value.invdate;
                p.invid = value.invno;
                p.tamt = value.amt;
                p.tottax = value.tax;
                p.suplier = value.suplier;
                p.status = "Purchased";
                p.remarks = value.comment;
                p.isTaxed = true;
                var pitm = new List<purchaseitem>();
                foreach (var item in stk)
                {
                    var tm = new purchaseitem();
                    tm.actp = item.actprice;
                    tm.cmname = item.cmname;
                    tm.company = item.company;
                    tm.discount = item.discount;
                    tm.minalert = item.minalert;
                    tm.mrp = item.mrp;
                    tm.name = item.name;
                    tm.pid = item.Id;
                    tm.qty = item.qty.ToString();
                    tm.tax = item.tax;
                    tm.warranty = item.warranty;
                    pitm.Add(tm);
                    var st = await _repo.GetStockID(item.Id);
                    var qt = Convert.ToInt32(st.qty) + Convert.ToInt32(item.qty);
                    st.qty = qt.ToString();
                    var r = await _repo.UpdateStock(st);
                }
                p.content = pitm;
                var res = await _purchase.InsertPurchase(p);

                if (res.Contains("successfull"))
                {
                    HttpContext.Session.Remove("sessionStock");
                    var result = new { status = res };
                    var l = new log()
                    {
                        cdate = DateTime.Now,
                        message = "Purchase Added for " + p.invid + " Inserted Sucessfully",
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
            else
            {
                return BadRequest("Requset Not Completed");
            }
        }
        public async Task<IActionResult> MekePuchaseWithOutBill([FromBody] PurchaseMake value)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));

                var p = new purchase();
                p.cdate = DateTime.Now;
                p.invdate = value.invdate;
                p.invid = value.invno;
                p.tamt = value.amt;
                p.tottax = value.tax;
                p.suplier = value.suplier;
                p.status = "Purchased";
                p.remarks = value.comment;
                p.isTaxed = false;
                var pitm = new List<purchaseitem>();
                foreach (var item in stk)
                {
                    var tm = new purchaseitem();
                    tm.actp = item.actprice;
                    tm.cmname = item.cmname;
                    tm.company = item.company;
                    tm.discount = item.discount;
                    tm.minalert = item.minalert;
                    tm.mrp = item.mrp;
                    tm.name = item.name;
                    tm.pid = item.Id;
                    tm.qty = item.qty.ToString();
                    tm.tax = item.tax;
                    tm.warranty = item.warranty;
                    pitm.Add(tm);
                }
                p.content = pitm;
                var res = await _purchase.InsertPurchase(p);
                if (res.Contains("successfull"))
                {
                    HttpContext.Session.Remove("sessionStock");
                    var result = new { status = res };
                    return Ok(result);

                }
                else
                {
                    return BadRequest(res);
                }


            }
            else
            {
                return BadRequest("Requset Not Completed");
            }
        }
        public async Task<IActionResult> SetSlaesStockSession([FromBody] stockSPpost value)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                if (stk.Any(x => x.Id == value.Id))
                {

                    foreach (var s in stk)
                    {
                        if (s.Id == value.Id)
                        {
                            s.qty = s.qty + value.qty;
                            s.amount = (Convert.ToInt32(s.mrp) * s.qty).ToString();
                        }
                    }

                }
                else
                {

                    var p = new PurchaseModel()
                    {
                        Id = sk.Id,
                        actprice = sk.actprice,
                        cmname = sk.cmname,
                        name = sk.name,
                        discount = sk.discount,
                        minalert = sk.minalert,
                        mrp = sk.mrp,
                        photo = sk.photo,
                        qty = value.qty,
                        tax = sk.tax,
                        amount = (Convert.ToInt32(sk.mrp) * value.qty).ToString(),
                        company = sk.company,
                        warranty = sk.warranty
                    };
                    stk.Add(p);
                }

                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            else
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = new List<PurchaseModel>();
                var p = new PurchaseModel()
                {
                    Id = sk.Id,
                    actprice = sk.actprice,
                    cmname = sk.cmname,
                    name = sk.name,
                    discount = sk.discount,
                    minalert = sk.minalert,
                    mrp = sk.mrp,
                    photo = sk.photo,
                    qty = value.qty,
                    tax = sk.tax,
                    amount = (Convert.ToInt32(sk.mrp) * value.qty).ToString(),
                    company = sk.company,
                    warranty = sk.warranty
                };
                stk.Add(p);
                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            var result = new { status = "Stock Session Added Successfully" };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MekeSalesWithBill([FromBody] SaleMake sale)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));

                var sls = await _sales.GetSales();
                var inv = await _invoice.GetInvoice();
                var py = await _pyment.GetPayments();
                var sl = new sales();
                sl.cdate = DateTime.Now;
                sl.clid = sale.clphone;
                sl.gst = sale.gst;
                sl.isTaxed = true;
                if (sls.Count() > 0)
                {
                    var latest = sls.Where(x => x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        sl.sid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.sid);
                    }
                    else
                    {
                        sl.sid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }
                   
                }
                else
                {
                    sl.sid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                sl.userid = HttpContext.User.Identity.Name;
                sl.clname = sale.clname;
                sl.tamt = sale.amt;
                sl.taxt = sale.tax;
                sl.status = "active";
                var slsitm = new List<salesitem>();
                foreach (var item in stk)
                {
                    var s = new salesitem();
                    s.actp = item.actprice;
                    s.cmname = item.cmname;
                    s.company = item.company;
                    s.discount = item.discount;
                    s.minalert = item.minalert;
                    s.mrp = item.mrp;
                    s.name = item.name;
                    s.pid = item.Id;
                    s.qty = item.qty.ToString();
                    s.tax = item.tax;
                    s.warranty = item.warranty;
                    slsitm.Add(s);
                    var st = await _repo.GetStockID(item.Id);
                    var qt = Convert.ToInt32(st.qty) - Convert.ToInt32(item.qty);
                    st.qty = qt.ToString();
                    var r = await _repo.UpdateStock(st);
                }
                sl.content = slsitm;

                var i = new invoice();
                if (inv.Count() > 0)
                {
                    var latest = inv.Where(x => x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        i.invid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.invid);
                    }
                    else
                    {
                        i.invid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }
                    
                }
                else
                {
                    i.invid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                i.paid = sale.paid;
                i.sid = sl.sid;
                if (Convert.ToDouble(sale.balance) == 0)
                {
                    i.status = "Paid";
                }
                else 
                {
                    i.status = "UnPaid";
                }
                i.amt = sale.amt;
                i.balance = sale.balance;
                i.cdate = DateTime.Now;
                i.isTaxed = true;
                var res =await _invoice.InsertInvoice(i,true);
                var p = new payments();
                p.amt = sale.paid;
                p.userid = HttpContext.User.Identity.Name;
                p.status = "paid";
                p.cdate = DateTime.Now;
                p.invid = i.invid;
                p.isTaxed = true;
                if (py.Count() > 0)
                {
                    var latest = py.Where(x => x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        p.pid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.pid);
                    }
                    else
                    {
                        p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }
                   
                }
                else
                {
                    p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                p.sid = sl.sid;
                res =await _pyment.InsertPayments(p,true);

                res =await _sales.InsertSales(sl,true);

                if (res.Contains("successfull"))
                {
                    HttpContext.Session.Remove("sessionStock");
                    var result = new { status = res };
                    var l = new log()
                    {
                        cdate = DateTime.Now,
                        message = "Sales " + sl.sid + " Inserted Sucessfully",
                        name = "Event",
                        isTaxed=true,
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
            else
            {
                return BadRequest("Request not Completed");
            }
        }
        public async Task<IActionResult> SetWithOutTaxSlaesStockSession([FromBody] stockSPpost value)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                if (stk.Any(x => x.Id == value.Id))
                {

                    foreach (var s in stk)
                    {
                        if (s.Id == value.Id)
                        {
                            s.qty = s.qty + value.qty;
                            s.amount = (Convert.ToInt32(s.mrp) * s.qty).ToString();
                        }
                    }

                }
                else
                {

                    var p = new PurchaseModel()
                    {
                        Id = sk.Id,
                        actprice = sk.actprice,
                        cmname = sk.cmname,
                        name = sk.name,
                        discount = sk.discount,
                        minalert = sk.minalert,
                        mrp = sk.mrp,
                        photo = sk.photo,
                        qty = value.qty,
                        tax = sk.tax,
                        amount = (Convert.ToInt32(sk.mrp) * value.qty).ToString(),
                        company = sk.company,
                        warranty = sk.warranty
                    };
                    stk.Add(p);
                }

                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            else
            {
                var sk = await _repo.GetStockID(value.Id);
                var stk = new List<PurchaseModel>();
                var p = new PurchaseModel()
                {
                    Id = sk.Id,
                    actprice = sk.actprice,
                    cmname = sk.cmname,
                    name = sk.name,
                    discount = sk.discount,
                    minalert = sk.minalert,
                    mrp = sk.mrp,
                    photo = sk.photo,
                    qty = value.qty,
                    tax = sk.tax,
                    amount = (Convert.ToInt32(sk.mrp) * value.qty).ToString(),
                    company = sk.company,
                    warranty = sk.warranty
                };
                stk.Add(p);
                HttpContext.Session.SetString("sessionStock", Newtonsoft.Json.JsonConvert.SerializeObject(stk));
            }
            var result = new { status = "Stock Session Added Successfully" };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> MekeSalesWithOutBill([FromBody] SaleMake sale)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var stk = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));

                var sls = await _sales.GetSales();
                var inv = await _invoice.GetInvoice();
                var py = await _pyment.GetPayments();
                var sl = new sales();
                sl.cdate = DateTime.Now;
                sl.clid = sale.clphone;
                sl.gst = sale.gst;
                sl.isTaxed = false;
                if (sls.Count() > 0)
                {
                    var latest = sls.Where(x => !x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        sl.sid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.sid);
                    }
                    else
                    {
                        sl.sid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }

                }
                else
                {
                    sl.sid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                sl.userid = HttpContext.User.Identity.Name;
                sl.clname = sale.clname;
                sl.tamt = sale.amt;
                sl.taxt = sale.tax;
                sl.status = "active";
                var slsitm = new List<salesitem>();
                foreach (var item in stk)
                {
                    var s = new salesitem();
                    s.actp = item.actprice;
                    s.cmname = item.cmname;
                    s.company = item.company;
                    s.discount = item.discount;
                    s.minalert = item.minalert;
                    s.mrp = item.mrp;
                    s.name = item.name;
                    s.pid = item.Id;
                    s.qty = item.qty.ToString();
                    s.tax = item.tax;
                    s.warranty = item.warranty;
                    slsitm.Add(s);
                    var st = await _repo.GetStockID(item.Id);
                    var qt = Convert.ToInt32(st.nonTaxQty) - Convert.ToInt32(item.qty);
                    st.nonTaxQty = qt.ToString();
                    var r = await _repo.UpdateStock(st);
                }
                sl.content = slsitm;

                var i = new invoice();
                if (inv.Count() > 0)
                {
                    var latest = inv.Where(x => !x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        i.invid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.invid);
                    }
                    else
                    {
                        i.invid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }

                }
                else
                {
                    i.invid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                i.paid = sale.paid;
                i.sid = sl.sid;
                if (Convert.ToDouble(sale.balance) == 0)
                {
                    i.status = "Paid";
                }
                else
                {
                    i.status = "UnPaid";
                }
                i.amt = sale.amt;
                i.balance = sale.balance;
                i.cdate = DateTime.Now;
                i.isTaxed = false;
                var res = await _invoice.InsertInvoice(i,false);
                var p = new payments();
                p.amt = sale.paid;
                p.userid = HttpContext.User.Identity.Name;
                p.status = "paid";
                p.cdate = DateTime.Now;
                p.invid = i.invid;
                p.isTaxed = false;
                if (py.Count() > 0)
                {
                    var latest = py.Where(x => !x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                    if (latest != null)
                    {
                        p.pid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.pid);
                    }
                    else
                    {
                        p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                    }

                }
                else
                {
                    p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }
                p.sid = sl.sid;
                res = await _pyment.InsertPayments(p,false);

                res = await _sales.InsertSales(sl,false);

                if (res.Contains("successfull"))
                {
                    HttpContext.Session.Remove("sessionStock");
                    var result = new { status = res };
                    var l = new log()
                    {
                        cdate = DateTime.Now,
                        message = "Sales " + sl.sid + " Inserted Sucessfully",
                        name = "Event",
                        isTaxed=false,
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
            else
            {
                return BadRequest("Request not Completed");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PayInvoice([FromBody]PayInvoice pay)
        {
            var inv =await _invoice.GetInvoiceID(pay.Id);
            var py = await _pyment.GetPayments();
            inv.paid = (Convert.ToDouble(inv.paid) + Convert.ToDouble(pay.amount)).ToString() ;
            inv.balance = (Convert.ToDouble(inv.balance) - Convert.ToDouble(pay.amount)).ToString();
            if (Convert.ToDouble(inv.balance) == 0)
            {
                inv.status = "Paid";
            }
            else
            {
                inv.status = "UnPaid";
            }

            var p = new payments();
            p.amt = pay.amount;
            p.cdate = DateTime.Now;
            p.invid = inv.invid;
            p.sid = inv.sid;
            p.userid = HttpContext.User.Identity.Name;
            p.status = "paid";
            p.isTaxed = true;
            if (py.Count() > 0)
            {
                var latest = py.Where(x => x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                if (latest != null)
                {
                    p.pid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.pid);
                }
                else
                {
                    p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }

            }
            else
            {
                p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
            }
            var res =await _pyment.InsertPayments(p,true);

            res =await _invoice.UpdateInvoice(inv,true);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Invoice  " + inv.invid + "is paid sum of Rs. "+pay.amount+" Inserted Sucessfully",
                    name = "Event",
                    isTaxed=true,
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

        [HttpPost]
        public async Task<IActionResult> PayNonTaxInvoices([FromBody] PayInvoice pay)
        {
            var inv = await _invoice.GetInvoiceID(pay.Id);
            var py = await _pyment.GetPayments();
            inv.paid = (Convert.ToDouble(inv.paid) + Convert.ToDouble(pay.amount)).ToString();
            inv.balance = (Convert.ToDouble(inv.balance) - Convert.ToDouble(pay.amount)).ToString();
            if (Convert.ToDouble(inv.balance) == 0)
            {
                inv.status = "Paid";
            }
            else
            {
                inv.status = "UnPaid";
            }

            var p = new payments();
            p.amt = pay.amount;
            p.cdate = DateTime.Now;
            p.invid = inv.invid;
            p.sid = inv.sid;
            p.userid = HttpContext.User.Identity.Name;
            p.status = "paid";
            p.isTaxed = false;
            if (py.Count() > 0)
            {
                var latest = py.Where(x => x.isTaxed).OrderByDescending(x => x.cdate).FirstOrDefault();
                if (latest != null)
                {
                    p.pid = GetFInancialYear.ToFinancialYearShort(DateTime.Now, latest.pid);
                }
                else
                {
                    p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
                }

            }
            else
            {
                p.pid = GetFInancialYear.ToFinancialYear(DateTime.Now) + "0001";
            }
            var res = await _pyment.InsertPayments(p, false);

            res = await _invoice.UpdateInvoice(inv, false);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Invoice  " + inv.invid + "is paid sum of Rs. " + pay.amount + " Inserted Sucessfully",
                    name = "Event",
                    isTaxed = false,
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

        [HttpPost]
        public async Task<IActionResult> CancelPayment([FromBody] string id)
        {
           
            var py = await _pyment.GetPaymentsID(id);
            var inv = await _invoice.GetInvoiceByInvoiceID(py.invid,true);
            py.status = "Canceled";
            inv.status = "UnPaid";
            inv.paid = (Convert.ToDouble(inv.paid)-Convert.ToDouble(py.amt)).ToString();
            inv.balance = (Convert.ToDouble(inv.balance) + Convert.ToDouble(py.amt)).ToString();
            var res =await _invoice.UpdateInvoice(inv,true);
            res =await _pyment.UpdatePayments(py,true);
            if (res.Contains("successfull"))
            {
                var result = new { status = "Payment Canceled!!!!" };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Payment  " + py.pid + " Canceled Sucessfully",
                    name = "Event",
                    isTaxed=true,
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
        [HttpPost]
        public async Task<IActionResult> CancelNonTaxPayment([FromBody] string id)
        {

            var py = await _pyment.GetPaymentsID(id);
            var inv = await _invoice.GetInvoiceByInvoiceID(py.invid, true);
            py.status = "Canceled";
            inv.status = "UnPaid";
            inv.paid = (Convert.ToDouble(inv.paid) - Convert.ToDouble(py.amt)).ToString();
            inv.balance = (Convert.ToDouble(inv.balance) + Convert.ToDouble(py.amt)).ToString();
            var res = await _invoice.UpdateInvoice(inv, false);
            res = await _pyment.UpdatePayments(py, false);
            if (res.Contains("successfull"))
            {
                var result = new { status = "Payment Canceled!!!!" };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Payment  " + py.pid + " Canceled Sucessfully",
                    name = "Event",
                    isTaxed=false,
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
