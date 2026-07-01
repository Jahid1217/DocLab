using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationDocLab.Context;
using WebApplicationDocLab.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Configuration;
using Org.BouncyCastle.Crypto.Generators;
using static System.Net.WebRequestMethods;

namespace WebApplicationDocLab.Controllers
{
    public class LoginController : Controller
    {
        public DoctorLab _contextdb;
        public LoginController()
        {
            _contextdb = new DoctorLab();
        }

        private static string generatedOtp;
        private static DateTime otpExpiration;
        private static string resetOtp;
        private static DateTime otpResetExpiration;
        private static string _resetEmail;
        private static string _resetOtp;
        private static DateTime _otpResetExpiration;
        // GET: Login


        public ActionResult DoctorRegister()
        {
            PopulateDoctorTypes();
            return View();
        }

        private void PopulateDoctorTypes()
        {
            ViewBag.DoctorTypeName = new SelectList(
                _contextdb.Doctor_Types.OrderBy(x => x.TypeName),
                "TypeName",
                "TypeName");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorRegister(User_Info model, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = _contextdb.User_Infos.FirstOrDefault(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        PopulateDoctorTypes();
                        return View(model);
                    }
                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(ImageFile.FileName);
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                        if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                        {
                            ModelState.AddModelError("ImageFile", "Please upload a JPG, PNG, GIF, or WebP image.");
                            PopulateDoctorTypes();
                            return View(model);
                        }

                        var uploadDirectory = Server.MapPath("~/image/DoctorImage/");
                        Directory.CreateDirectory(uploadDirectory);
                        var fileName = Guid.NewGuid().ToString("N") + extension.ToLowerInvariant();
                        var path = Path.Combine(uploadDirectory, fileName);
                        ImageFile.SaveAs(path);
                        model.Image = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageFile", "A profile image is required.");
                        PopulateDoctorTypes();
                        return View(model);
                    }
                    model.UserType = "Doctor";
                    model.Status = "Inactive";
                    model.CreatedDate = DateTime.Now;
                    // Generate OTP (skip if not needed here)
                    generatedOtp = new Random().Next(100000, 999999).ToString();
                    otpExpiration = DateTime.Now.AddMinutes(5);
                    if (!SendOtpEmail(model.Email, generatedOtp))
                        TempData["DevelopmentOtp"] = generatedOtp;

                    Session["UserTempData"] = model;

                    return RedirectToAction("EnterOtp");
                }

