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
        public HomeController(ILogger<HomeController> logger, EditBuilder builder, ISuplierRepository suplier, IClientRepostory client,
                        IPurchaseRepository purchase, ISalesRepository sales, IInvoiceRepository invoice,IPymentRepositroy pyment,
                        ICompanyProfileRepository company)
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
        }

        public async Task<IActionResult> Index()
        {
            var p = await _purchase.GetPurchase();
            var s = await _sales.GetSales();
            var cl = await _client.GetClient();
            var sup =await _suplier.GetSuplier();
           
            ViewBag.newsales =( s.Where(x => x.cdate.Date> DateTime.Now.Date && x.status != "Canceled").Select(x => Convert.ToDouble( x.tamt)).ToList()).Sum();
            ViewBag.totalsales = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.newpurchase = (s.Where(x => x.cdate > DateTime.Now.Date && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.totalpuchase = (s.Where(x => x.cdate.Month == DateTime.Now.Month  && x.status != "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.clients = cl.Count();
            ViewBag.supliers = sup.Count();
            ViewBag.csls = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.status=="Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            ViewBag.sprs = (s.Where(x => x.cdate.Month == DateTime.Now.Month && x.status == "Canceled").Select(x => Convert.ToDouble(x.tamt)).ToList()).Sum();
            return View();
        }

        public IActionResult Stock()
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
        public IActionResult Payments()
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
        public IActionResult ViewSales()
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
            var sls = await _sales.GetSalesBySalesID(inv.sid);
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
            var sls = await _sales.GetSalesBySalesID(py.sid);
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
