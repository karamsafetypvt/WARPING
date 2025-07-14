using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Numerics;

namespace CRUDWithRepository.UI.Controllers
{
    public class WarpingPlanController : BaseController
    {
        private readonly IWarpingPlanRepository _PlanRepo;
        private readonly ILoomMachineRepository _LoomMachineRepo;
        private readonly IWarpingMachineRepository _WarpingMachineRepo;
        private readonly IProductRepository _ProdRepo;

        public WarpingPlanController(IWarpingPlanRepository PlanRepo, ILoomMachineRepository LoomMachineRepo, IWarpingMachineRepository WarpingMachineRepo, IProductRepository ProdRepo)
        {
            _PlanRepo = PlanRepo;
            _LoomMachineRepo = LoomMachineRepo;
            _WarpingMachineRepo = WarpingMachineRepo;
            _ProdRepo = ProdRepo;
        }

        public async Task<IActionResult> Index(string? filterId, DateTime? filterDate, int? filterStatus)
        { 
            if (filterId != null && filterId.StartsWith("WP00"))
            {
                filterId = filterId.Substring(2);  // Remove the "WP" prefix
            }
             
            string filterIds = HttpContext.Request.Query["filterId"];
            string filterDates = HttpContext.Request.Query["filterDate"];
            string filterStatuss = HttpContext.Request.Query["filterStatus"];
             
            ViewBag.FilterId = filterIds ?? "";
            ViewBag.FilterDate = filterDates ?? "";
            ViewBag.FilterStatus = filterStatuss ?? "";
             
            int? UserRole = HttpContext.Session.GetInt32("UserRole");
            string? CreatedBy = HttpContext.Session.GetString("UserName");
             
            int? filterIdInt = null;
            if (int.TryParse(filterId, out int parsedFilterId))
            {
                filterIdInt = parsedFilterId;
            } 
            var machine = await _PlanRepo.GetAll(UserRole, CreatedBy, filterIdInt, filterDate, filterStatus);

            return View(machine);
        }

