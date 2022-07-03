using RSWEBProject.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
namespace RSWEBProject.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }
        public string? Email { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string? RSWEBProjectUserId { get; set; }
        public RSWEBProjectUser? RSWEBProjectUser { get; set; }
    }
}
