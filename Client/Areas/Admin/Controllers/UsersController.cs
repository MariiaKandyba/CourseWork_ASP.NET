using Client.Areas.Services;
using DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            var a = await _userService.GetAll(token);
            return View(a);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserDto);
            }
            var token = HttpContext.Request.Cookies["jwtToken"];
            var result = await _userService.AddUser(createUserDto, token);

            if (result)
            {
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index", "Users"); 
            }
            else
            {
                TempData["Error"] = "Error adding user";
            }

            return View(createUserDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            var result = await _userService.DeleteUser(id, token);

            if (result)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the user.";
            }

            return RedirectToAction("Index");
        }

    }
}
