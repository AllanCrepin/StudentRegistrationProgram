using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Drawing;
using System;
using Microsoft.EntityFrameworkCore;

namespace StudentRegistrationProgram
{
    public class StudentService
    {
        private AppDbContext context;

        public StudentService(AppDbContext context)
        {
            this.context = context;
        }

        public int GetNumberOfStudents()
        {
            return context.Students.Count();
        }

        public void RegisterStudent(string firstName, string lastName, string city)
        {
            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                City = city
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
        }

        public Student GetStudentFromId(int id)
        {
            return context.Students.Where(s => s.StudentId == id).FirstOrDefault();
        }

        public Student GetClosestStudent(string name)
        {

            var items = context.Students.ToList();

            var closestMatch = items
                .OrderByDescending(student => StringMetrics.JaroWinklerSimilarity(name, student.FirstName + " " + student.LastName))
                .FirstOrDefault();

            return closestMatch;

        }

        public List<Student> GetClosestStudents(string name)
        {
            var items = context.Students.ToList();

            var exactMatches = items
                .Where(student => (student.FirstName + " " + student.LastName).Equals(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (exactMatches.Count > 0)
                return exactMatches;

            var closestMatch = items
                .OrderByDescending(student => StringMetrics.JaroWinklerSimilarity(name, student.FirstName + " " + student.LastName))
                .FirstOrDefault();

            return closestMatch != null ? new List<Student> { closestMatch } : new List<Student>();
        }

        public List<Student> GetClosestStudents2(string name)
        {
            var items = context.Students.ToList();

            List<Student> closestMatch = items.Where(student => StringMetrics.JaroWinklerSimilarity(name, student.FirstName + " " + student.LastName) > 0.82).ToList();

            return closestMatch != null ? closestMatch : new List<Student>();
        }

        public bool StudentExists(int id)
        {
            return context.Students.Any(student => student.StudentId == id);
        }

        public bool UpdateStudent(int studentId, string newFirstName, string newLastName, string newCity)
        {
            var student = context.Students.Find(studentId);

            if (student == null)
                return false;

            if (!string.IsNullOrEmpty(newFirstName)) student.FirstName = newFirstName;
            if (!string.IsNullOrEmpty(newLastName)) student.LastName = newLastName;
            if (!string.IsNullOrEmpty(newCity)) student.City = newCity;

            context.SaveChanges();
            return true;
        }

        public List<Student> ListStudents()
        {
            return context.Students.ToList();
        }
    }
}
