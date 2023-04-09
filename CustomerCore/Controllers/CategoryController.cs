using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using CustomerCore.ViewModels;
using CustomerCore.Context;


namespace CustomerCore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public CategoryController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {
            List<CategoryModel> categoryModel = new List<CategoryModel>();
            categoryModel = db.Categories.Select(x => new CategoryModel  
            {
                Id = x.Id,              
                ParentId = x.ParentId,
                Category = x.Category,
                IsTheMostChild =x.IsTheMostChild
            }).ToList();

            return View(categoryModel);
        }
        public IActionResult New()
        {
            return View("CategoryForm", new CategoryEkleModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Record(CategoryEkleModel category)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)
            {
                mesajmodel.Mesaj = category.Category + " kategorisi eklenemedi!!!...";
                mesajmodel.Status = false;
                return View("CategoryForm", category);
            }

            if (category.Id == 0)
            {
                db.Categories.Add(new Categories
                {
                    Id = category.Id,
                    ParentId=category.ParentId,
                    Category = category.Category,                                 
                    IsTheMostChild = category.IsTheMostChild,
                });
                mesajmodel.Mesaj = category.Category + " kategorisi başarıyla eklendi...";
                mesajmodel.Status = true;
            }
            else
            {
                var updatecategory = db.Categories.FirstOrDefault(x => x.Id == category.Id);
                if (updatecategory == null)
                {
                    return NotFound();
                }
                else
                {
                    updatecategory.Category = category.Category;
                    updatecategory.ParentId = category.ParentId;
                    updatecategory.IsTheMostChild = category.IsTheMostChild;

                }
                mesajmodel.Mesaj = category.Category + " kategorisi başarıyla güncellendi...";

            }
            db.SaveChanges();
            mesajmodel.Status = true;
            mesajmodel.LinkText = "Ürünler Listesi";
            mesajmodel.Url = "/Item";

            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id) 
        {
            var categorydb = db.Categories.FirstOrDefault(x => x.Id == id);
            if (categorydb == null)
            {
                return NotFound();
            }
            var categoryModel = new CategoryEkleModel()    
            {
                Id = categorydb.Id,
                ParentId = categorydb.ParentId,
                Category = categorydb.Category,
                IsTheMostChild = categorydb.IsTheMostChild,
            };
            return View("CategoryForm", categoryModel);
        }
        public IActionResult Delete(int id)
        {
            var deleteCategory = db.Categories.FirstOrDefault(x => x.Id == id);
            if (deleteCategory == null)
            {
                return NotFound();
            }
            db.Categories.Remove(deleteCategory);

            db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        public string RenderPartialViewToString(ICompositeViewEngine viewEngine, string viewName, object model)
        {
            viewName ??= ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;
            using StringWriter sw = new StringWriter();

            IView view = viewEngine.GetView(viewName, viewName, false).View;
            ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());
            view.RenderAsync(viewContext).Wait();
            return sw.GetStringBuilder().ToString();
        }
    }
}

