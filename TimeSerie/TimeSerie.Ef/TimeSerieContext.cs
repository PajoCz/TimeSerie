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
        public DbSet<TimeSerieValueDecimal> TimeSerieValueDecimals { get; set; }
        public DbSet<TimeSerieValueString> TimeSerieValueStrings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TimeSerie.db");
            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSerieHeader>().ToTable("TimeSerieHeader");
            modelBuilder.Entity<TimeSerieValueDecimal>().ToTable("TimeSerieValueDecimal");
            modelBuilder.Entity<TimeSerieValueDecimal>().Property(tsv => tsv.TimeSerieValueId)
                .HasColumnName("TimeSerieValueDecimalId");
            modelBuilder.Entity<TimeSerieValueString>().ToTable("TimeSerieValueString");
            modelBuilder.Entity<TimeSerieValueString>().Property(tsv => tsv.TimeSerieValueId)
                .HasColumnName("TimeSerieValueStringId");
        }
    }
}
