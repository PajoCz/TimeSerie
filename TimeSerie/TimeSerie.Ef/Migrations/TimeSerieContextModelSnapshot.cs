﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeSerie.Ef;

namespace TimeSerie.Ef.Migrations
{
    [DbContext(typeof(TimeSerieContext))]
    partial class TimeSerieContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieHeader", b =>
                {
                    b.Property<int>("TimeSerieHeaderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TimeSerieType");

                    b.Property<DateTimeOffset?>("ValuesFrom");

                    b.Property<DateTimeOffset?>("ValuesTo");

                    b.HasKey("TimeSerieHeaderId");

                    b.ToTable("TimeSerieHeader");
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieHeaderProperty", b =>
                {
                    b.Property<int>("TimeSerieHeaderPropertyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("TimeSerieHeaderId");

                    b.Property<string>("Value");

                    b.HasKey("TimeSerieHeaderPropertyId");

                    b.HasIndex("TimeSerieHeaderId");

                    b.ToTable("TimeSerieHeaderProperty");
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieValueDecimal", b =>
                {
                    b.Property<long>("TimeSerieValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TimeSerieValueDecimalId");

                    b.Property<DateTimeOffset>("DateTimeOffset");

                    b.Property<int>("TimeSerieHeaderId");

                    b.Property<decimal>("Value");

                    b.HasKey("TimeSerieValueId");

                    b.HasIndex("TimeSerieHeaderId");

                    b.ToTable("TimeSerieValueDecimal");
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieValueString", b =>
                {
                    b.Property<long>("TimeSerieValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TimeSerieValueStringId");

                    b.Property<DateTimeOffset>("DateTimeOffset");

                    b.Property<int>("TimeSerieHeaderId");

                    b.Property<string>("Value");

                    b.HasKey("TimeSerieValueId");

                    b.HasIndex("TimeSerieHeaderId");

                    b.ToTable("TimeSerieValueString");
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieHeaderProperty", b =>
                {
                    b.HasOne("TimeSerie.Core.Domain.TimeSerieHeader", "TimeSerieHeader")
                        .WithMany("TimeSerieHeaderProperties")
                        .HasForeignKey("TimeSerieHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieValueDecimal", b =>
                {
                    b.HasOne("TimeSerie.Core.Domain.TimeSerieHeader", "TimeSerieHeader")
                        .WithMany("ValueDecimals")
                        .HasForeignKey("TimeSerieHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeSerie.Core.Domain.TimeSerieValueString", b =>
                {
                    b.HasOne("TimeSerie.Core.Domain.TimeSerieHeader", "TimeSerieHeader")
                        .WithMany("ValueStrings")
                        .HasForeignKey("TimeSerieHeaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
