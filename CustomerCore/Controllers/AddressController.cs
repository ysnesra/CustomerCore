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
using CustomerCore.ViewModels;
using CustomerCore.ViewModels.AddressModels;

namespace CustomerCore.Controllers
{
    public class AddressController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public AddressController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {
            List<AddressModel> addressModel = new List<AddressModel>();
            addressModel = db.Address.Select(x => new AddressModel  
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
            return View(addressModel);
        }
        public IActionResult New()   
        {
            List<CustomerModel> customerMod = new List<CustomerModel>();
            customerMod = db.Customers.Include(x => x.Address).Select(x => new CustomerModel
            {
                Id=x.Id,
                UserName = x.UserName,
                Password = x.Password,
                NameSurname = x.NameSurname,
                Birthdate = x.Birthdate,
                CreatedDate = x.CreatedDate,
                Email = x.Email,
                Gender = x.Gender,
                Phone = x.Phone
            }).ToList();
            List<CountryModel> countryMod = new List<CountryModel>();
            countryMod = db.Countries.Include(x => x.Address).Select(x => new CountryModel
            {
                Id = x.Id,
                Country = x.Country
            }).ToList();
            List<CityModel> cityMod = new List<CityModel>();
            cityMod = db.Cities.Include(x => x.Address).Select(x => new CityModel
            {
                Id = x.Id,
                CountryId = x.CountryId,
                City = x.City
            }).ToList();
            List<TownModel> townMod = new List<TownModel>();
            townMod = db.Towns.Include(x => x.Address).Select(x => new TownModel
            {
                Id = x.Id,
                CityId = x.CityId,
                Town = x.Town
            }).ToList();
            List<DistrictModel> districtMod = new List<DistrictModel>();
            districtMod = db.Districts.Include(x => x.Address).Select(x => new DistrictModel
            {
                Id = x.Id,
                TownId = x.TownId,
                District = x.District
            }).ToList();

            var addressMod = new AddressFormViewModel()
            {
                CustomerVM = customerMod,
                CountryVM = countryMod,
                CityVM = cityMod,
                TownVM = townMod,
                DistrictVM = districtMod
            };
            return View("AddressForm", addressMod);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Record(AddressFormViewModel address)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)
            {
                return View("AddressForm", address);
            }

            if (address.Id == 0)
            {
                db.Address.Add(new Context.Address
                {
                    
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    CountryId = address.CountryId,
                    CityId = address.CityId,
                    TownId = address.TownId,
                    DistrictId = address.DistrictId,
                    PostalCode = address.PostalCode,
                    AddressText = address.AddressText,
                });
              
                mesajmodel.Mesaj = address.Id + " Id'ye sahip adres başarıyla eklendi...";
                mesajmodel.Status = true;
            }
            else
            {
                var updateAddress = db.Address.FirstOrDefault(x => x.Id == address.Id);
                if (updateAddress == null)
                {
                    return NotFound();
                }
                else
                {
                    updateAddress.CustomerId = address.CustomerId;
                    updateAddress.CountryId = address.CountryId;
                    updateAddress.CityId = address.CityId;
                    updateAddress.TownId = address.TownId;
                    updateAddress.DistrictId = address.DistrictId;
                    updateAddress.PostalCode = address.PostalCode;
                    updateAddress.AddressText = address.AddressText;
                   
                }
                mesajmodel.Mesaj = address.Id + " Id'ye sahip adres başarıyla güncellendi...";

            }
            db.SaveChanges();
            mesajmodel.Status = true;
            mesajmodel.LinkText = "Adres Listesi";
            mesajmodel.Url = "/Address";

            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id)  
        {
            List<CustomerModel> customerMod = new List<CustomerModel>();
            customerMod = db.Customers.Include(x => x.Address).Select(x => new CustomerModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                NameSurname = x.NameSurname,
                Birthdate = x.Birthdate,
                CreatedDate = x.CreatedDate,
                Email = x.Email,
                Gender = x.Gender,
                Phone = x.Phone
            }).ToList();
            List<CountryModel> countryMod = new List<CountryModel>();
            countryMod = db.Countries.Include(x => x.Address).Select(x => new CountryModel
            {
                Id = x.Id,
                Country = x.Country
            }).ToList();
            List<CityModel> cityMod = new List<CityModel>();
            cityMod = db.Cities.Include(x => x.Address).Select(x => new CityModel
            {
                Id = x.Id,
                CountryId = x.CountryId,
                City = x.City
            }).ToList();
            List<TownModel> townMod = new List<TownModel>();
            townMod = db.Towns.Include(x => x.Address).Select(x => new TownModel
            {
                Id = x.Id,
                CityId = x.CityId,
                Town = x.Town
            }).ToList();
            List<DistrictModel> districtMod = new List<DistrictModel>();
            districtMod = db.Districts.Include(x => x.Address).Select(x => new DistrictModel
            {
                Id = x.Id,
                TownId = x.TownId,
                District = x.District
            }).ToList();

            var dbAddress = db.Address.FirstOrDefault(x => x.Id == id);
            if (dbAddress == null)
            {
                return NotFound();
            }
            var addressModel = new AddressFormViewModel()
            {
                CustomerId = dbAddress.CustomerId,
                CountryId = dbAddress.CountryId,
                CityId = dbAddress.CityId,
                TownId = dbAddress.TownId,
                DistrictId = dbAddress.DistrictId,
                PostalCode = dbAddress.PostalCode,
                AddressText = dbAddress.AddressText,
                CustomerVM = customerMod,
                CountryVM = countryMod,
                CityVM = cityMod,
                TownVM = townMod,
                DistrictVM = districtMod
            };
            return PartialView("AddressForm", addressModel);
        }
        public IActionResult Delete(int id)
        {
            var deleteAddress = db.Address.FirstOrDefault(x => x.Id == id);
            if (deleteAddress == null)
            {
                return NotFound();
            }

            db.Address.Remove(deleteAddress);

            db.SaveChanges();
            return RedirectToAction("Index", "Address");
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


