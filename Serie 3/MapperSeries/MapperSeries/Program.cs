using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperSeries
{
    class Program
    {
        static void Main(string[] args)
        {
           
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
        }
    }
}
