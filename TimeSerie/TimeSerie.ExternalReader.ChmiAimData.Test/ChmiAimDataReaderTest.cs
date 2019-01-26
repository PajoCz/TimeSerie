using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TimeSerie.Core.Domain;
using Xunit;

namespace TimeSerie.ExternalReader.ChmiAimData.Test
{
    public class ChmiAimDataReaderTest
    {
        [Fact]
        public async Task Test1()
        {
            using (var fs = File.OpenRead("AIMdata.xml"))
            {
                var result = await new ChmiAimDataReader().Process(fs);
            }
        }
    }
}
