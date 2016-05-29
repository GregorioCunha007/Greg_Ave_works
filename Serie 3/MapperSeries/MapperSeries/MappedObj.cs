<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class MappedObj<TSrc, TDest> : Mapper<TSrc, TDest>
    {
        public Type _dest
        {
            get;set;
        }
        public Type _src
        {
            get;set;
        }
        public Type attrToAvoid
        {
            get;set;
        }
        public string propNameToAvoid
        {
            get; set;
        }
        
        public ForMemberObj<TSrc,object> forMember 
        {
            get;set;
        }

        public TDest Map(TSrc src)
        {
            ConstructorInfo[] dst_ctors = _dest.GetConstructors();
            TDest destiny = default(TDest);
            TDest out_Result = default(TDest); 
            // Fill func parameter 
            if (forMember != null)
            {
                forMember.src = src;
            }
                

            if (_src == src.GetType())
            {
                if (_dest.IsValueType) // Means its a Value type.
                {
                    if(dst_ctors.Length > 0) // Means it has a parameterized constructor
                    {
                        destiny = (TDest) ReflectUtils.ActivateConstructor(dst_ctors);
                    }

                    if (!ReflectUtils.FillWithProperties(out out_Result,destiny,src,propNameToAvoid,attrToAvoid,forMember))
                    {
                        return default(TDest); // Properties dont match
                    }
                }
                else // Means its a Reference type
                {
                    destiny = (TDest) ReflectUtils.ActivateConstructor(dst_ctors);
                    if(!ReflectUtils.FillWithProperties(out out_Result,destiny, src,propNameToAvoid, attrToAvoid, forMember))
                    {
                        return default(TDest); // Properties dont match
                    }
                }

            }
            else
            {
                Console.WriteLine("MappedObj was not created {0} ...", src.GetType());
            }

            return out_Result;
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

        public TDest [] MapToArray(IEnumerable<TSrc> src)
        {
            List<TDest> list = new List<TDest>();
            IEnumerator<TSrc> enumerator = src.GetEnumerator();
            while(enumerator.MoveNext())
            {
                list.Add(Map(enumerator.Current));
            }
            
            return list.ToArray();
        }

        public IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src)
        {
            foreach(TSrc t in src)
            {
                yield return Map(t);
            }
        }
    }
}
=======
﻿using System;
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
>>>>>>> 46ca7f29fa30d89964ae900f26ff0314af3408f0
