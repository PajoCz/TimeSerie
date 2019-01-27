using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeSerie.Core.Convertor;
using TimeSerie.Core.Domain;
using TimeSerie.Ef;

namespace TimeSerie.Service
{
    public class TimeSerieHeaderService
    {
        //todo: IAnotherPocoConvertor
        public async Task ProcessStreamByConvertorAsync(Stream p_Stream, ITimeSerieConvertor p_TimeSerieConvertor)
        {
            var tsFromStream = (await p_TimeSerieConvertor.Convert(p_Stream)).ToList();
            using (var db = new TimeSerieContext())
            {
                //db.Database.EnsureCreated();
                var tsFromDb = await db.TimeSerieHeaders.Include(tsh => tsh.TimeSerieHeaderProperties).ToListAsync();
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

        public async Task<IEnumerable<TimeSerieHeader>> GetAllAsync()
        {
            using (var db = new TimeSerieContext())
            {
                return await db.TimeSerieHeaders.Include(tsh => tsh.TimeSerieHeaderProperties).Include(tsh => tsh.ValueDecimals)
                    .Include(tsh => tsh.ValueStrings).ToListAsync();
            }
        }

        private static async Task ValueDecimalsInsertUpdate(TimeSerieContext p_DbContext, TimeSerieHeader p_TimeSerieHeaderFromDb,
            ICollection<TimeSerieValueDecimal> p_ValueDecimalsForInsertUpdate)
        {
            var dbValues = p_DbContext.TimeSerieValueDecimals
                .Where(tsv => tsv.TimeSerieHeaderId == p_TimeSerieHeaderFromDb.TimeSerieHeaderId).ToList();
            foreach (var p_ValueDecimalsForInsertUpdateItem in p_ValueDecimalsForInsertUpdate)
            {
                var dtoFound = dbValues.Find(d => d.DateTimeOffset == p_ValueDecimalsForInsertUpdateItem.DateTimeOffset);
                if (dtoFound != null)
                    dtoFound.Value = p_ValueDecimalsForInsertUpdateItem.Value;
                else
                    await p_DbContext.TimeSerieValueDecimals.AddAsync(p_ValueDecimalsForInsertUpdateItem);
            }
        }

        private static async Task ValueStringsInsertUpdate(TimeSerieContext p_DbContext, TimeSerieHeader p_TimeSerieHeaderFromDb,
            ICollection<TimeSerieValueString> p_ValueStringsForInsertUpdate)
        {
            var dbValues = p_DbContext.TimeSerieValueStrings
                .Where(tsv => tsv.TimeSerieHeaderId == p_TimeSerieHeaderFromDb.TimeSerieHeaderId).ToList();
            foreach (var p_ValueStringsForInsertUpdateItem in p_ValueStringsForInsertUpdate)
            {
                var dtoFound = dbValues.Find(d => d.DateTimeOffset == p_ValueStringsForInsertUpdateItem.DateTimeOffset);
                if (dtoFound != null)
                    dtoFound.Value = p_ValueStringsForInsertUpdateItem.Value;
                else
                    await p_DbContext.TimeSerieValueStrings.AddAsync(p_ValueStringsForInsertUpdateItem);
            }
        }
    }
}
