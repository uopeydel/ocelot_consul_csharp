using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MigrateTest
{
    class Program1
    {
        static void Main1(string[] args)
        {
            Console.WriteLine("start");

            try
            {
                var con = new TestContext();
                var datas = con.Test.ToList();

                datas.ForEach(f =>
                {
                    var tmpLastName = f.LastName;
                    tmpLastName += " , ก";
                    f.LastName2 = tmpLastName;
                });
                con.UpdateRange();
                con.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            Console.ReadKey();
        }
    }

    public class TestContext : DbContext
    {
        public DbSet<Test> Test { get; set; }

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
            var capTest = "Password=P@ssw0rd;Persist Security Info=True;User ID=CAPUser;Initial Catalog=CAPDB_TEST;Data Source=103.91.189.212;Connect Timeout=30;Column Encryption Setting=enabled;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(capTest);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>()
            .ToTable("Test", "dbo");
            modelBuilder.Entity<Test>().Property("LastName2").HasMaxLength(50);
            modelBuilder.Entity<Test>()
                .HasKey(c => new { c.Id });
        }
    }

    public class Test
    {
        [Key]
        public int Id { get; set; }  //  int
        public string SSN { get; set; }  // char (11)
        public string FirstName { get; set; }  //  nvarchar(50)
        public string LastName { get; set; }  // nvarchar(50)
        public string LastName2 { get; set; }  // nvarchar(50)

    }



}
