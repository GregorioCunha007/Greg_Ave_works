using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class PropUtils
    {
        public static bool FillWithProperties(object dest, object src)
        {
            PropertyInfo[] src_props = src.GetType().GetProperties();
            PropertyInfo[] dest_props = dest.GetType().GetProperties();

            foreach (PropertyInfo p in src_props)
            {
                PropertyInfo prop;
                if (PropertyWithThyName(out prop, p.Name, dest_props))
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(dest, p.GetValue(src));
                    }

                }
                else
                {
                    Console.WriteLine("Properties dont match .. Exiting program");
                    return false;
                }
            }
            return true;
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
