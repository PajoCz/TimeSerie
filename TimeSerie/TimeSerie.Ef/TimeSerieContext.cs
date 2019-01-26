using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using TimeSerie.Core.Domain;

namespace TimeSerie.Ef
{
    public class TimeSerieContext : DbContext
    {
        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new [] {new DebugLoggerProvider()});

        public TimeSerieContext()
        {
        }

        public TimeSerieContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TimeSerieHeader> TimeSerieHeaders { get; set; }
        public DbSet<TimeSerieValue<decimal>> TimeSerieValueDecimals { get; set; }
        public DbSet<TimeSerieValue<string>> TimeSerieValueStrings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TimeSerie.db");
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSerieHeader>().ToTable("TimeSerieHeader");
            modelBuilder.Entity<TimeSerieValue<decimal>>().ToTable("TimeSerieValueDecimal");
            modelBuilder.Entity<TimeSerieValue<decimal>>().Property(tsv => tsv.TimeSerieValueId)
                .HasColumnName("TimeSerieValueDecimalId");
            //modelBuilder.Entity<TimeSerieValue<decimal>>().Property(tsv => tsv.TimeSerieHeaderId)
            //    .HasColumnName("TimeSerieHeaderId");
            modelBuilder.Entity<TimeSerieValue<string>>().ToTable("TimeSerieValueString");
            modelBuilder.Entity<TimeSerieValue<string>>().Property(tsv => tsv.TimeSerieValueId)
                .HasColumnName("TimeSerieValueStringId");
        }
    }
}
