using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CRUDWithRepository.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _AccRepo;

        public AccountController(IAccountRepository AccRepo)
        {
            _AccRepo = AccRepo;
        }

        // GET: Login Page
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handle Login
        [HttpPost]
        public async Task<IActionResult> Index(User model)
        {
            var user = await _AccRepo.Login(model.UserName, model.Password);
            if (user != null)
            { 
                HttpContext.Session.SetInt32("UserID", user.ID);   
                HttpContext.Session.SetString("UserName", user.UserName); 
                HttpContext.Session.SetInt32("UserRole", user.UserRole);   
                HttpContext.Session.SetString("EmployeeCode", user.EmployeeCode);   
                HttpContext.Session.SetString("Email", user.Email);
                if (user.UserRole == 1)
                {
                    HttpContext.Session.SetString("Role", "Floor Manager");
                }
                else if (user.UserRole == 2)
                {
                    HttpContext.Session.SetString("Role", "Supervisor");
                }
                else if (user.UserRole == 3)
                {
                    HttpContext.Session.SetString("Role", "Operator");
                }
                else if (user.UserRole == 4)
                {
                    HttpContext.Session.SetString("Role", "Admin");
                } 
                else
                {
                    HttpContext.Session.SetString("Role", "");
                }
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.Message = "Invalid username or password.";
            }
            return View(model);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Index");
        }
    }
}
