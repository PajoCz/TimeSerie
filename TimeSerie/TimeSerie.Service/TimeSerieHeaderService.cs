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
            var tsFromStream = (await new ChmiAimDataReader().Process(p_Stream)).ToList();
            using (var db = new TimeSerieContext())
            {
                db.Database.EnsureCreated();
                var tsFromDb = db.TimeSerieHeaders.Include(tsh => tsh.TimeSerieHeaderProperties).ToList();

                foreach (TimeSerieHeader tsstr in tsFromStream)
                {
                    var dbFoundToUpdate = tsFromDb.Find(tsdb =>
                    {
                        foreach (var dbProp in tsdb.TimeSerieHeaderProperties)
                        {
                            TimeSerieHeaderProperty propSameNameValueFound = tsstr.TimeSerieHeaderProperties.ToList()
                                .Find(strProp => strProp.Name == dbProp.Name && strProp.Value == dbProp.Value);
                            if (propSameNameValueFound == null)
                            {
                                return false;
                            }
                        }

                        foreach (TimeSerieValue<decimal> tsstrItem in tsstr.ValueDecimals)
                        {
                            tsstrItem.TimeSerieHeaderId = tsdb.TimeSerieHeaderId;
                        }
                        return true;
                    });

                    if (dbFoundToUpdate != null)
                    {
                        //UPDATE TimeSerieHeader
                        var dbDecimals = db.TimeSerieValueDecimals.Where(tsvd => tsvd.TimeSerieHeaderId == dbFoundToUpdate.TimeSerieHeaderId).ToList();
                        //decimalsToUpdate
                        foreach (var tsstrItem in tsstr.ValueDecimals)
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
                    else
                    {
                        //INSERT TimeSerieHeader
                        await db.TimeSerieHeaders.AddAsync(tsstr);
                    }
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
