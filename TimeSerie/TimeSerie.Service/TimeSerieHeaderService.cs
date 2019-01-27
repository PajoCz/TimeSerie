using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeSerie.Core.Domain;
using TimeSerie.Ef;
using TimeSerie.ExternalReader.ChmiAimData;

namespace TimeSerie.Service
{
    public class TimeSerieHeaderService
    {
        public async Task Process(Stream p_Stream)
        {
            //todo: dynamic call convertor plugin
            var tsFromStream = (await new ChmiAimDataReader().Process(p_Stream)).ToList();
            using (var db = new TimeSerieContext())
            {
                //db.Database.EnsureCreated();
                var tsFromDb = db.TimeSerieHeaders.Include(tsh => tsh.TimeSerieHeaderProperties).ToList();
                foreach (TimeSerieHeader tsFromStreamItem in tsFromStream)
                {
                    var tsFromDbForUpdate = tsFromDb
                        .Where(tsFromDbItem => tsFromDbItem.TimeSerieType == tsFromStreamItem.TimeSerieType)
                        .FindWithSameProperties(tsFromStreamItem.TimeSerieHeaderProperties);

                    if (tsFromDbForUpdate != null)
                    {
                        if (tsFromDbForUpdate.TimeSerieType == TimeSerieType.Decimal && tsFromStreamItem.TimeSerieType == TimeSerieType.Decimal && tsFromStreamItem.ValueDecimals != null)
                        {
                            tsFromStreamItem.ValueDecimals.ToList().ForEach(i => i.TimeSerieHeaderId = tsFromDbForUpdate.TimeSerieHeaderId);
                            await ValueDecimalsInsertUpdate(db, tsFromDbForUpdate, tsFromStreamItem.ValueDecimals);
                        }
                        if (tsFromDbForUpdate.TimeSerieType == TimeSerieType.String && tsFromStreamItem.TimeSerieType == TimeSerieType.String && tsFromStreamItem.ValueStrings != null)
                        {
                            tsFromStreamItem.ValueStrings.ToList().ForEach(i => i.TimeSerieHeaderId = tsFromDbForUpdate.TimeSerieHeaderId);
                            await ValueStringsInsertUpdate(db, tsFromDbForUpdate, tsFromStreamItem.ValueStrings);
                        }
                    }
                    else
                    {
                        await db.TimeSerieHeaders.AddAsync(tsFromStreamItem);
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        private static async Task ValueDecimalsInsertUpdate(TimeSerieContext db, TimeSerieHeader TimeSerieHeaderFromDb,
            ICollection<TimeSerieValue<decimal>> p_ValueDecimalsForInsertUpdate)
        {
            var dbDecimals = db.TimeSerieValueDecimals
                .Where(tsvd => tsvd.TimeSerieHeaderId == TimeSerieHeaderFromDb.TimeSerieHeaderId).ToList();
            foreach (var tsstrItem in p_ValueDecimalsForInsertUpdate)
            {
                var dtoFound = dbDecimals.Find(d => d.DateTimeOffset == tsstrItem.DateTimeOffset);
                if (dtoFound != null)
                {
                    //UPDATE TimeSerieValueDecimal
                    dtoFound.Value = tsstrItem.Value;
                }
                else
                {
                    //INSERT TimeSerieValueDecimal
                    await db.TimeSerieValueDecimals.AddAsync(tsstrItem);
                }
            }
        }

        private static async Task ValueStringsInsertUpdate(TimeSerieContext db, TimeSerieHeader TimeSerieHeaderFromDb,
            ICollection<TimeSerieValue<string>> p_ValueStringsForInsertUpdate)
        {
            var dbStrings = db.TimeSerieValueStrings
                .Where(tsvd => tsvd.TimeSerieHeaderId == TimeSerieHeaderFromDb.TimeSerieHeaderId).ToList();
            foreach (var tsstrItem in p_ValueStringsForInsertUpdate)
            {
                var dtoFound = dbStrings.Find(d => d.DateTimeOffset == tsstrItem.DateTimeOffset);
                if (dtoFound != null)
                {
                    //UPDATE TimeSerieValueDecimal
                    dtoFound.Value = tsstrItem.Value;
                }
                else
                {
                    //INSERT TimeSerieValueDecimal
                    await db.TimeSerieValueStrings.AddAsync(tsstrItem);
                }
            }
        }
    }
}
