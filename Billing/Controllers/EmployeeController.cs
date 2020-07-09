using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.SchemaModel;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using BL.BLService;

namespace Billing.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUserRepository _repo;

        private readonly ILogRepository _log;
        public EmployeeController(IUserRepository repo, ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> EmployeePost([FromBody] AddEmployee value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new user()
            {
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                dob = Convert.ToDateTime(value.dob),
                email = value.email,
                joindate = Convert.ToDateTime(value.joindate),
                phone = value.phone,
                role = value.role,
                uname = value.uname,
                remarks = value.remarks,
                password = value.password
            };
            var res = await _repo.InsertUser(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Employee " + value.name + " Inserted Sucessfully",
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
        public async Task<IActionResult> EmployeeEditPost([FromBody] EditEmployee value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new user()
            {
                Id = value.Id,
                cdate = DateTime.Now,
                name = value.name,
                address = value.address,
                dob = Convert.ToDateTime(value.dob),
                email = value.email,
                joindate = Convert.ToDateTime(value.joindate),
                phone = value.phone,
                role = value.role,
                uname = value.uname,
                remarks = value.remarks,
            };
            if (!string.IsNullOrEmpty(value.password))
            {
                stk.password = value.password;
            }
            var res = await _repo.UpdateUser(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Employee " + value.name + " Updated Sucessfully",
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
        public async Task<IActionResult> DeleteEmployee([FromBody] string id)
        {
            var res = await _repo.DeleteUser(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Employee " + id + " Deleted Sucessfully",
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
