using System.ComponentModel.DataAnnotations;

namespace ECommerce_App.Models
{
    public class Product
    {
        [Required(ErrorMessage ="Product Code can't be blank")]
        public int? ProductCode { get; set; }

        [Required(ErrorMessage ="Product Price can't be blank")]
        public double? Price { get; set; }

        [Required(ErrorMessage ="Product Quantity can't be blank")]
        public int? Quantity { get; set; }
    }
}
