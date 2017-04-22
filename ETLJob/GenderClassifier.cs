using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using nullpointer.Metaphone;
using System.Data.SqlClient;

namespace ETLJob
{
    class GenderClassifier
    {
        static String Male = "Male";
        static String Female = "Female";
        private static string GetHash(string word)
        {
            word = word.ToLower();
            Match singleMatch = Regex.Match(word, @"");
            MatchCollection matches = Regex.Matches(word, @"");
            word = Regex.Replace(word, @"ain$", "ein");
            word = Regex.Replace(word, @"ai", "ae");
            word = Regex.Replace(word, @"ay$", "e");
            word = Regex.Replace(word, @"ey$", "e");
            word = Regex.Replace(word, @"ie$", "y");
            word = Regex.Replace(word, @"^es", "is");
            word = Regex.Replace(word, @"a+", "a");
            word = Regex.Replace(word, @"j+", "j");
            word = Regex.Replace(word, @"d+", "d");
            word = Regex.Replace(word, @"u", "o");
            word = Regex.Replace(word, @"o+", "o");
            word = Regex.Replace(word, @"ee+", "i");
            word = Regex.Replace(word, @"yi+", "i");
            singleMatch = Regex.Match(word, @"(ar)");
            if (singleMatch.Success)
            {
                word = Regex.Replace(word, @"ar", "r");
            }
            word = Regex.Replace(word, @"iy+", "i");
            word = Regex.Replace(word, @"ih+", "eh");
            word = Regex.Replace(word, @"s+", "s");
            singleMatch = Regex.Match(word, @"[rst]y");
            if (singleMatch.Success && (!word.EndsWith("y") || !word.EndsWith("Y")))
            {
                word = Regex.Replace(word, @"y", "i");
            }
            word = Regex.Replace(word, @"ya+", "ia");
            singleMatch = Regex.Match(word, @"[bcdefghijklmnopqrtuvwxyz]i");
            if (singleMatch.Success)
            {
                word = Regex.Replace(word, @"i$", "y");
            }
            singleMatch = Regex.Match(word, @"[acefghijlmnoqrstuvwxyz]h");
            if (singleMatch.Success)
            {
                word = Regex.Replace(word, @"h", "");
            }
            word = Regex.Replace(word, @"k+", "k");
            word = Regex.Replace(word, @"k", "q");
            word = Regex.Replace(word, @"q+", "q");

            return word;
        }

        private static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        

        private static List<string[]> DoubleMetaphoneGenerateLists(string name, IEnumerable<dynamic> DbNames)
        {
            DoubleMetaphone metaphone = new DoubleMetaphone(name);
            string code = metaphone.PrimaryKey;
            List<string[]> namesWithSameSound = new List<string[]>();

            

            //DataTable table = new ExcelReader(path).GetWorksheet(worksheet);

            //var tableValues = table.ToJagged<string>("NAMES", "GENDER");
            //string[] names = new string[tableValues.Length];
            //string[] gender = new string[tableValues.Length];
            //for (int i = 0; i < tableValues.Length; i++)
            //{
            //    names[i] = tableValues[i][0].ToLower();
            //    gender[i] = tableValues[i][1];
            //}

            //for (int i = 0; i < names.Length; i++)
            //{
            //    metaphone.computeKeys(names[i]);
            //    if (metaphone.PrimaryKey == code)
            //    {
            //        namesWithSameSound.Add(new string[] { names[i], gender[i] });
            //    }
            //}


            foreach(var item in DbNames)
            {
                metaphone.computeKeys(item.Name.ToString());
                if (metaphone.PrimaryKey == code)
                {
                    namesWithSameSound.Add(new string[] { item.Name.ToString() , item.gender.ToString() });
                }
            }
            return namesWithSameSound;
        }

        private static List<string[]> GetRomanUrduHashes(string name, IEnumerable<dynamic> DbNames)
        {
            string code = GetHash(name);
            List<string[]> namesWithSameSound = new List<string[]>();

            //Moiz's Commented Code

            //DataTable table = new ExcelReader(path).GetWorksheet(worksheet);
            //var tableValues = table.ToJagged<string>("NAMES", "GENDER");
            //string[] names = new string[tableValues.Length];
            //string[] gender = new string[tableValues.Length];

            //for (int i = 0; i < tableValues.Length; i++)
            //{
            //    names[i] = tableValues[i][0].ToLower();
            //    gender[i] = tableValues[i][1];
            //}

            //for (int i = 0; i < names.Length; i++)
            //{
            //    if (object.Equals(GetHash(names[i]), code))
            //    {
            //        namesWithSameSound.Add(new string[] { names[i], gender[i] });
            //    }
            //}

            foreach(var item in DbNames)
            {
                if(object.Equals(GetHash(item.Name.ToString()), code))
                {
                    namesWithSameSound.Add(new string[] { item.Name.ToString(), item.gender.ToString() });
                }
            }
            return namesWithSameSound;
        }

