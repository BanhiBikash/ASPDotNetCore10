using Microsoft.AspNetCore.Mvc;
using BankApplication.Models;

namespace BankApplication.Controllers
{
    public class BankController : Controller
    {
        //this is welcome page
        [Route("/")]
        public IActionResult Index()
        {
            return Content("\nWelcome to the Best Bank.","text/html");
        }

        //this is account details
        [Route("/account-details")]
        public IActionResult AccountDetails()
        {
            Account acc = new Account() {
            AccountNumber=1001,
            AccountHolderName = "ExampleName", 
            Balance = 5000M
            };

            return Json(acc);
        }

        //this is account statement
        [Route("/account-statement")]
        public IActionResult AccountStatement()
        {
            return File("/bank-statement-1234.pdf","application/pdf");
        }

        //account balance
        [Route("/get-current-balance/{accountNumber:int?}")]
        public IActionResult Balance()
        {
            if (Request.RouteValues.ContainsKey("accountNumber"))
            {
                int AccNum=Convert.ToInt32(Request.RouteValues["accountNumber"]);

                if (AccNum==1001)
                {
                    return Content("5000", "text/html");
                }
                else
                {
                    return BadRequest("\nAccount Number should be 1001.");
                }
            }
            else
            {
                return NotFound("\nAccount Number should be supplied.");
            }
        }
    }
}
