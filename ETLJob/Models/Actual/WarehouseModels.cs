using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ETLJob.Models.Actual
{
    public class Date
    {
        [Key]
        public String DateId { get; set; }
        public DateTime DateEntry { get; set; }
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
        public virtual ICollection<OrderFact> Orders { get; set; }

    }

    public class Location
    {
        [Key]
        public String LocationID { get; set; }
        public string City { get; set; }
        public String CityCode { get; set; }
        public String Country { get; set; }
        public String Province { get; set; }

        public virtual ICollection<OrderFact> Orders { get; set; }
    }

    public class Product
    {
        [Key]
        public String ProductId { get; set; }

        //public String skuNumber { get; set; }
        public String ProductDescription { get; set; }
        public String BrandDescription { get; set; }
        public String CategoryDescription { get; set; }
        public String ProductSize { get; set; }
        public String ProductRelatedInfo { get; set; }

        //public int itemPrice { get; set; } will get by drill through
        public virtual ICollection<OrderFact> Orders { get; set; }
    }

    public class Customer
    {
        [Key]
        public String CustomerId { get; set; }
        public String Name { get; set; }
        public String Age { get; set; }
        public String City { get; set; }

        //public String address { get; set; }
        public String Gender { get; set; }
        public virtual ICollection<OrderFact> Orders { get; set; }
    }

    public class OrderFact
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
    