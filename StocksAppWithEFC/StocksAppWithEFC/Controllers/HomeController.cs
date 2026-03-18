using Microsoft.AspNetCore.Mvc;
using StocksAppWithEFC.Models;

namespace StocksAppWithEFC.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            Message? msg = null;
            return View(msg);
        }

        [Route("Order")]
        public async Task<IActionResult> Order(string? orderAction)
        {
            if (orderAction == null) 
            {
                return View("Orders");
            }else if(orderAction == "sell")
            {
                return View("Index");
            }else if(orderAction == "buy")
            {
                return View();
            }

            return View("Orders");
        }
    }
}
