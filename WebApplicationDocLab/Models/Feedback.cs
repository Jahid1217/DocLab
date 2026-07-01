using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string Message {  get; set; }
        public DateTime Created_at { get; set; }
       public Feedback() {
            Created_at = DateTime.Now;
       }

    }
}