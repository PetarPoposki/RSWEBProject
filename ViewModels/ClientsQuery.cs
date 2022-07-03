using RSWEBProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RSWEBProject.ViewModels
{
    public class ClientQuery
    {
        public IList<Client> Clients { get; set; }


        public string Email { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}