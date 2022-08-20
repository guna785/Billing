using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Billing.Models;
using BL.SchemaModel;
using SchemaGenerator;
using DAL.Models;
using BL.SchemaEditBuilder;
using BL.BLService;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using OfficeOpenXml;
using Billing.Helper;
using System.Data;
using System.Globalization;

namespace Billing.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EditBuilder _builder;
        private string schema;
        private readonly ISuplierRepository _suplier;
        private readonly IClientRepostory _client;
        private readonly IPurchaseRepository _purchase;
        private readonly ISalesRepository _sales;
        private readonly IInvoiceRepository _invoice;
        private readonly IPymentRepositroy _pyment;
        private readonly ICompanyProfileRepository _company;
        private readonly IUserRepository _user;
        private readonly IStocksRepository _stock;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public HomeController(ILogger<HomeController> logger, EditBuilder builder, ISuplierRepository suplier, IClientRepostory client,
                        IPurchaseRepository purchase, ISalesRepository sales, IInvoiceRepository invoice,IPymentRepositroy pyment,
                        ICompanyProfileRepository company, IUserRepository user, IStocksRepository stock)
        {
            _logger = logger;
            _builder = builder;
            _suplier = suplier;
            _client = client;
            _purchase = purchase;
            _sales = sales;
            _invoice = invoice;
            _pyment = pyment;
            _company = company;
            _user = user;
            _stock = stock;
        }

        public async Task<IActionResult> Index()
        {
            var p = await _purchase.GetPurchase();
            var s = await _sales.GetSales();
            var cl = await _client.GetClient();
            var sup =await _suplier.GetSuplier();
           
            ViewBag.newsales =( s.Where(x => x.cdate.Date> DateTime.Now.Date && x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble( x.tamt)).ToList()).Sum();
            ViewBag.totalsales = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.newpurchase = (s.Where(x => x.cdate > DateTime.Now.Date && x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.totalpuchase = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.clients = cl.Count();
            ViewBag.supliers = sup.Count();
            ViewBag.csls = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.isTaxed && x.status=="Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.sprs = (p.Where(x => x.cdate.Month == DateTime.Now.Month && x.isTaxed && x.status == "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            return View();
        }

        public async Task<IActionResult> NonTaxDashBoard()
        {
            var p = await _purchase.GetPurchase();
            var s = await _sales.GetSales();
            var cl = await _client.GetClient();
            var sup = await _suplier.GetSuplier();

            ViewBag.newsales = (s.Where(x => x.cdate.Date > DateTime.Now.Date && !x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.totalsales = (s.Where(x => x.cdate.Month == DateTime.Now.Month && !x.isTaxed && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.newpurchase = (s.Where(x => x.cdate > DateTime.Now.Date  && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.totalpuchase = (s.Where(x => x.cdate.Month == DateTime.Now.Month  && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.clients = cl.Count();
            ViewBag.supliers = sup.Count();
            ViewBag.csls = (s.Where(x => x.cdate.Month == DateTime.Now.Month && !x.isTaxed && x.status == "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.sprs = (p.Where(x => x.cdate.Month == DateTime.Now.Month  && x.status == "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            return View();
        }


        public IActionResult Stock()
        {
            return View();
        }
        public IActionResult UntaxedStock()
        {
            return View();
            
        }
        public IActionResult Caytogory()
        {
            return View();
        }
        public IActionResult ProductType()
        {
            return View();
        }
        public IActionResult PCompany()
        {
            return View();
        }
        public IActionResult Employee()
        {
            return View();
        }
        public IActionResult Suplier()
        {
            return View();
        }
        public IActionResult Client()
        {
            return View();
        }

        public IActionResult Invoices()
        {
            return View();
        }
        public IActionResult NonTaxInvoices()
        {
            return View();
        }
        public IActionResult Payments()
        {
            return View();
        }
        public IActionResult NonTaxPayments()
        {
            return View();
        }
        public async Task<IActionResult> Purchase()
        {
            ViewBag.sup = await _suplier.GetSuplier();
            return View();
        }
        public IActionResult ViewPurchase()
        {
            return View();
        }
        public async Task<IActionResult> Sales()
        {
            ViewBag.cus = await _client.GetClient();
            return View();
        }
        public async Task<IActionResult> NonTaxSales()
        {
            ViewBag.cus = await _client.GetClient();
            return View();
        }
        public IActionResult ViewSales()
        {
            return View();
        }
        public IActionResult ViewNonTaxSales()
        {
            return View();
        }
        public IActionResult Logs()
        {
            return View();
        }

        public async Task<IActionResult> bill(string id)
        {
            var inv = await _invoice.GetInvoiceID(id);
            var sls = await _sales.GetSalesBySalesID(inv.sid,true);
            var clt = await _client.GetClientByPhone(sls.clid);
            var prof = await _company.GetCompanyProfile();
            var bill = new bill()
            {
                inv = inv,
                sls = sls,
                clt = clt,
                prof = prof.FirstOrDefault()
            };

            return new ViewAsPdf("bill",  bill, null);
        }
        public async Task<IActionResult> reciept(string id)
        {
            var py = await _pyment.GetPaymentsID(id);
            var sls = await _sales.GetSalesBySalesID(py.sid,true);
            var clt = await _client.GetClientByPhone(sls.clid);
            var prof = await _company.GetCompanyProfile();
            var voucher = new reciept()
            {
                py = py,
                sls = sls,
                clt = clt,
                prof = prof.FirstOrDefault()
            };
            return new ViewAsPdf("reciept", voucher,null);
        }
        public IActionResult Reporting()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReportGet()
        {
            var rdate = HttpContext.Request.Form["rdate"].ToString();
            var frmDate = DateTime.ParseExact(rdate.Split('-')[0].Trim(), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var toDate = DateTime.ParseExact(rdate.Split('-')[1].Trim(), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var rtype = HttpContext.Request.Form["rtype"];
            byte[] data = null;
            if (rtype == "Active Sales")
            {
                var rep = await _sales.GetSales();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x=>x.status=="active" && x.cdate>=frmDate && x.cdate<=toDate).ToDataTable());
            }
            else if (rtype == "Active purchase")
            {
                var rep = await _purchase.GetPurchase();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.status == "active" && x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "Canceled Sales")
            {
                var rep = await _sales.GetSales();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.status == "Canceled" && x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "Canceled purchase")
            {
                var rep = await _purchase.GetPurchase();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.status == "Canceled" && x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "All Sales")
            {
                var rep = await _sales.GetSales();

                data = ExcelHelper.CreateExcelPackage(rep.Where( x=>x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "All purchase")
            {
                var rep = await _purchase.GetPurchase();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x=>x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "All Invoices")
            {
                var rep = await _invoice.GetInvoice();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "Paid Invoices")
            {
                var rep = await _invoice.GetInvoice();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x=>x.status=="Paid" && x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "UnPaid Invoices")
            {
                var rep = await _invoice.GetInvoice();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.status == "UnPaid" && x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "All Payments")
            {
                var rep = await _pyment.GetPayments();

                data = ExcelHelper.CreateExcelPackage(rep.Where(x => x.cdate >= frmDate && x.cdate <= toDate).ToDataTable());
            }
            else if (rtype == "Clients")
            {
                var rep = await _client.GetClient();

                data = ExcelHelper.CreateExcelPackage(rep.ToDataTable());
            }
            else if (rtype == "Supliers")
            {
                var rep = await _suplier.GetSuplier();

                data = ExcelHelper.CreateExcelPackage(rep.ToDataTable());
            }
            else if (rtype == "Employees")
            {
                var rep = await _user.GetUser();

                data = ExcelHelper.CreateExcelPackage(rep.ToDataTable());
            }
            else if (rtype == "All Stock")
            {
                var rep = await _stock.GetStock();

                data = ExcelHelper.CreateExcelPackage(rep.ToDataTable());
            }
            
            return File(data, XlsxContentType, "report.xlsx"); 
        }
        public IActionResult companyProfile()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> PopUpModalShow(string ID)
        {
            if (ID.Contains("AddStock"))
            {
                schema = await GSgenerator.GenerateSchema<AddStock>();
                ViewBag.modalTitle = "Add Stock";
            }
            else if (ID.Contains("EditStock"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditStock>(objId);
                data.qty = "0";
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                schema = await GSgenerator.GenerateSchema<EditStock>();
                ViewBag.modalTitle = "Edit Stock";
            }
            else if (ID.Contains("addEmployee"))
            {
                schema = await GSgenerator.GenerateSchema<AddEmployee>();
                ViewBag.modalTitle = "Add Employee";
            }
            else if (ID.Contains("EditEmployee"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditEmployee>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditEmployee>();
                ViewBag.modalTitle = "Edit Employee";
            }
            else if (ID.Contains("addCaytogory"))
            {
                schema = await GSgenerator.GenerateSchema<AddCategory>();
                ViewBag.modalTitle = "Add Caytogory";
            }
            else if (ID.Contains("EditCaytogory"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditCategory>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditCategory>();
                ViewBag.modalTitle = "Edit Caytogory";
            }
            else if (ID.Contains("addProductType"))
            {
                schema = await GSgenerator.GenerateSchema<AddProductType>();
                ViewBag.modalTitle = "Add ProductType";
            }
            else if (ID.Contains("EditProductType"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditProductType>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditProductType>();
                ViewBag.modalTitle = "Edit ProductType";
            }
            else if (ID.Contains("addPCompany"))
            {
                schema = await GSgenerator.GenerateSchema<PCompany>();
                ViewBag.modalTitle = "Add Product Company";
            }
            else if (ID.Contains("EditPCompany"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditPCompany>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditPCompany>();
                ViewBag.modalTitle = "Edit Product Company";
            }
            else if (ID.Contains("addSuplier"))
            {
                schema = await GSgenerator.GenerateSchema<AddSuplier>();
                ViewBag.modalTitle = "Add Suplier";
            }
            else if (ID.Contains("EditSuplier"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditSuplier>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditSuplier>();
                ViewBag.modalTitle = "Edit Suplier";
            }
            else if (ID.Contains("addClient"))
            {
                schema = await GSgenerator.GenerateSchema<AddClient>();
                ViewBag.modalTitle = "Add Client";
            }
            else if (ID.Contains("EditClient"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditClient>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditClient>();
                ViewBag.modalTitle = "Edit Client";
            }
            else if (ID.Contains("PayInvoices"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<PayInvoice>(objId);
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<PayInvoice>();
                ViewBag.modalTitle = "Pay Invoice";
            }
            else if (ID.Contains("CompanyProfile"))
            {
                
                var data = await _builder.ReturnObjectData<CompanyProfileEdit>("profile");
                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<CompanyProfileEdit>();
                ViewBag.modalTitle = "Company Profile";
            }
            ViewBag.schema = schema;

            return View();

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