        private async Task PopulateViewBagAsync()
        {
            // Populate Loom Machine List
            var loommachineList = await _LoomMachineRepo.GetAll();
            var loommachineSelectList = loommachineList.Select(loom => new SelectListItem
            {
                Value = loom.ID.ToString(),
                Text = $"{loom.After} ({loom.Before})"
            }).ToList();
            ViewBag.LoomMachineList = loommachineSelectList;

            // Populate Product List
            var productList = await _ProdRepo.GetAll();
            var productSelectList = productList.Select(product => new SelectListItem
            {
                Value = product.ID.ToString(),
                Text = product.ProductName,
                Group = new SelectListGroup { Name = product.Description  }
            }).ToList();
            ViewBag.ProductList = productSelectList;

            // Populate Warping Machine List
            var warpingmachineList = await _WarpingMachineRepo.GetAll();
            var warpingmachineSelectList = warpingmachineList.Select(warping => new SelectListItem
            {
                Value = warping.ID.ToString(),
                Text = $"{warping.After} ({warping.Before})"
            }).ToList();
            ViewBag.WMList = warpingmachineSelectList;
        }

        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            await PopulateViewBagAsync();
            if (id == 0)
            {
                WarpingPlan plan = new WarpingPlan();
                plan.Role = HttpContext.Session.GetInt32("UserRole");
                return View(plan);
            }
            else
            {
                try
                {
                    WarpingPlan plan = await _PlanRepo.GetById(id);
                    plan.Role = HttpContext.Session.GetInt32("UserRole");
                    if (plan != null)
                    {
                        return View(plan);
                    }
                }
                catch (Exception ex)
                {
                    TempData["successMsg"] = ex.Message;
                    TempData["ReturnCode"] = -1;
                    return View();
                }
                TempData["successMsg"] = $"Warping Machine details not found with Id : {id}";
                TempData["ReturnCode"] = -1;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(WarpingPlan model, string actionType)
       {
            try
            {
                if (model.Status == null)
                {
                    model.Status = HttpContext.Session.GetInt32("UserRole") ?? 0;
                }
                
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        model.CreatedDateL1 = DateTime.Now;
                        model.CreatedByL1 = HttpContext.Session.GetString("UserName");  
                        model.Status = HttpContext.Session.GetInt32("UserRole");  
                        await _PlanRepo.Add(model);
                        TempData["successMsg"] = "Warping Plan created successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                    else
                    { 
                        model.Status = model.Status == 4 ? 4 : HttpContext.Session.GetInt32("UserRole");
                        if (model.Status == 1)
                        {
                            model.CreatedDateL1 = DateTime.Now;
                            model.CreatedByL1 = HttpContext.Session.GetString("UserName");
                        }
                        if (model.Status == 2)
                        {
                            model.CreatedDateL2 = DateTime.Now;
                            model.CreatedByL2 = HttpContext.Session.GetString("UserName");
                        }
                        if (model.Status == 3)
                        {
                            model.CreatedDateL3 = DateTime.Now;
                            model.CreatedByL3 = HttpContext.Session.GetString("UserName");
                        }
                        if (actionType == "Complete") //For Complete and Close Warping status
                        {
                            model.Status = 4;
                            model.UpdatedDate = DateTime.Now;
                            model.UpdatedBy = HttpContext.Session.GetString("UserName");
                        }
                        else if (actionType == "Close")
                        {
                            model.Status = 5;
                            model.UpdatedDate = DateTime.Now;
                            model.UpdatedBy = HttpContext.Session.GetString("UserName");
                        }

                        await _PlanRepo.Update(model);
                        TempData["successMsg"] = $"Warping Plan {(actionType == "Complete" ? "Completed" : actionType == "Close" ? "Closed" : "Updated")} successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    TempData["successMsg"] = "Model state is Invalid ";
                    TempData["ReturnCode"] = -1;
                    await PopulateViewBagAsync();
                    model.Status = null;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["successMsg"] = ex.Message;
                TempData["ReturnCode"] = -1;
                await PopulateViewBagAsync();
                model.Status = null;
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> ClosePlan(int id,int status)
        {
            try
            { 
                var plan = await _PlanRepo.GetById(id);

                if (plan == null)
                {
                    
                    return Json(new { success = false, message = "Warping Plan not found." });
                } 
                plan.Status = status;   
                plan.UpdatedDate = DateTime.Now;
                plan.UpdatedBy = HttpContext.Session.GetString("UserName");
                 
                await _PlanRepo.ClosePlan(plan);
                 
                return Json(new { success = true, message = "Plan closed successfully." });
            }
            catch (Exception ex)
            { 
                return Json(new { success = false, message = "Error closing plan: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPreviousProducts(string machineNo)
        {
            if (string.IsNullOrEmpty(machineNo))
            {
                return Json(new { message = "Machine number is required." });
            }
             
            var product = await _PlanRepo.GetPrevProductByMachineNo(machineNo);
             
            if (product == null)
            {
                return Json(new { message = "No product found for the given machine number." });
            }
             
            var result = new
            {
                Value = product.ID,
                Text = product.ProductName
            };

            return Json(new[] { result });  
        }

        //public async Task<IActionResult> Report(string? WarpingId, string? Machine, DateTime? FromDate, DateTime? ToDate, int? filterStatus)
        //{
        //    var loommachineList = await _LoomMachineRepo.GetAll();
        //    var loommachineSelectList = loommachineList.Select(loom => new SelectListItem
        //    {
        //        Value = loom.ID.ToString(),
        //        Text = $"{loom.After} ({loom.Before})"
        //    }).ToList();
        //    ViewBag.LoomMachineList = loommachineSelectList;

        //    if (WarpingId != null && WarpingId.StartsWith("WP00"))
        //    {
        //        WarpingId = WarpingId.Substring(2); 
        //    }

        //    string WarpingIds = HttpContext.Request.Query["WarpingId"];
        //    string WarpingMachines = HttpContext.Request.Query["Machine"];
        //    string FromDates = HttpContext.Request.Query["FromDate"];
        //    string ToDates = HttpContext.Request.Query["ToDate"];
        //    string filterStatuss = HttpContext.Request.Query["filterStatus"];

        //    ViewBag.WarpingId = WarpingIds ?? "";
        //    ViewBag.WarpingMachine = WarpingMachines ?? "";
        //    ViewBag.FromDate = FromDates ?? "";
        //    ViewBag.ToDate = ToDates ?? "";
        //    ViewBag.FilterStatus = filterStatuss ?? "";

        //    int? UserRole = HttpContext.Session.GetInt32("UserRole");
        //    string? CreatedBy = HttpContext.Session.GetString("UserName");

        //    int? filterIdInt = null;
        //    if (int.TryParse(WarpingId, out int parsedFilterId))
        //    {
        //        filterIdInt = parsedFilterId;
        //    }

        //    var machine = await _PlanRepo.GetReport(UserRole, CreatedBy, filterIdInt, Machine, FromDate, ToDate, filterStatus);

        //    return View(machine);
        //}
        public async Task<IActionResult> Report(string? WarpingId, string? Machine, DateTime? FromDate, DateTime? ToDate, int? filterStatus)
        {
            var loommachineList = await _LoomMachineRepo.GetAll();
            var loommachineSelectList = loommachineList.Select(loom => new SelectListItem
            {
                Value = loom.ID.ToString(),
                Text = $"{loom.After} ({loom.Before})"
            }).ToList();
            ViewBag.LoomMachineList = loommachineSelectList;

            if (WarpingId != null && WarpingId.StartsWith("WP00"))
            {
                WarpingId = WarpingId.Substring(2);
            }

            string WarpingIds = HttpContext.Request.Query["WarpingId"];
            string WarpingMachines = HttpContext.Request.Query["Machine"];
            string FromDates = HttpContext.Request.Query["FromDate"];
            string ToDates = HttpContext.Request.Query["ToDate"];
            string filterStatuss = HttpContext.Request.Query["filterStatus"];

            ViewBag.WarpingId = WarpingIds ?? "";
            ViewBag.WarpingMachine = WarpingMachines ?? "";
            ViewBag.FromDate = FromDates ?? "";
            ViewBag.ToDate = ToDates ?? "";
            ViewBag.FilterStatus = filterStatuss ?? "";

            int? UserRole = HttpContext.Session.GetInt32("UserRole");
            string? CreatedBy = HttpContext.Session.GetString("UserName");

            int? filterIdInt = null;
            if (int.TryParse(WarpingId, out int parsedFilterId))
            {
                filterIdInt = parsedFilterId;
            }

            // Check if the request is coming from a button click
            bool isFormSubmitted = Request.Query.Any(); // This checks if any query parameter is present

            if (isFormSubmitted)
            {
                // If at least one filter is applied, fetch filtered data
                var machine = await _PlanRepo.GetReport(UserRole, CreatedBy, filterIdInt, Machine, FromDate, ToDate, filterStatus);
                return View(machine);
            }

            return View(new List<WarpingPlanReport>()); // Return an empty list initially
        }


    }
}
