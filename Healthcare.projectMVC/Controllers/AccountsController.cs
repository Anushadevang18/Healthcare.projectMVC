using Healthcare.projectMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Healthcare.projectMVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Accounts/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);

                        return RedirectToAction("Index", "Doctors");


                    }
                    ModelState.AddModelError("", "Invalid LoginID or Password");
                }
            }
            return View(login);
       
        }


        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            RolesRegisterViewModel model = new RolesRegisterViewModel
            {
                Values = new List<SelectListItem>
                    {
                        new SelectListItem {Value = "Doctor", Text="Doctor"},
                        new SelectListItem{Value="Patient",Text="Patient"}
                    }
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RolesRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    RegisterViewModel user = new RegisterViewModel
                    {
                        UserName = model.RegisterRoles.UserName,
                        Password = model.RegisterRoles.Password,
                        ConfirmPassword = model.RegisterRoles.ConfirmPassword,
                        Role = model.SelectedValue
                    };


                    var result = await client.PostAsJsonAsync("Accounts/Register", user);
                    if (result.IsSuccessStatusCode)
                    {


                        return RedirectToAction("Login");
                    }
                }
            }
            RegisterViewModel user1 = new RegisterViewModel
            {
                UserName = model.RegisterRoles.UserName,
                Password = model.RegisterRoles.Password,
                Role = model.SelectedValue
            };
            RolesRegisterViewModel model1 = new RolesRegisterViewModel
            {
                RegisterRoles = user1,
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Patient", Text = "Patient" },
                    new SelectListItem { Value = "Doctor", Text = "Doctor" },
                }
            };
            return View(model1);
        }

        //[HttpGet]
        //public IActionResult RegisterDoctor()
        //{
        //    RolesRegisterViewModel model = new RolesRegisterViewModel
        //    {
        //        Values = new List<SelectListItem>
        //            {
        //                //new SelectListItem {Value = "Employee", Text="Employee"},
        //                new SelectListItem{Value="Doctor",Text="Doctor"}
        //            }
        //    };
        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> RegisterDocotr([FromForm] RolesRegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
        //            client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
        //            RegisterViewModel user = new RegisterViewModel
        //            {
        //                UserName = model.RegisterRoles.UserName,
        //                Password = model.RegisterRoles.Password,
        //                ConfirmPassword = model.RegisterRoles.ConfirmPassword,
        //                Role = model.SelectedValue
        //            };

        //            var result = await client.PostAsJsonAsync("Accounts/Register", user);
        //            if (result.IsSuccessStatusCode)
        //            {

        //                //if (model.SelectedValue=="Customer")
        //                //{
        //                //    return RedirectToAction("Create", "Customers");
        //                //}
        //                //else if (model.SelectedValue=="Employee")
        //                //{
        //                //    return RedirectToAction("Create", "Employees");
        //                //}


        //                return RedirectToAction("Login");
        //            }
        //        }
        //    }
        //    RegisterViewModel user1 = new RegisterViewModel
        //    {
        //        UserName = model.RegisterRoles.UserName,
        //        Password = model.RegisterRoles.Password,
        //        Role = model.SelectedValue
        //    };
        //    RolesRegisterViewModel model1 = new RolesRegisterViewModel
        //    {
        //        RegisterRoles = user1,
        //        Values = new List<SelectListItem>
        //        {
        //            new SelectListItem { Value = "Doctor", Text = "Doctor" },
        //            //new SelectListItem { Value = "Employee", Text = "Employee" },
        //        }
        //    };
        //    return View(model1);
        //}

    }
}
                   
