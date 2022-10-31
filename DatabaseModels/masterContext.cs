using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace netCoreApi.Models
{
    public partial class masterContext : DbContext
    {

        public masterContext(DbContextOptions<masterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Ipaddress> Ipaddresses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThreeLetterCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TwoLetterCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
            modelBuilder.Entity<Ipaddress>(entity =>
            {
                entity.ToTable("IPAddresses");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("IP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
