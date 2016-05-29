using System;
using System.Collections.Generic;
<<<<<<< HEAD
using Microsoft.VisualStudio.TestTools.UnitTesting;

=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> 46ca7f29fa30d89964ae900f26ff0314af3408f0

namespace MapperSeries
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD

            MapperTests test = new MapperTests();
            test.Test_Types_With_Default_Ctor();
            test.Test_With_Value_Types();
            test.Test_Types_With_Parameterized_Ctor();
            test.Test_With_Collections();
            test.Test_With_Avoidable_Custom_Attributte();
            test.Test_With_Avoidable_String_Name();
            test.Test_With_ForMember_Active();

        }
    }

    
    [TestClass]
    class MapperTests
    {
        [TestMethod]
        public void Test_Types_With_Default_Ctor()
        {
            Mapper<Student, Person> m = AutoMapper
                                                 .Build<Student, Person>()
                                                 .CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
            Assert.AreEqual(s.Nr, p.Nr);
        }

        [TestMethod]
        public void Test_Types_With_Parameterized_Ctor()
        {
            Mapper<Student, Worker> m = AutoMapper
                                                 .Build<Student, Worker>()
                                                 .IgnoreMember("Id")
                                                 .CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Worker w = m.Map(s);
            Assert.AreEqual(s.Name, w.Name);
            Assert.IsNull(w.Id);
        }

        [TestMethod]
        public void Test_With_Value_Types()
        {
            Mapper<Student, Teacher> m = AutoMapper
                                                 .Build<Student, Teacher>()
                                                 .CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Teacher t = m.Map(s);
            Assert.AreEqual(s.Name, t.Name);
            Assert.AreEqual(s.Nr, t.Nr);
        }

        [TestMethod]
        public void Test_With_Collections()
        {
            Student[] stds = {
                              new Student{ Nr = 27721, Name = "Ze Manel"},
                              new Student{ Nr = 15642, Name = "Maria Papoila"}};
            Person[] expected = {
                                 new Person{ Nr = 27721, Name = "Ze Manel"},
                                 new Person{ Nr = 15642, Name = "Maria Papoila"}};
            Mapper<Student, Person> m = AutoMapper
             .Build<Student, Person>()
             .CreateMapper();
            List<Person> ps = m.Map<List<Person>>(stds);
            CollectionAssert.AreEqual(expected, ps.ToArray());
        }

        [TestMethod]
        public void Test_With_Avoidable_Custom_Attributte()
        {
            Mapper<Student, Person> m = AutoMapper
                                                 .Build<Student, Person>()
                                                 .IgnoreMember<AvoidMapping>()
                                                 .CreateMapper();            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreEqual(s.Name, p.Name);
        }

        [TestMethod]
        public void Test_With_Avoidable_String_Name()
        {
            Mapper<Student, Person> m = AutoMapper
                                                 .Build<Student, Person>()
                                                 .IgnoreMember("Name")
                                                 .CreateMapper();            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Person p = m.Map(s);
            Assert.AreEqual(s.Nr, p.Nr);
            Assert.IsNull(p.Name);
        }

        [TestMethod]
        public void Test_With_ForMember_Active()
        {
            Mapper<Student, Worker> m2 = AutoMapper
                                                 .Build<Student, Worker>()
                                                 .ForMember("Id", src => src.Nr.ToString())
                                                 .CreateMapper();
            Student s = new Student { Nr = 27721, Name = "Ze Manel" };
            Worker w = m2.Map(s);
            Assert.AreEqual(s.Name, w.Name);
            Assert.AreEqual(s.Nr.ToString(), w.Id);
=======
           
            Mapper<Student, Person> map = AutoMapper.Build<Student, Person>().CreateMapper();
            Student s = new Student { Nr = 40622, Name = "Pedro Greg" };
            Student s1 = new Student { Nr = 40637, Name = "Chico" };
            Student[] stds = { s, s1 };

            List<Person> persons = map.Map<List<Person>>(stds);
            
        }
    }

    class Student
    {
        public string Name { get; set; }
        public int Nr { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public int Nr { get; set; }
    }

    struct Teacher
    {
        public string Name { get; set; }
        public int Nr { get; set; }

        public override string ToString()
        {
            return Name + " -> " + Nr;
>>>>>>> 46ca7f29fa30d89964ae900f26ff0314af3408f0
        }
    }
}
