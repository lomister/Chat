namespace TeamABootcampAplication.Controllers
{
    #pragma warning disable SA1600 // Elements should be documented

    using Microsoft.AspNetCore.Mvc;
    using RestSharp.Extensions;
    using TeamABootcampAplication.Models;
    using TeamABootcampAplication.Models.Repositorys;

    public class RegisterController : Controller
    {
        private IUserService userService;

        public RegisterController()
        {
            this.userService = new UserService(new UserRepository());
        }

        public IActionResult Index()
        {
            if (this.Request.Cookies["sessionID"].HasValue())
            {
                return this.Redirect("/chat");
            }
            else
            {
                return this.View("Index");
            }
        }

        public IActionResult Register(string username, string password, string email, string avatar = "")
        {
            if (!this.userService.CreateUser(new Models.User(username, email, avatar, password)))
            {
                this.ViewBag.MessageGood = "*Registration successful";
            }
            else
            {
                this.ViewBag.Message = "Cannot register";
            }

            return this.View("Index");
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}