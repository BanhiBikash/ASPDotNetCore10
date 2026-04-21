using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Core.Domain.Entity
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage = "OrderNumber is required.")]
        [RegularExpression(@"^Order_\d{4}_\d+$",ErrorMessage = "OrderNumber must be in the format Order_YYYY_N (e.g., Order_2024_1).")]
        public string? OrderNo { get; set; }

        [Required(ErrorMessage = "CustomerName is required.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        public decimal TotalAmount { get; set; }
    }
}
