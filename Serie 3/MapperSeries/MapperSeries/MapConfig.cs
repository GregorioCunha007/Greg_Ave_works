using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class MapConfig<T,R>
    {
        public Mapper<T,R> CreateMapper()
        {
            return new MappedObj<T,R>(typeof(T),typeof(R));  
        } 
    }
}
