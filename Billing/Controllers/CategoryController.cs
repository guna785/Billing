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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;
        private readonly ILogRepository _log;
        public CategoryController(ICategoryRepository repo, ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> CategoryPost([FromBody] AddCategory value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new category()
            {
               
                name = value.name,
               
                status = "active",
                 cdate=DateTime.Now
            };
            var res = await _repo.InsertCatagory(stk);
            if (res.Contains("successfull"))
            {

                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                     message="Category Added Sucessfully",
                      name="Event",
                       uid=HttpContext.User.Identity.Name                
                };
                res =await _log.InsertLogs(l);

                return Ok(result);
            }
            else
            {
                return BadRequest(res);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CategoryEditPost([FromBody] EditCategory value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new category()
            {
                Id = value.Id,
                cdate = DateTime.Now,               
                name = value.name,
                status = "active"
            };
            
            var res = await _repo.UpdateCatagory(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Category Name "+value.name+" Updated Sucessfully",
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
        public async Task<IActionResult> DeleteCategory([FromBody] string id)
        {
            var res = await _repo.DeleteCatagory(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Category Id "+id+" Deleted Sucessfully",
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
        public async Task<IActionResult> GetCategory()
        {
            return Ok(await _repo.GetCatagory());
        }
    }
}
