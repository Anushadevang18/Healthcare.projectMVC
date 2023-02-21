using Healthcare.projectMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Healthcare.projectMVC.Controllers
{
    public class DoctorsController : Controller
    {
       // private readonly string ApiUrl = "https://localhost:44371/api";
        private readonly IConfiguration _configuration;

        public DoctorsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<DoctorViewModel> doctors = new();
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Doctors/GetAllDoctors");
                if (result.IsSuccessStatusCode)
                {
                    doctors = await result.Content.ReadAsAsync<List<DoctorViewModel>>();
                }
            }
            return View(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DoctorViewModel doctor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctors/GetDoctorsById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                }
            }
            return View(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Doctors/CreateDoctor", doctor);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "Doctors");
                    }
                }
            }
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                DoctorViewModel doctor = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Doctors/GetDoctorsById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                        doctor.Specializations = await this.GetSpecializations();
                        return View(doctor);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Doctor doesn't exists");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DoctorViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Doctors/UpdateDoctor/{doctor.Id}", doctor);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            DoctorViewModel viewModel = new DoctorViewModel
            {
                Specializations = await this.GetSpecializations()
            };
            return View(viewModel);
        }
        [NonAction]
        public async Task<List<SpecializationViewModel>> GetSpecializations()
        {
            List<SpecializationViewModel> specializations = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Doctors/GetSpecializations");
                if (result.IsSuccessStatusCode)
                {
                    specializations = await result.Content.ReadAsAsync<List<SpecializationViewModel>>();
                }
            }
            return specializations;
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            DoctorViewModel doctor = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Doctors/GetDoctorsById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    doctor = await result.Content.ReadAsAsync<DoctorViewModel>();
                    return View(doctor);

                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View(doctor);
        }

        [HttpPost("Doctors/Delete/{Id}")]
        public async Task<IActionResult> Delete(DoctorViewModel doctor)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Doctors/DeleteDoctor/{doctor.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View();

        }


    }
}

