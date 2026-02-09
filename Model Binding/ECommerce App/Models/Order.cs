using System.ComponentModel.DataAnnotations;

namespace ECommerce_App.Models
{
    public class Order
    {
        public int? OrderNo { get; set; }

        [Required(ErrorMessage = "Order date can't be blank")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Invoice price can't be blank")]
        public double? InvoicePrice { get; set; }

        [Required(ErrorMessage = "Products list can't be blank")]
        public List<Product>? Products { get; set; }
    }
}

