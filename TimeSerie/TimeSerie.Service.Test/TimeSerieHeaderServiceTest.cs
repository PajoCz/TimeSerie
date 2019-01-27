using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TimeSerie.Core.Domain;
using TimeSerie.Ef;
using TimeSerie.Plugin.Convertor.Chmi;
using Xunit;

namespace TimeSerie.Service.Test
{
    public class TimeSerieHeaderServiceTest
    {
        [Fact]
        public async Task ProcessStreamByConvertorAsyncTest()
        {
            // In-memory database only exists while the connection is open
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                //Run In-Memory
                var service = new TimeSerieHeaderService(new DbContextOptionsBuilder<TimeSerieContext>().UseSqlite(connection).Options);
                //Run standard against TimeSerie.db file
                //var service = new TimeSerieHeaderService();

                var convertor = new ChmiAimDataConvertor();

                using (var dbContext = service.CreateTimeSerieContext())
                {
                    await dbContext.Database.EnsureCreatedAsync();
                }

                using (var fs = File.OpenRead("Files\\AIMdata-2019-01-26 18-00 2 stations.xml"))
                {
                    await service.ProcessStreamByConvertorAsync(fs, convertor);
                }

                var result = (await service.GetAllAsync()).ToList();
                Assert.Equal(4, result.Count);
                Assert.Equal("code=ABREA, component=NO2, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[0].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=14,9", result[0].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ABREA, component=PM10, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[1].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=5,0", result[1].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ACHOA, component=NO2, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[2].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=17,2", result[2].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ACHOA, component=PM10, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[3].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=22,0", result[3].ValueDecimals.ToSortedJoinedString("; "));

                using (var fs = File.OpenRead("Files\\AIMdata-2019-01-27 09-00 3 stations.xml"))
                {
                    await service.ProcessStreamByConvertorAsync(fs, convertor);
                }

                result = (await service.GetAllAsync()).ToList();
                Assert.Equal(6, result.Count);
                Assert.Equal("code=ABREA, component=NO2, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[0].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=14,9; 27.01.2019 9:00:00 +01:00=13,2", result[0].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ABREA, component=PM10, latitude=50.084385, longtitude=14.380116, name=Praha 6-Bøevnov", result[1].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=5,0; 27.01.2019 9:00:00 +01:00=3,0", result[1].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ACHOA, component=NO2, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[2].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=17,2; 27.01.2019 9:00:00 +01:00=8,8", result[2].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=ACHOA, component=PM10, latitude=50.030172, longtitude=14.51745, name=Praha 4-Chodov", result[3].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("26.01.2019 18:00:00 +01:00=22,0; 27.01.2019 9:00:00 +01:00=1,0", result[3].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=AKALA, component=NO2, latitude=50.094238, longtitude=14.442049, name=Praha 8-Karlín", result[4].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("27.01.2019 9:00:00 +01:00=22,4", result[4].ValueDecimals.ToSortedJoinedString("; "));
                Assert.Equal("code=AKALA, component=PM10, latitude=50.094238, longtitude=14.442049, name=Praha 8-Karlín", result[5].TimeSerieHeaderProperties.ToSortedJoinedString());
                Assert.Equal("27.01.2019 9:00:00 +01:00=5,0", result[5].ValueDecimals.ToSortedJoinedString("; "));
            }
        }
    }
}
