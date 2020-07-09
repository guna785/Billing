using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.BLService;
using BL.SchemaModel;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class SuplierController : Controller
    {
        private readonly ISuplierRepository _repo;
        private readonly ILogRepository _log;
        public SuplierController(ISuplierRepository repo,ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> SuplierPost([FromBody] AddSuplier value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new suplier()
            {
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                email = value.email,
                cphone = value.cpname,
                cpname = value.cpname,
                gst = value.gst,
                pan = value.pan,
                status = "active"
            };
            var res = await _repo.InsertSuplier(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Suplier " + value.name + " Inserted Sucessfully",
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
        public async Task<IActionResult> SuplierEditPost([FromBody] EditSuplier value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new suplier()
            {
                Id = value.Id,
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                email = value.email,
                cphone = value.cpname,
                cpname = value.cpname,
                gst = value.gst,
                pan = value.pan,
                status = "active"
            };

            var res = await _repo.UpdateSuplier(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Suplier " + value.name + " Updated Sucessfully",
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
        public async Task<IActionResult> DeleteSuplier([FromBody] string id)
        {
            var res = await _repo.DeleteSuplier(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Suplier " + id + " Deleted Sucessfully",
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
    }
}
