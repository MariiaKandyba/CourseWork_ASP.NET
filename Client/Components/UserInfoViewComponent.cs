using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Components
{
    public class UserInfoViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var userModel = new User
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated
            };

            if (userModel.IsAuthenticated)
            {
                userModel.FirstName = HttpContext.User.FindFirst("firstName")?.Value;
                userModel.LastName = HttpContext.User.FindFirst("lastName")?.Value;
            }

            return View(userModel);
        }
    }
}
