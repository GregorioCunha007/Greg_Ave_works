
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class MapConfig<TSrc,TDest>
    {
        private MappedObj<TSrc, TDest> mapper = new MappedObj<TSrc, TDest>();

        public Mapper<TSrc, TDest> CreateMapper()
        {
            mapper._dest = typeof(TDest);
            mapper._src = typeof(TSrc);
            return mapper;
        } 
        
        public MapConfig<TSrc, TDest> IgnoreMember(string v)
        {
            mapper.propNameToAvoid = v;
            return this;
        }   
        
        public MapConfig<TSrc,TDest> IgnoreMember<A>()
        {
            mapper.attrToAvoid = typeof(A);
            return this;
        } 

        public MapConfig<TSrc,TDest> ForMember(string propName, Func<TSrc,object> func)
        {
            mapper.forMember = new ForMemberObj<TSrc,object>(propName, func, default(TSrc));
            return this;
        }
    }
}
