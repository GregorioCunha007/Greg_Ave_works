using System;

public class Student
{

    public int student_number;
    public string student_name;
    public string [] disciplines = { "Ave", "SO", "Mpd", "Redes", "LS"};

    public Student(int number, string name)
	{
        this.student_number = number;
        this.student_name = name;
	}
}

public class Course
{

    public int course_id;
    public string course_name;
    public Student [] students;

    public Course(int course_id, string course_name, Student [] students)
    {
        this.course_id = course_id;
        this.course_name = course_name;
        this.students = students;
    }
}

public class Teacher
{

    public Student TalkToStudent
    {
        get;set;
    }

    public event Action ExpellStudent
    {
        add{

        }
        remove{

        }
    }

    public Teacher(Student s1)
    {
        TalkToStudent = s1;
    }
}