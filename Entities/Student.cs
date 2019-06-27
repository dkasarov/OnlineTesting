﻿using System;
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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IP { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Hostname { get; set; }

        public string PostalCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public ICollection<Exam> Exams { get; set; }
    }
}
