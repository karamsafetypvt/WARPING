using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CRUDWithRepository.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _UserRepo;

        public UserController(IUserRepository UserRepo)
        {
            _UserRepo = UserRepo;
        }
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var Machine = await _UserRepo.GetAll();
            return View(Machine);
        }
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new User());
            }
            else
            {
                try
                {

                    User Machine = await _UserRepo.GetById(id);
                    if (Machine != null)
                    {
                        return View(Machine);
                    }
                }
                catch (Exception ex)
                {
                    TempData["successMsg"] = ex.Message;
                    TempData["ReturnCode"] = -1;
                    return View();
                }
                TempData["successMsg"] = $"User details not found with Id : {id}";
                TempData["ReturnCode"] = -1;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ID == 0)
                    {
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = "Admin";
                        await _UserRepo.Add(model);
                        TempData["successMsg"] = "User created successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                    else
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = "Admin";
                        await _UserRepo.Update(model);
                        TempData["successMsg"] = "User updated successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    TempData["successMsg"] = "Model state is Invalid ";
                    TempData["ReturnCode"] = -1;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["successMsg"] = ex.Message;
                TempData["ReturnCode"] = -1;
                return View();
            }

        }
 
    }
}
