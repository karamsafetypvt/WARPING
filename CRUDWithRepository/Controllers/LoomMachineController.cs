using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace CRUDWithRepository.UI.Controllers
{
    public class LoomMachineController : BaseController
    {
        private readonly ILoomMachineRepository _MachineRepo;

        public LoomMachineController(ILoomMachineRepository MachineRepo)
        {
            _MachineRepo = MachineRepo;
        }
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var Machine = await _MachineRepo.GetAll();
            return View(Machine);
        }
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new LoomMachine());
            }
            else
            {
                try
                {

                    LoomMachine Machine = await _MachineRepo.GetById(id);
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
                TempData["successMsg"] = $"Loom Machine details not found with Id : {id}";
                TempData["ReturnCode"] = -1;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(LoomMachine model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ID == 0)
                    {
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = "Admin";
                        await _MachineRepo.Add(model);
                        TempData["successMsg"] = "Loom Machine created successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                    else
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = "Admin";
                        await _MachineRepo.Update(model);
                        TempData["successMsg"] = "Loom Machine updated successfully!";
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
                TempData["ReturnCode"] = 11;
                return View();
            }

        }
 
    }
}
