using System.ComponentModel.DataAnnotations;
namespace RSWEBProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int? SerialNumber { get; set; }
        public int? Price { get; set; }
    }
}
