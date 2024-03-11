using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicIS.Models
{
    public class Complaint
    {
        [Key]
        public int complaint_id { get; set; }

        public int? user_id { get; set; }

        [Required]
        public string complaint_text { get; set; }

        [Display(Name = "Complaint Date")]
        public DateTime complaint_date { get; set; } = DateTime.Now;

     
    }
}
