using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDocLab.Models
{
    public class Payment
    {
        public Payment()
        {
            Created_at = DateTime.Now;
        }
        [Key]
        [Required(ErrorMessage = "ID is required.")]
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }
        public string Status { get; set; }

        public DateTime Created_at { get; set; }
    }

    public enum PaymentMethod
    {
        Cash,
        Card,
        Bkash,
        Rocket,
        Nagad,
        OnlineBanking
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}