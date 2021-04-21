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
                    t.label = item.photo==null? "<img width='50' height='50' src='data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABVAAD/4QRaaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjAtYzA2MSA2NC4xNDA5NDksIDIwMTAvMTIvMDctMTA6NTc6MDEgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcFJpZ2h0cz0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3JpZ2h0cy8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcFJpZ2h0czpXZWJTdGF0ZW1lbnQ9Imh0dHA6Ly93d3cuZ2V0dHlpbWFnZXMuY29tIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjY1QjdCOTRFNTU1MTExRTM4NjM3RjE1MUE5NDY3QjZGIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjY1QjdCOTRENTU1MTExRTM4NjM3RjE1MUE5NDY3QjZGIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzUgTWFjaW50b3NoIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9Ijk2NUIyNjE2QzY1NzZFRUFBOUJGODdFNkE1MDMzRDI0IiBzdFJlZjpkb2N1bWVudElEPSI5NjVCMjYxNkM2NTc2RUVBQTlCRjg3RTZBNTAzM0QyNCIvPiA8ZGM6Y3JlYXRvcj4gPHJkZjpTZXE+IDxyZGY6bGk+RGlyayBFcmNrZW48L3JkZjpsaT4gPC9yZGY6U2VxPiA8L2RjOmNyZWF0b3I+IDxkYzp0aXRsZT4gPHJkZjpBbHQ+IDxyZGY6bGkgeG1sOmxhbmc9IngtZGVmYXVsdCI+MTAxODYxNjU4PC9yZGY6bGk+IDwvcmRmOkFsdD4gPC9kYzp0aXRsZT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7QBIUGhvdG9zaG9wIDMuMAA4QklNBAQAAAAAAA8cAVoAAxslRxwCAAACAAIAOEJJTQQlAAAAAAAQ/OEfici3yXgvNGI0B1h36//uAA5BZG9iZQBkwAAAAAH/2wCEAAIBAQEBAQIBAQIDAgECAwMCAgICAwMDAwMDAwMFAwQEBAQDBQUFBgYGBQUHBwgIBwcKCgoKCgwMDAwMDAwMDAwBAgICBAMEBwUFBwoIBwgKDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIARkB1AMBEQACEQEDEQH/xADPAAEAAQQDAQEAAAAAAAAAAAAABwUGCAkBAwQCCgEBAAIDAQEBAQAAAAAAAAAAAAUGAwQHAggBCRAAAQIFAQUEBgYFCAcHBAMAAQIDABEEBQYHITFBEghRYRMJcYGRoSIUscEyQlIjYqIzJBXRcoKSsqOzCsJTYzQlFibw4UNzZDUX8YNUpHSEGBEAAgEDAQQHBQUFBwQBBAMAAAECEQMEBSExEgZBUWFxgTIHkbHBIhOh0XIjM+FCUhQk8GKCsmNzNMIVJQhD8ZLiF1ODFv/aAAwDAQACEQMRAD8A3+QAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACY7YATEAcFSQOaewcYAsfUnqR0I0lbU7qFllutjqN9O5UJcfJ7AyzzOE+hMal/Os2vNJIntM5X1HUWv5exOfbSi9roi3tFetTp16gMhqcW03vqXcgpieSkrWnKRyoQN7lOl4ArHo290YcXVbGRJxhLaiR17kPVNHtq5kWqQfSmml303EsoPwie+JEqBzACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAccyd0AUPM9TtOtO6FVwzq+UNppEiZVX1LTPsCyCfUIw3ciFvzSSJDB0rJzZcNi3Kb7E2QJqV5q3Slg/iU+PVtVk1ybmAi0U60slQ4eNU8ifWAYiMjmDHt7ItyfYjoelej+sZdHcjG0v7z2+xV+Bj7qV5zGqV3DlNpVjFFZWST4dVdHF1r4HbyoCG5+qIa/zPcl5Ipd+06TpXodh2qPJvSuPqj8q+LMfdSesfqc1Z8RvM8yr10Dk50dC58lTy7PDpeXZ6TEPe1K/d8033bjo+l8k6Vp1HZx4Jrpa4n7XUjVxxx51TrpUt5U1KWokqM+MzMmNBlpSpsR2W+419orWrpa310txYWHGKinWptxtxJmlSVpIII7o9KTTqjxdtQuxcZJNNbU9q8UZwdG/mnZHSPUWmvUa29caJxSKWjymmbJqEFSghAqm0j8zaZc6dvaDFo03mFxahe29v3nCOdPRyN1SyNP8AldG3be7t4W93c9hsBTuEXM+bTmAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAEygD58QTlIzgCydTOpPQbRxtStS8rt1peTP93qKhBfMuAab5lk+qNS9nWbXmkkTul8sahqP6Fic11pbPbuITynzgejLHX1MUlfcrny7PEoba9yGXYXy2TEfLX8ddLfgXC16SazJfNGMe+S+FSraJeal0fa55hT4FZb29aMqqzy0bGQMGiRUOEyDbbqlFsrPBJIJ4RsY+r2LzonR9pE6x6e6np1t3JwUore4utPDeYoeZ71v9QGm/VZfNFMZyWqtOm1LSWx1lm18jDoVVUKH3St1A8RQKlT37IgNczL0bzhGVFs2eB1n0v5a029p0Mi/ZjO45S2y27nTduMWKvIbhlbxvN0rnrjUuzUqpq33KhxRPat0kxV5NvfvO7WLduEKW0lHsSXuOvsPpjwZjlttx08rSSpXYkT+iFT0ot7keynsFyfkSjkSds1/yCPDmkZo4sn2HtYxVsAGqcKv0UCXv3x4dw2I4cVvZ7aa0W5hfIy0FPHYmc1qPq2x4c2zYjZjHoJO0e6bNdNQcgt1bieK179obqad1yqWwpinS2l5KlEuPcqdw7Y38PT796S4YulUVXmDnLTNPtTjeyIRk4tJVq60fQqs25iUhLdHVz4FEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAEyE4AtDWDWvA9FsbVf8ANqoNlYIpaNqSqipWNvK2jj3k7BGpl5tvHjxSZP8AL3LWXrN76WPGtN7flj3v4GAXU318dQGp79RY8TfXimCL5kIYtqv3t1B2fnVIkoE9iJCKJqOvX7zpH5Y9m/xZ9WcnekmladFTupX7y2ty8qf92O7xdWYr3u31FdUrrK1a36twlS3X1qWtR7SpZKifSYhVcfWdQljxS4UqJdFPgW9crIADNM0/9vVGdTRH3bBQa/B626A/K061kEEFCSJEbiDsIlGRXkjQuYMrm6NfA+cqxjWjLLoL7k6qq816WWaP5m4Ph2o8CmbDTKOdxXMQhACRPhHueXGbq3tNDH0G5jR4bVukat0VFte9nipKLL8IvtJbrxTP292sDbqWaptSA6w6oAOICthSeBGyE1VbT1j3XG5wp7U6NV3PqfaS7TWC2symjnc/TM/cIi3NlzjjRXQfVddrHZW/36oZpm+xakoPs3x+qLZ+zuwtra0j5suQ2jImHKqzu+NTtK8NSwCBzSnL4hH5KDjvFnIhdTcXWjMqfL26RdLepGiv+Q6lmrcZstTT0rFJRvmnbcDrRcJcUgcx3SkCIsWg6Tay1J3K7Kbjjnqzz9ncvSs28XhTuRbbaq1R02J7DN/T7pg0B0vbQMLxW30tSgACpcYS+/s4+I/zKn64uuPpmPY8kEj5o1bnbVtTb+vkTafQnRexURfQZSkcqdiBsAAkAO6Ub5VntdT7G6AEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgDouNfTWuiduNaoN0TCFvvOGckttpK1HZ2AR+N0VT1CDnJRW9uiMUdRvOG6cMbS4xp5R3DKqxPMhD1M2ilpCoEja7UHnlMcEGK9kcyWoVUE5NeB1/SPRfUslKV6cLUX28T9i2faSd0udcWjvVHbU0uPVAteoLaOarx24LQmpTLepkzk6jvG3tAjd0/V7WUqLZLqe/wKtzbyDnaDJymuO10Tju8f4X3kzBRIiVKOWXrjrVj+imGO5FdyHbm7Nm3UII533iNmyc+VO9R7I083MjjQ4nve7vLHyvy3e1rJVqGyK2yfQl976DA3UfOMm1VyV/KswqDUXR8kJSSQ2y3wbbTuSkdgijZN2V+XFJ1Z9WaJpdjS7EbNiNIr2t9bfSyzLvjjFS2pC0BSTsIPERpThUsuPmOG1MjjMseoLTUhhtxPiq2lnikdpjQuQUS14mU70ateJQU2+lSoDkCnVSCQRMzPYO+MXEzY4FXcSBhfTJ1BagNJqcPw+51NCqRS+aZTLMju+N7lEbljTsi9tjBsrWpc66TgPhvZNuLXRVN+xF1VHQB1ZtBrx8Te8F1aG1raep3C2FqkVlKFkyAM90bP8A2HL/AICCh6q6A60yVsVdqar4vrM99ROirQTWXSSx6Xal2VupZsVNTU1tuNOfBrqRbCEp5mX0DmEyNoOw9kdAnplq5ZVuS2JU7UfJONzrnYmfczLM/mnNyae2Lq67V/ZmN/mQdKOhXTt0S3/IdNLR8tliKm307d3qHnX6sBypSFgOLOzmG+QiF1DR8fGsNxjtqtr2s6Xyd6h6vrWrQhkXnwNSfDFKK3dhqucddfcLj6ytat5WSSfbFbpQ7e3Xa9pKehyZYvUntqFD9QRH5T+YtGi7LT/F8DZP5PqOXT3NHe250g9lIf5YuHKK/Lud69x85/8AsG/6vF/25e8zETui3nz0cwAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAcKJG70wBY+qnUjojotQOV2pWSUVsS2CpTTryC7IdiAZx7halLcj9oYha1/5hnob0rqXKCzVzl6rW5j93KADLsE/rjOsbraR+8DMf8AJ/8ANVaTU1UprG8TeeZBkkr5pkeox+qzDrPXAeey/wCat06cqUNXjCqgMqMlONn7Pq5oOzDrP36RMmkf+ZY6H87q2Lfmaa3Hqh0hJcqm/wAsE7N4J2euPP8ALp7mefpszO0U6wOnLqFtrdz0nyygurbgBDbL7fiCfApnOcY5WpI/HFoktCgpIIM59kYjyfK1kKlOQgfhR8v1FwXAKFVyzi80VookjmLlwqWmBLuDhBPqjDdvwt7ZSS7zewdMycyXDZtym31Js7cPzbFNQLCxlOE3KnuuOVO1isoXEutL7QFJ49x2x6tXo3I8UXVHjNwb2Hcdu9Bwmt6aoyrRkNUQBRNS1+Dp1f3/AMFtrleylWYx3vI+5m3p6rft/ij7z89VryS5WStcdpFTYUtfO0o/CqayY5lOKlvPuKxflbpQkHBM0q6u5MXXFH36PJ6NSX2nKZam32VJOxaFoII2mNd1tutSWg4ZkXBxTT3p7U+/rNkPl99eWo2p1zVpNrc02/W0VBUXJvJeZKF/L0aAtz5tG4q5dvOn1jjFt0XXJXZfTudCbr2LrPnj1M9LrWnWll4nyqU1H6e/bJ7OHs6KPwIW136rBr1q1W3tLqm8XpVro7JTqPwimSuQXL8TkuYxA5+p/wAzdb6NyOucpcirRNPjbW25Kjm/73V3LcinUN0ZqWwoK3jf28YxxuVJC7juLKBqJnzVgZ/hVsIXeXBtOwhlJGwnvPCMOTfUdi3klpWmu8+KXlX2nR06dMupvVDlrlBi6C1Y2VA3a+VYUaenCtsp71uEbkj1x407TbmZKkd3S3uMfN/OuFy3jqd51k18sF5pdvculmwzQPoh0J0JpGam3WxF2zFAHi3m7IQ89z8S0kjkbE93KJ98dAwNFsYy2Lil1s+SuafUzVNak1K47dp7oQbS8Xvfj7CYEtoQgNoACAJADYAB2RLnPjktpMD8ocygfpif50LvhdCt4H47nake2on9UQ2uP+n8UdH9KlXV4fhl7jTcN8+yKSfUJK+iKf8ApF09tQv3JTEfleYtWifpPv8AuNlnlANEaV5a8fvXZkD+jSJ/li5co/pT717j5t/9gZf12Mv9N/5jL4S4RbT5/EAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQBwqALF136jtJOm/D3811Vu7NvtzSFOIZWtIedIE5ISTOPcLbk9h+pNmmbzFv8AM5ZBU1Fdp30ts/LU3xsfPoM1kfZmXAfo9sbUbcI9rMiga1sh1d6wOrnJUXLL7rX1661YcZt1Iah157mVs5WmipRHCZj9lc2baJewz27UpukVXsW8nXSHyhuoTURpm557cLFgVnfAWXsruIVVcp4/J28PPA9xAMaFzVMe3vmveT2LypqWQqwx5062uH30Jgtfkk9OlsAOc64O1VVL404tita+3PiErrHGifWI0pcwWE9lWS1r051Oe1xhHvkvgfd08oXoxpGlCj1Ry9T0phasUpygn0Cq5pR4fMNrqfsNr/8AWeofx2v/ALv2EWal+Vpidhbcq9L9SGrpKZFLkdmuVpeVLaJOJDzPtVGSHMGO99UYLvpxqcNyhLukv2ESW/G+ovphyRGQ4ncK201dMsLTX2aoWWfhM58zRlLuUIk8fNtXvJJPsqVvUNFy8J/nWpR706e3cbIvLs/zB+e4fcKHTfq8R8/jCyinRk9Og87M9gVVNgnZ2rRu4gxnnbUuxkQ7VdxuLwTU7CdWMFaznAbgzccerKc1NPU0riHUKSpBIIUgkGNOcHHYzFGPzLvNEF61eyG+ZXXO5xW1FzJqX+WpqnVvOJHiqlPxCdg7o5bei5tturPurSrsMa1GEYpRotyp0dhLvTb1Xaq9ON9Rkelty5rK8oKrbRUFTlBVDeQ40COVUtykyUDx4R6w8+7iyrB+Br8x8qYOv2uG/Gr6JKnEu5/DcbNukvrt0o6pqNNpoVmz6mst89ZYKxQ51co+JymXucR+sOIi86dq9vLVN0uo+WOcfTzN0B8cvnsN7Jro7JdT+wnKJYoJbesT3y+kmUP8EWm4q9lGuMV/yS7mb+lKuVaX9+PvR+eIK5pqHGZ9pnHNUfbO4vfQtP8A1JVL7Kcj9dMauX5Sa0RfmPu+4lyh1MvemNqu9XYFeHWXihesLroJCkU9WQHeWXEpHL6Iw4l1226b2mvA3dbwLeVG39RVULkZrvW72MtSzX8DlSDIDv8A5I/JWzdtZRe+N6h11Ez4YPiAD4Zk7CN0I3XEX8K3e27iuaL6T5X1C6r0Gn9kJNxuLhcrKtYJTT06PiddXLgkbu+Qj3h4s8q6oLe/sXWRXMmvWNCwJ5N3ywWxdbexJd7NsGkeleHaM4LRafYPTJp7HQoCeaQ8R5w7VuuqH2lqO0mOp4mLDHgoQWxf2qfCGva7kaxlTyciXFOT8EuhJdCRcwAG6NohxACAEAYiedm94XQ7VtT/AGl6tKf71Sohde/4/ijpfpOq6svwSNPImdgiln06yWtFBy4eo8DUuH3JER2V5i1aL+l4s2ZeUO0E6M5K9+K8S/q0qBF05SX5M/xHzP6/v/yNhdVr/qZlrFrOCiAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAcKlLbAEBde3X1pR0NaW1eXZhVtKyYtqVQUCliZVKQUoAzl3cYzWrXFte49RjU/Nt5gnmT6/9b2oT5uFdVfwardLdDbWCtSlpUqSB4aZmZ4JEbLdFRbDLsPrp88sTN6nL6ZrqRpqzHVqQzWmyFvluPhvth5tVT4k/CCkKCpEFUjuiA1LWvoPgtril27l950blDkOOowWTlT+njvdTbKVOqu7vNiGjnTtpPpHY26XBLfTtSAC3WQkuLIH/iOGa1esy7oqV+/dvutyTfZ0Ha8DEwtPhwYduMV/Fvk++T+BfCaGmppltCWxxVKU/SYxRVDLKc7j2upT7hkFkoSUPPAuj7rXxfRHiWTGO9m3Y0e/e8sWUirzILWUUNIpfDnWQgeyMEtRitxL2eT709sthSa+6X+4tlKW6dtJ2SUFLP8AJGGWp9hIW+S4LfPaWNl2kNVla1Pu1DDTqp/YbKZ7OMoxrUGnVLb2G3PlS24cDakupqpDGpnRVk5C7zhbdOu4oms07KkpQ9xIKVASV6N8WnS+aeFqN6tOvq7zkHN3o3OUZXsJLi38Fdj7uruJo8sLr6z3ozydeG5U9VO6JPvG35HZa3nL9iqHZo+ZYbJn4UzNaRvG0bYvScbsdjqmth875GLOzccZxanF0aexqm8t/XDSvItINRKzFr+tFQ06RX2640pCqavo6n81moYWnYpCkqG70Ry3NxJY1xwkfXmga1Z1TFhetPY1tXU9zQ0aSqoyhVG6eak8Jbimz9kqBAme/bEZk7I1Lfo+27TooZp+WfStf/61shaQEhuluChIAb6cj643eXduXHuZV/WRqOgXkumUP8yNn/GcdLPiktLXx75fQ7Maj8FkuivZRORgyf05dzJPRVXMs/jj70fnoYM2EH9EH27Y5uj7VZfuhI/49WK/2Eva4I1cvcib0Pzy7i6NWKv5LGkOTlN9sfTGvjKsiT1WXDa8UWha75MgBWyNyUKEPayKEgYgVvWoVrm90nk9A2Ro3t9Cw4lXCvWbEvKc0bYsWnl01mubQN3vjyqCgWobUUVKr4uX+e5v9EXflTDUbbuv97d3L9p8vevHMTv5dvBi/ltLil2ylu9i95l5FsOAiAEAIAQBhx54bwa6LkM8XL9ax7PEMQev/oeKOo+kirqr/wBuXwNQsU0+l2S7ouCMMBPF5z6ojcnz+Ba9H/R8WbOPKPaKNBL26fv3l73U7Yi78qKliX4vgfMHr5KuqWl1Wl/mZlZFpOFiAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBAHHMmAHMJTgCO+qLqMwnpe0duuq2aPtt0tG0s0zLigkvPBJKUiZ3cT3RktW+Nn7GNWfmW69+r7Vzr01qrcprF1FZZXqn5eyWxoOLLylq5EcjaZkqJ+yAPfG9TZs3Gdumw2t+Sj5EmFdLGP23qw6wbazd+pKuSius1irkIepcdbX8bayhYIVVyMyTsb3DbtjTuXehbjDKRY/UFklJleuuZZ68oc1xulU4Xl8WmnPBbEzuAQgSik5DTuSl01PonSLcoYlq1t+WK2dvSRPf8AXGnxl5TWMqLtaJ/H/wCH/wB8aF3Ij0bS3YGj3HSUnRFfxvMr7m+OU95vK5OPBU22ppRIKlOQiCyb8uJqp1LSdOtRtKXCq9p7aamW+6lilbUuoUeVKG0lSlE8ABMkxqb2TfEoLekvAlnTPob6ntVGW6yxY27Q2pwAoq74o0LZB2zSl4eIR6ExKY2i5V/dGi7dhRNa9TNF0xuNy+pTXRD534tbPtJnxXyg9QqtsO5pllDRrO9q307tQof0nOQewRM2+U7j800vA53nf+wGJF0s405dsml9iqXBUeT3bEsfu2cPfNy2c9vb5fc5GZ8oKn6m3uIiH/sLPi24ip+N19xghrrk1m6duoO+9OuqCl27J7O+Gae4PIIo62ndQHGH21z+FLiCDI7jMcIr+ZpFyxJrY6HYOXOfsLVbELqrb4uvcn1VXUWDrdpoxdS3q1iLQdyShb/4gwzJSLpbpTWlXLsU4hPxIPqic5Y1l2Z/QueWW6vQ/uZSfVrkWGfYefjr82C+ZL9+PX3r7S99H7a51O9O130VB+a1S03Y/wCZcFqV7Xa/G6hU6qhmZlRYWeZA4DZFt13T/wCZtcUfNHb4dKOJen/M3/asxQm/yrux9j6H8GR7omhQy50LBCksOAgjaJKAM/XHMsryn1nojrd8GZteWIzz9WFtVxRQXBX90kRvctquWu5lP9aXTQZ/jh7zZoBHSj4vLI6mn/lunTOn+KLDdT/+k5Gvl/pS7mS/L6rnWP8Acj70fnzp/wDd2x+in6I5wj7Se8v7QgH+L1yhwZT/AG41cvcib0NfPLuKp1A1YpMOp1/iqkD9UxixF8zNnX7nDZX4vgRjar0vmCUmZOyUSLK3bvVJ+stIKKz01GN6G0A955dvtMQ05bWXyzDhgl2G4LpYxJnB+njDsaZTy+Da6d1Y/wBpUJ+YWfTzLMdX0u19PHhHs/afAvPOoPN1jKuvpuNLuj8q+xEgRvlUOOZPbADnEAOdMAcgg7oAws89J/k6QLa0P/EyG3g+pl1X1RBcwforvOq+kK/8nJ/6cvejUiIpx9JkwaNpKcIan9550n+sIjMrzFt0j9BeJs98pxHhdOlwc3c95qvc02IvfKv/AB3+I+WPXmVdXgv9KPvZlF4nbFmOIn0DPaIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBAHBUAZQPyp4cjyjHcPsz2Q5TWs2+x06St+rq3EtNIAE9qlED1RjuXY248UnRI2sPCu5dxWrMHOb3JKrZihrh5uGluDvPWnSOzv5PdGypIrahXydDzD8PMC6sf0RFbyeZ7UNltcXuO1aH6F52RFTzJqyn+6vml49C9pjZmPnEdYdwrCcXobJbG1qAap0UjtSSSZAczq5nbs3RGvmLIluSRdI+i+lWY/PK5J9baXuRjT5o/Vj1DdWd3xTpdvdUy/lbbba8hTa2zT0/j1BDgZ5ElUikbFE9nfHQ8GM1aXH5nvPnbW7WNbyrkcVflxlRVda06amQfks+Vzp8zm6up7PaZq5YpiD67fhrVQ2lSai7tDkq69aVbJMK+Bv9KZ+6I95F2nyohrq4XQ2V6iZG4zjl3vKCU0NuoqurKu5lhTkz7I0LkuGLfUme8S19W7CC3ykl7WaJNRdUbpmFyqAwotW5bil8qTtVzEkknvnHPL2Q5s+x9N0iGPFV2uhaR7Tv7Y1yYJv06aDOE25I2Ta5vaomIq/tky66eqWI9xkt5aNDTV/VhaUVTaXUN0lwdSFpCgFJpzJW3iDtBiY5cinlxquh+45x6zXZQ0G64ujcoLq3y2o2cpQQqfCOlnxSfcD9OFAndAGo7/MgaKUtLqRp/rVb2Ql67UVbYLi4kbVuUS0P05J4nkdWPVFd1mPDOMuvYdl9ML/ANSzesv91prx/ajDHpR1euNtvKNLMjeU7bagE2lxxRJacSOZTMz90jaOwxU9QxlRTW8+gOVdWlGTxrm2LWyu3wJd0XzJjpu6vcLytlfg41/E0UNQ0PhSq13mdM+0eHKhxRUOzZHROX855eNGT8y2Pw+8+ZvUvlxaRqdy3BUhL5odz6PBl99QeiyNE+rzIrHb2+THq9lV1t4SJJCXXZOJSO5YJ9cUbmXEWPeaW5uq8T6C9J9ZepYMZydZxTi/Dd7UT55W7Rc6qmFb+S116vaECMXLH/KXczx63SpoUv8Ach8TZZHSD40I66vKj5Tpa1DqTs5Mdu6p/wD9JyNXNdLM+5k3y0q6jjr/AFI+9GgFpJSygfoj6I5yj7OktpIGg4/f7gr/AGaB+tONXL3IndC80u5e8+Oqms+UwigIMuatA9jSjDAVZvuPPNE+GxH8XwIgxe6ePd6Ngn4XH2UH0KcAiQnCiZUcW7Wa70ZWthKCkH7CZS9G6II6jJdBua03cbGn1h8H9l/DqLlPd8uiOwYv6ce5H86tZr/OXq7/AKkv8zKw4+lP2jGcjTocuLSD/wBvRAHUq8spMlETgKnKbu0T2iAPQirbXu3iAMJvPcqZdK+PMcXckpgP6NG+qIDmF/kr8X3nWvR5f+Ruf7b96NUHp3xUD6OJj0hEsIpz2uOH9aIzJ85btJ/QXibNvK1qBTdNjs/tLu9Yr2IQIvvK3/Gf4mfKPrvKutRXVaj72ZOU1UFjbuiyHFz0oVw4QB9wAgBACAEAIAQAgBACAEAIAQAgBACAEAJwAgBAHClcvCALX1Y1XxbSHFHcryhwhA/LpqZBHi1D0phtA+k8BGtlZUcePFImtB0HI1fIVmyu99EV1v8AttMCeoXV3OuoG6LqssfKMfST8laGVEUzCeBl95ct6lRQ8/KnkyrLd1H1jyfy7i6DbSsr535pvzP7l2IgXKdP3aXnepAVNjeBvHsiEnacdx0/Gz43VSWx+8p2B47bbZdq/PL+hP8Ay/i9HUX2q8SQSVU6PyU7eJclsib5cxf5nKinujtfgUT1S1f/ALVpNyUXSdz5I+O+ngRV04YpV5Pb8v6mMlPNll1qTbrQtzaoP1q/jUJ/6tqXojqOZlKxblcfQq/sPj/Q9LnqWVax4b7kkvBvazKro81wynpjTUVNndUrTipKRcbMtR8J5Q2F5sfddI+8N/GOZadq963OVyTqpOrX3H1Lzh6e4GfZt4lm2o3YRpGaW1JdfXXft6zOfWjUrEcg6Hsy1TwiqFRY6zHLk+zUCQUFLpVNKQofdUlRkR2xcLmTG9jO5B7HFnzXp2h38DWrWJfjScbsU14711prajRWgkoBO+UUCO4+uGcndHoE7YW2WsStyDuFO2famcRN3zMvGGqWo9yMmvK8ZC+q+iUd6LbcVf3aR9cTnLS/ql3M5Z62SpoMv9yHvNmMdIPjIQAgDX5/mGLMxcOnPCq5aZv09/UlKuwO0SwfoiB17ZCL7Tq/pNty7q/ufE0+lmqsl4pr5QfDW0brdSgjtbUFSitJ1TT6jtvC7c4zW+LTJ06lvBq8Ps+XUxk5zDkcGwgLSl9Ej3KTsiW5MuuM7lro3+xlc9d8KNyzi5S31cfaqozV647YjKKbCtWwkfO11tpvmFjiK63sViZ/0uaM3Olr5Lc+1r4kV6AZjV7KsdHCpeNaMrHlVN8/U+tZ+5aK0+1bYiA5XX9T/hfwL165Sponfdj8TZJHRj45Ia8wu/NY50S6m3F1XKpVgrqRs8S5Ut/LoA9KliNLUpcNifcWbkzHd7VceK/jT9m34GhOlrKihAZqxzMASCtvMJdsUBqp9dcbg6Mk/QNSH37g60qaShoAjvJjRzFuLNoTTcmuwqHUxpNm2e6IXbOcUpzU23DXKa7XpDYUpxNG+58oXkgT+FC1Dm7BGzpNtycmuhfEhee9QhYt2YSdHObS71Gpi9jWRGluVJUuH4W3WnJ/zVhUSNyCaZUcXIpJPqZmww8mpYRUNGYcSlY7JKSCIq8tjZ2mO1Jm2LpP1Ips96a8SyZCwXkUDdBVAnal+jnTrCuw/AD6DHVtHvq7jQa6qew+C/ULSpafrWTaa2Obku6XzL30PdqHrvptpzRrrs0vtHbWUjb80+hKz3BE+Y+oRt3sq3aVZySIHTdDzNQmoWLM5t9Sfv3GL+tfmnY7TNvWXRGkVcLgZt/xe4ILdMg9rTR+JfdzSHdFYz+Z4xqrKq+tnb+UvQ25ekruozUYb+CLrJ976PCveWnoV5gucWutNDqO+u7WuoWVrcXIPtFRmfDIABT+iYj9P5juW3S7tT+zuLfzj6MYmTbU8L8qcVsX7r/F95lbg2s+O5tbm7tjtYmool7fgMlJPEKTsIIi74+TC/Hig6nzBrGi5Ol3nZyIOEl7H3PpL6tWQpfCRPf3xnIshrzF+mRjq00HYxFN/Zx+72etF5t9RW8vyr76GVshh5U5pSQskEbj3RGaphfzNulaNOqLnyLzJ/2XMdxwc1ONHTelVOq69q6TTnqRpnmWlOTv4lmtL8vdGVKQlxs87D6UqkHGXBsWk8CIpF6zK1LhkqM+ptO1Sxn21ctSqvtXY10MkXSRMsFpTwKnCO/4zERkecv+k/oR8TZJ5bNQabpzbSnYF3KtVMf0BF+5W/4v+JnyT65Srrn/APXH4mTVqqFLb2mcWM46VhlU2weMAdw2icAIAQAgBACAEAIAQAgBACAEAcEygD5W8ltBccIS2napRIAA7yYMJVdERPq51x9MWipcp8xymmdvDW+32rmrqmfYUUwUE/0iIjcjV7Fl0lKr6ltLno3p/q+ppO1Yaj/FL5V9u/wMcdRfO3wy1qWxpphVXcdpCKm7VTVIgy4+G0lxRHriIvczRXkhXvdDouD6G32q5F+MeyKr9roRq957mr1urg5W6eWt+1AzU2xW1bb3L3KUFJnL9GMcOZJPfFG9f9E7EY/LflXtSMnej3zRunzq2q04eytWLasyn/y7e3EA1Et5pKgSQ9/N2K7om8PVLeRs3PqOWcy8jZmj/NJcdv8AiSezvXQZKIUVTBiSKYeW/Xm2Y9aKm+3p5NPaKNlyqqn3CAltppJWtRJ4ACPM5qCbe5GbGx55FyNuCrKTSS629iNeet3UZUa959UX9LpGK06lU9opSfhbYBlzkfiXvPsjn2bqTyrnF+6tx9fcr8lx0LEjba/Me2b631dy3ItlC0PIBSZp3SnGCvETvC4lsagZPa8cY8AJS7d3BNtn8IP3lS4fTGrk3FDZ0kxpeLO+67orp+4iHqKySpx7pDyW5KVK6ZNdbfYWynZ+W3N92Q7+ZOzui3ck2eL6lx79iONevOdSeNjJ7EpSa7diXxLB1E1NrOn3RLT6zWmkRWUbQXW3cAzBfuAU5KYlJYZSZT4CJfmdcdjg/ifuKL6WXv5bUP5jhUvpx3d+z/6Ej2fPaTOcSoLnZHAqw1DYeZ5dxCuCh2g7JRy69Jr5X0H15gWrVxfXht41Ul/TPXmrsXSRq/odeKgi119jfuVmSs/YqfFbafaTP8aDP1RL6TncNq5ab3qq+Jzf1A5XV7PwtQhH5oXFCf4aPhfg9niYM8AeMhP2RhSoTbG8y4R+n4T1jCAnHKBI2AU7P+GIiLvmZesZflx7l7jJ7ysW/E6p0L4otNwV7VND64n+WF/VeDOS+uLpoT/3YfH7jZTHRj43EAIA1/8An/3Zr/4XwXGAQamqvNRV8n6FPRkE+1Yiu8wzpCK7Tsfo7jOeTfn1QS9r/Yam7pagoEEbN42bYq8ZHc71qpImtNQsdOthccM3lKpECf8A5Kh9USnKapmT7viV31ia/wCxWK73OPuZn71V0CqDQvG7TVbKu3UGM0igd4cbsyQoeqcTfOMV/KQ/F8DnvoPOmrXV12n70d/lQtc3UjXOy+xZ6n3vNiKtysv6n/D9x0712dNGj/ux9zNjK3CBMR0Q+PzDvzlNUmrN06U2k1C5/wAZymsbL7Y3poaM+M4pQ7FOciRFd5iylC0odMmdh9G9Cll508lr5bMd/wDelsX2VNSt4xlSCVcplFRjM+jr2PRtF39PdsVRfxR8ghCiygekTJjVzXWhL6Ba4ON9xsH8q3BbHlrOoNPlVI3XYtcaFixV1JUp5mX2arnLrSwd4UkEGLDyna4pXG91KHGPX/PdqGJCLpJSlJeFFX7TCTzJPJ11W6Vb5c9WNBqN/Jem9a11fh0iS7X2Ntaubwn2kzWtpG5LgGwAc3bElm6dK2+KO2JUOWec7WYlbuvhu9u6Xc+vsKVoDm9PnelttuSVhdfStigrADtDrI5Ns/xJkYo+Xadu4z6e0DNWViwkt6VH4feVvUG86uUOPpVpvfrlRN0xUp610FbUMMvpVtJCG1BJX9MZcTKlD5eJpM1Nd0GzlNXvpRlOKptSezq2kXWrUKsudeXL4865d0khxdapan+YcCXfijZuQrtI3DyI21wpUXUlTwoXfaMhSoA83Zt4xrShUnLOUXLabk8vl8FJMuMtka0o0JSzdruJM0g1ky3T/LbdV0FUpqiNQwisTzEhbBWEqQobuMbmBnzx7kWnRV29xWeb+WMbVsK7C5BSlwvh7JU2NdKew2LYTfV1bTa0KmhQCh6Dtjq9anwK4uLo96PnqJcS/p0w24OZK6tuYO4yQTGrlv5UTvLqrfa/umLWq+jWI6kWVdmyOiRW21YKkIc2ONL/ABNL3pIiMuW43FSSqXnEyr+Bc+pjycX9niukizA+lnG8RdatdY9VXGysOlXyqfylqaUufIpxIJnIynEP/wD5+3Kbc5OnUdA//bmdHHVqxZjGdKOTq9vYjOrpyxzBcf07prVp3QO26wocWpdJVKUt1LypFZUpRM59sWjAsW7Nvht7jhnNeqZuoZjvZkuK40uimxblQlm0MlKAOEbpWyuMiTQgDuG6AEAIAQAgBACAEAIAQBwd8oAbZwBzAHB+13QBp06juvTWzWHUzI9P8vyKot2OW+5V1spLbb1GlpHGqeoUygOeFIqUQkT5jHONTzb92Uk5PhT3LYfZvI3K+lYOPanGzFXnFNyfzOrVdld3gRY9RJb3AAnbMSkrv2RDqZ0ucKniqKEGYCfi4jfOMkZGCUDzqw643ISZYJSd5XJIl6Tvg7iRieJKe5HlqNEa+sqG6xqrTS3BlYeZfYKw604k8yVoWgghQO4gx6jm8BrXtBV1OMmqMzg6J/Mh1Y0sapdNupytGU4QjlYpMnQki60bewAVKdz6EjeofGO+LHp/M3DSN1bOs4rzd6IO9xXsCUVL+B7E+59D7NxKfmg9V1gotCbHhunNzaq05wr5lVVSLmFWunIKxs2jncKUkHsIMbvMGfGViMYOqn0rqRXvSDlG7b1S5eyrbi8fZRr99/cqswRxvMaq3upWy4eXZzJJ+mKMqxPqmXBeXzF+0WqLDFqXUp/31KZIa7VHd6uMbCyaIh72j1mup9J06MaS5r1I6r0mD2ElVyrnC9XVqwSimpkGbjqu5I2AcTICPzExZ5d1QW97+xHjmTX8bl/BnkXPLBUS/il0Jd/3sqfnj6S4Z0+6YaVaZYQz4FiTWvuVlQ5tcqqkITN90/iPuGyOu6RhQxbXBDxPh/mLmbK1zJlk5EqtuiXRFdEV2L9pjprtiqMk6B06mBKVVKM5ZtaVo+KTLFiUlIn/ADlkxH8zPZHvLV6bOuXdj1w9zLQ6MMwcdtdywGsVP5RXz1Gknc24eVwCf6cj6451qlqjUkfUPJmW3CdlvdtXd0kyZHQfxXH622zIL7K0SBI4TA2d4iNhKjqXDJtK5baarWv7CBFhSVlKt42ezZEsijUocbZ/ox+gn6wCVhoR2U7P+GIh572XvHX5ce5e4yi8qVvn6nn1n7lkrj7X2BFi5W/5P+F/A4966SpokV13Y+6Rshjoh8eiAOFq5fTAGrnzys5byzW3GNOqVYWzjlscqqlIM5P3F1KgDL/ZtJ9sU3mHITuqPUj6S9G9LdvBu33/APJOi7or72YD3OyrUrkQklZ2AAT27ohEzq162qF83vB6/US+6R6BUzajc8kutIw40naoMJKQ4qXcCo+qLHyjZf1Ltx9VDl3rZqEViYeOt7rL2Ki95m/1zZHS1NKaSmWkM1d2e+XSOLFCyKZJA7Ngjb51nTHtx65fAhfQGxxZ+Rc/htpe1nq8qauapuoyvZdMnXrPUBA7Sl5tR90VnlV0yO+L+B0L13g3o8Wtyux9zRnbqNqlium1hcyLLKtNNQoB5ElQ8R1XBDSN6id2yL9kZMLEeKbofKmj6NkapeVqxHik/Yu1vop9vQa8Oqe43TqPziozi5qLfIj5W30pM0sUyCSlA7zOZPbHPdTuSypufs7j7F5IwLOg4ccZKtdsn1ye9/d2GNGaacVdseWipaKd5Gzft4RFxm47y8zx4XY8UNqPvBbELFaFoUmTrzinT6Jco9wjHelxPuNnCsfShTrZsb8uTGVYRoai6VCeWuv9Uu5nm2HwAkNM+0AkemL/AMs4308bia2ydT5C9a9ZWbrLtRdVZioeO9/d4GTdDUCpbLbgCkKSUqSqRBB2EEHeCN8WKlTkS3mHPVn5ZejVNcrlrlok/S4VktdN692cybs9ze3h1Daf2L/CaBJXEcYr2saJDIjxRdJI676eep2TpF1Wr6d2zLY/4o9q60Ya3myXTHq5VuvDKmKpGwhU5HbvB3H1Rz29YnZk4zTTPrvTNUx9QtK7jzU4vq6O9b0+8ol1xXGr65493oWX6jg4tA5/6wkY8xuSjubNi7i2rjrKKfafNFiGMW8j5SibQRtGwn6SYSuye9iGHbj5YnuUumpkcswhPYAB9EbGPhXr7pCLZE6vzNgaXHiyLsY9m9vwW0lrpo010+z27t1WQ1HzNzZXzotLg8NB5TsVMkFY7QPZFr0zluKdbzq+ro9p8/8AO3rPkTi7OnwduLVHcfmfct0e91ZnVp3b1sMNtpTJKQEpEtgCRICLkfOrbk6vez16/NqGB0STvNWnf/5SjGrmeVFg5aVbz/D8SG6ii5hKURpdXHYe202lPyYVySWSZkDbvjHJ7TbsR2E26B2kDDlrlvqHD7gIlMHyeJQuaXXJX4USNRUgSAJbI3CtnubTw4CAPuAEAIAQAgBACAOivuFHbaZytuLyKeiaHO68+pKG0JG8qUogAemPyUklV7j3btyuSUYptvcltfsMf9ZPMy6YtJFPWyiubmTZKzNJpLCjxWwobJLqFSaG3sJiFytex7WxPifZ950rQvSXV9QSnOCswfTPY/CO8xe1U85bWu+KdptLLBQY9RElLVTWqVcakD8R5ghoHu5TEHe5luy2QSj9p1TS/Q7BspPJuyuPqXyr4v7SCMw8wfrfv9QqrVqBX0iiSQi2pp6VsT7EtIAjResZLe2bLXH020a1GkceL722/tZ6NL/Nr639H781V5LeG80xMKHzNrvjLYdcQN/h1TKA4hUuJmO4xu42t3oNcT4kVbW/S3TsiL+lH6UuhqtPYbJuj/r70F6yrB4uA13yGoFOgKumKXJSUXClVKZKBsDrfYtExLfKLXiZ9vIVYvb1HAOYOVMvR50uxrHoktz+7xJvTtO2N0rZ+eHV975jV3LHvxXq6n/992OaXvPLvZ9uaaqY1r8EfciqaVZHda28tY1WL8a2rStSefatHKmewnhsjRv20lVFm0rJlKatvaiVrHjVVdLgzZ8fo3Ky8PqDVPT0yFOvOLO4JSkEk+iNSHFNpJVbJ+7ct48HObUYpVbbSp2tvYiecB8uLqSzC2rv+QUTGOWJttVSpy8PBLvIhHOfyWgpY2D7wETOPy7kXVxNcK7fuOZav6xaNhy+nbm70q0pBbK97oveYU5P1SHHLs/QmwuPUjDrjJeRUpBUEKKZhPLLbKYmRGrHAr+8WW5zZw7fpOj7VX3F86a6oYvqnZFXfHFqC2VBqppnhyusrInJQ2iR4EGUaV/Hdp0ZP6ZqlrNt8Vt7t66U+oalF9NupbitxSmqZRYQgqJS2l08x5RuE1CeyPViTrQ8ajbUVxpUb39veUK1X6QHxbOO2MzgatrJ2l62cuLoEPvHav4gOwRqTVGTVptqvWbJPLB0PY0/0VOp10ZllWVkPoUsfE3b2iUsIHEBZms9uyL/AMsYKtWfqNbZ+79u8+Q/W3maWdqX8pB/l46p2Ob3+zYvaY2/5kzFKp3Q3Cs+YQTTWu7Bh5QEwE1CeQT9couWK9hxy0jDXD8voM28rLKMZCwq62TUC116kT+JLVxtjiEq9ZZUIg+Z/wBOL7TpXpfWWoT7bb+BD2gNa5jGsNtWDy01aV0L0+xxJl7wIo2ZSVpn0ToE3Yy49TqjKP07/riAOoECZRRigyOuowJJQ+4AO7mJES9t1iiiZMOC5KPUzwzMxHpmEn+yf+yUX/8AHZ/wxERPey+WPJHuXuMqfKaZC+pG5OS+xYqog+mrphFk5VX9S/wv3o4z68S/8NBf60f8szYySBvjoR8gnyV9kAU/JMhtOLWGsyW+vBiz0DLlZVPKIAQ00krUdvcI8XJqEXJ7kjYxMW5k3YWrarObSS7XsNK/UXmd4161oyDU6qQpVReqtx6na2nkpxJtlA7ktpAjmGRlPIuyn1s+69F0OGj4FrH6IRo+172/aUW26ZUWMW9eQX5AVWpH5TRlJJOwekzjatW+CPEyv5uZLKuq1a6XvL76EsVVnfVhk/UZXo5sR0ntosllcUPgcyC5NFkBE+LaFKPqi+8u4jt46b3zdX8PsPnn1U1aOXqcrcHWFpKC8PN9p89V2q1NUdTlm0cQ7zKtdkcfqETnKrqXA8oHv5BEBzrc45RS/d+J0v0HsfSV2b/+XYu6P7T5wHV3UHQ+9uagaZVCabLmKd5hpx1pLyeR1MljkXsJkJifGKXh5U8e4pRdGdz5j0LH1fFePfi5QbUqJ0rTtLeuXU7qPqxfhleeX2oul74LqXPhRM7UoQmSEjuAjYybt27LinJtmloOn4OBZ+jj2owh2Lf3ve33kiYTqzSV6UU1xVyun4eY7jHm3kbdplzdITXFbKhqNdcZRYyuoSh6vdEmGky5pn7x7BHrJlHh7TBpVm99Siqkt/3FpaVafV2pGY0mN0wKbeVJdrXgNjbCTNR9J3CPGm4Msu6oLx7v7bjJzjzRZ0HAnkzfzUpBfxSe5ff2GxPTFLFtt1LabegNW+mbbp2WwNiUNpCQNncI6tbtqEVFbkqHwNlZc8m7K7N1lNtt9rdWXzlmpVj0zxk369K5n1fl0tIkjnfcI2JHYBxPCE58KqfuNjSvS4V4mM2p2oeT6m3c3PIXiplJnTUqDJphB3BKdx7zvjQnccmWjHxo2Y0XiRvlmPUl0YLNYyl1G2QWkGXonujBctQuKkkmu0lcLUcjDnx2bkoS7HQj+s0jbuFQpqz0fxgy+CYSPSTsjQlo2Lv4ftZb7PqNrtKRvt98Yv7aHWNBqhJ/fXCO1DIPs5t8ereBjw8sEYcvmfV8tUvZM6dSdF9lCpWjQ6zMLClUoUqe9wlR98bnGV/+SjKXE9r63tftZIWC6U0TdzpkUdOlutK0JaW2JKSoqABBHER+KbqqGWePbhBtxVEvgZp4di66GnZbWCVISlJUdpKgNpibRy2TTewpXUfShvDbakbCazd6GVRp5j2IsfLP6svw/FEOoaKifE4bojy6FatNJzW9BB4nf6YxS3m7Z3E3aF0qG8G27Z1Dv1RK4Pk8TnvNP/K/wovRCNkhujcK4fYEhKAOYAQAgBACAEAIAw187/IrvjvShanrNUuU1Q9kNE0vwlFPiIFM+soUB9pJltBiB5i/QS7TrHo43HVJSW9W5e9Gs3F9QbZeQmhufLS3I7AdzTh7vwkxQZ2XHduPrLG1GN10nsl7yuPUalHlSmaj90bZiPCkbrhU4bxSvqxMgNtnZ8Z/kj9d1I/FiSZ2jTW0viVa4tc9hCJJHvjz/MPoP3/tsHvZ22TTqy4tkFLluJ1FZactoVh6iuduqnWKhlaTMFKkEezaI92827bfEnRmnm8uYeVbcLsOKL3rebCOjTzIH8leodKuoJ8HInZUtBk/KhpFSuUkJq0ICUIWrcFpABO8CLdpXMn1JKF7Y3uf3nzxz56MPDtyytPblCO2Vt7Wl1x60ureapdRHC9qNkbx2892uap+mudPCIKfmfezreEqWLf4I+5FR0fE84p+5t0/qRqZPkJzSf113MzK8vNrxurzFAdyfnV+ykXGfl/blw8fcQvq1KnL+T/h/wAyM6ut/V46V6AXtFpVzZreqZ602lhJ+PnqGyhx2XYhBJ9MovWsZv8AL2W15nsR8uem/LT1fU7alstW2pSfduXe2aSs80ndaWsONnxZnm5htmdpn64oNq/7T67ztK8Tx9PGP1eM6i1rSAU0dTSq8VO6am1gpPvj9zJ8UFUxcvY7sZEu2JImsFaihwxZJkpbzSEDvmVfVGljecsOrz4bD70R/jVcu43KnoG/tvLQ2P6RkfdG7cjRNkBjycpKPW6E6YxYXshv9txihTN6tqKehbSNs/FdS0NnriMhD6kkut+8tWXkLGszuPdCLb7kqm5rFccoMSxq34vbUhNBbqdmiZCRs5GWw2PojsdqHBBRXQj+dOdmSyr870t85OT8WY+ebN0+u9R3RHl+F29ou5BTUxuluAE1fMUhFQiXeeSUbNiVHQw2nRmhTp21SulutGS6M1H/ALfkfyVWtCtkqu0OLWgSP3uVa0xHcx2nPHquhpnSvTG/C1qsYydOOMku97S46GlVaMnoLqjYaepZcn3JcE/cYoEnWPgfRcI/TuRl1NMymmFJCuBAOyIHcdPIU1OZDGc16BuK0r/rIBiUsOsUU3Uo0vyKCnfP0RlNEn+xHmslEr/07P8AhiIiW9l8seSPcvcZY+UmyVa/3x/gixOj+tWsfyRZeVF/US/D8UcR9fJU0qyuu8v8sjYWt5CN5joJ8knnerkp2DfwlvgGYt9dusL+XWl3QzDXuanfKVX+qaVsCUnmTTAjftkVeyKvrmU7q+jDxO5elehQwprUslbv00/83wRiUjSmkxgKeQjxXjvdUJlPq+uK9axVa2s7Jn6/LP2LYuohvqLzSts7dNZcYYVXZVW1DdtsdtZHM5WXF8hLSUp4gEzPYBG5p+NLNvqC8q2t9nURPMOpQ5c06WTN/mz2QXa+nw3mWfTnoWNDNM8a6brKRXZTTvfxfK6xvaa/Ia4hSwTxS1MNjuBMdStwVuPUkfI169K7Jyk6tlq+YB5PuY2XNz1hdOV6Vcsopi3V5Hi15eQlVVyNBt5VBULMgVJn+Ur+iZ7IpmsaY8jikt76DqfIXPq065bt3Y0UXsa6nvTXjvIFpKj5phL5QtpxQHO08kocbXxQtKtoUDvjnFyDhJpraj7Hxcq3lWo3LbUoSSapuoWvddJrXU3ZV5slSugedPO802AplSuKglW4nujLDJaVGaN3SYOblF8Le+m4rVlx9VrSPFqFOqHcECfbGOU6m7ZsO3vdS7cQwXKs/uIprMypbSSEu1Tk/DbHer6o2sLT7uXKkF49RA8z834GgWfqZM0m90VtlJ9i+JlFoXpFQ4JQIpKBBXWukKqqgj4nFAe4DhHSNM02GHDhW1vez4w5151yuY8n6l3ZCOyEOhL4t9L8DIPEaOns9uXc7goN0NO2p51xW5KUCZ9wiRk6KrKdGLk0lvZCuoecXHUjJ3bzUkpt7U2qCnJ2NshWzZ2q3mI+5PidS3YmMrMFFb+kpKWOdISeHbGM2nFo9tjwF3IlmpqPy7ak7VcVniExjnc4TcxcJ3dr2Iuag01duDzdlx+kK3VSCGmhw3TJ+uMUaz3byXuytY0Ky+VIvSw9KVOoCoyhwqc409Pw7ivfG9bwulsqGZzO91qPi/uLvs2guE2sBNPbGiofecHOo+tUbcceC6CCu6tk3N9x+Gwua26ZY1TOIdboGUuNkLQoNpBSobiI9fSj1Gu8286ridH2sumjtiWhu9MezWI/6nk8mMWttPGrUf7lUaeZuRZeWP1Z/h+KIbSmQlxiPLmVm0CVvQPT9MYZbzcteUm/RASwRvvedPvES+F+mc75mf8AVPuRd8bZXxACAEAIAQAgBACAMIPPhf5OlzHGP9ZkdP8Aq0VQYgOYf0o9/wAGda9HV/5G5/tv3o1O7Dt475eiKgfRxMmk7jr+GMPPErd5lpCl7VSCpATMRmRskW/Sm5WU2ZR9KvQLmnUrjwz52709mwPx3KTxuRVRVuraI5+RscqRKYE1KiW0vQp5kePiSjU53z16q43Lt7+X+nK5e4a02KKru27X7EW95rPTljHRppbh9TpNc6p7O73XVTFbVV/guBVPTsJX8DJQUp+NXfEhqOi2cWMd7b6yqcmepep69evcShCEEmlFdLfS2YK2Hqc1Mxm8N/8AOSW7pYCoB4pZbZfQkn7SS2ACRvkREZPAhJbNjL3j8z5Vif5tJR7qGQ9sr6S5UTF0t6/Eon0IeZcSZcyFCYPaDL3xDyTi6PejoVuUbkU47U93VtIU1AtgtGY19Gn7BcLont/afHv9JiUtS4o1KZm2VauyitxU9GhzZsg9jTv9mMWT5Ta0f9bwZkj0/wCtB6e9VaHVZFCLjUW9upbZpFueElS6hlTKCpQB2AmZjHp+X/LXVcpWi+Bsc28vrXMCeJKfApuNXSrSTrs7y/751IXzXTI3MkzapDl3c+FtgfC2y1OYbbTuAHtjeuajLIk5T3/22FewuTrOjWFaxo0iunpb62W/mOmtiy+mU82kIq5bFpGwk9sYblhS2reSeLqc7Pyz2rtIiXgDWGZHU+IE/OcvhTRwSTzcI0Lsn5WWTDt23+ZDc0RH1EZ/TVF8p8RoHAtFH+dVlJEvFUJJTs7E/TG7h2tnEV3mLOTmrS6Nr7ym6L1CbhndCyTMJ53dv6CCRHvIVIGto0+O/FdVWZfdJdqavPUxhNvfHMyq7Uq5dvhr8T6owaXFSyba/vI3OfLzs6NlzX/8UvtVDbnHWT4EPPcrfTXO3u2+sSFU7yC2tJ2iREfqdGE6H52vN36N8l6IuraozrEaZTOmmRVbl6sj7aT4TFVzByopVEbJTUVJ7UnujcnFXIUe5oksPKnZnG7B0lFpplt2i6WrOsapstshHydSma0byy6n9o2e9J90cxzcSWLdcH4H11oWsW9XxI34b90l1S6f2dhkla3vmbXTP8VtNqPrSDFZl0nWbW2K7iItW0gZ3VEcUtH9QRJY/kKnqypffh7i2gZbezbGcjifMaVz45QLG407P+GIiLnmZesZ/lx7l7jLfym1BjWLJqpWwJs6UT/nVaD9UWflJfny/D8Thnr/AC/8bYX+r/0szouGRUtIhbzziUMpBUVrIAAlMkkxfanyik26Lf7SDtZuqelQ09jGnj4crFgtP3FBmlvgoNdqu/hERmaktsYe06Py3yXKVL2VFpb1F7339S7CC0hDnMuZW4slS1LJUtSlGZJJ2kxCqKR025elJ7fZ0eBaOsGd4jpjhVdleW1jdFZaRsuP1DpGzglCRvUpR2JSNpMYpwldlwQVZMksS9bw4PIvvhtw+3s7ewj/AKUNJ7rUZUOsDVe3qo8wq2nGdNsZqkzXaKF0cirlUo//ACHhtQCJpB9EXnSNJhhW6Le97OF86c3X9dyncm6QjsiuhL730mbXThiFrwGwVWuefbKVkKFuSvat11c0qcSDvJPwp9Zjazb/AAqn9u4qVm1K7JRjvZZ+qmq2T6qXVVZdnC1aGyflKBB/LaTPZ2cyu1RiCnccu4tONiQsKiW0iLUDTTGsuJqLixyXDcKlj4F7O3tHpERubp1rJ8629a2MunLvOedorasyrB74y2rwXQ+4jx/QFxypLVtqXXSNyQ0knumqYiHfLUE/1H7Do9v1ovyVP5VN9knT2U+J6afp0r2wFVTgSrsUeY+iSZCNizomPDa05d72ewh9S9SdYyk423CzH+6qy9rqXTp9p3k2B3EVtirZNqI8amdbmy4BwUJ+8bRE5auq0qRVF1HM9Q0yWdJzvTcpvpdW/bUyc0hVacnbbZUgU96SJuU6j9qQ2lCpCY98SFrIUynZ+kXcXa9sesq/UPcF47hlNi9H8NZdF/m8u8MNSKhs7SQI85M6KnWZNFxvqXOJ7o+8himofDHxDZuEaNCzqJUsbx5V8uKaXaGQQtxQ+6kHd64/JvhRms47uSp0EnY/jTt1qqfH7O0PEVJttKRsSBvJPYBGvGDm6E1ev28a25PZGJOGH4FZ8OtopKJIVWLANRUEDmWqW3fuHYBE1Zsq2u05hqWpTzJtydF0LsKr8g3GYjj6TQtAbYA7U06U7UiAPvkltPugCL+qJX/A7Skf/kuH+6MaWZuRZuWP1J93xIa3RoFyK1ah+4t+v6YwveblvyonHRZMsDYPa46f1ol8L9NHOeZP+U+5F2RtkCIAQAgBACAEAIAQBgr59j3J05Ygz+PIkn+rb3zFf5i/Sj+L4M696OR/rrr/ANP/AKkaqIqB9FEy6SpIwel71OH+8MRuT52W/Sv0F4+82peWCEs9KFEpexKrhcFzO6XOkT90dA5Y2Yq72fIvra+LXpL/AE4e5mHPmxZq9r1rLT0NoBdwfF2F26ifT8Tb9S6oLqHRLvASD3RAa7n/AFb9F5Y7PvOv+lXKP8jpnHc/Vuvia6Uv3V7NviYLZhpwptSwpskbjs2y/wDpGhavFuy9OpvJf0kpX6PTaz0tRPxG2OTb2BZA90RmQ/zGW7SYuONBPoRHeq9Qh/PK3l2hHI2SO1KAI3sfyIr+qOt+R69FUzzPmPBlw+4CPGT5TNov63gyTMkJFpXw2pkfXGjb3ljydkCm2a8XGlfCWeZahuKJzjNKKNezfe5qpIdp1PyGhtppykGqIAQ6v7vCZEFkSiqHi9pVq4+J7OwmTou6Lsi6kr6jUbNl+BpJTVBNS+FgvXF5pYK6dvlJKQDsUo7huiX0fRp5UlOeyC+3sOa+o3qXY5ftPGx/myWqJdEE1sk+hvqXtMIfNL0iHTz13Zth1DSikxa4vNX+ystjlQKSuaC+VA7EuBafVErnYqtXHFbF0FM5S1qWdhQuTlxT2qTfWvvLH6brol3U2kamCFtPhO3eeSITNjSDOl8t3q5Me5mZ/StfWMd6j8Lu9UrlYbu1KlajsADjnh7f60aemXODItt9aJvnjGeRo+VBbW7UvdU27lct27tjrR8AHnq6+mpGV1NWsIp20lbjjhCUpSNpJJ3AR+OSW8/YxcnRKreyiMFuvG+aOdY7FforltGmo00cbNKxe2Uj5pquSqbdYwo7g2rYPxCfAxEW9eir3D+519vX3HXbXpjfhpv1X/yPNwdHD/D+LpNPequkWsPl/asv4pm1Oblp1cFc9HX0/N8pc6UfYfplmYS6kH4knaNoMS2p6dDMhTc1uf8AboKzyvzRf0PI4o1cW6Sg+n7mZZaKahacZ1hdHdLO8mutKm0NKdaP5jK0pALbiPtJUDwMcuysSWLccLsadvWfX+k63b1jFjfwriaSVY9KfUyk6gacYblOW1VRa67leHhoUiYJBCBwjcsWISj8rKvq+sZNm/L6kOr3Fvu6DcWq0S/STGb+U7TQjzF1xJixDS4Ixm3odq58rKEEpT+ESiMuYK4trLjjc2JWo0j0dZLfTxqWz0zXK63210puF1ulM3RI8VzkQ2EOeISqQJ9QiW0q4sRyklVtUOf8/YkuZLdq3KXBGEnLdVuqoe7PepDUnU1ZbvlcWbWTNNFRzba7fi2zPrMbWTnXb+90XUiI0TlTB0zbbhxS/iltf3LwRSLZli2pB4zT2g8BxjTjcafaWC5ixntTKNqn1R6d6Q2lDt4qVVWR1Z8C2We3oL9fWvnYlphlM1Ek8dw4xuYsLmVLhtqvW+hEPq08fSofVypU6o/vS7l8WW5p3oxmerGU0WuPVewlhijWKvE9PObxKahX9ypuRE0vPgSknalMXnTdKhix65PezhXMnNV7Vpqvy24+WK3Lv7e32GWeiOkl01WyH/mXIEqTjTSgXFq2F5Q3NpI4bNvYI3r95QVFvKmXT1HZb/Echp8BtBDeP2ZIQpprYlTykA7h+FPwj1xXsm45SoWfR8Thhx9MvciNFtlBkfsxrbSWkj0WTEKjJKjlE0USP2rn+iO+Pyc0kZcbFd19hd9n03XWPosuN0hcqFbktiZP6Sj/ACxgVZ9FSam7WLCrokip5h0/XXFMVcyW4uNrebUgKp2wpXKFq5T8R+qNmWK4xqyEx9etX7ytxi6PpZadvslOsiaOUmNVk7CSZcdmtjtsq2q+jUUVDSg42tO8Edko/FNp1Ms7UZxcWtjR96yZEcyyOmrHAUGnpGWVIO7xNqlkekmNud76lH2FbxtP/lOKC3Nv2dBZ6qVSNhGyPJmoXjhVqTb7T8yR+fUfGTx5RsSI17kqkviW+GNelk2aGYsiltTmTvp/eqoltgkfZaQZEj0mJHBtUXEym80Z3HcVpbo7+8kBIlvjfKocyHZACAEAIAinqhURbbQjh4zp/u40s3ci08sL5p9y95D0p7I0S3lctglQNej6413vNy3uROWjaeXAqXvU4f1omcP9NHOOY3/Vy8C6Y2iCEAIAQAgBACAEAIAwK8/R3l0LwVicue/vH+rbnP5YrvMb/Lj3nY/Rpf1d9/6a/wAyNWcVM+hSaNKk8uC0XYfEP94YjMjzsuOlKlmPj7zJDTfrTybBdEKXQHHm/wCHW8O1L1wuqVkvvpqV83hol9hMthO890SePq87VhWo7FV1ZRtU9OMfO1OWo3XxypFRh0Ki3vr7Oo6uazZPR8j3K40scdu+Pz5Zo3OG5jS2VRGWp+iVE3TO3Oi5RRDatKtktvAxqztu3tW4ncbOhlUjNfMUnEcVul6rKPFMYpzUXN38mmp2/tKKUlR9QAmTwjVt25XZJLa2SeVlWsKw7lx8MIKrZjze6565XmqrqgzecdcUZbfvERJxVFQp1y59STl17S5tExPLldzCz7wIwZT+UlNGX5vgSymiVclppG2i+4sgJbSkqKjOYACY0Ip9BZ5tJNy2LtPZqRiOV6NaeI1S1Js1bZNPVvt0DVyrKVxptb7oKkNpBHMZgGRlKNpYF6S4uF06yAfNemRufSV+DmttIur+wsXEtdtLM1uQs1iuiTdl/s6d9K2VufzOcSJ7px4uYdyCq0bWJruLlS4IT+Z7k1SpPHTP1NZ9015oxeccqFuYhUPI/jNnWolipamApQSdiXQmclDb6o2NO1K5iTTT2dK6KEFzlyRh8wY8oXIpXafLNLan0d660S752fRk91c9P9g6tdC6U3POcZovmqilpU8z1wx+pR8wsISnapxhR5wneRzDhF9z7KyLauQ6vsPlTlPUp6PmXMLI+X5mnXomnT2Go7RDKm8c1Ps1fUK5WBUJYeJ2codmyZ8ANsVjKg522jvmiZv0sqDe6vvMz6GtqbbXMXKjPLV0zjdQ0rcQtpQWk+0RWU2nU7DctxuRcZbmmn47DOi9ecz0641j9LQv0F1uOoaadk11vZYQw0moLY5x47ipcvNPaBHRFzHa+mnRuVNx8gT9FtQeXO25whbUnRttunQ6LaY+6weZFqjrvUm2oQ3ZcAJ/9qo1kuOie99/YpXoEhFbz9cvX3TdHqXxZ2rlH0t0zSIqdXcvfxy3L8Mdy795T8Zzi3XpkFCx4myaTsIjTtXkyyZmmStdxWM1o9NtQ8BqcD1noae66fv7FtVsgW1HYFsuj4m3BwUn1xbdI15WFwXfJ19X7Di/Ofp1LUJO/hr83pj/ABdq7feYgal+WnrRolentVOiq/qvOPrm4bC8pKa8NE83hrQohmpHokrsE4tmRiWs23RpTi937Djem6tnaJlcVqUrN2Oxrrp0NPf4ka5x1I31eX06dVsUqcGzOmpaahrvAafTTVD9Ojw1VBbdAcQViU5TExFXzeW2ttl7F0M6nofqlG5xRz4tylJviSTSr0U6kXPinUQHkJTbrtSXBrYAguoC/Wlcle6Ie5j5NnzQZdrOoaRnKtu9Db20fsdDIDSvWihu+IUztWyA6jmaWELTsIVwE+wxG3r7UtsX7C2afo1q7ZThej7UVDLNWsapGWlOuJbCSVLLrzTYSJcS4oRks3a7ov2Grn6SrSq7sF3yS97LGvHV3pZaakW631gut8OxugswXXvrVuASmmCk++JSxp2Rf8sGl27CoZ3MOmYP6mRGT6oVl9u77Sq2K3dXuu6AzjFnRp9hbmxV3yRPjV62jvWzQtH4SRu8UiR4GJ7E5XrtvOvYtxQdV9VZRrHDt8H96W2XeluJZ0b6Z9LtELgrI0ePkWq1UP3i/XZQqK5ZO8N7OVlHcgD0xasfGhZjwwVF7DlGdqF7NuO5enKUpb29plLpxieH5Fp7UXGkpFPZFRczeSULh53KihdPMipYBGwtSBkN+2P2U9vYacouL2onfTtVhYxMVVtS2ihoWedTbMuQIS2VhSZfdWNv/fEbdTTdTyo1dDG68OVF1ulTeKok1NS4t5at8ytRMQjlV1OhQtqMVFdCPMKBdQ4mnaTNxZCUjvMfvEfqg20l0khWCwot9EzaaJHM8ZDZvU4s/wAsarfEyctRjZh1JbSeMBwKgxCyoZCJ3R0ByqdI+IqInyz7BE1j2Fbicz1fUpZlyv7q3L4+J4NbqBk6X3Y8fDQRs4+KmGT5GedG/wCTCn9thjqwltB7DEU0dEUi4LSUvUgWftg8pjDJUN63LYUvLqQKqWqgbykpP9Ez+uPdtmplx21KMKXx3UsS2qITs7zKMzZo/T2l7sM8iEU7e4BKE+yUau9kyvlRkPjluTaLJS21AkllpCJDtCdvviwW40ikchyrzu3ZTfS2e6PZgEAIAQAgCJuqJX7tZ0fpvH9UCNHN6C18sLbPuREJMtsaJbSuW2XyDZ/RjA95uW/KiZNPcrxrGNPKN7IK9ikQAtX57qEmXMdwJmfUImcVpW0c61y1O7lz4U2XZZ77aMgoEXSyVCKq3ubUOsqCkn2bvXGynUgpwcXRqjPXH6eRACAEAIAQAgBAGAHn8Oy0m0+p577zVr9lCR9cVvmN/JDvZ2f0aX9TkP8AuL3mr2KofQJNOl45cFodn3Vn2uGIy/52XPTP0I/26TpvF9+QyJ2mnIJCdnpEeoQ4omO5k8NxouzDM6u1vKXKbmVT7CpKp8sfik4My3LMcmPzLb1lYyXNr1mC2ba0gil5glqnbmpTjm4TA3nsELt93NhixMC3i1l9r3UM0ejDoxrtI9PLzrhqpTeFn1Ra63+E290fFbqddKvmccnudWPYO+LnomjuzB3bi+amxdR80+p/qRHU78MDDlWzGa4pL991Wxf3V9rNRDhKnVqO8qUfaomK8js5eWh6f+qnldlOv3qAjWy/KiZ0X9R9xnB5XVGxW9UrKqltLgbtla4nnSFcqglIBE+Mb/LUU8pV6mUr1uuOGhujpW5H3syL847TE6neXpnlNTN+JcLKzT5FTJA2g0D6Vql6GyqcXrUrfHZfZtPl/kfL/l9TtP8AirH2n5/0Bxp1uto1FqsaUHWnEkgpWNoUDwIipbHvPo2UKPiWxrpMytKMrXm2ntryR8g1rzKU1BGz85r4Fn0lQis5Fvgm0dg0vKeTjxm97W3vNqHlsZ6nMOlq32qtWHamx1NVZ1IVIyaSrxmwe7lXs9EdC5avfUxUq+VtfcfH3rTpn8rrk5pUV2Kn47n9qMdvMS8jXTrXutr9Z+lZ1jEtanVKrKuyrARaLo99sqQlI/d3lHikcpO8DfGzmaWrlXHY/eRHLvPN3Epbv/NCux/vL7zEmxWrUbE2lYJrLZ6iwarWYCjvFtr0FCitA5UvtK+y424NqVJJB7Y53n4kse5wtUqfYnKvMFnV8SN23JSpsdOv4VKTqLp0jM2UVlveFJkVOJMvkTQpPFDoG2XYRujDYvcDp0EjqGn/AMyqxdJLcW1Y7RqRZ3RS3O3qWE7A8wtK21d4JkfdGxOcJEbYs5Np0lHxW4v3G6vIKIpecBYWNu0/F6wI1ZNJ7CcsqUo/MvaXC9cslzGqYtaA5U1KiEMUrAJJUdmxI3mP1OV2Siqt9SMNx4+Fbldm1CKW2Te7xMpumnp5VY8acRnFU8u8VgSpunYckij4jlmCCo8eEdI5fwbuFb+d7X+70L9p8d+qXO2LruUljW4qFvZx0pKf/wCPVXeVvVzpLXfLA/U5HbLbleKMp51tXSnZU82me8CoSof1VCLQsiDW05labnKkd7MYc18vXpByCqU5kGCu2p9c/wA61vVNMn0yHiJjLBW5bn9plnG7b8yp/brLcHlk9GbbnOy5e2UTn4bd2KR/hx+vGjLefiyriVE6FYx/y++ifH3UvIsFTdn0GY/i1wq6xM9/2G+UR6ViKPEsib3slvT3SDDMXbFv0jwtqknIf8LoG6dR4fEtCfFPfzGPT4Y9Rj2ska29Pus9+SF1NCuhojvaZSkLl2cyjPd3RrXc2MdybM9jHjLzTjFFQx3Ta3WF1VM21++oUUul0zWFpMjMnbOIDK1K5c2PYuovmnaLj2kpL5n1sue2/wAbwu4sZjiw5rxRTLlP92ppln81lY3GY3Thp2WoNwl5X7+gx69pKyLfHFfPH7S9qe/s0dq/ieFHnw7JGHm7ag7EsVCwS9RL7DzTU33zHGJu/bc4tdKKHjSUbkXLodSMF0ykK+MSUN4O+Y2GK21R06ToSpNVR7sXoUv3hLyh8LQLhHfuEY7jojPiQrPuJT0htCbtnDC3UzapkqqiOHMnYmf9Ix7xIVn3GDmHI+litLfLZ95NgBHGJo5oWnrjIaY3PvS2Pa6kRgyfIyV0T/kx8fcY5FpJ3bDEWdClEquOc/y7qV7+b/RjFcNjHrQ+MlQVhojeOYfRH7bPzIjWhTLfTpNxYMtywTPjtj3LcattfMi77UhLt0pm1fZU62D6CsRghvRu33SEn2MyLSJRYkcdRzH6fogBAHHNtlAHRXXW32xg1Nyfbp2BtK3lpQn2qIg2kfsYuW5VIT6gs/w/Kqu22/Hq9qrqaYul4MkqSnmAA+KXKfUYj8uSdKFv5dsStcfEqVoR4d3eNsvfGmWcrtvH7k1L8MYJbzdt7kWNmiivJKhJJKUlIAmSB8A3Rt238pD5X6jPXgOpuWacXH57Haj92V+2pHSVMODvTPYZcRGaE3EjcrDhfVJ+0yT0o1uxvU+mFOwlVJkSE8z1E7PgNqkKlIj3xuW7yls6Sq5mm3Mfbvj1l6xmI8QAgBACAEAIA16+f48RgWnNOONyuDkvRSoH1xWuY/LDvZ2v0Yj+dkP+7H3msiKsd9Js00SBg9vA/AfesxF3/Oy56d+hHuK9Q4y5d67/AIbb1VVyWQPyGC44eAHwgmPEeKWxGW7O1b+ebS7W0TJpL0FdSuq7zTlPYnLHYlyJuF9SqkbCTxQ0oeKvZ2J9cSmJoeTf/daXW9n7Sh696p6NpSad5XJr92FJeFdy8WZq9MPl/wClHT+81k93P/MWpKEgi5VjaQxTq/8ATMmYSR+Ikn0Rc9M0C1i/M/ml9ngfN3Onqvna6natr6Nh/up7X+J9PdsRLuq6002l2SvDZy2u4LJ9FIuJm95JdzOc6av6i3+OPvR+d5CuZIV27Y5qfbbL20NAOS1J7Kc/20xq5e4mdE/Ufd8TObyr0gdTLjp+5aawy9aBEpyuv6r/AAsoXrm6aJ33I/E2Cah4ratSMBven13ANsvlBV2qoChMclWwpgmXdzTjoc48Sa6z5Bxr7sXI3Fvi0/Yfmg1d00yvRLUy+aVZvRPUF/slXUULjNU2ptSktOqbQ4kLAKkLSJpUBIjdFJuW3CVGfU+BnwyrUbkGmmveicekC9/Paf1lmWqbtFVqUnt5HkAj3gxAalGk0+tHUuUr6njyj1P7GbG/Kfz80NwyvTt1cvGTTXhhHe2Sw5IegiLBylfpKcOvb95xv/2C0ytvGyl0OUH47V8TMq9ZjZsXsrt9yCoTT2tkTW6rjs2AT3k8BF2k10nzPbtuckoraYn9UmY4x1HrRRXC0Ms2yi5vkK8tpFxE+PjD4kpO/knLtiIz7FvLjwyWzr6ToXKmqZWg3VdsTak96/da6U18TFzKdBsms7612VxFZQj7EyG3EjvB2ewxTcjl+9F/JSS+0+jdH9W9PyYr+YUrU/bHwa2+1FtPYPlVOrleo1I4Ekol7jGmtHyW6cDLHP1D0aKr/MR8E/uPtnB8heGxhZH6IJA9e6JDH5duS88lH3lT1X1fxLapi2p3ZdbXCvvJD0Qqrxp3dy5c7L8xQPn46hlCTVND9Ek7R3RadPw7GJ5Vt62cI5u5g1TmB1vSait0I7I+zpfazL/SdFtyGhbuVndD1MZTlPmQeIWDtB7QYnIyUtzOY3rMrTpJUJDzS2qZ0yuxGz932bO1xIjHf8jNzSl/Uw7yEKenSWeVwbYiVs3HRpRT2M9dsxy21SXPmKdpcpSUttB3+qP15M4/vM/bWFZnWsE/BF/aLaXYjf7jWO3W3svJp0tqaCm0yBUTOYA27o2sS5KbdW34sr/MkY4sIfSio1rWiXYS3R4fbKBsMULDbTI3IabSkexIiQKTKTk6vae9iyNIlygbO6B+VIJ1ksjNj1HrUsjlQ/yVSQOHiJ2+8GIfJhSbOi6Ffc8aO3athTLZUJdHhn9oNxjVaoWCM0zsxl6hx+/O4neNun+TOJadTzBKqG4b2qhondM75cYsOnZP1Y0fmj7ig8xaYrE/qx3S6OpnVmGL3Knv1baLmhKMrpUIfW20PhrmAn4qhkcVSkVJHpj3lYaurijvNbTNW+jSEtsfceHEmkJW+8mRSUpAI4iZivXarY95fMPam10ktdPzKV3u4vnapDLaQeyazP6I2tP8zIDm10tW12v3EqxKlELP13Vy6Y3DvLQ/vUxr5PkJbQ/+THx9xjxEYdCKpYBJhw/pf6IjDM2bO47rxb6ldCLiEk0SV+CpfALUOYD2CP2CdK9BjvXFxKNdr2lNpWkt1zS+xUe5PYeYraV+meNPUt1A3trSsf0VAxhjvNmceKLXWjIuiqUVdM3VNGbbiEuJ9ChMRYouqqceuQcZNPoZ2KJG6P08Fn6j624TpxTLTX1Cam9yPh2+mUlbpVw5pfZHeYxzuKJt42FO89i2dZ5tMte8R1JpvAaWKHI0pKl0FSpIKpCc21bOYe+PyF1SPeTgTsvbtXWRBnmvur9XUPM0lUijtgWtCfkWpLACiPiWqZ3RpvIkyzw0O1bSdOIju6Xy83x41F5q3qt071VDinD+sTHhts3IW4w8qoeX7O0bCNoMfh7KraL/AFExTVCS6k7EqQCSPZHmUDZtXm9m8vigChQtgzCuXiO6NOW8mIbkUa44Mi63h64VL5DLigQ22nbIADaTGVXqKhqTw+OTk2euixHHqEzQyHHB9508x9h2R4dyTMsMWEdyJC0StdQ3mDdUzTluhS06OdDZCASmQ2gATjZw0/qVIPmW5BYzjXa2iZhuETBzwQAgBACAEAIA11ef69/09pvTz31VzXL/AOy0IrHMm6Hidw9GF8+Q+yPxNaZ3xWDvBN2nSQnCLcO1oH2mIrI8zLpgL8mHcbYfL6sFlpulfE7o3RsJuTrVStyoDTYdVOsclzLA5j7Y6RoEF/KwdNu33nxf6tZdyeu5EHJ8KcdlXTyro3E4DkPpibOZn0ABugC09dHyxoxlzw+7ZrmQO/5NcYch0ty7mSOjquXa/HH3o/POyQplKkmYIG0RzZH2w0X3oSgKyGsJ4U/0rEauXuJrQ188u74ozi8rtYp+oOuf/DaKkT9LqBEvyqv6n/CznXrxKmjRXXdj7mbCKWv5lcy1SSNszw4zjoR8gmDPXrptpf1nN1Fiziy0tNXW5x1mz5NQNJRdmkIJCeZ87VIO/kVMRE5UY3t6Og6DevaZSUJN1W1PcYH4P0y6qdMeqFbj98aNywO7tctvvVEhSmS80uaEPIEy2spO47DwMVHWMGUYp70mfQXp5zRYv3ZW2+FuO5um7qfUZedCuH6q47rHSai0FqdThVM29SXureUhlLVK63zKUQ4Qo8pAVsENAxb9u+p8L4dqb3bGePVnXdKy9Lu4rvxd3Y4JbXxLurTpJd1k1cuWp2QFunUW8To1FNEwJgLIMi8sdpG4cBF0uXOI+a8HDViO3zMtloczST94iRjEbiZ102K1+QPmno0/APtuEfCgdpj8lKhnsY8rroj1nSy1UKZlrx6s/fcE9v6IEYXcbJe1gwhv2nsp9Ccvujfj0ttdDCQSFLSUCQE9gVKfqj3GzN9Bq3dTx7TpKar2HgteEtB7k5ZLBkpMpGYMYW2SMVUvvBE3TE7gi4WhZbemOdJmULAO5Qj1G7K3uMeVp9rIjwzVSbr1kVDleit2uFOA3VoaQioZ4oWXUe48IkXeVy2ylW9Nlh5sIvaq1T695C7bKZBMt0R5dGVfHUAIdJ3TH0RiuGxYewlfp9aBeuapCcmR7yY3sDp8Cq83PZb8fgSYG9sSZSTkoAgwQN1IOJb1BQEyn8q1P+sqI3L85d+XZ0sP8TLJoKvkqEKG+cvbGpKJZISVT3ZJZWshtDttWeVxXxMrG9DidqFA9s494eS7E1L+1DxqeGsmy4dPR3mLvWN5w+mWjeOI02slteybqoxN3wXHKZxDVHRpTLZUvCa1rkdqED0mLS7ii6rczlVyDhJp70QN0c+dfmmomv4xTqLt9psun+QveDSVNoZcYbttU4QGy4pxaptKVsV+Eme6IrUMVT+ZbyyaDrDsyVufle7sNtvTyUrrbk6kzQW2SCCDMEqIMxsPqjSwN8iQ5tdYW+9koxJlILL1+VLTKsB3FbI/vBGvleQmdB/5MfH3GPkRZfyq2Afu6j2r/wBERimbNncSbpPjdBleJ3izXITYeW2EqA2oWEGSh3gxu4dtTi0yq8xZUse/anHek/eWBl+E3rC7maO5oPIDzMvpB5HEjiD9Ua9y04bHuJzDzbeVHig9vSulHDa/EbCk8R7I12SZ77x1FahYFa6W2W6ipX7a2jwW6l8O84KT9lQCpbt0SNjKfDTqKfqehwd13KukizMm6htVctaVT1FyVS0ithZoEeDMcQVD4vfHuV2T6TWtadahtpt7SzXHFuuF51RU8ozUpRKlE95MYzeoctOv07qX6dam3kmaHEEpUk8OUp2+yAarsZc+PMX6vo1vVjJVSgTC1g865n8J2ntnGGTVdhK4/G41lsQbxClutYAw4WUkFSgkTEu6cfjucKPf8qpvZsKrR4LYaP4nkqecHFxWz2JjE7zZswwoLftK1a7GVkMWWk5lHYE07Uz7UiPK4pdbMs527S2tIue06RZxdAlS2BStn71SrlP9UTMbEMScuwib/MOLa3S4u77y57ToFRpCV3quW4eLbCeRP9ZUzGzHAXSyDv8ANkv/AI4Jd+0ue0aa4XZiF01Chbo3OP8A5iv1tkbUMaEegg8jWcm9vm6dmwrrTLLKA2ygIbG5KQAPYIzJUI2UnLftPqP0/BACAEAIAQBwoyHfAGt7z/H/AMvTam4811X7UtJir8yPyePwO5+jMf8Akv8AD8TXAd+3fFZO6k44Cnlwy2j/AGCDETd87Lrp/wCjDuRth6FKsU/SphzE9vyripempcMdN0Ff0lvufvZ8R+qjrr+V+Jf5UTMxUBwbTtiXOfHehXAwB5LuWBbKhVW2HaQNLLrS0hSVp5TzJIMwQRskdkfj3M9268So6Ou/qNV3Wf0aYLqNktVn+hdnZxS8v8ztTZ0Ok0FUucypuQAZUrsSOWfZFUzdNjcfFb2PqO98s855GBFWcxu5BbpdK7+v3mM2l2LZDh2ZXOw5PRu0N4ZaDa2KhJSrY5vE9hHYRFUzYOGySod+5ZyreSnctSUouJm75auIZTb9VbllNdbaljH12xxlqteZWhlbin0EJSpQAJlPjE1ytYmr8pNOlN/sOX+u+p489Ot2Y3IOf1E+FNNpJPbTqMzs0v6scwO63kn8xqmcS32la/gT9MXq46RbPlzDtfUuxj2mLqqZTk1K+Iq2mf0xE1L+0jyM425drgmka+Hm2knaABxlByojzbx+OVFsKpU4gxjdpqqtuoe8VxssrCFciFhWzlUE7x3GEbzkzZuadbhDie19Bb9LR+HuG3cf+3ZGQinEqthsFReLiihphJKviWvglPEmPMpcJnsY7uSoiTcdw/n8CwWJkrfcIQlIHxKVxUTGsuKctm8nnK3i2qvZFEvYborY8XZTV1raam+kTW8sTSg9iAdnriWs4yhv3nPdV125lNxi+GHV0vvK+MbZJEkAnvjaIIx61DxZnH8+uduZTyspeLiB2BwBwf2ohr8eGbR0vSrzu48Jdn7D6tTKXEeGr7Y2xrSJm3Kuwq38QqrdZbjb6bbT1rIaeT/NcS4D6iI925uNV1mDKxo3HGT3xdUW0kACPZiKpj6Zod9I+gxiubzYsbmS10+J+G5q72R7jEhp/SVHm1/p+JJUSJTThW6AMadf74m46p1yGVTbpQ1SbO1CAVD2kxGZLrJl50WPBYXbtLVYqZqEjtjXZMxdC7USIGz1xrEojU55xXRbX6T6hvdX2ldMpWFXqo5Muo2gpQpK1ZkKiUpBt4/aPBfpidwb6nHge9HP+YdO4Ju7FbG9vY+swbvtK0ks5HZ9lvf2yH/hucUmXA8IkF1MrJuk/wAv35hdFrVjVT0z6r1w/wDl6y0rZsNTUrHPdLbT8wLfMozU7Tp38SiR4GNT6HBJy6yQys+V+1CEtrjXb7DZvOe6PRHlkdQhlppU97rA/XjXyvITWgKuSu5mP4iLL8yrWH/dFdvMf7IjFPebNrcS/oAkfwi4HteR7kRI4HlZSubf1Idz95e14strv1Eq3XdlL9GvehYnLvB3g98b04qSoysWMidmSlB0aIrzTRm5WALrsc5qq0TKiyf2zY+hQ98RmRhtbVtRetL5jhdpG78suvof3Fj1VKxVtLo6xsKaUOVbax7iI0U6FmlFSVHtRbddphbXXC5b31sT+4QFpHo3RmV5reR09Mi/K6HwxpigH94qyQP9WgAn2mP3655jpfXIq1qw+wWxaXGmvFqN4U8Qsz7huEY3ckzbt4du30e0kTTvTnJbjdKa9rbNLbGVpc8R9MisA7UpQeBHbGfHx5SaluRD6xrFi1blb80mqUXR4l13/QnFLveP4zblLoHHJ/MNU6RyLJ2zSDsSfRsjeu4kZPqKzhcw3bEaNcXVXo+8qFp0iwi0yUqlNU8Pv1Kir9USHuj1DDhHoqY8jmLKu/vcK7C46KhoqBAaomUMtylytpCR7o2FFLciHndlcdZNvvO+P08CAEoAQAgBACAEAIAQB8LO2ANd/ns6d6gZLaMLzuwWqorcNsor27rXUrZcRSrqC34Zd5QSlJCT8UpRWeYbUpcLS2Kp230dz7Fqd61KaU58NE3StK1NaO/aNx3RVjvZOmEJ5cPtg/8ATte9ERd3zF3wtlmHcbQ+jW4mm6cMQppyAogf6zy1fXHTtDX9Jb7vifDfqbLi17L/AB/9KJttNWXUjbEqUQrDS5pCoA82QSTY63sDD3+GY8z3My2FWce9GH1wom6hnw3khSOIPfEMjqF2CZaVdpZQ3m6prGmGF1KE8iXqhCVOIRMGQVKcpx5moN1kk34HnHeRbTjauOMXvSbS+xkydOTeT224nEq2uVU2NLS3GadYmGSg7OQnaBt3bo3cS+5S4egrmvaXC1b+rX56rxr1l09SdULbp21a0K5Xa+pS3LtS2Cs/QIz5UqRoRGhW+K9xdS/YQbTsHlkreBGgXChXcNtIcefqynYkBtPdzDbGG5KhvYMNtRnjI+UZpkp2KVzmW8hIkIWj1nPYkWum3ymCJDdKM5ESgX5hNibtdqFQpIFVUSWqY3IlsH1xr3JVZNYVn6ca9LJw0Sw9ugtX/M9Yn98qppp+YbUtdvpV9ESOFZouIpvM2o/UufRi9kd/a/2F+lsnfG8VU45JcIMGPOvqkt6p1wRu5Kecv/ITEXk+dl90KdMaPe/eW3anwipQTuJlP0xqzVUWC3PaVpaApop4EFMYUbL2luESJHYSI2DRZVMfB+XcP6Q+gxinvNixuJc6exKkuZ/Ta/smJDT9z7yn83ea33P3kjkgCZ3RIlPKZmGTW/D8arMjuSgmmpG1OSP3lS+FI7yZCPyToqmWzadyaiukw+r75VXm51F4rFc1XVOKqHPS4oqPsnES9rqX+EVBKK3LYd9uUKiqaZR9pSkpl6TKPDNm03xJF8EhKSTuAJ9gjVJmtCys2xDG9RcVuWEZlRt1+KXZlyjr6N8BSHWnRtBn7QeBlGzGTjRojr1tXU4yWxmkrq/6XMi6Ltcq7SzIkuVOl125qzHLo4Dyv0qlbATKXitHYsD08Yn7N5XY1W9bznOo4MsW5wvc9q7iPtN8/wBQen/VS16maeV67dmdiqmrla69gyKVtq5kkS3pUJhQOwgkGM3mRoH6RfLo65cH69enS36s2FTdLm9Ly23LLMlQ5qG5toHPypPxeE79tsneDLeDGtJUBIXUKh5zTh4MoK5PMqXygmSQraTLgI1ctVgTXL7SyVXqZACTsiMRfSsWKQpCf0z9AjFM27O4mHQASx+uV21AH6giSwPKykc2v86HcX/G+VQHdAFtZNpdiuTumpfZNPXKmVP08klR7wRIxr3MWEyYwdcyMZUTqup7S2n+nxJXOnuZ5exxkE+0KjWen9pNR5tfTb+39h3UXT/bUKBuFe64n8LSEo955o/Vp8elmO7zbcflgl3tv7i57HpziWPqDtBRoVUjc898a/TNWweoRs28aENyIPK1fIv7JSdOpbCuJ2CUZyNOYAQAgBACAEAIAQAgBACAEAIA618RAEN9VGRZDabFSWiyVRp6evDzVWkIQsOthIHIrnB2GZjUy5tKnQyf0LEjdk5PfGlDWn1EdE9DX1FRkmmSE0F5WVOuWxWylfUTzKLR+4o9m6K5l6Wp/Nb2dh2jQee7mM1ay/mj0S6V39fvOrSfQvO8iprVjNShm1VKmm2FPXR0MtIWlISeYiZnPsEV6GiZF6b+Wir07DrWV6maRp9iP5jm0lsguJmxvQfC6/T3TOx4VXOoqK220qKdx+nmWlmZJKJ7ZGfZHQ8DHdizCDpVLoPj7mvWIarqV/KgnGNyTaT3rdv9hK9iSoIBMbhXyv037KAPPlJ5car19lM9/hmPE9zM+Mq3I9695ietgOIE98ohjqLO+y0SVVS+Yfd+sR4mzLYW0kbQe3JVmbhSPiFO5t9YEbGD5yF5r/4y/Ei1upnMKe95y3jtAsLobUgtuFJBBfcM3Bs/DICNjJlV9xFaLY4Lbk98vcWGytDg5ZbY1CbTLwxGlFNaAs73FKV6ZbIwXHtJXDjSBSsuUX7p4ezkbSE+s7YyQWw1sz5pdxTKC1/OV7NMdqVqAPonMx7k6I1bceKSRIdotxudzprUwPiecQyAOwqA+iNeEeJpdpLX7qtQlPqTf2GQ9FRtUNI1R04ky0lLaR3JEosMVRJHIbk3OTk97dTuj9PBwr7MAYu6x3lNz1Qu9S2qbaXvAEj/AKpIa/0YirzrJl+0qPBYiuz3lEoXwaluW/mT9MYGSsHRousdvDZ/LGtQky3XDNxR7z9MbKNFlTsH+7OfzvqEYp7zZs7iXen1MrbcV8PFbH6kSGnLY+8pvN3nt9z95IFVVU1HTLqqtaW6VsFbjiyEpSkbSSTsESNSopN7jGXqE1tTqDchjWOrJw+kXMubR806n7/80fd9saV65xOi3Fm07C+kuJ72RshZG2fpMa5LwudDLhwOjcrboatQPgU4n/SUJARhu7ESWDDilXqLyq6Opes9ZXMj8mnQlTqtuwLUEAD1mMMI129Rv376g1HpluLdnwG09kZTARV1p9HOL9aOgNfp1cktsZtRqXX4tdVgc1JXpRsSTwbdA5Vjs27xGSxkuzNPoNTUNOjl2XHpW5mkbIMVyjF7/c9I8/pV27UKwPvULlPUghYcaUUqRtlMGU0niJRYFLZVbmc1uW5W5OMt6ZMXlpdeuZdAPUbSah0vi1GnFxLdszGypKv3qg8T4lpTu8ZkkrbPpG4x+zjVHg/SLp9n2Gau4FatRsErWbrgt+pGrjba1gpW0/T1CAtJ4ie2Sgdx2GNZrrCdNxZuovT9Q3MuXfC+WmuBmtdEdjThO34D90+6NO7i12os2ncwShSN3auvp8esj+x4llPOu1G3v/xBtwpcb8NWw+ndEdO1JulC5W82zG3xOaSfaTFpBjd4xuwP095a8Goee8VCCQTy8gG2W6JLDtuEaMo3MObbybqdt1SVC7o2yAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAfLm+cAQr1Ys8wtCZT2Pn+yI0czoLXy0qqfgQLd7O1WILL6ApEuI2emNIs04KWxnmodMrKW01LzZcWsBUnDMD1CUeZXHuMlrBhRPeZKaOWNTWCWyQPKGyANpkAtQ4zMS2M6wRz7WoxhlTUVRVJBtlEppMiIzkWVVpMkhMAeLMVhGKXJR4Ur5/uzHi55WbGGq3Yd695iwlAKBLfIfREMdQPfjzcqhyf4frEY7hnsb2e+vzy96d29+5Y/yJudWj5JDyhMtBfxFaRuJ2bJxkxp8L8DQ1uxG9binuUqkXLU88+upfWVvuKK3FrJUpSlGZJJ48Yz0IpScUeuiLrryKdsFS1kISBxJMo/GjYty4nTpJIt1IqnpmaBsFbqUpbATtJVx98aT2ssEaQjt3JFs16TVVLj6vtKUTt7OHujOlQjpurqd+L0xF28Qj7CFEendH5cewy40ayqSPpNS/NZ9QpUJpQVu+tCCY9YircRq8wXOHEn20X2k4jdE2cyEAUvMclpMSxiuyOsISxSMrd28VASSn1qlHmToqmWxadyaiukw/duT1dUu1tUSX3lrdWT+Jaio++Ihs6FFJKi3I9lnPj3JhtG1RWNno2x+S3Gew6yReTivDaUs/ZAJn6o1UTDLeBmJnfGwaJVbB/uyz2q+oRiubzZs7i8MQ1mxTSix1bd5S6/dqlYcp6VhE+cJTyzKzsSJ9sb2FcUIupV+Y8OeRdhw7kviR3qfrvmOphNE8oUOOz+Ggp1GSgNxdUZFR9gjJO65Gni6fCz2sskdnCMRvndQUNXcqtFHRJKnl7Bs2AdpPCPLdD1C07jot5I+PWNNqo27ZSJ8SpUQCUiZccVs2S37Y1JScmWGzbjZh3bWTZjOlNsbwd3Hr8jnfr5OVZSZKSpJmgAj8P0xL2cZKFH0nPtR1md3JVyD2R3fH2kW6iaMZDhK119FOux7/Xtpm433OIH0iNS7juG1biw6drNvJ2SpGX9txQrCr91WeHNv4bu2NOe8sVncYSecb5blx1h01retXRSkKtTsRCE5XQUjf5txtbaNlUlKdqnWB9riUeiJjTLlYuL3FH5qtRjdjJLbJbTU/cXGL9bxkFEJVKJIrWk8FbueXfEmthVjZJ5Anmco0bzSn6MNbLh4eluR1P8A0fcKtcm7Zdn1/wC6qUrYlmpVunsS5/OjHciDduBPdu3RhBzykGcAOXZLjAHMAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAETEoAhrqnSDVWdtX4Xz70xoZj2ot3LC2Tfd8SH3qALG4ExplpaKvQ21CqJoyl8KfojXe83oLYif9LLahvA7ZskS1OXpWYnMZfIjl+ubcu53l0NMBvYBMxnIo7kplv3wBSs6PLht0V/6V7/DMeLnlZtYKreh3oxdR9geiIY6cyo2H9s5L8I+mMdwz2Ok8GpACrOyk8XhL08pj1Z3mDUVWCXaWhS2y4Vzgbo2VuqP4UmXt3RsOSRDRtSk6JF34lhqrSsXK5yVXbkoG0N7O3dONe5cqS+LhfT+aW8mHRHErXdULyp99t5TfNTssNqClNqlyqUscD2CNvDx/3mV3mTVmvyY97fX2EUZDaF2y8VducEnGHnWT/RWRGCWxtFgsyV2EZLpVRjrXJVOc2/l2e2MVwz48aNkgaNLSjUCmnvUh5I9Phn+SMuG/nXiRvMariS717ybBuiaObHHNAGP3VPqozdqtGnlhdC6SmV4txWgzSp1P2Wpjfy8e/wBEamRc6CxaRjcH5klte4h1p4jYTtjUcSxJp7i4sEpTUXBdYf2bKSJ/pK2CMN3cb2FHilUum5MVC7VUPMp/Kb5A4rgOcyHvjFCNTevXVGi6XuKD6N28RlNcq1i/3RXeo/RGK5vNmzuLY1M23WlA2gMnbv8AvnZGewtjIvU/Mu74luJQtxfK2krWdg5QSfYIzEdQrNnwW83JSXKlPy1LPaXB8fqTGKV1I3LWDOe/Yi9Mcxdm3lNvs7Knq10hJKRzOLUeHojBVzZKQt27Ea7l0tky6Y6WJx0pvt+AXe5flNb0sTHsKvoiSxsXh2veUrWteeR+Xb8nS+v9hfIEhKN4rB8raQtJSsTSd4O4jvgKlvI0nwJFwcuQt6PFdPOpvb4XN2hE+WMDxoN1oSsdayYw4FNpfb7Sst2S0M0K7a3TNC2uJLTrAbT4a0KBSUqTKRBBkZxljFR3EdcuyuOsm2+0/P550nlyV3Qhr8dYtLqJS+mfNn3XqVpoKU3bK5wl2ooFSmAk7VNd2zhGzGXEjGYYVdMhlxFdbHCKVcn6d5pRStCh8QkRtCkmXrj9SP2pvq8j7zMU9YWj3/wfq1WpX1H4VTIbeddUPEvVpb5WWq4T3uoJCHgOMlfejFONAZ5DaI8H4IAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBAEN9UZndLQg8Gnj+ukRoZm9Fw5YXyz70RUUdkaRaC4KJATQt9vIn6IwM3o7kWJkV9yGjyKpFvr6hhDayG0tPOJCZcAAZCN2EnQrGbajK5JtLeXVpz1NZ/hdYinyB5V4sG5xmpI8dA4ltyUye5U42IX2t5D5OmwubVsZkXgepWJ6j2sXTF6pLoAHjML+F5lRH2VpP07o3IzUivXsedp0kjt1CXy4Ldl9lK9/YMebvlZm0/wDXh3oxhT9kRDnTCo4+JuuegD3xjuGex0nvetzNxUhp5nxik8yUcvNt3bANseVXoMs1GnzUKmvDsgobK9e3qFxmy06fEccWkIATOWxJkT6hHv6M6VpsNL/uePGSgpLifQtpSGblRVH2FHb2gj/ujHRo3Fci9xUrDfbljVeLpZnPBfEucp+ysA7lDjHu3clDczBlYVvJjwzVfeu479SqVF9Uzn9vb5KWv/KrUDc1WIElA9yhIgxnvPj+ddJGafF2W7DdXHan1xf3FtWuTVbI7ykj1xgk6olbW8ufDbsmyZTQ3NZ/LbdTz/zVHlP0x+2J8M0zFqWO7+POHWvdtJxvGU47j1Ia291rFLSAT53nEp2dwJmYneJdZymNqUtiRC+r3VSzUUruP6ZFR55tu3VYKeUbiGUnbPvMa9y/0ImsPSnXiuewgxTji1lxxRU4o8ylKMySd5JjVJ5Kh9tBb7iWmhN5R5UpHEncI/NwjVPYSJjloFktaKRW2oVJbp7VHh6o05yqyyY9r6cEukmTE9J7dX6fuWnIUKTVXApqFlBktvlE2/ZvlEpYxkoUfSUbVdal/NcVvdDZ39ZGWoGkmSYG8qoWk1dhJ+CsaB+Edjg+6e/dGtdsuHcTun6tbylTdLq+4pdh20XNw5zt9kak95PWtxzX2S1XN5NTXsJefSORBWSZJnPcDKPxSa6RPHhN1aqe+yYrVVSvAsFApat35DX1gbPbHpKUt20xzvWbC+ZqKLzx/Q7IbiUu3xxNFTHapAk46fUNg9sbVvBlLfsILM5os26q2nJ/YSJiuDY7ibXJa2f3kiS33PicV6zu9USFqxGG4p+dql7LdZvZ1dBWgAN0ZiPEAIAQAgCP+qHpv026stD79oPqrSioxW+MKZ8QAF2kqAklmpZJ3ONLkoezdH6nQH5oepzpn1H6Ltfb902autlFXQPlVsuABDNZSOEmmq2ifuOJlPsMxwjPv2g8OgutupXTDrFY9bNK6s0OeY7UpqmCD+U8gGTtO8NymnUTSobpHtlB7dgP0pdFHV5p51tdPtl1008WG0VrYp7xa1KBetlzaSPmKR0bD8KjNJ+8kg8YwNUBLQ3bd8fgEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQBC/VAv/j9qTPcw7s/+4Ij8zei48s7Lc+9EYenfGmWcuCkA+VQOHInd6IwM3o7igVGAMXC6P3GvfJadWVJbaEpA9pMZVeoqEfLAUpOUnvPcxheMsgD5UL/SdKlGftjw7sjPHCtLoPdZ7fS45cW7vj4NFc29qXaZSkGXYRORHcRH7G7KO5ni7p9m6qSiqElL1UayfArtaLuA1fBSO8qk7EPSTtKew90SEMrji095Vb2gvGvwnB1hxLvW0hdO6NMtBUcfn4jx7An64x3DPY6SUNAmm3L5XLWkKKWUlJIBI/M4Rt6f5n3Fb5tdLUKdfwPX1OZpSY9gJsQX/wATui0soQCJ+Eg861EdmyUbuTKkaFb0WxxXuLoj7yAqC6gGU5CIxxLxC6XPYa01tMrm2lJl7RGCSoSVmfEiTtGrXRZDZ7vYLqjxbc4WllJ4KkRMHgY38KPGmnuKtzNelYnauQ2SVS2c50kvmGVBuNGDV2NJmH0D4kDfJxI3ekbIx3sWUNu9Ehpmt2sqkX8s+rr7iipUFALTx2zjSJ4sfO8ZrqauVd6QKeoXPiXtKlNnjLeZGNu3dqtpB5mG4y4orYy3gpKjs9HfGU0KndRUFbcnQzQtKcdJl8AmPbHlySPcISnuVS9MVwxuzSrrhJdxM+UDalv0dpjXndruJnEw/p7ZbyWtKNNnbnUoyW+N8ttbPNTNLEi6oblEHgPfG1iY1XV7iE1/Wlai7Vt/M976v2ksJAIiUKEfLrDL7SmX0BbKgUqQoAgg8CDH41U9JtOqLVOi+AfNuVCKVaEuKKyyhxSWwTvkOHtjXeHBupM2+YcqEVFPxoqlToNPcLtkjS25nnEtq0lw/rzj3HHhHcjXu6vk3fNcfu9xV2mGadAaYQlDY3JQAAPUIy0I6UnJ1bqfXKN8fp+HMhACAEAIAQAgAQDvgDC7zofLYoOuvp/cyjAqRI6jMOZercddSAF3CnSkuO25auPP9psncvuJj3CdNgPz7M/xBl9/H76yunyK3LXTVDD6ShwFlRbUlSVblpIIIjK1TaDKbynfMTvfQN1CM1F/dcf0AydbVvy+3oJUGE8wQ1cWkf6xie38SJjsj8lGqPxH6JsfyGy5XYqLJ8bqm67Hriy3WUNZTKC2n2HkBxtaFDeFJIIjAfp7oAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgCFOp5Q/wCZbaOIplH2uERH5fmRc+Wf0p96IzjTLKXBT7GG+zlH0CNdm7HcShhOjdgq7TTXi9uLqXKhtD4ZSfDQkKTzS2bTEpZw4tVZSNT5jvQuShbSjwtqu97Cw9d02vGs3Zs9hQKamRTNqW02TLmUomZ2nbKMWTaUZURIaJm3btpucqupbluvCnCEPGYOyNOUCxQu1KitCXWy0dyhy+0SjwntMzVS3SnlJTwEx7DKNhGiypY8Nrqv5o+mMdwz2OkuXGtVrVpOxWXKtYXVV9UhLNIyiSQpaVFR51HcI2MO4oNkHzFiPIjBJ02/AivNs6yDUC/O5FkTnPUr+FDaf2bTc5pQgcAOMZpz4ntNHHx42Y0iU1ipWg79keKG3C7TeX1hTLotHzLmwvKJSP0U7AfbGpce0m8NfJXrJw0HtLlLj1TdnRL5t38v+Y2JT9sSWDBqNespnNWQp3owX7q2+JfKkIUkoWApCpgg7iDwM43qFXTLCzfRWhuHPc8UlTV21S6U7GnD+j+E+6NC/hp7Ylq0vmOVqkL22PX0r7yNLpZLtZH1Ul2p1sPAyIcTs9u6I2UHF7UXaxk270awaaKW7ZbM8vxnKVpTvbyJn7o/FJnp2IPeir2LDr7eVJYsVCtTe6aEBDY9KjIR7jblPcjXv5tjGXzSS9/sJGwvROlt7iLjlSxUVI+JNK3tbSR+I/ePuiQsYSW2RUNS5mlcTjZXCuvp8C/2kISgIQAEAAAASAA4SjfpQqlW959yA3QAgBIQAgBACAEAIAQAgBACAEAcLMoA0x/5hPyzXMGv7nXloVQeHjte6hOeW+jRspaxU+W4hI3NumSXJDYrbxMZ7cq7Aax8Vst+zqsTRYnQvV1e4klVPStlZbI2q5uAT3mUfraitp7hbcnRKpuY8hPqd1SxHDU9J+v7zf8AAELKsAq11CX3mARN22OeHzJCBLmaJVs2p7I1VfhcfymzewbluPFJGz2PRpiAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAQh1Oq/wCrqBPZST/vVRHZnm8C6csr8qX4vgRvwjULGXAz+wblu5R9Ea8jfRO1uv1pxXTqlvd7eSxbaakaW44r/wAsSAHEncBE9adIKpyjNg7mTNLe5P3mLOa51VZrmNdk74KE1K5stznyNJHKhPqSPbGhc+Z1LbhQVm2oroPihuUykTjC0SNu5tL2QZJBVvAmY1iZLdcM3VHvJ98bCNJ7WVLH/su+lP1xjuGezuKLqcT4NGOHM5/Zj3Y6TR1PdEtFCVOLDaASs7gJk+6NhkStpcGPYLXV7qam6pLNCDMpV+0X3S4RhndSN/HwJS2y3El4ZiFblNybtNtRy0qJeM6kfC0gbJ9k+7jHi1alclRG3n59vDt8T8ETxaLbS2i3M2yiTyUrCA2gdwETkYqKojl1687s3OW1s9MejEIA6amkp6tHh1LaXG/wuJCh7FTj8onvPUZyi6ptM86cdsLavEboacL7Qy3P6I8K3HqRmlmXmqccvaz2NtpQkJQAlI4ASEZDXbrvPqAEAIAQAgBACAEAIAQAgBACAEAIAQBSs1w/GtQcVuODZnRN3HErvTPW65UNSgLafp30FtxCge0GP1A1t4d5NN20S1Av2JYdbl1mjdU+ty0qoCyipdpXTzBuqqHlghSN05bY8Kwrr/Mls6kStvOjaglBbTLHpo6HMZ0hraW/19vo6J6jIdpaGkSHXfFG5yoqFAFSgdwTs74z/l248MI0NK/lTuusnUyIjEa4gBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBAEG9TKp5tSJ4ijR73VxG5nm8C78tL8l/i+CI64RqlgLhpwfCR2co+iNdm+iz9RNSctzN1Fnu1RKx0H5FLSMjlb/LHIFqH3lbOMSP1G0io/wAtC3cnJLa2y2QqW07uMfhkp1FZw63vXO7ICZ/KMkOOmW6W0D1xiuNJG5hRc59iJWwnGFZfkLNoWFfKGa6lSdnK2kdvDujFj2vqSSN7Vc1Ythz6ejvOvUXRK/4eXLnaQqvx4TVzoTN1of7QDf6RG3dx3DduIXTtbhkbJfLP3lt48QptwjaJ7x6I0rm0slo77nZ7ZduQ3FsOBsko5iRv37jHiM6Hq5Zjc8yqeuyYouocDNgoCtw7B4DRJ/rbfpj2lKRinctY6+ZqKL4xnQ693BSX8icFJSbCWkyU6r2bBG3aw5PzbEV/N5otQqrS4n19BJlgxy041Qi3WdkNU42mW1Sj2qO8mJK3bUFRFLy8u5kz4purPeI9msIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgCCupZROe0wP2RRtyPf4qyZRGZfm8C88t7LD/ABfAj0DZKNZk+XCzsaRLgB9Ea5vIjistdzrbnUfK07i5urI5UHiqcbsZJLeVydqUpuie8qNq08ulUsKuShTM7yJhS5egR4leXQbFrT5y2y2IvfF8WWtTVix2nU4+rZyo2knipSvrjCk7j2ElOdrFhWTpFf29pOOnmB0+F2woUQu7PSVUOjd3IT3CJnHsK2u1nONX1SWbc6orcviXCWgrYransO6M5ElsV2jeC19xdua6UtvPEKcbYWptskceVOwH0RrzxbcnVomMfXsmzDhUl4qp7bfprhNskqnt7RcH3nQXD+tOP2ONBdBivazlXN837vcVhmkp6ZvwqZCW290m0hI9gjMkluI6cnJ1bqzsCZR+nk5gBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIApOWYVj+aUBob6wHAP2bqfhcbPalW8R4nbU95tYmbcxpcUHQhDUPRnIMHcVXUgNbjgJPzDYPO0P9qkbvSNkRt3HcO1F107WbeTsfyy6vuPDTMv1KUpp21OKIAkhJV9AjRSruLLKajvdCr2zT3MrsR8pb3Q0dy3B4afaqUZo485bkaF/V8a1vmq+33F1WHQWtdWlzIapLbY2lmmmVegqMgPVONq3gP957CBy+a4rZai32v7iQMdxOw4tTfLWZhLYP23DtWr+co7TG/btRgthVMzPu5Mq3JV9xUQJRkNM5gBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIA+VNIWkpWJpOwg7QRALYfDVFR04kw0hA/QSB9EfnCj1Kcpb22dnKOMfp5ASAZiAOYAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAEAIAQAgBACAP/2Q==' class='img - circle elevation - 2'> &nbsp;&nbsp;&nbsp;" + item.name + "&nbsp;&nbsp;&nbsp;" + item.cmname + "&nbsp;&nbsp;&nbsp; Rs. " + item.actprice: "<img width='50' height='50' src='data: image/png;base64," + Convert.ToBase64String(item.photo) + "' class='img - circle elevation - 2'> &nbsp;&nbsp;&nbsp;" + item.name + "&nbsp;&nbsp;&nbsp;" + item.cmname + "&nbsp;&nbsp;&nbsp; Rs. " + item.actprice;
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
