using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MapperSeries
{
    class ReflectUtils
    {
        /*  CTOR DEALING METHODS */

        public static object ActivateConstructor(ConstructorInfo [] ctors)
        {
            // Choose less parameterized ctor
            int min = 0;
            for (int i = 0; i < ctors.Length; ++i)
            {
                if(ctors[min].GetParameters().Length < ctors[i].GetParameters().Length)
                {
                    min = i;
                }
            }

            object toReturn = CallConstructor(ctors[min]);

            return toReturn;
        }

        private static object CallConstructor(ConstructorInfo ctor)
        {
            ParameterInfo [] parameters = ctor.GetParameters();
            if (parameters.Length == 0) // Default Constructor found
            {
                return ctor.Invoke(new object[] { }); 
            }   

            object [] enterParams = new object[parameters.Length];
            int index = 0;
            foreach(ParameterInfo p in parameters)
            {
                if(p.ParameterType.IsPrimitive)
                {
                    enterParams[index] = Activator.CreateInstance(p.ParameterType);
                }
                else
                {
                    enterParams[index] = ActivateConstructor(p.ParameterType.GetConstructors());
                }
                ++index;    
            }
            return ctor.Invoke(enterParams);
        }
    
        /* PROPERTIES DEALING METHODS */
       
        public static bool FillWithProperties<T,TDest>(out TDest ret,
                                                    object dst,
                                                    object src,
                                                    string propNameToAvoid,
                                                    Type attrToAvoid,
                                                    ForMemberObj<T, object> forMember)
        {
            PropertyInfo[] src_props = src.GetType().GetProperties();
            PropertyInfo[] dest_props = dst.GetType().GetProperties();
            PropertyInfo prop;

            foreach (PropertyInfo p in dest_props)
            {
                Attribute[] attrs = Attribute.GetCustomAttributes(p);

                /* Case which we have a name of property to avoid*/
                if (propNameToAvoid != null)
                {
                    if (IsAvoidableProp(propNameToAvoid, p))
                    {
                        continue; // skip this prop
                    }
                }
                /* Case which we have a custom attribute to avoid */
                if (attrToAvoid != null)
                {
                    if (IsAvoidableProp(attrToAvoid, attrs))
                    {
                        continue; // skip this prop
                    }
                }
                /* Case which we have ForMember situation */
                if (forMember != null)
                {
                    if (forMember.propName == p.Name)
                    {

                        if (p.PropertyType == forMember.func(forMember.src).GetType())
                        {
                            p.SetValue(dst, forMember.func(forMember.src));
                            continue; // Skip normal case
                        }
                    }
                }

                /* Normal case of finding the prop name */
                if (PropertyWithThyName(out prop, p.Name, src_props))
                {
                    p.SetValue(dst, prop.GetValue(src));
                }
                else
                {
                    Console.WriteLine("Properties dont match .. Exiting program");
                    ret = (TDest)dst;
                    return false;
                }
            }
            ret = (TDest)dst;
            return true;
        }
        
    private static bool IsAvoidableProp(Type attrToAvoid, Attribute[] propAttrs)
        {
            foreach (Attribute a in propAttrs)
            {
                if(a.GetType() == attrToAvoid)
                {
                    return true;
                }
            }
            return false;
        }

        /* Overload */
        private static bool IsAvoidableProp(string propNameToAvoid, PropertyInfo prop)
        {
            return propNameToAvoid == prop.Name;
        }

        /* Find the given property in the given object */
        public static bool PropertyWithThyName(out PropertyInfo prop, string nameToFind, PropertyInfo[] array)
        {
            foreach (PropertyInfo p in array)
            {
                if (p.Name == nameToFind)
                {
                    prop = p;
                    return true;
                }
            }
            prop = null;
            return false;
        }
    }
}
  