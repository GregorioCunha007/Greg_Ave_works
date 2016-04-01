using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Serie1
{

    class MainProgram
    {
        public static void Main(String [] args)
        {
            JsonfierTest testing = new JsonfierTest();
            //testing.TestJsonfierStudent();

            Student student1 = new Student(321, "xpto");
            Teacher teacher1 = new Teacher(student1);

            Console.WriteLine(Jsonfier.ToJson(teacher1, 1));

        }
    }


    [TestClass]
    class JsonfierTest
    {
        
        [TestMethod]
        public void TestJsonfierStudent()
        {
            Student student1 = new Student(40622, "Joao Carlos");

            /* Field test */ 
            string expected = "{\n\"student_number\":40622,\n\"student_name\":\"Joao Carlos\",\n\"disciplines\":[\"Ave\",\"SO\",\"Mpd\",\"Redes\",\"LS\"]\n}\n";
            string result = Jsonfier.ToJson(student1, 0); 
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void TestJsonfierTeacher()
        {
            Student student1 = new Student(321, "xpto");
            Teacher teacher1 = new Teacher(student1);

            /* Propriety test */
          //  string expected = "{\n\"talk_to_student\":}"
        }

    }
    
}
