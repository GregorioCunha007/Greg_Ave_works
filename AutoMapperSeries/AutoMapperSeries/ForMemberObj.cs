using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class ForMemberObj<T,Object>
    {
        public string propName;
        public Func<T,object> func;
        public T src;

        public ForMemberObj(string propName, Func<T,object> func, T src)
        {
            this.propName = propName;
            this.func = func;
            this.src = src;
        }
    }
}
