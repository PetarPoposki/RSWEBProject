using System.ComponentModel.DataAnnotations;
namespace RSWEBProject.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public int? DeliveryManId { get; set; }
        [Display(Name = "Delivery Man")]
        public DeliveryMan? DeliveryMan { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
