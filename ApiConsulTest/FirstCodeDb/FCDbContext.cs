using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(b => { b.AddConsole(); });
        public FCDbContext(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<FCDbContext>>();
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
        }


        public virtual DbSet<Taxonomy> Taxonomy { get; set; }
        public virtual DbSet<Master> Master { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           

           
            

            //modelBuilder.Entity<Taxonomy>().HasData(
            //    new Taxonomy() { Id = 1, Key = "FPTV", Value = "Test 1" });

            //base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder
#if DEBUG
                .UseLoggerFactory(loggerFactory)
#endif
                .UseSqlServer(_connectionString);
        }
    }
}
