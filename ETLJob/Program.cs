using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FuzzyString;
using System.Data.SqlClient;
namespace ETLJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlConnectionString = new SqlConnectionStringBuilder()
            {
                UserID = "sa",
                Password = "Wajeeh_ahmed93",
                InitialCatalog = "Buyon",
                DataSource = "127.0.0.1"
            }.ConnectionString;
            var ordersDataTable = new DataTable();
            var sqlDataAdapter = new SqlDataAdapter("select * from Staging.buyon_order", sqlConnectionString);
            sqlDataAdapter.Fill(ordersDataTable);
            var ordersList = ordersDataTable.AsEnumerable().Select(row => new
            {
                OrderID = row["order_id"],
                FirstName = row["firstname"],
                LastName = row["lastname"],
                PaymentCity = row["payment_city"],
                PayentCountry = row["payment_country"],
                PaymentCountryID = row["payment_country_id"],
                OrderStatusID = row["order_status_id"],
                DateAdded = row["date_added"],
                DateModified = row["date_modified"]
            }).ToList();
            var citiesListDataTable = new DataTable();
            sqlDataAdapter = new SqlDataAdapter("select * from Staging.cities", sqlConnectionString);
            sqlDataAdapter.Fill(citiesListDataTable);
            var citiesList = citiesListDataTable.AsEnumerable().Select(row => new
            {
                City = row["City"],
                Province = row["Province"]
            }).ToList();
            foreach (var row in ordersList)
            {
                
            }
        }

        public static String CityMatched(String city, ref List<String> cities)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();

            // Choose which algorithms should weigh in for the comparison
            options.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);

            FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Strong;

            foreach (var c in cities)
            {
                var result = city.ApproximatelyEquals(c, options, tolerance);
                if (result)
                {
                    return c;
                }
            }

            return null;
        }
    }
}
