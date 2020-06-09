using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AlwaysEncryptedTest
{
    class Program2
    {



        private static SqlConnection _sqlconn;

        /// <summary>
        /// Insert a row for a new patient.
        /// </summary>
        /// <param name="ssn">Patient's SSN.</param>
        /// <param name="firstName">Patient's First name</param>
        /// <param name="lastName">Patient's last name</param>
        /// <param name="birthdate">Patient's date of bith</param>
        private static void AddNewPatient(string ssn, string firstName, string lastName, DateTime birthdate)
        {
            SqlCommand cmd = _sqlconn.CreateCommand();

            // Use parameterized SQL to insert the data.
            //
            cmd.CommandText = @"INSERT INTO [dbo].[Patients] ([SSN], [FirstName], [LastName], [BirthDate]) VALUES (@SSN, @FirstName, @LastName, @BirthDate);";
            //cmd.CommandType = CommandType.Text;

            SqlParameter paramSSN = cmd.CreateParameter();
            paramSSN.ParameterName = @"@SSN";
            paramSSN.DbType = DbType.AnsiStringFixedLength;
            paramSSN.Direction = ParameterDirection.Input;
            paramSSN.Value = ssn;
            paramSSN.Size = 11;
            cmd.Parameters.Add(paramSSN);

            SqlParameter paramFirstName = cmd.CreateParameter();
            paramFirstName.ParameterName = @"@FirstName";
            paramFirstName.DbType = DbType.String;
            paramFirstName.Direction = ParameterDirection.Input;
            paramFirstName.Value = firstName;
            paramFirstName.Size = 50;
            cmd.Parameters.Add(paramFirstName);

            SqlParameter paramLastName = cmd.CreateParameter();
            paramLastName.ParameterName = @"@LastName";
            paramLastName.DbType = DbType.String;
            paramLastName.Direction = ParameterDirection.Input;
            paramLastName.Value = lastName;
            paramLastName.Size = 50;
            cmd.Parameters.Add(paramLastName);

            SqlParameter paramBirthdate = cmd.CreateParameter();
            paramBirthdate.ParameterName = @"@BirthDate";
            paramBirthdate.SqlDbType = SqlDbType.Date;
            paramBirthdate.Direction = ParameterDirection.Input;
            paramBirthdate.Value = birthdate;
            cmd.Parameters.Add(paramBirthdate);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Query the DB to find the patient with the desired SSN, and print the data in the console
        /// </summary>
        /// <param name="ssn">Patient's SSN</param>
        private static void FindAndPrintPatientInformation(string ssn)
        {
            SqlDataReader reader = null;
            try
            {
                reader = (ssn == null) ? FindAndPrintPatientInformationAll() : FindAndPrintPatientInformationSpecific(ssn);

                PrintPatientInformation(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Query the DB to find all the patients and print the data in the console
        /// </summary>
        private static void FindAndPrintPatientInformation()
        {
            FindAndPrintPatientInformation(null);
        }

        /// <summary>
        /// Implementation for querying all patients in the DB
        /// </summary>
        /// <returns>A datareader with the query resultset</returns>
        private static SqlDataReader FindAndPrintPatientInformationAll()
        {
            SqlCommand cmd = _sqlconn.CreateCommand();

            // Normal select statement.
            //
            cmd.CommandText = @"SELECT [SSN], [FirstName], [LastName], [BirthDate] FROM [dbo].[Patients] ORDER BY [PatientId]";
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Implementation for querying a single patient, based on SSN
        /// </summary>
        /// <param name="ssn">Patient's SSN</param>
        /// <returns>A datareader with the query resultset</returns>
        private static SqlDataReader FindAndPrintPatientInformationSpecific(string ssn)
        {
            SqlCommand cmd = _sqlconn.CreateCommand();

            // Use parameterized SQL to query the data.
            //
            cmd.CommandText = @"SELECT [SSN], [FirstName], [LastName], [BirthDate] FROM [dbo].[Patients] WHERE [SSN] = @SSN;";

            SqlParameter paramSSN = cmd.CreateParameter();
            paramSSN.ParameterName = @"@SSN";
            paramSSN.DbType = DbType.AnsiStringFixedLength;
            paramSSN.Direction = ParameterDirection.Input;
            paramSSN.Value = ssn;
            paramSSN.Size = 11;
            cmd.Parameters.Add(paramSSN);

            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Format and print the patient data in the console.
        /// </summary>
        /// <param name="reader">the rowset with the patient's data</param>
        private static void PrintPatientInformation(SqlDataReader reader)
        {
            string breaker = new string('-', (19 * 4) + 9);
            Console.WriteLine();
            Console.WriteLine(breaker);
            Console.WriteLine(breaker);
            Console.WriteLine(@"| {0,15} |  {1,15} |  {2,15} | {3,25} |", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine(breaker);
                    Console.WriteLine(@"| {0,15} |  {1,15} |  {2,15} | {3,25} | ", reader[0], reader[1], reader[2], ((DateTime)reader[3]).ToLongDateString());
                }
            }
            Console.WriteLine(breaker);
            Console.WriteLine(breaker);
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Print usage help on console
        /// </summary>
        static void PrintUsage()
        {
            Console.WriteLine(@"Usage: AlwaysEncryptedDemo <server_name> <database_name>");
            Console.WriteLine();
        }

        static void Main2(string[] args)
        {
            try
            {

                var cont = new TestContext();

                // var seond = cont
                //.test
                //.FromSqlRaw($"[dbo].[test]  @req='SSNSSN2'")
                //.ToList();
                var alis = new List<string> { "SSNSSN2" ,"SSNSSN2"};
               var ssss =  cont.Patients.Where(w => alis.Contains(w.SSN)).ToList();

                 var newd = new Patients
                {
                    BirthDate = DateTime.Now,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    SSN = "SSNSSN2",
                };
                cont.Patients.Add(newd);
                cont.SaveChanges();
                var abb2b = cont.Patients.ToList();
                var searchFull = "SSNSSN2";
                var searchQuery = "N2";
                var abbb = cont.Patients.Where(w => w.SSN == searchFull).ToList();

                var aaaaa = cont.Patients.Where(w => w.SSN.Contains(searchQuery)).ToList();



                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ReadKey();
                throw e;
            }
            Console.ReadKey();
            return;


            //if (args.Length != 2)
            //{
            //    PrintUsage();
            //    return;
            //}

            SqlConnectionStringBuilder strbldr = new SqlConnectionStringBuilder();

            strbldr.DataSource = "DESKTOP-HDMVNBB";
            strbldr.InitialCatalog = "THENY";//Thai_BIN2
            strbldr.IntegratedSecurity = true;
            strbldr.Password = "123456";
            strbldr.UserID = "sa";
            // Enable Always Encrypted in the connection we will use for this demo
            //
            strbldr.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;

            _sqlconn = new SqlConnection(strbldr.ConnectionString);

            _sqlconn.Open();

            try
            {
                SqlCommand cmd = _sqlconn.CreateCommand();

                // Use parameterized SQL to query the data.
                //
                cmd.CommandText = @"SELECT [SSN], [FirstName], [LastName], [BirthDate] FROM [dbo].[Patients] WHERE [SSN] =  @SSN ;";

                SqlParameter paramSSN = cmd.CreateParameter();
                paramSSN.ParameterName = @"@SSN";
                paramSSN.DbType = DbType.AnsiStringFixedLength;
                paramSSN.Direction = ParameterDirection.Input;
                paramSSN.Value = "123-45-6789";
                paramSSN.Size = 11;
                cmd.Parameters.Add(paramSSN);
                //cmd.Parameters.AddWithValue("@SSN", "123");

                SqlDataReader reader = cmd.ExecuteReader();
                PrintPatientInformation(reader);

                // Add a few rows to the table. 
                // Please notice that as far as the app is concerned, all data is in plaintext
                // 
                //AddNewPatient("123-45-6789", "ทดสอบ", "ชื่อ", new DateTime(1971, 5, 21));
                //AddNewPatient("111-22-3333", "ทดสอบ/2!", "!@#$#%^&*****_)(*&^", new DateTime(1974, 12, 1));
                //AddNewPatient("562-00-6354", "l;;สวัสดี", "Park", new DateTime(1928, 11, 18));

                // Print a few individual entries as well as the whole table
                // Once again, the app handles the data as plaintext
                //
                // // // FindAndPrintPatientInformation("123-45-6789");
                // FindAndPrintPatientInformation("111-22-3333");
                //  FindAndPrintPatientInformation();


            }
            finally
            {
                _sqlconn.Close();
            }
        }



    }



    public class TestContext : DbContext
    {
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Patients> test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder strbldr = new SqlConnectionStringBuilder();

            strbldr.DataSource = "DESKTOP-HDMVNBB";
            strbldr.InitialCatalog = "THENY";//Thai_BIN2
            strbldr.IntegratedSecurity = true;
            strbldr.UserID = "sa";
            strbldr.Password = "123456";
            //DESKTOP-HDMVNBB\MSSQLSERVER2019
            // Enable Always Encrypted in the connection we will use for this demo
            //
            strbldr.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
            var str = $@"Server={@"DESKTOP-HDMVNBB\MSSQLSERVER2019"};Database={strbldr.InitialCatalog};Trusted_Connection=False;Encrypt=True;Integrated Security=False;
MultipleActiveResultSets=true;persist security info=True;user id={strbldr.UserID};password={strbldr.Password};Column Encryption Setting=enabled;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(str);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patients>().Property("SSN").HasMaxLength(11);


            modelBuilder.Entity<test>()
            .ToTable("test", "dbo");
            modelBuilder.Entity<test>().Property("SSN").HasMaxLength(11);
            modelBuilder.Entity<test>()
                .HasKey(c => new { c.PatientId });
        }

    }


    public class Patients
    {
        [Key]
        public int PatientId { get; set; }  //  int
        public string SSN { get; set; }  // char (11)
        public string FirstName { get; set; }  //  nvarchar(50)
        public string LastName { get; set; }  // nvarchar(50)
        //public string MiddleName { get; set; }  //  nvarchar(50)
        //public string StreetAddress { get; set; }  //  nvarchar(50)
        //public string City { get; set; }  //  nvarchar(50)
        //public string ZipCode { get; set; }  //  char (5)
        //public string State { get; set; }  //  char (2)
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }


    public class test
    {
        [Key]
        public int PatientId { get; set; }  //  int
        public string SSN { get; set; }  // char (11)
        public string FirstName { get; set; }  //  nvarchar(50)
        public string LastName { get; set; }  // nvarchar(50) 
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }


}

