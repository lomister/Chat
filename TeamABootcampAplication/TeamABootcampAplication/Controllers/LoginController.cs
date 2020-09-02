namespace TeamABootcampAplication.Controllers
{
    using System;

    #pragma warning disable SA1600 // Elements should be documented

    using Microsoft.AspNetCore.Mvc;
    using RestSharp.Extensions;
    using TeamABootcampAplication.Models;
    using TeamABootcampAplication.Models.Repositorys;
    using TeamABootcampAplication.Models.Services;

    public class LoginController : Controller
    {
        private IUserService userService;
        private IEncryptionService encryptionService;
        private ISessionService sessionService;

        public LoginController()
        {
            this.userService = new UserService(new UserRepository());
            this.sessionService = new SessionService(new SessionRepository());
            this.encryptionService = new EncryptionService();
        }

        public IActionResult Index()
        {
            if (this.Request.Cookies["sessionID"].HasValue())
            {
                return (IActionResult)this.Redirect("/chat");
            }
            else if (this.Request.Cookies["username"].HasValue() && this.Request.Cookies["password"].HasValue())
            {
                this.ViewBag.username = this.Request.Cookies["username"];
                this.ViewBag.password = this.encryptionService.Decrypt(this.Request.Cookies["password"]);
            }

            return this.View();
        }

        public IActionResult Login(string username, string password, string remember)
        {
            int id;
            try
            {
                id = this.userService.Login(username, password);
            }
            catch (ArgumentNullException)
            {
                return this.View("index");
            }

            if (id == 0)
            {
                this.ViewData["Message"] = "*Username or password invalid";

                return this.View("Index");
            }
            else
            {
                if (remember == "on")
                {
                    this.Response.Cookies.Append("username", username);
                    this.Response.Cookies.Append("password", this.encryptionService.Encrypt(password));
                }

                this.Response.Cookies.Append("sessionID", this.sessionService.GenerateSessionID(id));
                this.Response.Cookies.Append("username", username);

                return (IActionResult)this.Redirect("/chat");
            }
        }

        public IActionResult Logout()
        {
            if (!this.Request.Cookies["sessionID"].HasValue())
            {
                return this.Content("Cannot logout");
            }

            if (this.sessionService.RemoveSessionID(this.Request.Cookies["sessionID"].ToString()))
            {
                this.Response.Cookies.Delete("sessionID");
            }

            return (IActionResult)this.Redirect("/");
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}