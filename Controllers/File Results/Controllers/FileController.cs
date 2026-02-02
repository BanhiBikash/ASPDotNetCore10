using Microsoft.AspNetCore.Mvc;

namespace File_Results.Controllers
{
    public class FileController : Controller
    {
        //using virtual file result whe the file is in wwwroot folder
        [Route("/")]
        public VirtualFileResult Index()
        {
            return new VirtualFileResult("/doc.pdf","application/pdf");
        }
    }
}
