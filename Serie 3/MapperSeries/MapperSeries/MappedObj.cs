using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class MappedObj<TSrc, TDest> : Mapper<TSrc, TDest>
    {
        Type _dest; Type _src;

        public MappedObj(Type src, Type dest)
        {
            this._src = src;
            this._dest = dest;
        }

        public TDest Map(TSrc src)
        {
            ConstructorInfo[] dst_ctors = _dest.GetConstructors();
            TDest destiny = default(TDest);
            if (_src == src.GetType())
            {
                if (dst_ctors.Length == 0) // Means its a Value type.
                {
                    destiny = default(TDest);
                    if(!PropUtils.FillWithProperties(destiny, src))
                    {
                        return default(TDest); // Properties dont match
                    }
                }
                else
                {
                    if (dst_ctors[0].GetParameters().Length == 0)
                    {
                        // Default constructor 
                        destiny = (TDest)Activator.CreateInstance(_dest);
                        if (!PropUtils.FillWithProperties(destiny, src))
                        {
                            return default(TDest); // Properties dont match
                        }
                    }
                    else
                    {
                        // It's a parametized constructor
                        // TO DO : Idea -> Activator.CreateInstance(_dest, new object [] { null, null, null });
                        // Tantos null's quantos parametros receber.
                    }
                }

            }
            else
            {
                Console.WriteLine("MappedObj was not created {0} ...", src.GetType());
            }

            return destiny;
        }

        public TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>
        {

            TColDest coll = (TColDest)Activator.CreateInstance(typeof(TColDest));

            foreach (TSrc t in src) 
            {
                coll.Add(Map(t));
            }

            return coll;
        }
    }
}
