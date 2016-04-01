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
        private static Dictionary<int, Mapper> map = new Dictionary<int, Mapper>();

        static Jsonfier()
        {
            map.Add(0, new Fields());
            map.Add(1, new Properties());
            map.Add(2, new Events());
            map.Add(3, new Methods());
        }

        public static string ToJson(object src, int typeOfMember)
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
                    ToJson(it.Current, typeOfMember);
                    finalJsonObject.Append(',');
                }
                finalJsonObject.Remove(finalJsonObject.Length - 1, 1);
                finalJsonObject.Append(']');
            }
            else // Is object
            {

                Mapper mapped;
                bool valid = map.TryGetValue(typeOfMember, out mapped);

                if (!valid)
                {
                    return "Wrong Option";
                }
                else
                {
                    MemberInfo[] mI = mapped.m(src);
                    finalJsonObject.Append('{');
                    finalJsonObject.Append('\n');
                    foreach (MemberInfo member in mI)
                    {
                        ToJson(member.Name, typeOfMember);
                        finalJsonObject.Append(":");
                        ToJson(mapped.GetInfoMember(src, member), typeOfMember);
                        finalJsonObject.Append(',');
                        finalJsonObject.Append('\n');
                    }
                    finalJsonObject.Remove(finalJsonObject.Length - 2, 1);
                    finalJsonObject.Append('}');
                    finalJsonObject.Append('\n');
                }

            }

            return finalJsonObject.ToString();
        }

    abstract class Mapper
    {
        public abstract MemberInfo[] m(object target);
        public abstract object GetInfoMember(object instance, MemberInfo m);
    }

    class Fields : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            FieldInfo[] fields = target.GetType().GetFields();
            return fields;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            FieldInfo f = (FieldInfo)m;
            return f.GetValue(instance);
        }
    }

    class Properties : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            PropertyInfo[] props = target.GetType().GetProperties();
            return props;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            PropertyInfo prop = (PropertyInfo)m;
            return prop.GetValue(instance);
        }
    }

    class Events : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            EventInfo[] events = target.GetType().GetEvents();
            return events;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            EventInfo eventt = (EventInfo)m;
            return eventt.EventHandlerType.ToString();
        }
    }

    class Methods : Mapper
    {
        public override MemberInfo[] m(object target)
        {
            MethodInfo[] methods = target.GetType().GetMethods();
            return methods;
        }

        public override object GetInfoMember(object instance, MemberInfo m)
        {
            MethodInfo method = (MethodInfo)m;
            return method.ReturnType.ToString();
        }
    }
   }
}