using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class AutoMapper
    {
        public static MapConfig<T,R> Build<T, R>()
        {
            return new MapConfig<T,R>();
        }
    }
}