        public static string GetGender(string name, IEnumerable<dynamic> DbNames)
        {
            name = name.ToLower();
            char firstCharacterFromName = name[0];
            //char firstCharacterFromIndexSpecifiedListValue;
            List<string[]> namesWithSameSoundWithDoubleMetaphoneAlgorithm = DoubleMetaphoneGenerateLists(name, DbNames);
            List<string[]> namesWithSameSoundWithRomanUrduHashes = GetRomanUrduHashes(name, DbNames);
            List<string[]> concatenatedNamesList = namesWithSameSoundWithDoubleMetaphoneAlgorithm.Concat(namesWithSameSoundWithRomanUrduHashes).ToList();

            if (concatenatedNamesList.ElementAtOrDefault(0) == null)
            {
                //return new string[] { "Name Not Found", "M" };
                return Male;
            }
            int lowestLevenshteinDistance = ComputeLevenshteinDistance(concatenatedNamesList.ElementAt(0)[0], name.ToLower());
            int lowestLevenshteinDistanceIndex = 0;
            int levenshteinDistanceAtSpecifiedIndex = 0;

            //
            /*for (int i = 0; i < concatenatedNamesList.Count; i++)
            {
                firstCharacterFromIndexSpecifiedListValue = concatenatedNamesList.ElementAt(i)[0][0];
                if (firstCharacterFromIndexSpecifiedListValue != firstCharacterFromName)
                {
                    concatenatedNamesList.RemoveAt(i);
                }
            }*/


            List<int> indexes = new List<int>();

            for (int i = 0; i < concatenatedNamesList.Count; i++)
            {
                levenshteinDistanceAtSpecifiedIndex = ComputeLevenshteinDistance(concatenatedNamesList.ElementAt(i)[0], name);

                if (levenshteinDistanceAtSpecifiedIndex < lowestLevenshteinDistance)
                {
                    indexes.Add(i);
                    lowestLevenshteinDistance = levenshteinDistanceAtSpecifiedIndex;
                    lowestLevenshteinDistanceIndex = i;
                }
            }


            // Shady's Code
            int maleCounter=0, femaleCounter=0;
            foreach(var item in DbNames)
            {
                if(item.Name.ToUpper() == concatenatedNamesList.ElementAt(lowestLevenshteinDistanceIndex)[0].ToUpper())
                {
                    if (item.gender.ToString() == "M")
                    {
                        maleCounter++;
                        
                    }

                    else if (item.gender.ToString() == "F")
                    {
                        femaleCounter++;
                    }
                }
            }

            if(maleCounter >= femaleCounter)
            {
                return Male;
            }
            else
            {
                return Female;
            }

            //return concatenatedNamesList.ElementAt(lowestLevenshteinDistanceIndex);
        }


        //static void Main(string[] args)
        //{

        //    string[] output = GetGender("ustad", @"D:\Projects\VS 15\ML\ML\MuslimNames.xls", "Muslim Names");
        //    Console.WriteLine(output[1]);

        //}

        public static String ComputeGender(String name, IEnumerable<dynamic> DbNames)
        {
            //string[] output = GetGender(name, @"D:\Projects\VS 15\ML\ML\MuslimNames.xls", "Muslim Names");

            //var connection = new SqlConnection();
            //SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            //connBuilder.DataSource = "127.0.0.1";
            //connBuilder.UserID = "sa";
            //connBuilder.Password = "";
            //connBuilder.InitialCatalog = "Buyon";

            //connection.ConnectionString = connBuilder.ConnectionString;
            //connection.Open();

            //StringBuilder sql = new StringBuilder();
            //sql.AppendLine("SELECT * FROM Staging.Muslim_Names;");


            //var da = new SqlDataAdapter(sql.ToString(), connection);
            //var table = new DataTable();
            //da.Fill(table);

            //var names = (from rows in table.Rows.OfType<DataRow>()
            //             select new
            //             {
            //                 Name = rows["name"].ToString(),
            //                 gender = rows["gender"],
            //             });

            String Gender = GetGender(name, DbNames);
            //Console.WriteLine("Final Gender : "+Gender);
            return Gender;
        }


    }
}