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
    public class ClientController : Controller
    {
        private readonly IClientRepostory _repo;
        private readonly ILogRepository _log;
        public ClientController(IClientRepostory repo, ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> ClientPost([FromBody] AddClient value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new client()
            {
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                email = value.email,
                phone = value.phone,
                remarks = "none",
                gender = value.gender,
                gst = value.gst,
                pan = value.pan,
                state = value.state,
                status = "active"

            };
            var res = await _repo.InsertClient(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Client Name "+value.name+" Inserted Sucessfully",
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
        public async Task<IActionResult> ClientEditPost([FromBody] EditClient value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new client()
            {
                Id = value.Id,
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                email = value.email,
                phone = value.phone,
                remarks = "none",
                gender = value.gender,
                gst = value.gst,
                pan = value.pan,
                state = value.state,
                status = "active"
            };
            
            var res = await _repo.UpdateClient(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Client Name " + value.name + " Updated Sucessfully",
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
        public async Task<IActionResult> DeleteClient([FromBody] string id)
        {
            var res = await _repo.DeleteClient(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Client ID " + id + " Deleted Sucessfully",
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
        public async Task<IActionResult> GetClient(string ID)
        {
            return Ok(await _repo.GetClientByPhone(ID));
        }
    }
}
