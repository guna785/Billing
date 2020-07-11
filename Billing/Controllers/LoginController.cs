using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Billing.Models;
using Billing.Services;
using BL.BLService;
using DAL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Billing.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticateService _authenticate;
        private readonly IUserRefreshTokenRepository _userRefreshToken;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _admin;

        public LoginController(IAuthenticateService authenticate, IUserRefreshTokenRepository userRefreshToken, IConfiguration configuration,
                                IAdminRepository admin)
        {
            _authenticate = authenticate;
            _userRefreshToken = userRefreshToken;
            _configuration = configuration;
            _admin = admin;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("err") != null)
            {
                ViewBag.err = HttpContext.Session.GetString("err");
                HttpContext.Session.Remove("err");
            }
            else
            {
                ViewBag.err = "";
            }
            return View();
        }
        public IActionResult ForgetPassword()
        {
            if (HttpContext.Session.GetString("err") != null)
            {
                ViewBag.err = HttpContext.Session.GetString("err");
                HttpContext.Session.Remove("err");
            }
            else
            {
                ViewBag.err = "";
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PassChange()
        {
            var newpass = HttpContext.Request.Form["newpass"];
            var comfirmpass = HttpContext.Request.Form["comfirmpass"];
            if (newpass.Equals(comfirmpass))
            {
                var adm =await _admin.GetAdmin();
                var a = adm.FirstOrDefault();
                a.password = newpass;
                var res =await _admin.UpdateAdmin(a);
                if (res.Contains("successfull"))
                {
                    return Redirect("/Login/");
                }
                else
                {
                    HttpContext.Session.SetString("err", res);
                    return Redirect("/Login/ForgetPassword");
                }
            }
            else
            {
                HttpContext.Session.SetString("err", "Password and ConfirmPassword Not Match");
                return Redirect("/Login/ForgetPassword");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LoginSubmit()
        {
            var uname = HttpContext.Request.Form["Username"];
            var pass = HttpContext.Request.Form["Password"];
            var res = await Authenticate(new AuthenticateModel() { Username = uname, Password = pass });
            if (res.Equals("success"))
            {
                return Redirect("/Home/");
            }
            else
            {
                HttpContext.Session.SetString("err", "User Name / Password Error");
                return Redirect("/Login/");
            }

        }
        [AllowAnonymous]
        public async Task<string> Authenticate(AuthenticateModel model)
        {
            if (!ModelState.IsValid)
            {
                return "Required Fields not Filled";
            }
            var usr = await _authenticate.Authenticate(model.Username, model.Password);

            if (usr == null)
                return "Username or password is incorrect";

            HttpContext.Session.SetString("JWToken", usr.Token.Token);
            _userRefreshToken.SaveOrUpdateUserRefreshToken(new Helper.UserRefreshToken() { RefreshToken = usr.Token.RefreshToken, UserName = usr.uname });

            return "success";
        }
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> UserRefreshToken([FromBody] JwtToken jwtToken)
        {
            if (jwtToken == null)
            {
                return BadRequest("Invalid request");
            }
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            IPrincipal principal = handler.ValidateToken(jwtToken.Token, GetTokenValidationparameter(), out validatedToken);
            var userName = principal.Identity.Name;
            if (_userRefreshToken.CheckIfRefreshTokenIsValid(userName, jwtToken.RefreshToken))
            {
                var role = principal.IsInRole(Role.Admin) ? Role.Admin : Role.User;
                var token = _authenticate.GenerateJwtToken(userName, role);
                HttpContext.Session.SetString("JWToken", token);
                var newjwtToken = new JwtToken() { RefreshToken = new RefreshTokenGenerator().GenerateRefreshToken(32), Token = token };
                _userRefreshToken.SaveOrUpdateUserRefreshToken(new Helper.UserRefreshToken() { RefreshToken = newjwtToken.RefreshToken, UserName = userName });
                return Ok(newjwtToken);

            }
            return BadRequest("Invalid Request");
        }

        private TokenValidationParameters GetTokenValidationparameter()
        {

            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return Redirect("/Login/");
        }
    }
}
