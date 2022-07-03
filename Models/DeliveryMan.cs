using RSWEBProject.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
namespace RSWEBProject.Models
{
    public class DeliveryMan
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }
        public DateTime? HireDate { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
        public string? RSWEBProjectUserId { get; set; }
        public RSWEBProjectUser? RSWEBProjectUser { get; set; }
    }
}
