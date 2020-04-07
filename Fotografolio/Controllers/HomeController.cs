using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fotografolio.Data.Interfaces;
using Fotografolio.Data.ViewModels;
using Fotografolio.Helpers;
using Fotografolio.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Fotografolio.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IUnitOfWork unitOfWork,
            IHostEnvironment env,
            NpmManager npmManager)
        {
            this.UnitOfWork = unitOfWork;
            this.env = env;
            this.npmManager = npmManager;
        }

        public IUnitOfWork UnitOfWork { get; private set; }
        public IHostEnvironment env { get; private set; }
        public NpmManager npmManager { get; private set; }

        public IndexPageViewModel NewModel()
        {
            var VueFilesModel = npmManager.CopyBuildFilesToModel();
            var AuthModel = new AuthViewModel(UnitOfWork);
            return new IndexPageViewModel() { VueFilesViewModel = VueFilesModel, AuthViewModel = AuthModel };
        }

        public IActionResult Index()
        {

            var model = NewModel();

            return View(model);
        }

        public IActionResult Changer()
        {

            //var appCss = @"//*[@id='app']/main/div/link[1]";
            /// css / app.0f82a377.css
            //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //doc.Load(Path.Combine(env.ContentRootPath,"clientapp","dist", "index.html"));

            //// select recursively all A elements declaring an HREF attribute.
            //foreach (HtmlAgilityPack.HtmlNode node in doc.DocumentNode.SelectNodes("//a[@href]"))
            //{
            //    node.ParentNode.ReplaceChild(doc.CreateTextNode(node.InnerText + " <" + node.GetAttributeValue("href", null) + ">"), node);
            //}
            return Ok();
        }

        [ValidateAntiForgeryToken]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] UserViewModel model)
        {
            //bool validAntiForgeryToken = false;
            //string errorMessage=null;
            //if (UnitOfWork.antiforgery.IsRequestValidAsync(HttpContext).GetAwaiter().GetResult())
            //    try
            //    {
            //        UnitOfWork.antiforgery.ValidateRequestAsync(HttpContext).GetAwaiter().GetResult();
            //        validAntiForgeryToken = true;
            //    }
            //    catch (Exception exception)
            //    {
            //        validAntiForgeryToken = false;
            //        errorMessage = exception.Message;
            //    }
            //if(!validAntiForgeryToken) return Ok(new { errors = "AntiForgeryToken Error"+ errorMessage });
            //return Ok(new
            //{
            //    alma = UnitOfWork.antiforgery.IsRequestValidAsync(HttpContext).GetAwaiter().GetResult().ToString()
            //});

            if (!ModelState.IsValid)
                return Ok(new { errors = ModelState.Errors() });
            //var login = UnitOfWork.UserService.LoginAsync(model.email, model.password).GetAwaiter().GetResult();
            var login = "";

            var user = await UnitOfWork.UserManager.FindByEmailAsync(model.email);

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Email, user.Email.ToString())
            },
             CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.User = new ClaimsPrincipal(claimsIdentity);

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, HttpContext.User);
            //await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
            var userPrincipial = await UnitOfWork.UserService.LoginAsync(model.email, model.password);

            //await  HttpContext.SignInAsync(User);
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //if (userPrincipial == null)
            //    return Ok("Bad");
            return Ok(new {
                loginStatus = UnitOfWork.SignInManager.IsSignedIn(new ClaimsPrincipal(claimsIdentity)).ToString(),
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString(),
                userPrincipials= userPrincipial.Identities.Count().ToString(),
                status66 = UnitOfWork.SignInManager.IsSignedIn(userPrincipial).ToString(),
                status67 = userPrincipial.Identity.IsAuthenticated.ToString(),
            });
        }

        [Route("test1")]
        [HttpGet]
        public IActionResult TestAsync1()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test2")]
        [Authorize]
        [HttpGet]
        public IActionResult TestAsync2()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test3")]
        [Authorize(AuthenticationSchemes =AuthorizationPolicies.JWT)]
        [HttpGet]
        public IActionResult TestAsync3()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test4")]
        [Authorize(AuthenticationSchemes = AuthorizationPolicies.Cookie)]
        [HttpGet]
        public IActionResult TestAsync4()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test5")]
        [Authorize(AuthenticationSchemes = AuthorizationPolicies.JWTorCookieNormalAuthorize)]
        [HttpGet]
        public IActionResult TestAsync5()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test5and")]
        [Authorize(Policy = AuthorizationPolicies.JWTorCookieNormalAuthorize)]
        [HttpGet]
        public IActionResult TestAsync5and()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }

        [Route("test6")]
        [AuthorizeMultiplePolicy(AuthorizationPolicies.JWTandCookie, true)]
        [HttpGet]
        public IActionResult TestAsync6()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }
        [Route("test7")]
        [AuthorizeMultiplePolicy(AuthorizationPolicies.JWT, false)]
        [HttpGet]
        public IActionResult TestAsync7()
        {
            return Ok(new
            {
                status2 = User.Identity.IsAuthenticated.ToString(),
                status3 = HttpContext.User.Identity.IsAuthenticated.ToString(),
                status4 = UnitOfWork.SignInManager.IsSignedIn(User).ToString(),
                status5 = UnitOfWork.SignInManager.IsSignedIn(HttpContext.User).ToString()
            });
        }

        //[Authorize(Policy = AuthorizationPolicies.JWTandCookie)]
        //[Authorize(Policy =AuthorizationPolicies.Cookie)]
        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await UnitOfWork.UserService.LogoutAsync();
            return Ok(new { message="logged out"});
        }
    }
}