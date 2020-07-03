using System;
using FINDFIELD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace NTE.DbContextENCRYPT
{
    public partial class capdb : DbContext
    {
        public capdb()
        {
        }
        public DbSet<SPHELPTEXT> SPHELPTEXT { get; set; }
        public DbSet<INFORMATIONSCHEMA> INFORMATIONSCHEMA { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var GetApplNoBuilder = modelBuilder.Entity<SPHELPTEXT>();
            GetApplNoBuilder.HasKey(x => new { x.Text });

            var INFORMATIONSCHEMABuilder = modelBuilder.Entity<INFORMATIONSCHEMA>();
            INFORMATIONSCHEMABuilder.HasKey(x => new { x.ROUTINE_NAME });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
