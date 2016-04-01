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

            /* Propriety test */



        }

    }
    
}
