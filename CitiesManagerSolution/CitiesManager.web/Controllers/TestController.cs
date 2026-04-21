using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.web.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TestController : CustomControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public string Index()
        {
            return "Hello World!";
        }
    }
}
