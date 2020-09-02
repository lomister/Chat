namespace TeamABootcampAplication.Controllers
{
    using System;

    #pragma warning disable SA1600 // Elements should be documented

    using Microsoft.AspNetCore.Mvc;
    using RestSharp.Extensions;
    using TeamABootcampAplication.Models;
    using TeamABootcampAplication.Models.Repositorys;
    using TeamABootcampAplication.Models.Services;

    public class ResetPasswordController : Controller
    {
        private IUserService userService;
        private IEmailService emailService;

        public ResetPasswordController()
        {
            this.userService = new UserService(new UserRepository());
            this.emailService = new EmailService();
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

        public IActionResult ResetPassword(string username, string email)
        {
            try
            {
                string newPassword = this.userService.ChangePassword(username, email);

                if (newPassword.Length != 0)
                {
                    if (this.emailService.SendEmail(email, newPassword))
                    {
                        this.ViewBag.MessageGood = "Check your email!";
                    }
                    else
                    {
                        this.ViewBag.message = "*Can not send email. Try Again!";
                    }
                }
                else
                {
                    this.ViewBag.message = "*Can not generate new password. Try Again!";
                }
            }
            catch (ArgumentNullException)
            {
                this.ViewBag.message = "*Something went wrong. Try Again!";
            }

            return this.View("Index");
        }

        public IActionResult ResetPasswordFromUserPanel(string password, string currentPassword)
        {
            string username = this.Request.Cookies["username"];
            if (this.userService.ChangePasswordFromUserPanel(username, password, currentPassword) != default)
            {
                return this.View("Index");
            }
            else
            {
                return this.ViewBag.message = "Invalid old password";
            }
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}