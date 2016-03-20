using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Reflect
{
    public class Jsonfier
    {

        private static bool dealingWithArray = false;
        private static bool dealingWithObject = true;
        private static BindingFlags fieldsFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static StringBuilder allFields = new StringBuilder();

        public static string ToJson(object src)
        {
            if (src == null || src.GetType().IsPrimitive || src.GetType() == typeof(String))
                Console.WriteLine("Recursion ended");

           
            String result = RunTroughFields(src);

            return result;
        }

        private static string RunTroughFields(object instance)
        {
            
            FieldInfo[] fields = instance.GetType().GetFields(fieldsFlags);

            foreach ( FieldInfo f in fields)
            {

                if (f.GetType().IsPrimitive)
                {
                    if (dealingWithObject)
                        allFields.Append(f.Name + ":" + f.GetValue(instance) + ", ");
                    else if (dealingWithArray)
                        allFields.Append(f.GetValue(instance) + ","); // Change
                }
                else if( f.GetType().IsArray)
                {
                    DealWithArray(f.GetValue(instance));
                }
                else
                {
                    DealWithObject(f.GetValue(instance));
                }
                
            }

            String ret = allFields.ToString();
            return ret; 
          }

        private static void DealWithObject(object new_src)
        {
            dealingWithObject = true; dealingWithArray = false;
            ToJson(new_src);
        }

        private static void DealWithArray(object new_src)
        {
            dealingWithObject = false; dealingWithArray = true;
            ToJson(new_src);
        }
    }
}