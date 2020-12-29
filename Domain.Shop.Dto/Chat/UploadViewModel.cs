using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Shop.Dto.Chat
{
    public class UploadViewModel
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
