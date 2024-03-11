using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicIS.Models
{
    public class User
    {
        [Key] // Explicitly mark user_id as the primary key
        public int user_id { get; set; } // Change from UserId to user_id to match the column name in the database

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string full_name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public int role { get; set; }

        [Display(Name = "Created At")]
        public DateTime created_at { get; set; } = DateTime.Now;
    }

  
}
