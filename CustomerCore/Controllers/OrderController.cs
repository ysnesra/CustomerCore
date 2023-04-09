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
using CustomerCore.ViewModels.OrderModels;
using CustomerCore.ViewModels.AddressModels;
using static CustomerCore.ViewModels.OrderModels.EditOrderOrderDetailsViewModel;
using System.Threading.Tasks;

namespace CustomerCore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public OrderController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {
            List<OrderModel> orderModel = new List<OrderModel>();
            orderModel = db.Orders.Select(x => new OrderModel  
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Date = x.Date,
                Status =x.Status,
                AddressId = x.AddressId,
            }).ToList();

            return View(orderModel);
        }
        public IActionResult New()               
        {
            return View("OrderForm");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Record(EditOrderOrderDetailsViewModel order)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)
            {
                return View("OrderKayıt", order);
            }
            if (order.OrderMId == 0)
            {
                db.Orders.Add(new Orders
                {
                    Id = order.OrderMId,
                    CustomerId = order.OrderMCustomerId,
                    Date = order.OrderMDate,
                    Status = order.OrderMStatus,
                    AddressId = order.OrderMAddressId,
                });
                mesajmodel.Mesaj = "Idsi" + order.OrderMId + " olan siparişi başarıyla eklendi...";
                mesajmodel.Status = true;
            }
            else
            {
                var updateorder = db.Orders.FirstOrDefault(x => x.Id == order.OrderMId);
                if (updateorder == null)
                {
                    return NotFound();
                }
                else
                {
                    updateorder.AddressId = order.OrderMAddressId;
                    updateorder.Date = order.OrderMDate;
                    updateorder.Status = order.OrderMStatus;
                }
                mesajmodel.Mesaj = "Idsi" + order.OrderMId + " olan siparişi başarıyla güncellendi...";

            }
            db.SaveChanges();
            mesajmodel.Status = true;
            mesajmodel.LinkText = "Siparişler Listesi";
            mesajmodel.Url = "/Order";

            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id)  
        { 
            var orderdb = db.Orders.Include(x => x.OrderDetails).FirstOrDefault(x => x.Id == id);
            if (orderdb == null)
            {
                return NotFound();
            }
            List<CustomerModel> customerMod = new List<CustomerModel>();
            customerMod = db.Customers.Select(x => new CustomerModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                NameSurname = x.NameSurname,
                Birthdate = x.Birthdate,
                CreatedDate = x.CreatedDate,
                Email = x.Email,
                Gender = x.Gender,
                Phone = x.Phone,
            }).ToList();
            List<AddressModel> addressMod = new List<AddressModel>();
            addressMod = db.Address.Select(x => new AddressModel
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                CountryId = x.CountryId,
                CityId = x.CityId,
                TownId = x.TownId,
                DistrictId = x.DistrictId,
                PostalCode = x.PostalCode,
                AddressText = x.AddressText,
            }).ToList();

            var orderrModel = new EditOrderOrderDetailsViewModel()      
            {
                OrderMId = orderdb.Id,
                OrderMCustomerId = orderdb.CustomerId,
                OrderMDate = orderdb.Date,
                OrderMStatus = orderdb.Status,
                OrderMAddressId = orderdb.AddressId,
                CustomerVMM = customerMod,
                AddressVMM = addressMod,

                OrderDetailsMModels = orderdb.OrderDetails.Select(x => new OrderDetailsFormViewModel()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ItemId = x.ItemId,
                    Amount = x.Amount,
                    UnitPrice = x.UnitPrice,
                    SalePrice = x.SalePrice,
                }).ToList(),
            };
            return PartialView("OrderKayıt", orderrModel);
        }
        public IActionResult Delete(int id)
        {
            var deleteOrder = db.Orders.FirstOrDefault(x => x.Id == id);
            if (deleteOrder == null)
            {
                return NotFound();
            }
            db.Orders.Remove(deleteOrder);

            db.SaveChanges();
            return RedirectToAction("Index", "Order");
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

        [HttpGet]
        public IActionResult OrderDetailList(int id)
        
        {
            var model = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView("ListOrderDetail",model);   
        }

    
    }

}


