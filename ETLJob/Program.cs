using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FuzzyString;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ETLJob
{
    public class City
    {
        public String Name { get; set; }
        public String Province { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            List<City> citiesList = citiesListDataTable.AsEnumerable().Select(row => new City
            {
                Name = row["City"].ToString(),
                Province = row["Province"].ToString()
                //Province = row["Province"]
            }).ToList();

            var sqlConnection = new SqlConnection(sqlConnectionString);
            sqlConnection.Open();

            
            foreach (var row in ordersList)
            {
                if (row.PaymentCity.ToString() == "")
                {

                }
                else
                {
                    var result = CityMatched(row.PaymentCity.ToString().ToUpper(), citiesList);
                    if (result != null)
                    {
                        SqlCommand sql = new SqlCommand();
                        sql.Connection = sqlConnection;
                        sql.CommandText = "UPDATE Staging.buyon_order set matched_city = @matched_city where order_id = @order_id";
                        sql.Parameters.AddWithValue("@matched_city", result);
                        sql.Parameters.AddWithValue("@order_id", row.OrderID);
                        sql.ExecuteNonQuery();

                    }
                }
               
            }
            sw.Stop();
            Console.WriteLine($"Time Elapsed {(float)(sw.ElapsedMilliseconds / 1000)}");
            Console.ReadLine();
        }

        public static String CityMatched(String city, List<City> cities)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();

            // Choose which algorithms should weigh in for the comparison
            options.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);

            FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Normal;

            foreach (var c in cities)
            {
                var result = city.ApproximatelyEquals(c.Name.ToUpper(), options, tolerance);
                if (result)
                {
                    return c.Name;
                }
            }

            return null;
        }
    }
}
