
using CollageSystem.Core.Models;
using CollageSystem.Data.Contexts; // Adjust this namespace to where your models are defined

namespace CollageSystem.Data
{
    public static class SeedData
    {
        public static void Seed(ApplicationDbContext context)
        {
         

            if (!context.Departments.Any())
            {
                var depts = new List<Department>()
                {
                    new Department()
                    {
                        Name = "CS",
                    },
                    new Department()
                    {
                        Name = "IS",
                    },
                    new Department()
                    {
                        Name = "IT",
                    },
                    new Department()
                    {
                        Name = "Bio",
                    }
                };
                context.AddRange(depts);
                context.SaveChanges();
            }

            if (!context.Students.Any())
            {
                var students = new List<Student>()
                {
                    new Student()
                    {
                        Name = "Abdullah Khalid",
                        Age = 22,

                        TotalCreditHours = 72,
                        TotalQualityPoints = 3.2,

                        DepartmentId = 2,
                    },
                    new Student()
                    {
                        Name = "Mustafa Muhammed",
                        Age = 21,

                        TotalCreditHours = 72,
                        TotalQualityPoints = 3.0,
                        DepartmentId = 2
                    },
                    new Student()
                    {
                        Name = "Ali Sayed",
                        Age = 22,

                        TotalCreditHours = 72,
                        TotalQualityPoints = 3.5,
                        DepartmentId = 1
                    },
                    new Student()
                    {
                        Name = "Ali Muhammed",
                        Age = 22,

                        TotalCreditHours = 69,
                        TotalQualityPoints = 2.7,
                        DepartmentId = 1
                    },
                    new Student()
                    {
                        Name = "Mahmoud Sobhy",
                        Age = 20,

                        TotalCreditHours = 72,
                        TotalQualityPoints = 2.5,
                        DepartmentId = 3
                    },
                    new Student()
                    {
                        Name = "Omar Muhammed",
                        Age = 18,

                        TotalCreditHours = 16,
                        TotalQualityPoints = 3.7,
                        DepartmentId = 2
                    },
                    new Student()
                    {
                        Name = "Amr Sayed",
                        Age = 21,

                        TotalCreditHours = 72,
                        TotalQualityPoints = 3.1,
                        Department = context.Departments.Where(x => x.Name == "IT").First(),
                        DepartmentId = context.Departments.Where(x => x.Name == "IT").First().Id,
                    },
                    new Student()
                    {
                        Name = "Khalid Ahmed",
                        Age = 20,
                        TotalCreditHours = 72,
                        TotalQualityPoints = 3.2,
                        DepartmentId = 2
                    },
                    new Student()
                    {
                        Name = "Gamal Ahmed",
                        Age = 22,

                        TotalCreditHours = 69,
                        TotalQualityPoints = 1.9,

                        DepartmentId = 4
                    },
                };
                context.AddRange(students);
                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                var courses = new List<Course>()
                {
                    new Course()
                    {
                        Name = "Introduction to CS",
                        DepartmentId = 1,
                        CourseCode = "CS301",
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime(DateTime.Now.AddDays(90)),
                    },
                    new Course()
                    {
                        Name = "Introduction to IT",

                        CourseCode = "IT201",
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime(DateTime.Now.AddDays(90)),
                    },
                    new Course()
                    {
                        Name = "Calculus ",
                        DepartmentId = 2,
                        CourseCode = "MATH 201",
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime(DateTime.Now.AddDays(90)),
                    },
                    new Course()
                    {
                        Name = "Linear Algebra",
                        DepartmentId = 1,
                        CourseCode = "MATH 101",
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime(DateTime.Now.AddDays(90)),
                    },
                    new Course()
                    {
                        Name = "Algorithms Design & Analysis",
                        DepartmentId = 1,
                        CourseCode = "CS305",
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime(DateTime.Now.AddDays(90)),
                    }
                };
                context.AddRange(courses);
                context.SaveChanges();
            }

            if (!context.Professors.Any())
            {
                var profs = new List<Professor>
                {
                    new Professor()
                    {
                        Name = "Ahmed Muhammed",
                        DepartmentId = 1,
                        Salary = 18200.0M,
                        Age = 40,
                        HireDate = DateTime.Now,
                    },
                    new Professor()
                    {
                        Name = "Mustafa Ahmed",
                        DepartmentId = 3,

                        Salary = 20000.0M,
                        Age = 50,
                        HireDate = DateTime.Now.AddYears(-20),
                    },
                    new Professor()
                    {
                        Name = "Hamada Ahmed",
                        DepartmentId = 1,
                        Salary = 15500.0M,
                        Age = 42,
                        HireDate = DateTime.Now.AddYears(-15),
                    },
                    new Professor()
                    {
                        Name = "Gamal Ali",
                        DepartmentId = 2,
                        Salary = 18000.0M,
                        Age = 41,
                        HireDate = DateTime.Now.AddYears(-5),
                    },
                };
                context.AddRange(profs);
                context.SaveChanges();
            }
        }
    }
}