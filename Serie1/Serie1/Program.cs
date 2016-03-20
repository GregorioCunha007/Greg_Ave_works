using System;
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
        public object[] ar = { "hugo", new int[] { 4,5,6 }, "morticia" };

        public Student(string n, int x)
        {
            name = n;
            student_number = x;
        }
    }

    class Teste
    {
        
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

            foreach (FieldInfo f in fields)
            {
               
                if ( f.FieldType.IsPrimitive || f.FieldType == typeof(string))
                {
                    allFields.Append(f.Name + ":" + f.GetValue(instance) + ", ");  
                }
                else if (f.FieldType.IsArray)
                {
                    allFields.Append(DealWithArray(f.GetValue(instance)));
                }
                else
                {
                    allFields.Append(DealWithObject(f.GetValue(instance)));
                }
                allFields.Append("\n");
            }

            String ret = allFields.ToString();
            return ret;
        }

        private static string DealWithObject(object new_src)
        {
            ObjectUnit ret = new ObjectUnit(new_src);
            return ret.ToString();
         //   ToJson(new_src);
        }

        private static string DealWithArray(object new_src)
        {
            ArrayUnit ret =  new ArrayUnit(new_src);
            return ret.ToString();
          //  ToJson(new_src); 
        }
    }
}
