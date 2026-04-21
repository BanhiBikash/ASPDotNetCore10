using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Core.Domain.Entity
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemID { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "OrderID is required.")]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage = "ProductName is required.")]
        [MaxLength(50, ErrorMessage = "ProductName cannot exceed 50 characters.")]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be a positive number.")]
        public decimal? UnitPrice { get; set; }

        // This property is not set directly; it’s computed from Quantity * UnitPrice
        [NotMapped] // prevents EF from trying to persist it directly
        public decimal? TotalPrice => Quantity * UnitPrice;
    }
}
