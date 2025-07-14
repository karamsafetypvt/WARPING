using CRUDWithRepository.Models;
using CRUDWithRepository.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using System.Diagnostics;
using System.Numerics;

namespace CRUDWithRepository.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDashboardRepository _DashRepo;

        public HomeController(IDashboardRepository DashRepo)
        {
            _DashRepo = DashRepo;
        }

        public async Task<IActionResult> Index()
        {
            DashboardModel model = new DashboardModel();
            model.CreatedBy = HttpContext.Session.GetString("UserName");
            model.ID = HttpContext.Session.GetInt32("UserRole");
            var data = await _DashRepo.GetAllData(model.ID,model.CreatedBy);
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
