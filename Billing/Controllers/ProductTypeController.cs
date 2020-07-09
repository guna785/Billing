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
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _repo;

        private readonly ILogRepository _log;
        public ProductTypeController(IProductTypeRepository repo, ILogRepository log)
        {
            _repo = repo;
            _log = log;
        }
        [HttpPost]
        public async Task<IActionResult> ProductTypePost([FromBody] AddProductType value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new producttype()
            {
                cdate = DateTime.Now,
                name = value.name,
                status = "active",
            };
            var res = await _repo.InsertProductType(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Product Type " + value.name + " Inserted Sucessfully",
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
        public async Task<IActionResult> ProductTypeEditPost([FromBody] EditProductType value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Request Not Completed");
            }
            var stk = new producttype()
            {
                Id = value.Id,
               
                cdate = DateTime.Now,
               
                name = value.name,
               
                status = "active",
            };
            
            var res = await _repo.UpdateProductType(stk);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Product Type " + value.name + " Updated Sucessfully",
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
        public async Task<IActionResult> DeleteProductType([FromBody] string id)
        {
            var res = await _repo.DeleteProductType(id);
            if (res.Contains("successfull"))
            {
                var result = new { status = res };
                var l = new log()
                {
                    cdate = DateTime.Now,
                    message = "Product Type " + id + " Deleted Sucessfully",
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
        public async Task<IActionResult> GetProductType()
        {
            return Ok(await _repo.GetProductType());
        }
    }
}
