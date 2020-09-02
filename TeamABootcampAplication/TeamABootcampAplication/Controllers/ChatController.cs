namespace SignalR.Hubs
{
    #pragma warning disable SA1600 // Elements should be documented

    using Microsoft.AspNetCore.Mvc;
    using RestSharp.Extensions;

    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            if (!this.Request.Cookies["sessionID"].HasValue())
            {
                return this.Redirect("/");
            }

            return this.View("Index");
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}