using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Entities
{
    [Table("Article")]
    public class Article
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contect { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateChanged { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
