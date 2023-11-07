using System.ComponentModel.DataAnnotations;

namespace ProductWebAPI.Models
{
    public class PostOrder
    {
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int Total { get; set; }
        [Required]
        public decimal Freight { get; set; }
        [Required]
        public string CustomerID { get; set; }
        [Required]
        public List<PostOrderDetail> OrderDetails { get; set; }
    }
}
