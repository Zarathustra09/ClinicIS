using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicIS.Models
{
    public class Order
    {
        [Key]
        public int order_id { get; set; }

        public int? user_id { get; set; }

        [Display(Name = "Order Date")]
        public DateTime order_date { get; set; }

        public int? item_id { get; set; }

        [Required]
        public int quantity { get; set; }


    }
}
