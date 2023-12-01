using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace AppointmentScheduling.Models.ViewModels
{
    public class Attachment
    {
        [Key]
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        // Add other properties as needed, such as ContentType, FileSize, etc.

    }
    public class ChatMessage
    {

        public int MessageId { get; set; }

        [Display(Name = "Sender")]
        [Required]
        public string SenderId { get; set; }


        [Display(Name = "Recipient")]
        [Required]
        public string RecipientId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Message length must be between 1 and 200 characters!")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }

        public List<Attachment> Attachments { get; set; }
    }

}


