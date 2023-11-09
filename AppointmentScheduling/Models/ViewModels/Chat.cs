using System.ComponentModel.DataAnnotations;
using System;

namespace AppointmentScheduling.Models.ViewModels
{
    public class Chat
    {

        public int MessageId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string RecipientId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

    }
}
