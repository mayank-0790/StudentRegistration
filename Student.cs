
using System;

namespace StudentRegistrationSystem
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}