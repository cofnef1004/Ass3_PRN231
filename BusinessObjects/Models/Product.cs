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
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; }
        public decimal? Weight { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? UnitInStock { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; } = null!;
    }
}
