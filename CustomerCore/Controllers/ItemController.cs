using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using CustomerCore.ViewModels;
using CustomerCore.Context;
using Microsoft.AspNetCore.Hosting;
using CustomerCore.ViewModels.PhotoModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CustomerCore.Controllers
{
    public class ItemController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostEnvironment;  

        public ItemController(ICompositeViewEngine viewEngine, IWebHostEnvironment hostEnvironment)   
        {
            _viewEngine = viewEngine;
            _hostEnvironment = hostEnvironment;   

        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {

            List<ItemModel> itemmodel = new List<ItemModel>();
            itemmodel = db.Items.Include(x => x.Photos).Select(x => new ItemModel  
            {
                Id = x.Id,
                ItemCode = x.ItemCode,
                Item = x.Item,
                UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                VatRateId = x.VatRateId,
                Brand = x.Brand,
                Amount = x.Amount,
                PhotoFiles = x.Photos.Select(a => a.Photo).ToList()  
            }).ToList();

            return View(itemmodel);
        }
        public IActionResult New()   
        {
            List<CategoryModel> categoryMod = new List<CategoryModel>();
            categoryMod = db.Categories.Include(x => x.Items).Select(x => new CategoryModel
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Category = x.Category,
                IsTheMostChild = x.IsTheMostChild
            }).ToList();

            List<VatRateModel> vatRateMod = new List<VatRateModel>();
            vatRateMod = db.VatRates.Include(x => x.Items).Select(x => new VatRateModel
            {
                Id = x.Id,
                VatRate = x.VatRate
            }).ToList();

            var ItemMod = new ItemFormViewModel()
            {
                CategoryVM = categoryMod,
                VatRateVM = vatRateMod
            };
            return View("ItemForm", ItemMod);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]                      
        public async Task<IActionResult> Record(ItemFormViewModel item)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)    
            {
                return View("ItemForm", item);
            }

            if (item.ItmId == 0)
            {
                if (item.ItmSelectFiles != null && item.ItmSelectFiles.Count > 0)  
                {
                    Items product = new Items();  
                    product.Id = item.ItmId;
                    product.Item = item.ItmItem;
                    product.ItemCode = item.ItmItemCode;
                    product.UnitPrice = item.ItmUnitPrice;
                    product.Brand = item.ItmBrand;
                    product.Amount = item.ItmAmount;
                    product.VatRateId = item.ItmVatRateId;
                    product.CategoryId = item.ItmCategoryId;

                    var filepath = Path.Combine(_hostEnvironment.WebRootPath, "uploads/products");  
                    if (!Directory.Exists(filepath)) 
                    {
                        Directory.CreateDirectory(filepath); 
                    }

                    foreach (var imageFile in item.ItmSelectFiles)  
                    {
                        var fileName = Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(imageFile.FileName);
                        var fullfileName = Path.Combine(filepath, fileName);  
                        using (var filestream = new FileStream(fullfileName, FileMode.Create))   
                        {
                            await imageFile.CopyToAsync(filestream);     
                        }
                        product.Photos.Add(new Photos { Photo = fileName }); 
                    }

                    db.Items.Add(product);
                    mesajmodel.Mesaj = item.ItmItem + " ürünü başarıyla eklendi...";
                    mesajmodel.Status = true;
                    mesajmodel.LinkText = "Ürünler Listesi";
                    mesajmodel.Url = "/Item";

                }
            }
            else 
            {

                var updateıtem = db.Items.Include(x => x.Photos).FirstOrDefault(x => x.Id == item.ItmId);
                if (updateıtem == null)
                {
                    return NotFound();
                }
                else
                {
                    updateıtem.ItemCode = item.ItmItemCode;
                    updateıtem.Item = item.ItmItem;
                    updateıtem.UnitPrice = item.ItmUnitPrice;
                    updateıtem.Brand = item.ItmBrand;
                    updateıtem.Amount = item.ItmAmount;
                    updateıtem.CategoryId = item.ItmCategoryId;
                    updateıtem.VatRateId = item.ItmVatRateId;


                    var dbphotoDeletelist = db.Photos.Where(x => x.ItemId == item.ItmId).ToList();
                    if (dbphotoDeletelist == null)
                    {
                        NotFound();
                    }
                    foreach (var photo in dbphotoDeletelist)
                    {
                        db.Photos.Remove(photo);
                        updateıtem.Photos.Remove(photo);
                    }

                    var filepath = Path.Combine(_hostEnvironment.WebRootPath, "uploads/products");  
                    if (!Directory.Exists(filepath)) 
                    {
                        Directory.CreateDirectory(filepath); 
                    }
                    foreach (var updateimage in item.ItmSelectFiles)  
                    {
                        var fileName = Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(updateimage.FileName);
                        var fullfileName = Path.Combine(filepath, fileName);  

                        using (var filestream = new FileStream(fullfileName, FileMode.Create))   
                        {
                            await updateimage.CopyToAsync(filestream);      
                        }
                        updateıtem.Photos.Add(new Photos { Photo = fileName }); 
                    }
                    mesajmodel.Mesaj = updateıtem.Item + " ürünü başarıyla güncellendi...";

                } 
                await db.SaveChangesAsync(); 
                mesajmodel.Status = true;
                mesajmodel.LinkText = "Ürünler Listesi";
                mesajmodel.Url = "/Item";

            }        
            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id)  
        {
            List<CategoryModel> categoryMod = new List<CategoryModel>();
            categoryMod = db.Categories.Include(x => x.Items).Select(x => new CategoryModel
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Category = x.Category,
                IsTheMostChild = x.IsTheMostChild
            }).ToList();

            List<VatRateModel> vatRateMod = new List<VatRateModel>();
            vatRateMod = db.VatRates.Include(x => x.Items).Select(x => new VatRateModel
            {
                Id = x.Id,
                VatRate = x.VatRate
            }).ToList();

            List<PhotoModel> photoMod = new List<PhotoModel>();
            photoMod = db.Photos.Select(x => new PhotoModel 
            {
                Id = x.Id,
                ItemId = x.ItemId,
                Photos = x.Photo,
            }).ToList();

            var dbItem = db.Items.Include(x => x.Photos).FirstOrDefault(x => x.Id == id);

            if (dbItem == null)
            {
                return NotFound();
            }
            var itemModel = new ItemFormViewModel()  
            {
                ItmId = dbItem.Id,
                ItmItemCode = dbItem.ItemCode,
                ItmItem = dbItem.Item,
                ItmUnitPrice = dbItem.UnitPrice,
                ItmBrand = dbItem.Brand,
                ItmAmount = dbItem.Amount,
                ItmCategoryId = dbItem.CategoryId,
                ItmVatRateId = dbItem.VatRateId,
                CategoryVM = categoryMod,
                VatRateVM = vatRateMod,
                PhotoFiles = photoMod,
            };
            return PartialView("ItemForm", itemModel);
        }
        public IActionResult Delete(int id)
        {

            var deleteItem = db.Items.Include(x => x.OrderDetails).Include(x => x.Photos).FirstOrDefault(x => x.Id == id);
            if (deleteItem == null)
            {
                return NotFound();
            }
            var silinenÜrününIcindekiTumSiparişDetaylarınınListesi = deleteItem.OrderDetails.ToList();
            foreach (var item in silinenÜrününIcindekiTumSiparişDetaylarınınListesi)
            {
                deleteItem.OrderDetails.Remove(item);
                db.OrderDetails.Remove(item);
            }
            var silinenÜrününIcindekiFotograflarınListesi = deleteItem.Photos.ToList();
            foreach (var item in silinenÜrününIcindekiFotograflarınListesi)
            {
                deleteItem.Photos.Remove(item);
                db.Photos.Remove(item);
            }
            db.Items.Remove(deleteItem);
            db.SaveChanges();
            return RedirectToAction("Index", "Item");
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

