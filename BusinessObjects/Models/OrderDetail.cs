using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; } = null!;
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; } = null!;
    }
}
