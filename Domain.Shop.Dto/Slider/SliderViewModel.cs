using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Shop.Dto.Slider
{

    public class SliderViewModel
    {
        public string Id { get; set; }
        public string PhotoName { get; set; }

        public bool Status { get; set; }
        public IFormFile ProfileImage { get; set; }

    }
}