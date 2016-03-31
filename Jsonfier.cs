using System;
using System.Collections;
using Jsonzai.Reflect;
using System.Reflection;
using System.Text;

namespace Serie1
{
    class Program
    {
        static void Main(string[] args)
        {

            Student s1 = new Student("gustavo", 40622);

            Console.WriteLine(Teste.ToJson(s1));
        }
    }

    class Student
    {

        public int student_number;
        public string name;
        public int[] array = { 1, 2, 3 };
        public object[] ar = { "hugo", new int[] { 4, 5, 6 }, "morticia" };

        public Student(string n, int x)
        {
            name = n;
            student_number = x;
        }
    }

    class Teste
    {


        private static BindingFlags fieldsFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static StringBuilder finalJsonObject = new StringBuilder();
        

        public static string ToJson(object src,)
        {

            Type t = src.GetType();

            if( t.IsPrimitive || t == typeof(string) )
            {
                finalJsonObject.Append(src.ToString());
            }
            else if ( t.IsArray )
            {
                IEnumerable sequence = (IEnumerable)src;
                IEnumerator it = sequence.GetEnumerator();
                finalJsonObject.Append('[');
                while ( it.MoveNext() )
                {
                    ToJson(it.Current);
                    finalJsonObject.Append(',');
                }
                finalJsonObject.Remove(finalJsonObject.Length - 1, 1);
                finalJsonObject.Append(']');

            }
            else // Is Object
            {
                FieldInfo [] fields_ = t.GetFields(fieldsFlags);
                finalJsonObject.Append('{');
                foreach ( FieldInfo f in fields_ )
                {
                    ToJson(f.GetValue(src));
                    finalJsonObject.Append(',');
                }
                finalJsonObject.Remove(finalJsonObject.Length - 1, 1);
                finalJsonObject.Append('}');
            }

            return finalJsonObject.ToString();   
        }
            
    }

}