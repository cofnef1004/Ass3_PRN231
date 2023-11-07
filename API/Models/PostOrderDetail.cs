using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.Models
{
    public class PostOrderDetail
    {
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
