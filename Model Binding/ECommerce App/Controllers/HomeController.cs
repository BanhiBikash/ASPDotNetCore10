using Microsoft.AspNetCore.Mvc;
using ECommerce_App.Models;

namespace ECommerce_App.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("\nWelcome to E Commerce Home","text/html");
        }

        [Route("/order")]
        public IActionResult Order(Order order)
        {
            string? errorMessage=null;

            if (!ModelState.IsValid)
            {
                errorMessage = String.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage));
                return BadRequest(errorMessage);
            }
            else
            {
                double? TotalPrice = 0D;

                //Findng the total price
                foreach (var Product in order.Products)
                {
                    TotalPrice += Product.Price*Product.Quantity;
                }

                if (TotalPrice != order.InvoicePrice)
                {
                    errorMessage = String.Join("\n","\nInvoicePrice doesn't match with the total cost of the specified products in the order.",errorMessage);
                }

                if (errorMessage == null)
                {
                    Random random = new Random();
                    order.OrderNo = random.Next(111111,999999);
                    return Json(new { order.OrderNo });
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
        }
    }
}
