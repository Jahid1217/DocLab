using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplicationDocLab.Models;
using WebApplicationDocLab.Context;
using System.Net;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WebApplicationDocLab.Controllers
{
    public class MTController : Controller
    {
        private DoctorLab _contextdb;
        private HttpClient _httpClient;
        public MTController()
        {
            _contextdb = new DoctorLab();
            this._httpClient = new HttpClient();
        }
        // GET: MT
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:64303/api/Text");
                var responseTask = client.GetAsync("Text");
                responseTask.Wait();
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IEnumerable<Test>>().Result;
                    return View(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(new List<Test>());
                }
            }
        }
        // GET: MT/Create
        public ActionResult Details(int? id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/Text");
            var response = _httpClient.GetAsync("Text/" + id.ToString());
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsAsync<Test>().Result;
                return View(data);
            }
            return HttpNotFound();
        }

        // POST: Save updated data to API
        public ActionResult Edit(int? id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/Text");
            var response = _httpClient.GetAsync("Text/" + id.ToString());
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsAsync<Test>().Result;
                return View(data);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,TestName,Category,Created_at")] Test test)
        {
            if (ModelState.IsValid)
            {
                _contextdb.Entry(test).State = EntityState.Modified;
                _contextdb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test);
        }
        // GET: MT/Create
        public ActionResult Create()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            return View();
        }
        // POST: MT/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TestName,Category,Created_at")] Test test)
        {
            if (ModelState.IsValid)
            {
                _contextdb.Tests.Add(test);
                _contextdb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test);
        }
        // GET: MT/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/Text");
            var response = _httpClient.GetAsync("Text/" + id.ToString());
            response.Wait();
            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsAsync<Test>().Result;
                return View(data);
            }
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/TextDelete");
                var response = await _httpClient.DeleteAsync($"TextDelete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "The test has been deleted successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DeleteError"] = "Failed to delete the test. Please try again.";
                    return RedirectToAction("Delete", new { id });
                }
            }
            catch (Exception ex)
            {
                // Log the error
                TempData["DeleteError"] = "An error occurred while deleting the test: " + ex.Message;
                return RedirectToAction("Delete", new { id });
            }
        }


        public ActionResult TypeIndex()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://localhost:64303/api/Type");
                var responseTask = client.GetAsync("Type");
                responseTask.Wait();
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Result.Content.ReadAsAsync<IEnumerable<Doctor_Type>>().Result;
                    return View(readTask);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(new List<Doctor_Type>());
                }

            }
        }
        public ActionResult TypeEdit(int? id)
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/Type");
            var response = _httpClient.GetAsync("Type/" + id.ToString());
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsAsync<Doctor_Type>().Result;
                return View(data);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TypeEdit([Bind(Include = "Id,TypeName")] Doctor_Type doctor_Type)
        {
            if (ModelState.IsValid)
            {
                _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/TypeUpdate");
                var response = _httpClient.PutAsJsonAsync("TypeUpdate/" + doctor_Type.Id, doctor_Type);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("TypeIndex");
                }
                return Json(new { success = false, message = "Error updating doctor type" });
            }
            return Json(new { success = false, message = "Invalid data" });
        }
        [HttpGet]
        public ActionResult TypeCreate()
        {
            if (Session["UserEmail"] == null)
                return RedirectToAction("Login", "Login");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TypeCreate([Bind(Include = "Id,TypeName")] Doctor_Type doctor_Type)
        {
            if (ModelState.IsValid)
            {
                _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/AddType");
                var response = _httpClient.PostAsJsonAsync("AddType", doctor_Type);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("TypeIndex");
                }
                return Json(new { success = false, message = "Error creating doctor type" });
            }
            return Json(new { success = false, message = "Invalid data" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TypeDeleteConfirmed(int id)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(@"http://localhost:64303/api/TypeDelete");
                var response = await _httpClient.DeleteAsync($"TypeDelete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "The doctor type has been deleted successfully.";
                    return RedirectToAction("TypeIndex");
                }
                else
                {
                    TempData["DeleteError"] = "Failed to delete the doctor type. Please try again.";
                    return RedirectToAction("TypeIndex");
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteError"] = "An error occurred while deleting the doctor type: " + ex.Message;
                return RedirectToAction("TypeIndex");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contextdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}