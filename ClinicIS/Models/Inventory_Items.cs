using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicIS.Models
{
    public class Inventory_Items
    {
        [Key]
        public int item_id { get; set; }

        [Required]
        [StringLength(100)]
        public string item_name { get; set; }

        public string item_description { get; set; }

        [Required]
        public int quantity { get; set; }

     

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? expiration_date { get; set; }
    }
}
