using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ETLJob.Models.Staging
{
    class StagingWarehouseContext : DbContext
    {
        public StagingWarehouseContext() : base("BUYON_CONNECTION") { }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
