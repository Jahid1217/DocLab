using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplicationDocLab.Context;
using WebApplicationDocLab.Models;

namespace WebApplicationDocLab.Controllers
{
    public class MedicineController : Controller
    {
        private DoctorLab _contextdb;
        private HttpClient _httpClient;

        public MedicineController()
        {
            _contextdb = new DoctorLab();
            this._httpClient = new HttpClient();
        }
        // GET: Medicine
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:64303/api/Medicine");
                var responseMedicine = client.GetAsync("Medicine");
                responseMedicine.Wait();
                if (responseMedicine.Result.IsSuccessStatusCode)
                {
                    var readMedicine = responseMedicine.Result.Content.ReadAsAsync<IEnumerable<Medicine>>().Result;
                    return View(readMedicine);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(new List<Medicine>());
                }
            }
        }
        public ActionResult Details(int id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:64303/api/Medicine");
                var responseMedicine = client.GetAsync($"Medicine/{id}");
                responseMedicine.Wait();
                if (responseMedicine.Result.IsSuccessStatusCode)
                {
                    var readMedicine = responseMedicine.Result.Content.ReadAsAsync<Medicine>().Result;
                    return View(readMedicine);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(new Medicine());
                }
            }
        }
        public ActionResult Create()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Medicine medicine, HttpPostedFileBase ImageFile)
        {
            // Validate model first before processing file
            if (!ModelState.IsValid)
            {
                return View(medicine);
            }

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}";
                var directoryPath = Server.MapPath("~/image/MedicineImage/");
                // Ensure directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var path = Path.Combine(directoryPath, fileName);
                ImageFile.SaveAs(path);
                medicine.ImageUrl = fileName;
            }
            try
            {
                // Call API
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:64303/api/MedicineCreate");

                    var response = await client.PostAsJsonAsync("MedicineCreate", medicine);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"API Error: {errorContent}");
                        return View(medicine);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(medicine);
            }
        }
        public ActionResult Edit(int id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:64303/api/Medicine");
                var responseMedicine = client.GetAsync($"Medicine/{id}");
                responseMedicine.Wait();
                if (responseMedicine.Result.IsSuccessStatusCode)
                {
                    var readMedicine = responseMedicine.Result.Content.ReadAsAsync<Medicine>().Result;
                    return View(readMedicine);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(new Medicine());
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Medicine medicine, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(medicine);
            }
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}";
                var directoryPath = Server.MapPath("~/image/MedicineImage/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var path = Path.Combine(directoryPath, fileName);
                ImageFile.SaveAs(path);
                medicine.ImageUrl = fileName;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:64303/api/MedicineUpdate");
                    var response = await client.PutAsJsonAsync($"MedicineUpdate/{id}", medicine);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"API Error: {errorContent}");
                        return View(medicine);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(medicine);
            }
        }
        //public ActionResult Delete(int id)
        //{
        //    if (Session["UserEmail"] == null)
        //        return RedirectToAction("Login", "Login");
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(@"http://localhost:64303/api/Medicine");
        //        var responseMedicine = client.GetAsync($"Medicine/{id}");
        //        responseMedicine.Wait();
        //        if (responseMedicine.Result.IsSuccessStatusCode)
        //        {
        //            var readMedicine = responseMedicine.Result.Content.ReadAsAsync<Medicine>().Result;
        //            return View(readMedicine);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //            return View(new Medicine());
        //        }
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Correct base address to the API root, not the action
                _httpClient.BaseAddress = new Uri("http://localhost:64303/api/");

                // Proper API delete call
                var response = await _httpClient.DeleteAsync($"MedicineDelete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "The medicine has been deleted successfully.";
                }
                else
                {
                    TempData["DeleteError"] = "Failed to delete the medicine. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteError"] = "An error occurred while deleting the medicine: " + ex.Message;
            }

            return RedirectToAction("Index"); // Go back to the medicine list page
        }

    }
}

