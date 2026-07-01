using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplicationDocLab.Context;
using WebApplicationDocLab.Models;

namespace WebApplicationDocLab.Controllers
{
    public class MessageController : Controller
    {
        private DoctorLab _contextdb = new DoctorLab();

        // GET: Message/Chat?userId=5
        public ActionResult Chat(int userId)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Login");

            int currentUserId = (int)Session["UserId"];
            var currentUserType = Session["UserType"].ToString();

            // Restrict patient-to-patient chat
            var otherUser = _contextdb.User_Infos.Find(userId);
            if (currentUserType == "Patient" && otherUser.UserType == "Patient")
            {
                return new HttpStatusCodeResult(403, "Patients cannot chat with other patients.");
            }

            var messages = _contextdb.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                            (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToList();

            ViewBag.OtherUser = otherUser;
            ViewBag.CurrentUserId = currentUserId;
            return View(messages);
        }

        // POST: Message/Send
        [HttpPost]
        public ActionResult Send(int receiverId, string content)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Login");

            int senderId = (int)Session["UserId"];
            var senderType = Session["UserType"].ToString();
            var receiver = _contextdb.User_Infos.Find(receiverId);

            // Restrict patient-to-patient chat
            if (senderType == "Patient" && receiver.UserType == "Patient")
            {
                return new HttpStatusCodeResult(403, "Patients cannot chat with other patients.");
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.Now
            };

            _contextdb.Messages.Add(message);
            _contextdb.SaveChanges();

            return RedirectToAction("Chat", new { userId = receiverId });
        }
    }
}
