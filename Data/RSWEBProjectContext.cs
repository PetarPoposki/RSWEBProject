using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RSWEBProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RSWEBProject.Areas.Identity.Data;
namespace RSWEBProject.Data
{
    public class RSWEBProjectContext : IdentityDbContext<RSWEBProjectUser>

    {
        public RSWEBProjectContext (DbContextOptions<RSWEBProjectContext> options)
            : base(options)
        {
        }

        public DbSet<RSWEBProject.Models.Client>? Client { get; set; }

        public DbSet<RSWEBProject.Models.DeliveryMan>? DeliveryMan { get; set; }

        public DbSet<RSWEBProject.Models.Restaurant>? Restaurant { get; set; }

        public DbSet<RSWEBProject.Models.Order>? Order { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
}

    }
}
