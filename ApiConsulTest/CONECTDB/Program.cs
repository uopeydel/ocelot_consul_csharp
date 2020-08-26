using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CONECTDB
{
    class Program
    {
        // DESKTOP-HDMVNBB\MSSQLSERVER2019
        // sa
        // 123456

        static void Main(string[] args)
        {
            OLD();
        }
        private static void OLD2()
        {
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                sqlBuilder.Encrypt = true;
                sqlBuilder.TrustServerCertificate = true;
                sqlBuilder.DataSource = @"DESKTOP-HDMVNBB\MSSQLSERVER2019";
                sqlBuilder.InitialCatalog = "CAPDB_Encrypt2";
                sqlBuilder.UserID = "sa";
                sqlBuilder.Password = "123456";
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                var coss = sqlBuilder.ConnectionString;
                var sqlConnection = @"Data Source=DESKTOP-HDMVNBB\MSSQLSERVER2019;Initial Catalog=CAPDB_Encrypt2;Integrated Security=True;User ID=sa;Password=123456;TrustServerCertificate=True;Column Encryption Setting=Enabled";

                FCDbContext cdb = new FCDbContext(coss);
                var dataxxx = "สวัสดี2";
                var newData = new CBMSCIF010Entity { brnCde = "00101", custCde = "CUS02", fstNamTH = dataxxx };
                cdb.CBMSCIF010.Add(newData);
                cdb.SaveChanges();
                var data = cdb.CBMSCIF010.Take(10).AsNoTracking().ToList();
                data.ForEach(f =>
                {
                    Console.WriteLine(f.brnCde);
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();

            Console.WriteLine(" ! ");
        }
        private static void OLD()
        {

            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                sqlBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                sqlBuilder.DataSource = "103.91.189.212";
                sqlBuilder.InitialCatalog = "CAPDB_Encrypt";
                sqlBuilder.UserID = "CAPUser";
                sqlBuilder.Password = "jun@2020";
                sqlBuilder.IntegratedSecurity = false;


                FCDbContext cdb = new FCDbContext(sqlBuilder.ConnectionString);
                //cdb.CBMSCIF010.Add(new CBMSCIF010Entity { brnCde = "001x1", custCde = "x0011", fstNamTH = "สวัสดี" });
                //cdb.SaveChanges();
                var data = cdb.CBMSCIF010.Take(10).AsNoTracking().ToList();
                data.ForEach(f =>
                {
                    Console.WriteLine(f.brnCde);
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();

            Console.WriteLine(" ! ");
        }
    }

    public class FCDbContext : DbContext
    {
        private readonly string _connectionString;

        //public FCDbContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        public FCDbContext(string serviceProvider)
        {
            _connectionString = serviceProvider;
        }


        public virtual DbSet<CBMSCIF010Entity> CBMSCIF010 { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder010 = modelBuilder.Entity<CBMSCIF010Entity>()
             .ToTable("CBMSCIF010", "dbo");
            builder010.HasKey(x => new { x.custCde });
            builder010.Property(x => x.fstNamTH).HasMaxLength(100);



            //modelBuilder.Entity<Taxonomy>().HasData(
            //    new Taxonomy() { Id = 1, Key = "FPTV", Value = "Test 1" });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class CBMSCIF010Entity
    {
        [Column("custCde")]
        [StringLength(13)]
        public string custCde { set; get; }
        [Column("brnCde")]
        [StringLength(5)]
        public string brnCde { set; get; }

        [Column("fstNamTH")]
        [StringLength(100)]
        public string fstNamTH { set; get; }
    }
}
