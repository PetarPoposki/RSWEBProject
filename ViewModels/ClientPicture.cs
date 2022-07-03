using RSWEBProject.Models;
using System.ComponentModel.DataAnnotations;

namespace RSWEBProject.ViewModels
{
    public class ClientPicture
    {
        public Client? client { get; set; }

        [Display(Name = "Upload picture")]
        public IFormFile? ProfilePictureFile { get; set; }

        [Display(Name = "Picture name")]
        public string? ProfilePictureName { get; set; }
    }
}