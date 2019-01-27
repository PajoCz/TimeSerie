using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TimeSerie.AspNetApp.Code.Scheduling
{
    public interface IScheduledTask
    {
        /// <summary>
        /// Minute Hour Day Month DayOfWeek
        /// </summary>
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
