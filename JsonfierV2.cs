using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Serie1
{
    public class Jsonfier
    {
        private static StringBuilder finalJsonObject = new StringBuilder();

        public static void Reset()
        {
            finalJsonObject = new StringBuilder();
        }

        public static string ToJson(object src)
        {
            if (src == null)
            {
                finalJsonObject.Append("No Value");
                return finalJsonObject.ToString();
            }

            Type t = src.GetType();

            if (t.IsPrimitive)
            {
                finalJsonObject.Append(src);
            }
            else if (t == typeof(string))
            {
                finalJsonObject.Append("\"" + src + "\"");

            }
            else if (t.IsArray)
            {
                IEnumerable seq = (IEnumerable)src;
                IEnumerator it = seq.GetEnumerator();
                finalJsonObject.Append('[');
                while (it.MoveNext())
                {
                    ToJson(it.Current);
                    finalJsonObject.Append(',');
                }
                finalJsonObject.Remove(finalJsonObject.Length - 1, 1);
                finalJsonObject.Append(']');
            }
            else // Is object
            {

					FieldInfo [] fields = t.GetFields();
                    finalJsonObject.Append('{');
                    finalJsonObject.Append('\n');
                    foreach (FieldInfo member in fields)
                    {
                        ToJson(member.Name);
                        finalJsonObject.Append(":");
                        ToJson(member.GetValue(src));
                        finalJsonObject.Append(',');
                        finalJsonObject.Append('\n');
                    }
                    finalJsonObject.Remove(finalJsonObject.Length - 2, 1);
                    finalJsonObject.Append('}');
                    finalJsonObject.Append('\n');
                

            }

            return finalJsonObject.ToString();
        }
    }

}