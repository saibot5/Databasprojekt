using Databasprojekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databasprojekt
{
    static internal class Menu
    {

        public static void MainMenu()
        {

            while (true)
            {


                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1.Get Employees");
                Console.WriteLine("2.Get Students");
                Console.WriteLine("3.Get grades");
                Console.WriteLine("4.Add Student");
                Console.WriteLine("5.Add Employee");


                int input;

                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Enter only digits");
                }


                switch (input)
                {
                    case 1:
                        GetEmployees();
                        break;
                    case 2:
                        GetStudents();
                        break;
                    case 3:
                        GetGrades();
                        break;
                    case 4:
                        AddStudent();
                        break;
                    case 5:
                        AddEmployee();
                        break;
                    case 6:
                        GetActiveCourses();
                        break;
                    default:
                        Console.WriteLine("Not a valid Input");
                        break;
                }
            }
        }

        private static void GetActiveCourses()
        {
            List<Course> courses = new List<Course>();
            using SchoolContext context = new SchoolContext();

            courses = context.Courses.Where(c => c.EndDate > DateOnly.FromDateTime(DateTime.Now)).Include(e => e.Fkemployee).ToList();
            Console.Clear();
            foreach (var course in courses)
            {
                Console.WriteLine($"Course name:{course.CourseName}\nStart Date:{course.StartDate} End Date:{course.EndDate}\nTeacher:{course.Fkemployee.FirstName} {course.Fkemployee.LastName}");
                Console.WriteLine("----------------------------------------------------------------------------");
            }

        }

        private static void AddEmployee()
        {
            Console.WriteLine($"Enter First name:");
            string firstName = Console.ReadLine();

            Console.WriteLine($"Enter Last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter person number. 12 numbers yyyymmddxxxx");
            string pNumber = Console.ReadLine();

            Console.WriteLine("Enter salary. numbers only");
            int pay;
            while (!int.TryParse(Console.ReadLine(), out pay))
            {
                Console.WriteLine("Enter only numbers");
            }

            Console.WriteLine("Enter start date. YYYY-MM-DD");
            DateOnly startDate;
            while (!DateOnly.TryParse(Console.ReadLine(), out startDate))
            {
                Console.WriteLine("Not a valid date");
            }


            int positionId = 0;
            int input;
            while (true)
            {

                Console.WriteLine("Select Position:");
                Console.WriteLine("1.Teacher");
                Console.WriteLine("2.Admin");
                Console.WriteLine("3.Principal");
                Console.WriteLine("4.Janitor");

                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Enter only numbers");
                }
                switch (input)
                {
                    case 1:
                        positionId = 1;
                        break;
                    case 2:
                        positionId = 2;
                        break;
                    case 3:
                        positionId = 3;
                        break;
                    case 4:
                        positionId = 4;
                        break;
                    default:
                        Console.WriteLine("not a valid input");
                        continue;
                }
                break;
            }

            using SchoolContext context = new SchoolContext();

            Employee employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Pnumber = pNumber,
                FkpositionId = positionId,
                StartDate = startDate,
                Pay = pay,
            };

            context.Employees.Add(employee);
            context.SaveChanges();
        }

        private static void AddStudent()
        {
            Console.WriteLine($"Enter First name:");
            string firstName = Console.ReadLine();

            Console.WriteLine($"Enter Last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter person number. 12 numbers yyyymmddxxxx");
            string pNumber = Console.ReadLine();
            int classId = 0;
            int input;

            while (true)
            {
                Console.WriteLine("Select Class:");
                Console.WriteLine("1.1A");
                Console.WriteLine("2.1B");
                Console.WriteLine("3.2A");
                Console.WriteLine("4.2B");


                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Use only digits");
                }
                switch (input)
                {
                    case 1:
                        classId = 1;
                        break;
                    case 2:
                        classId = 2;
                        break;
                    case 3:
                        classId = 3;
                        break;
                    case 4:
                        classId = 4;
                        break;
                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
                break;
            }
            using SchoolContext context = new SchoolContext();

            Student student = new Student()
            {
                FirstName = firstName,
                LastName = lastName,
                Pnumber = pNumber,
                FkclassId = classId,
            };

            context.Students.Add(student);
            context.SaveChanges();

        }

        private static void GetGrades()
        {


            using SchoolContext context = new SchoolContext();
            List<StudentCourseConnection> grades = new List<StudentCourseConnection>();

            while (true)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1.Get grades set last month");
                Console.WriteLine("2.Get average grades");
                int input;

                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Use only digits");
                }

                switch (input)
                {
                    case 1:
                        grades = GetGradesMonth();
                        foreach (StudentCourseConnection g in grades)
                        {
                            Console.WriteLine($"Name:{g.Fkstudent.FirstName} {g.Fkstudent.LastName} \nCourse:{g.Fkcource.CourseName} \nGrade:{g.Fkgrade.GradeName} \nGrade Date:{g.GradeDate}\n");
                            Console.WriteLine("---------------------------------------------");
                        }
                        break;

                    case 2:
                        grades = GetAllSetGrades();
                        decimal avarageGrade = 0;
                        foreach (StudentCourseConnection g in grades)
                        {

                            avarageGrade += Convert.ToInt32(g.FkgradeId);

                        }
                        avarageGrade /= grades.Count;
                        avarageGrade = Math.Round(avarageGrade);
                        Grade avgGradeName = context.Grades.Where(g => g.GradeId == avarageGrade).First();
                        Console.WriteLine($"Average grade:{avgGradeName.GradeName}");
                        break;

                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
                break;
            }

        }

        private static List<StudentCourseConnection> GetAllSetGrades()
        {
            using SchoolContext context = new SchoolContext();

            return context.StudentCourseConnections.Where(g => g.FkgradeId != null).ToList();


        }

        private static List<StudentCourseConnection> GetGradesMonth()
        {

            using SchoolContext context = new SchoolContext();
            return context.StudentCourseConnections.Where(g => g.GradeDate.Value.Month == DateOnly.FromDateTime(DateTime.Now).Month - 1).Include(g => g.Fkcource).Include(g => g.Fkstudent).Include(g => g.Fkgrade).ToList();
        }

        private static void GetStudents()
        {
            List<Student> students = new List<Student>();

            while (true)
            {


                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1.Get all students");
                Console.WriteLine("2.Get students from class");
                int input;
                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Use only digits");
                }


                switch (input)
                {
                    case 1:
                        students = GetAllStudents();
                        break;
                    case 2:
                        students = GetAllStudentsFromClass();
                        break;
                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
                break;
            }
            foreach (Student student in students)
            {
                Console.WriteLine($"Name:{student.FirstName} {student.LastName}\n personnumber: {student.Pnumber}\nclass:{student.Fkclass.ClassName}");
                Console.WriteLine("-----------------------------------");
            }

        }

        private static List<Student> GetAllStudentsFromClass()
        {
            using SchoolContext context = new SchoolContext();
            Console.WriteLine("What class? ");
            List<Class> classes = new List<Class>();
            classes = context.Classes.OrderBy(c => c.ClassName).ToList();

            foreach (Class c in classes)
            {
                Console.WriteLine(c.ClassName);
            }

            string classInput = Console.ReadLine();

            return context.Students.Where(s => s.Fkclass.ClassName == classInput).Include(s => s.Fkclass).ToList();
        }

        private static List<Student> GetAllStudents()
        {
            int nameinput;
            string sortby;

            while (true)
            {


                Console.WriteLine("Sort by First or Last name?");
                Console.WriteLine("1.First name");
                Console.WriteLine("2.Last name");

                while (!int.TryParse(Console.ReadLine(), out nameinput))
                {
                    Console.WriteLine("Use only digits");
                }


                switch (nameinput)
                {
                    case 1:
                        sortby = "FirstName";
                        break;
                    case 2:
                        sortby = "LastName";
                        break;
                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
                break;
            }

            int orderinput;
            while (true)
            {
                Console.WriteLine("Ascending or Decending?");
                Console.WriteLine("1. Asc");
                Console.WriteLine("2. Desc");
                while (!int.TryParse(Console.ReadLine(), out orderinput))
                {
                    Console.WriteLine("Use only digits");
                }

                using SchoolContext context = new SchoolContext();


                switch (orderinput)
                {
                    case 1:
                        return context.Students.OrderBy(s => EF.Property<Student>(s, sortby)).Include(s => s.Fkclass).ToList();
                    case 2:
                        return context.Students.OrderByDescending(s => EF.Property<Student>(s, sortby)).Include(s => s.Fkclass).ToList();
                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
            }
        }

        private static void GetEmployees()
        {
            int input;
            List<Employee> employees = new List<Employee>();
            while (true)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1.Get all teachers");
                Console.WriteLine("2.Get all Admins");
                Console.WriteLine("3.Get principal");
                Console.WriteLine("4.Get all Janitors");
                Console.WriteLine("5.Get all employees");
                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Use only digits");
                }
                Console.Clear();
                using SchoolContext context = new SchoolContext();

                switch (input)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        employees = context.Employees.Where(e => e.FkpositionId == input).Include(e => e.Fkposition).ToList();
                        break;
                    case 5:
                        employees = context.Employees.Include(e => e.Fkposition).ToList();
                        break;
                    default:
                        Console.WriteLine("Not a valid input");
                        continue;
                }
                break;
            }


            foreach (Employee employee in employees)
            {


                //get time worked in years
                var startdate = employee.StartDate;

                var yearsEmployed = DateTime.Now.Year - employee.StartDate.Year;

                Console.WriteLine($"Name:{employee.FirstName} {employee.LastName}\n Position:{employee.Fkposition.PositionName}\n Personnumber: {employee.Pnumber}\nPay:{employee.Pay}\nYears employed:{yearsEmployed}");
                Console.WriteLine("---------------------------------------------");
            }

            //get total and avarage salary for position and all employees
            var s = 0;

            foreach (var employee in employees)
            {
                s += employee.Pay;
            }

            
            var avgSalary = s / employees.Count;
            if (input == 5)
            {
                Console.WriteLine($"total employee count:{employees.Count}");
                Console.WriteLine($"Total salary: {s}");
                Console.WriteLine($"Avarage salary for all employees: {avgSalary}");
            }
            else
            {
                //if position specified get position name
                Console.WriteLine($"total {employees[0].Fkposition.PositionName} count:{employees.Count}");
                Console.WriteLine($"Total salary for {employees[0].Fkposition.PositionName}: {s}");
                Console.WriteLine($"Avarage salary for {employees[0].Fkposition.PositionName}: {avgSalary}");
            }
            Console.WriteLine("\npress any key to continue");
            Console.ReadKey();
        }
    }
}
