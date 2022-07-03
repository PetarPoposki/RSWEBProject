using RSWEBProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RSWEBProject.ViewModels
{
    public class DeliveryManQuery
    {
        public IList<DeliveryMan> DeliveryMen { get; set; }

        public string FullName { get; set; }

    }
}