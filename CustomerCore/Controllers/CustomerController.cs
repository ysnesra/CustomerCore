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
using CustomerCore.ViewModels.AddressModels;
using System.Threading.Tasks;
using CustomerCore.ViewModels.OrderModels;

namespace CustomerCore.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;

        public CustomerController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        CustomersDbContext db = new CustomersDbContext();
        public IActionResult Index()
        {
            List<CustomerModel> customerModel = new List<CustomerModel>();
            customerModel = db.Customers.Select(x => new CustomerModel  
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

            return View(customerModel);
        }
        public IActionResult New()
        {
            return View("CustomerForm", new CustomerEkleModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Record(CustomerEkleModel customer)
        {
            MesajViewModel mesajmodel = new MesajViewModel();
            if (!ModelState.IsValid)
            {
                mesajmodel.Mesaj = customer.NameSurname + " kişi eklenemedi!!!...";
                mesajmodel.Status = false;
                return View("CustomerForm", customer);
            }
            if (customer.Id == 0)
            {
                int yas = DateTime.Now.Year - customer.Birthdate.Value.Year;

                if (10 < yas && yas < 200)
                {
                    db.Customers.Add(new Customers
                    {
                        Id = customer.Id,
                        UserName = customer.UserName,
                        Password = customer.Password,
                        NameSurname = customer.NameSurname,
                        Birthdate = customer.Birthdate.GetValueOrDefault(),
                        CreatedDate = DateTime.Now,  
                        Email = customer.Email,
                        Gender = customer.Gender,
                        Phone = customer.Phone,
                    });
                    mesajmodel.Mesaj = customer.NameSurname + " kişisi başarıyla eklendi...";
                    mesajmodel.Status = true;
                }
                else
                {
                }
            }
            else
            {
                var updatecustomer = db.Customers.FirstOrDefault(x => x.Id == customer.Id);
                if (updatecustomer == null)
                {
                    return NotFound();
                }
                else
                {
                    updatecustomer.UserName = customer.UserName;
                    updatecustomer.Password = customer.Password;
                    updatecustomer.NameSurname = customer.NameSurname;
                    updatecustomer.Birthdate = customer.Birthdate.GetValueOrDefault();  //GetValueOrDefault() boş geçilirse değeri tutsun
                    updatecustomer.Email = customer.Email;
                    updatecustomer.Gender = customer.Gender;
                    updatecustomer.Phone = customer.Phone;
                }
                mesajmodel.Mesaj = customer.NameSurname + " kişisi başarıyla güncellendi...";

            }
            db.SaveChanges();
            mesajmodel.Status = true;
            mesajmodel.LinkText = "Müşteriler Listesi";
            mesajmodel.Url = "/Customer";

            return Json(RenderPartialViewToString(_viewEngine, "~/Views/Shared/_Mesaj.cshtml", mesajmodel));
        }
        public IActionResult Update(int id)  

        {
            var cust = db.Customers.FirstOrDefault(x => x.Id == id);
            if (cust == null)
            {
                return NotFound();
            }
            var custModel = new CustomerEkleModel()      
            {
                Id = cust.Id,
                UserName = cust.UserName,
                Password = cust.Password,
                NameSurname = cust.NameSurname,
                Birthdate = cust.Birthdate,
                Email = cust.Email,
                Gender = cust.Gender,
                Phone = cust.Phone
            };
            return View("CustomerForm", custModel);
        }
        public IActionResult Delete(int id)
        {
            var deleteCustomer = db.Customers.FirstOrDefault(x => x.Id == id);
            if (deleteCustomer == null)
            {
                return NotFound();
            }
            var silinenMüşterininTumAdresleri = deleteCustomer.Address.ToList();

            foreach (var item in silinenMüşterininTumAdresleri)
            {
                item.CustomerId = null;
            }
            db.Customers.Remove(deleteCustomer);

            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        public IActionResult AddressList(int id)
        {
            List<AddressModel> addressModel = new List<AddressModel>();
            addressModel = db.Address.Where(x => x.CustomerId == id).Select(x => new AddressModel 
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                CountryId = x.CountryId,
                CityId = x.CityId,
                TownId = x.TownId,
                DistrictId = x.DistrictId,
                PostalCode = x.PostalCode,
                AddressText = x.AddressText
            }).ToList();
            return PartialView(addressModel);
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

        public IActionResult NewModal()
        {
            return PartialView("_CustomerFormForModal", new CustomerEkleModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult RecordModal(CustomerEkleModel customer)
        {
            int yas = DateTime.Now.Year - customer.Birthdate.Value.Year;

            Customers dbCustomer = null;  

            if (10 < yas && yas < 200)
            {
                var dbcustomerIs = db.Customers.Where(x => x.UserName == customer.UserName).FirstOrDefault(); 
                if (dbcustomerIs != null)
                {

                    return Json("kullanıcı sistemde kayıtlı..");
                }

                dbCustomer = new Customers()
                {
                    Id = customer.Id,
                    UserName = customer.UserName,
                    Password = customer.Password,
                    NameSurname = customer.NameSurname,
                    Birthdate = customer.Birthdate.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,  
                    Email = customer.Email,
                    Gender = customer.Gender,
                    Phone = customer.Phone,
                };
                db.Customers.Add(dbCustomer); 
            }
            else
            {
                return Json("Yaş standartlarına uymuyor");
            }

            int res = db.SaveChanges();
            string Message = "";

            if (res > 0)
            {
                Message = "Kayıt başarılı";

            }
            else { Message = "KayıtBaşarısız"; }

            return Json(new { id = dbCustomer.Id, nameSurname = dbCustomer.NameSurname });
          
        }
        [HttpPost]
        public JsonResult GetCustomerList()
        {
            var CustomerList = db.Customers.ToList();
            return Json(CustomerList);
        }
        public IActionResult NewModalAddress()    
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

            var addressMod = new AddressFormViewModel()
            {
                CustomerVM = customerMod,
                CountryVM = countryMod,
                CityVM = cityMod,
                TownVM = townMod,
                DistrictVM = districtMod
            };

            return PartialView("_AddressFormForModal", addressMod);
        }
        [HttpPost]
        public JsonResult GetAddressList(int id)
        { 
            var AddressList = db.Address.Where(x => x.CustomerId == id).ToList();           
            return Json(AddressList);
        }
        public IActionResult RecordModalAddress(AddressFormViewModel address)
        {
            if (!ModelState.IsValid)
            {
                return View("AddressFormViewModel", address);
            }
            var dbAddress = new Context.Address
            {

                CustomerId = address.CustomerId,
                CountryId = address.CountryId,
                CityId = address.CityId,
                TownId = address.TownId,
                DistrictId = address.DistrictId,
                PostalCode = address.PostalCode,
                AddressText = address.AddressText,
            };
            db.Address.Add(dbAddress);
            db.SaveChanges();

            return Json(new { Id = dbAddress.CustomerId, NameSurname = dbAddress.AddressText });  
        }
        [HttpPost]
        public JsonResult GetCountryList()
        {
            var CountryList = db.Countries.ToList();
            return Json(CountryList);
        }
        [HttpPost]
        public JsonResult GetCityList(int id)
        {
            var CityList = db.Cities.Where(x => x.CountryId == id).ToList();
            return Json(CityList);
        }
        [HttpPost]
        public JsonResult GetTownList(int id)
        {
            var TownList = db.Towns.Where(x => x.CityId == id).ToList();
            return Json(TownList);
        }
        [HttpPost]
        public JsonResult GetDistrictList(int id)
        {
            var DistrictList = db.Districts.Where(x => x.TownId == id).ToList();
            return Json(DistrictList);
        }

    }

}

