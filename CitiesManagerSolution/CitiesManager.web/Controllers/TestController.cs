using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TestController : CustomControllerBase
    {
        /// <summary>
        /// Handles HTTP GET requests and returns a simple greeting message.
        /// </summary>
        /// <returns>A string containing the greeting message "Hello World!".</returns>
        [HttpGet]
        [Route("[action]")]
        public string Index()
        {
            return "Hello World!";
        }
    }
}
