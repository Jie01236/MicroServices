using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MessagingBus
{
    public class MessageRequest
    {
        [BindProperty]
        [Required]
        public string Message { get; set; }
    }
}
