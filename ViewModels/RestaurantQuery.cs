using RSWEBProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RSWEBProject.ViewModels
{
    public class RestaurantQuery
    {
        public IList<Restaurant> Restaurants { get; set; }

        public SelectList Types { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}

