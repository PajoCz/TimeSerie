using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeSerie.Core.Domain;
using Xunit;

namespace TimeSerie.Ef.Test
{
    public class DbContextTest
    {
        [Fact]
        public void TimeSeriesInsertTest()
        {
            //var options = new DbContextOptionsBuilder<TimeSerieContext>()
            //    .UseSqlite(@"Data Source=c:\Users\pajo\Source\Repos\TimeSerie\TimeSerie\TimeSerie.Ef\TimeSerie.db")
            //    .Options;

            //using (var db = new TimeSerieContext(options))
            using (var db = new TimeSerieContext())
            {
                db.Database.EnsureCreated();
                //db.Database.EnsureDeleted();

                db.TimeSerieHeaders.Add(new TimeSerieHeader()
                {
                    TimeSerieType = TimeSerieType.Decimal,
                    ValueDecimals = new List<TimeSerieValueDecimal>()
                    {
                        new TimeSerieValueDecimal(new DateTimeOffset(new DateTime(2000, 1, 1)), 10),
                        new TimeSerieValueDecimal(new DateTimeOffset(new DateTime(2000, 1, 2)), 20),
                    }
                });
                db.TimeSerieHeaders.Add(new TimeSerieHeader()
                {
                    TimeSerieType = TimeSerieType.String,
                    ValueStrings = new List<TimeSerieValueString>()
                    {
                        new TimeSerieValueString(new DateTimeOffset(new DateTime(2000, 1, 1)), "a"),
                        new TimeSerieValueString(new DateTimeOffset(new DateTime(2000, 1, 2)), "b"),
                    }
                });
                var count = db.SaveChanges();
                Debug.WriteLine("{0} records saved to database", count);

                Debug.WriteLine("");
                Debug.WriteLine("All ts in database:");
                foreach (var ts in db.TimeSerieHeaders)
                {
                    Debug.WriteLine($"{ts.TimeSerieHeaderId}");
                }
            }
        }

        [Fact]
        public void TimeSeriesReadTest()
        {
            //var options = new DbContextOptionsBuilder<TimeSerieContext>()
            //    .UseSqlite(@"Data Source=c:\Users\pajo\Source\Repos\TimeSerie\TimeSerie\TimeSerie.Ef\TimeSerie.db")
            //    .Options;

            //using (var db = new TimeSerieContext(options))
            using (var db = new TimeSerieContext())
            {
                db.Database.EnsureCreated();
                //db.Database.EnsureDeleted();

                var tsList = db.TimeSerieHeaders.Where(tsh => tsh.TimeSerieType == TimeSerieType.Decimal)
                    .Include(tsh => tsh.ValueDecimals).ToList();
                foreach (var ts in tsList)
                {
                    Debug.WriteLine($"TS DECIMAL Id={ts.TimeSerieHeaderId}");
                    foreach (var tsValueDecimal in ts.ValueDecimals)
                    {
                        Debug.WriteLine($"  ValueDecimal {tsValueDecimal.DateTimeOffset} = {tsValueDecimal.Value}");
                    }
                }

                tsList = db.TimeSerieHeaders.Where(tsh => tsh.TimeSerieType == TimeSerieType.String)
                    .Include(tsh => tsh.ValueStrings).ToList();
                foreach (var ts in tsList)
                {
                    Debug.WriteLine($"TS STRING Id={ts.TimeSerieHeaderId}");
                    foreach (var tsValueString in ts.ValueStrings)
                    {
                        Debug.WriteLine($"  ValueString {tsValueString.DateTimeOffset} = {tsValueString.Value}");
                    }
                }
            }
        }
    }
}
