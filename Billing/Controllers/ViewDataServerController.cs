using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billing.Models;
using BL.BLService;
using BL.DataTableModel;
using BL.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ViewDataServerController : Controller
    {
        IStocksRepository _stock;
        IUserRepository _user;
        ICategoryRepository _category;
        IProductTypeRepository _productType;
        IProductCompanyRepository _company;
        ISuplierRepository _suplier;
        IClientRepostory _client;
        IInvoiceRepository _invoice;
        IPymentRepositroy _pyment;
        ILogRepository _logs;
        IPurchaseRepository _purchase;
        ISalesRepository _sales;
        public ViewDataServerController(IStocksRepository stock,IUserRepository user,ICategoryRepository category, IProductTypeRepository productType,
                               IProductCompanyRepository company,ISuplierRepository suplier,IClientRepostory client,IInvoiceRepository invoice,
                                IPymentRepositroy pyment,ILogRepository logs,IPurchaseRepository purchase,ISalesRepository sales)
        {
            _stock = stock;
            _user = user;
            _category = category;
            _productType = productType;
            _company = company;
            _suplier = suplier;
            _client = client;
            _invoice = invoice;
            _pyment = pyment;
            _logs = logs;
            _purchase = purchase;
            _sales = sales;
        }
        [HttpPost]
        public async Task<IActionResult> LoadStockTables([FromBody]DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _stock.GetStock();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.cmname != null && r.cmname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.company != null && r.company.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.mrcode != null && r.mrcode.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.mrp != null && r.mrp.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _stock.GetStock();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadEmployeeTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _user.GetUser();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.uname != null && r.uname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.email != null && r.email.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.phone != null && r.phone.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.address != null && r.address.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.role != null && r.role.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _user.GetUser();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadCaytogoryTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _category.GetCatagory();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _category.GetCatagory();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadProductTypeTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _productType.GetProductType();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _productType.GetProductType();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadPCompanyTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _company.GetProductCompany();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _company.GetProductCompany();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadSuplierTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _suplier.GetSuplier();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.cpname != null && r.cpname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.email != null && r.email.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.cphone != null && r.cphone.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.address != null && r.address.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.gst != null && r.gst.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.pan != null && r.pan.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _suplier.GetSuplier();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadClientTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _client.GetClient();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.phone != null && r.phone.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.email != null && r.email.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.gender != null && r.gender.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.address != null && r.address.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.gst != null && r.gst.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.pan != null && r.pan.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _client.GetClient();
            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadInvoicesTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _invoice.GetInvoice();
            var result = resu.Where(x => x.isTaxed && x.status != "Canceled").ToList();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.invid != null && r.invid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.paid != null && r.paid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.amt != null && r.amt.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.balance != null && r.balance.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()) )
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _invoice.GetInvoice();
            var totalResultsCount = cntdb.Where(x => x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadNonTaxInvoicesTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _invoice.GetInvoice();
            var result = resu.Where(x => !x.isTaxed && x.status != "Canceled").ToList();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.invid != null && r.invid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.paid != null && r.paid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.amt != null && r.amt.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.balance != null && r.balance.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _invoice.GetInvoice();
            var totalResultsCount = cntdb.Where(x => !x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadPaymentsTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _pyment.GetPayments();
            var result = resu.Where(x => x.isTaxed && x.status != "Canceled").ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.invid != null && r.invid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.pid != null && r.pid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.amt != null && r.amt.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.userid != null && r.userid.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _pyment.GetPayments();
            var totalResultsCount = cntdb.Where(x => x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadNonTaxPaymentsTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _pyment.GetPayments();
            var result = resu.Where(x => !x.isTaxed && x.status != "Canceled").ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.invid != null && r.invid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.pid != null && r.pid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.amt != null && r.amt.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.userid != null && r.userid.ToUpper().Contains(searchBy.ToUpper()) ||
                                            r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _pyment.GetPayments();
            var totalResultsCount = cntdb.Where(x => !x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadLogsTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var res = await _logs.GetLogs();
            var result = res.Where(x => x.isTaxed).ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.message != null && r.message.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.uid != null && r.uid.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _logs.GetLogs();
            var totalResultsCount = cntdb.Where(x => x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        public IActionResult GetDataFromSession([FromBody] DtParameters dtParameters)
        {
            if (HttpContext.Session.GetString("sessionStock") != null)
            {
                var searchBy = dtParameters.Search?.Value;

                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                else
                {
                    // if we have an empty search then just order the results by Id ascending
                    orderCriteria = "Id";
                    orderAscendingDirection = true;
                }

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));

                if (!string.IsNullOrEmpty(searchBy))
                {
                    result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.cmname != null && r.cmname.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.company != null && r.company.ToUpper().Contains(searchBy.ToUpper()))
                        .ToList();
                }

                result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                var filteredResultsCount = result.Count();
                var cntdb = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PurchaseModel>>(HttpContext.Session.GetString("sessionStock"));
                var totalResultsCount = cntdb.Count();

                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = result
                        .Skip(dtParameters.Start)
                        .Take(dtParameters.Length)
                        .ToList()
                });
            }
            else
            {
                return Json(new
                {
                    draw = 0,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<PurchaseModel>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadPurchaseTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _purchase.GetPurchase();
            var result = resu.Where(x => x.isTaxed && x.status!= "Canceled").ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.invid != null && r.invid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.suplier != null && r.suplier.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _purchase.GetPurchase();
            var totalResultsCount = cntdb.Where(x => x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadSalesTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _sales.GetSales();
            var result = resu.Where(x => x.isTaxed && x.status != "Canceled").ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.gst != null && r.gst.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _sales.GetSales();
            var totalResultsCount = cntdb.Where(x => x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> LoadNonTaxSalesTables([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var resu = await _sales.GetSales();
            var result = resu.Where(x => !x.isTaxed && x.status != "Canceled").ToList();
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.sid != null && r.sid.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.gst != null && r.gst.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.status != null && r.status.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = await _sales.GetSales();
            var totalResultsCount = cntdb.Where(x => !x.isTaxed).ToList().Count();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }
    }
}