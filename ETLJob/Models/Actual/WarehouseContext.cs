namespace ETLJob.Models.Actual

{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WarehouseContext : DbContext
    {

        public DbSet<Date> Dates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OrderFact> OrderFact { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public WarehouseContext()
            : base("BUYON_CONNECTION")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}

