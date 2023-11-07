using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.Models
{
    public class PostProduct
    {
        [Required, StringLength(40)]
        public string ProductName { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UnitPrice { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UnitInStock { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public int CategoryID { get; set; }

    }
}
