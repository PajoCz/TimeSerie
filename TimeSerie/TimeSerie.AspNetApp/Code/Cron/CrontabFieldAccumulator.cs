using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeSerie.AspNetApp.Code.Cron
{
    public delegate void CrontabFieldAccumulator(int start, int end, int interval);
}
