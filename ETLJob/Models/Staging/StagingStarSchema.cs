using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETLJob.Models.Staging
{
    [Table("StagingDataMart.Dates")]
    public class Date
    {
        [Key]
        public DateTime date { get; set; }
        public int TimeQuadrant { get; set; }
        public String DateDescription { get; set; }
        public string DayOfWeek { get; set; }
        public System.Nullable<int> DayNumberInCalenderMonth { get; set; }
        public System.Nullable<int> DayNumberInCalenderYear { get; set; }
        public System.Nullable<int> LastDayInMonthIndicator { get; set; }
        public System.Nullable<int> CalenderWeekNumberInYear { get; set; }
        public String CalenderMonthName { get; set; }
        public System.Nullable<int> CalenderMonthNumberInYear { get; set; }
        public System.Nullable<DateTime> CalenderWeekEndingDate { get; set; }
        public int CalenderQuarter { get; set; }
        public System.Nullable<int> CalenderYear { get; set; }
        public String Season { get; set; }
        public String CalenderYearMonth { get; set; }
        public System.Nullable<int> HolidayIndicator { get; set; }
        public String HolidayDescriptor { get; set; }
        public System.Nullable<DateTime> SqlDateTimestamp { get; set; }
        public System.Nullable<DateTime> UnixDateTimestamp { get; set; }
        public System.Nullable<int> WeekdayIndicator { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("StagingDataMart.Locations")]
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public String Country { get; set; }
        [StringLength(255)]
        public String Province { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("StagingDataMart.Products")]
    public class Product
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public int ProductNumber { get; set; }
        //public String skuNumber { get; set; }
        [StringLength(255)]
        public String ProductDescription { get; set; }
        [StringLength(255)]
        public String BrandDescription { get; set; }
        [StringLength(255)]
        public String CategoryDescription { get; set; }
        [StringLength(255)]
        public String ProductSize { get; set; }
        [StringLength(255)]
        public String ProductStatus { get; set; }
        [StringLength(255)]
        public string RecordStatus { get; set; }
        //public String ProductRelatedInfo { get; set; }

        //public int itemPrice { get; set; } will get by drill through
        public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("StagingDataMart.Customers")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [StringLength(255)]
        public String Name { get; set; }
        //public String Age { get; set; }
        [StringLength(255)]
        public String City { get; set; }

        //public String address { get; set; }
        public String Gender { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("StagingDataMart.Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public virtual Date DateFK { get; set; }
        public virtual Product ProductFK { get; set; }
        public virtual Location LocationFK { get; set; }
        public virtual Customer CustomerFK { get; set; }
        public int TransactionAmount { get; set; }
        public int DiscountedAmount { get; set; }
        public int NetAmount { get; set; }
    }
}
