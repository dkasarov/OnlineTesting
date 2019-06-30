using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }

        public string NetworkIP { get; set; }

        public string LocalIP { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Hostname { get; set; }

        public string PostalCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<Exam> Exams { get; set; }

        public string StudentToTestId { get; set; }

        public StudentToTest StudentToTest { get; set; }

        public Student()
        {
            DateCreated = DateTime.Now;
        }
    }
}
