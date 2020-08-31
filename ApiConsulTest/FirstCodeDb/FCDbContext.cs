using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCodeDb
{
    // Add-Migration nameinit -Context FCDbContext
    // Update-Database nameinit -Context FCDbContext

    public class FCDbContext : DbContext
    {
        private readonly string _connectionString;

        //public FCDbContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        public FCDbContext(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<FCDbContext>>();
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
        }


        public virtual DbSet<Taxonomy> Taxonomy { get; set; }
        public virtual DbSet<Master> Master { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Taxonomy>()
                .ToTable("taxonomy") 
                .HasMany(o => o.Masters)
                .WithOne(o=> o.Taxonomy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Master>()
             .ToTable("master")
             .HasOne(o => o.Taxonomy)
             .WithMany(o => o.Masters)
             .OnDelete(DeleteBehavior.SetNull);
            

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
}
