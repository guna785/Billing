using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.BLService;
using BL.SchemaModel;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Authorize]
    public class CompanyprofileController : Controller
    {
        private readonly ICompanyProfileRepository _repo;
        private readonly ILogRepository _log;
        public CompanyprofileController(ICompanyProfileRepository repo, ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> updateCompanyprofile([FromBody] CompanyProfileEdit company)
        {
            var cmp = await _repo.GetCompanyProfile();
            if (cmp.Count() > 0)
            {
                var c = new companyprofile()
                {
                    Id = company.Id,
                    address = company.address,
                    bankaccno = company.bankaccno,
                    bankbranch = company.bankbranch,
                    bankname = company.bankname,
                    email = company.email,
                    gst = company.gst,
                    hsn = company.hsn,
                    ifsc = company.ifsc,
                    name = company.name,
                    pan = company.pan,
                    phone = company.phone,
                    state = company.state,
                    tin = company.state,
                    web = company.web


                };
                if (!string.IsNullOrEmpty(company.photo))
                {
                    c.photo = Convert.FromBase64String(company.photo);
                }
                else
                {
                    c.photo = cmp.ToList()[0].photo;
                }
                var res = await _repo.UpdateCompanyProfile(c);
                if (res.Contains("successfull"))
                {
                    var result = new { status = res };
                    var l = new log()
                    {
                        cdate = DateTime.Now,
                        message = "Company Profile Updated Sucessfully",
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
                var c = new companyprofile()
                {
                    address = company.address,
                    bankaccno = company.bankaccno,
                    bankbranch = company.bankbranch,
                    bankname = company.bankname,
                    email = company.email,
                    gst = company.gst,
                    hsn = company.hsn,
                    ifsc = company.ifsc,
                    name = company.name,
                    pan = company.pan,
                    phone = company.phone,
                    state = company.state,
                    tin = company.state,
                    web = company.web


                };
                if (!string.IsNullOrEmpty(company.photo))
                {
                    c.photo = Convert.FromBase64String(company.photo);
                }
                else
                {
                    c.photo = null;
                }
                var res = await _repo.InsertCompanyProfile(c);
                if (res.Contains("successfull"))
                {
                    var result = new { status = res };
                    var l = new log()
                    {
                        cdate = DateTime.Now,
                        message = "Company Profile Inserted Sucessfully",
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
}
