using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace Serie1
{
    class Program
    {
        static void Main(string[] args)
        {

            Student s1 = new Student("gustavo", 40622);
            
            Console.WriteLine(Jsonfier.ToJson(s1, 0));
        }
    }

    class Student
    {
       
        public int student_number;
        public string name;
        public int[] INTEIROS = { 1, 2, 3 };
        public object[] CENAS = { "hugo", new int[] { 4, 5, 6 }, "morticia" };
        public float t_id = 90.33f;
        public object[] bbva = { new int[] { 7, 8, 9 }, new object [] { "balelas", new int[] { 12, 13, 15 }, "shin" }};

        public event Action dolceDoge
        {
            add
            {

            }
            remove
            {

            }
        }

        public String tt
        {
            get; set;
        }

        public Student(string n, int x)
        {
            name = n;
            student_number = x;
        }
    }

    class Jsonfier
    {
        private static StringBuilder finalJsonObject = new StringBuilder();
        private static IDictionary<int, Mapper> map = new Dictionary<int,Mapper>();

        static Jsonfier()
        {
            map.Add(0, new Fields());
            map.Add(1, new Properties());
            map.Add(2, new Events());
            map.Add(3, new Methods());
        }

        public static string ToJson(object src,int typeOfMember)
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

            }else if( t.IsArray )
            {
                IEnumerable seq = (IEnumerable)src;
                IEnumerator it = seq.GetEnumerator();
                finalJsonObject.Append('[');
                while( it.MoveNext() )
                {
                    ToJson(it.Current,typeOfMember);
                    finalJsonObject.Append(',');
                }
                finalJsonObject.Remove(finalJsonObject.Length - 1, 1);
                finalJsonObject.Append(']');
            }
            else // Is object
            {

                Mapper mapped;
                bool valid = map.TryGetValue(typeOfMember,out mapped);
                
                if( !valid )
                {
                    return "Wrong Option";
                }
                else
                {
                    MemberInfo [] mI = mapped.m(src);
                    finalJsonObject.Append('{');
                    finalJsonObject.Append('\n');
                    foreach (MemberInfo member in mI)
                    {
                        ToJson(member.Name,typeOfMember);
                        finalJsonObject.Append(":");
                        ToJson(mapped.GetInfoMember(src, member),typeOfMember);
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
    }
}
