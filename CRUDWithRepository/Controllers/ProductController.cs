using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUDWithRepository.UI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository _ProductRepo;

        public ProductController(IProductRepository productRepo)
        {
            _ProductRepo = productRepo;
        }
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var product = await _ProductRepo.GetAll();
            return View(product);
        }
        private async Task PopulateViewBagAsync()
        {
            var products = await _ProductRepo.GetERP_Items();
            var productSelectList = products.Select(product => new SelectListItem
            {
                Value = product.Status.ToString() == "Active" ? "1" : "0",
                Text = product.ProductName,
                Group = new SelectListGroup { Name = product.Description }
            }).ToList();

            ViewBag.Products = productSelectList;
        }
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            await PopulateViewBagAsync();
            if (id == 0)
            {
                return View(new Product());
            }
            else
            {
                try
                {

                    Product product = await _ProductRepo.GetById(id);
                    if (product != null)
                    {
                        return View(product);
                    }
                }
                catch (Exception ex)
                {
                    TempData["successMsg"] = ex.Message;
                    TempData["ReturnCode"] = -1;
                    return View();
                }
                TempData["successMsg"] = $"Product details not found with Id : {id}";
                TempData["ReturnCode"] = -1;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ID == 0)
                    {
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = "Admin";
                        await _ProductRepo.Add(model);
                        TempData["successMsg"] = "Product created successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                    else
                    {
                        model.UpdatedDate = DateTime.Now;
                        model.UpdatedBy = "Admin";
                        await _ProductRepo.Update(model);
                        TempData["successMsg"] = "Product updated successfully!";
                        TempData["ReturnCode"] = 11;
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    TempData["successMsg"] = "Model state is Invalid ";
                    TempData["ReturnCode"] = -1;
                    await PopulateViewBagAsync();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["successMsg"] = ex.Message;
                TempData["ReturnCode"] = -1;
                await PopulateViewBagAsync();
                return View(model);
            }

        }
 
    }
}
