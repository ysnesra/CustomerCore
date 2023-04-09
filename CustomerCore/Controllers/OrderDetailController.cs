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
    public class OrderDetailController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public OrderDetailController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {
            List<OrderDetailsModel> orderDetailModel = new List<OrderDetailsModel>();
            orderDetailModel = db.OrderDetails.Select(x => new OrderDetailsModel  
            {
                Id = x.Id,
                OrderId = x.OrderId,
                ItemId = x.ItemId,
                Amount = x.Amount,
                UnitPrice = x.UnitPrice,
                SalePrice = x.SalePrice,
            }).ToList();
            return View(orderDetailModel);
        }
        public IActionResult New()   
        {
            List<OrderModel> orderMod = new List<OrderModel>();
            orderMod = db.Orders.Include(x => x.OrderDetails).Select(x => new OrderModel
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Date = x.Date,
                Status = x.Status,
                AddressId = x.AddressId,
            }).ToList();
            List<ItemModel> ItemMod = new List<ItemModel>();
            ItemMod = db.Items.Include(x => x.OrderDetails).Select(x => new ItemModel
            {
                Id = x.Id,
                ItemCode = x.ItemCode,
                Item = x.Item,
                UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                VatRateId = x.VatRateId,
                Brand = x.Brand,
                Amount = x.Amount
            }).ToList();

            var OrderDetailsMod = new OrderDetailsFormViewModel()
            {
                OrderVM = orderMod,
                ItemVM = ItemMod
            };
            return View("OrderDetailsForm", OrderDetailsMod);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Record(OrderDetailsFormViewModel item)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)
            {
                return View("OrderDetailsForm", item);
            }

            if (item.Id == 0)
            {
                db.OrderDetails.Add(new OrderDetails
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ItemId = item.ItemId,
                    Amount = item.Amount,
                    UnitPrice = item.UnitPrice,
                    SalePrice = item.SalePrice
                });
                mesajmodel.Mesaj = item.ItemId + " ürününün sipariş detayları başarıyla eklendi...";
                mesajmodel.Status = true;
            }
            else
            {
                var updateOrderDetails = db.OrderDetails.FirstOrDefault(x => x.Id == item.Id);
                if (updateOrderDetails == null)
                {
                    return NotFound();
                }
                else
                {
                    updateOrderDetails.OrderId = item.OrderId;
                    updateOrderDetails.ItemId = item.ItemId;
                    updateOrderDetails.Amount = item.Amount;
                    updateOrderDetails.UnitPrice = item.UnitPrice;
                    updateOrderDetails.SalePrice = item.SalePrice;
                }
                mesajmodel.Mesaj = item.ItemId + " ürününün sipariş detayları başarıyla güncellendi...";

            }
            int res = db.SaveChanges();
            if (res > 0)
            {
                mesajmodel.Status = true;
                mesajmodel.LinkText = "Sipariş Detay Listesi";
                mesajmodel.Url = "/OrderDetail";
            }
            else
            {

                mesajmodel.Status = false;
                mesajmodel.LinkText = "Hatalı kayıt yapılamadı";
                mesajmodel.Url = "/OrderDetail";


            }

            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id) 
        {  
            List<OrderModel> orderMod = new List<OrderModel>();
            orderMod = db.Orders.Include(x => x.OrderDetails).Select(x => new OrderModel
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Date = x.Date,
                Status = x.Status,
                AddressId = x.AddressId,
            }).ToList();
            List<ItemModel> ItemMod = new List<ItemModel>();
            ItemMod = db.Items.Include(x => x.OrderDetails).Select(x => new ItemModel
            {
                Id = x.Id,
                ItemCode = x.ItemCode,
                Item = x.Item,
                UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                VatRateId = x.VatRateId,
                Brand = x.Brand,
                Amount = x.Amount
            }).ToList();
            var dbOrderDetail = db.OrderDetails.FirstOrDefault(x => x.Id == id);
            if (dbOrderDetail == null)
            {
                return NotFound();
            }
            var orderDetailModel = new OrderDetailsFormViewModel()
            {
                Id = dbOrderDetail.Id,
                OrderId = dbOrderDetail.OrderId,
                ItemId = dbOrderDetail.ItemId,
                Amount = dbOrderDetail.Amount,
                UnitPrice = dbOrderDetail.UnitPrice,
                SalePrice = dbOrderDetail.SalePrice,
                OrderVM = orderMod,
                ItemVM = ItemMod
            };
            return PartialView("OrderDetailsForm", orderDetailModel);
        }
        public IActionResult Delete(int id)
        {
            var deleteOrderDetail = db.OrderDetails.FirstOrDefault(x => x.Id == id);
            if (deleteOrderDetail == null)
            {
                return NotFound();
            }

            db.OrderDetails.Remove(deleteOrderDetail);

            db.SaveChanges();
            return RedirectToAction("Index", "OrderDetail");
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


