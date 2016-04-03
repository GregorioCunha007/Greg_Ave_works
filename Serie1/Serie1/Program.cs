using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serie1
{

    class MainProgram
    {
        public static void Main(String [] args)
        {
            JsonfierTest testing = new JsonfierTest();

           testing.TestJsonfierStudent();
           testing.TestJsonfierTeacher();
           Console.WriteLine("All test running smoothly ");
        }
    }


    [TestClass]
    class JsonfierTest
    {
        
        [TestMethod]
        public void TestJsonfierStudent()
        {
            Student student1 = new Student(40622, "Joao Carlos");

            /* Field test : type of member -> 0*/
            string expected_fields = "{\n\"student_number\":40622,\n\"student_name\":\"Joao Carlos\",\n\"disciplines\":[\"Ave\",\"SO\",\"Mpd\",\"Redes\",\"LS\"]\n}\n";
            string result_1 = Jsonfier.ToJson(student1,0);
            Jsonfier.Reset();
            Assert.AreEqual(expected_fields, result_1);


            /* Method test : type of member -> 3*/
            string expected_methods = "{\n\"ToString\":\"System.String\",\n\"Equals\":\"System.Boolean\",\n\"GetHashCode\":\"System.Int32\",\n\"GetType\":\"System.Type\"\n}\n";
            string result_2 = Jsonfier.ToJson(student1,3);
            Jsonfier.Reset();
            Assert.AreEqual(expected_methods, result_2);
        }

        [TestMethod]
        public void TestJsonfierTeacher()
        {
            Student student1 = new Student(321, "xpto");
            Teacher teacher1 = new Teacher(student1);

            /* Propriety test : type of member -> 1*/
            string expected_prop = "{\n\"talk_to_student\":\"Student\"\n}\n";
            string result_1 = Jsonfier.ToJson(teacher1,1);
            Jsonfier.Reset();
            Assert.AreEqual(expected_prop, result_1);


            /* Event test : type of member -> 2*/
            string expected_event = "{\n\"expell_student\":\"System.Action\"\n}\n";
            string result_2 = Jsonfier.ToJson(teacher1,2);
            Jsonfier.Reset();
            Assert.AreEqual(expected_event, result_2);

        }

    }  
}