                PopulateDoctorTypes();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while registering: " + ex.Message);
                PopulateDoctorTypes();
                return View(model);
            }
            }

        // Send OTP to email
        private bool SendOtpEmail(string email, string otp)
        {
            var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                return false;

            using (var smtpClient = new SmtpClient(
                ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com"))
            using (var mailMessage = new MailMessage())
            {
                smtpClient.Port = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out var port) ? port : 587;
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                smtpClient.EnableSsl = true;

                mailMessage.From = new MailAddress(smtpUser);
                mailMessage.Subject = "Your DoctorLab OTP Code";
                mailMessage.Body = $"Your OTP code is: {otp}. This code is valid for 5 minutes.";
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                return true;
            }
            }

        // GET: Enter OTP
        public ActionResult EnterOtp()
        {
            ViewBag.DevelopmentOtp = TempData["DevelopmentOtp"];
            return View();
        }

        // POST: Validate OTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateOtp(string otp)
        {
            try
            {
                if (otp == generatedOtp && DateTime.Now <= otpExpiration)
                {
                    var user = Session["UserTempData"] as User_Info;
                    if (user != null)
                    {
                        _contextdb.User_Infos.Add(user); // or map to entity if using a DTO
                        _contextdb.SaveChanges();

                        return RedirectToAction("Login");
                    }
                }

                ViewBag.Error = "Invalid or expired OTP.";
                return View("EnterOtp");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while validating OTP: " + ex.Message;
                return View("EnterOtp");
            }
            }

        public ActionResult PatientRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRegister(User_Info model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Optional: Check if user with same email already exists
                    var existingUser = _contextdb.User_Infos.FirstOrDefault(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    // Set default values
                    model.UserType = "Patient";
                    model.Status = "Active";
                    model.Image = "default.png";
                    model.CreatedDate = DateTime.Now;
                    // Generate OTP (skip if not needed here)
                    generatedOtp = new Random().Next(100000, 999999).ToString();
                    otpExpiration = DateTime.Now.AddMinutes(5);
                    if (!SendOtpEmail(model.Email, generatedOtp))
                        TempData["DevelopmentOtp"] = generatedOtp;

                    Session["UserTempData"] = model;

                    return RedirectToAction("EnterOtp");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while registering: " + ex.Message);
                return View(model);
            }
            }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginForm(User_Info model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                {
                    TempData["MsgAdd"] = "Email and password are required.";
                    return RedirectToAction("Login");
                }

                var user = _contextdb.User_Infos.FirstOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    TempData["MsgAdd"] = "No user found with this email.";
                    return RedirectToAction("Login");
                }

                if (user.Password != model.Password)
                {
                    TempData["MsgAdd"] = "Incorrect password.";
                    return RedirectToAction("Login");
                }

                if (user.Status == "Delete")
                {
                    TempData["MsgAdd"] = "Your account is not Deleted.";
                    return RedirectToAction("Login");
                }

                if (user.Status != "Active")
                {
                    TempData["MsgAdd"] = "Your account is not active.";
                    return RedirectToAction("Login");
                }
              
                Session["UserId"] = user.Id;
                Session["UserType"] = user.UserType;
                Session["UserName"] = user.FirstName + " " + user.LastName;
                Session["UserEmail"] = user.Email;
                Session["image"] = user.Image;
                Session["Status"] = user.Status;

                if (user.UserType == "Doctor")
                    return RedirectToAction("Dashboard", "Doctor");

                if (user.UserType == "Patient")
                    return RedirectToAction("Dashboard", "Patient");
                if (user.UserType == "Admin")
                    return RedirectToAction("Index", "Admin");

                TempData["MsgAdd"] = "Unknown user type.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["MsgAdd"] = "An error occurred during login: " + ex.Message;
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult ResendOtp()
        {
            var user = Session["UserTempData"] as User_Info;
            if (user != null)
            {
                generatedOtp = new Random().Next(100000, 999999).ToString();
                otpExpiration = DateTime.Now.AddMinutes(5);
                if (!SendOtpEmail(user.Email, generatedOtp))
                    return new HttpStatusCodeResult(503, "SMTP is not configured.");
                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(400);
        }
        
        public ActionResult ForgotPassword()
        {
            ViewBag.ShowOtpModal = TempData["ShowOtpModal"] ?? false;
            ViewBag.ShowResetModal = TempData["ShowResetModal"] ?? false;
            ViewBag.Error = TempData["Error"];
            ViewBag.Success = TempData["Success"];
            ViewBag.Email = TempData["ResetEmail"] ?? _resetEmail;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    TempData["Error"] = "Email is required.";
                    return RedirectToAction("ForgotPassword");
                }

                var user = _contextdb.User_Infos.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    TempData["Error"] = "Email not found.";
                    return RedirectToAction("ForgotPassword");
                }

                // Generate OTP and set expiration
                _resetOtp = new Random().Next(100000, 999999).ToString();
                _otpResetExpiration = DateTime.Now.AddMinutes(5);
                _resetEmail = email;

                // Send OTP to user
                if (!SendOtpEmail(email, _resetOtp))
                {
                    TempData["Error"] = "Email delivery is not configured.";
                    return RedirectToAction("ForgotPassword");
                }

                TempData["ShowOtpModal"] = true;
                TempData["ResetEmail"] = email;
                TempData["Success"] = "OTP has been sent to your email.";
                return RedirectToAction("ForgotPassword");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("ForgotPassword");
            }
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterResetOtp(string otp)
        {
            try
            {
                if (otp == _resetOtp && DateTime.Now <= _otpResetExpiration)
                {
                    TempData["ShowResetModal"] = true;
                    TempData["ResetEmail"] = _resetEmail;
                    TempData["Success"] = "OTP verified successfully.";
                }
                else
                {
                    TempData["ShowOtpModal"] = true;
                    TempData["Error"] = "Invalid or expired OTP.";
                    TempData["ResetEmail"] = _resetEmail;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                TempData["ShowOtpModal"] = true;
            }
            return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    TempData["Error"] = "Both password fields are required.";
                    TempData["ShowResetModal"] = true;
                    return RedirectToAction("ForgotPassword");
                }

                if (newPassword != confirmPassword)
                {
                    TempData["Error"] = "Passwords do not match.";
                    TempData["ShowResetModal"] = true;
                    return RedirectToAction("ForgotPassword");
                }

                if (string.IsNullOrEmpty(_resetEmail))
                {
                    TempData["Error"] = "Session expired. Please try again.";
                    return RedirectToAction("ForgotPassword");
                }

                var user = _contextdb.User_Infos.FirstOrDefault(x => x.Email == _resetEmail);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("ForgotPassword");
                }

                user.Password = newPassword; 
                user.ConfirmPassword = user.Password;

                _contextdb.Entry(user).State = EntityState.Modified;
                if (!TryValidateModel(user))
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    TempData["Error"] = "Validation errors: " + string.Join(", ", errors);
                    TempData["ShowResetModal"] = true;
                    return RedirectToAction("ForgotPassword");
                }

                _contextdb.SaveChanges();
                SendPasswordChangeEmail(user.Email);

                _resetEmail = null;
                _resetOtp = null;
                _otpResetExpiration = DateTime.MinValue;

                TempData["Success"] = "Password reset successfully. Please log in.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                TempData["ShowResetModal"] = true;
                return RedirectToAction("ForgotPassword");
            }
        }
        private void SendPasswordChangeEmail(string email)
        {
            try
            {
                var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                    return;

                var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com")
                {
                    Port = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out var port) ? port : 587,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = "Your DoctorLab password has been changed",
                    Body = $"Hello,\n\n{email} password was successfully changed. If you did not request this change, please contact support immediately.",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending Forgate email");
            }
        }
        public ActionResult Logout()
        {
            if (Session["UserEmail"] == null )
                return RedirectToAction("Login", "Login");

            Session.Clear();
            Session.Abandon();
            ModelState.Clear();
            return View("Logout");
        }

    }
}
