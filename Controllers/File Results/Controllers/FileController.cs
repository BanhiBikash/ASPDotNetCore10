using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace File_Results.Controllers
{
    public class FileController : Controller
    {
        //using virtual file result whe the file is in wwwroot folder
        [Route("/")]
        public VirtualFileResult Index()
        {
            return new VirtualFileResult("/doc.pdf","application/pdf");
            //shorthand: File("path","content-type");
        }

        //when the file is not in the www root folder but any folder in the machine we use physical file
        [Route("physical-file")]
        public PhysicalFileResult FileDownload()
        {
            string fileName = "feeReciept.pdf";
            return new PhysicalFileResult(@$"C:\Users\Banhi\Downloads\Govt_certificates\{fileName}", "application/pdf");
            //shorthand: PhysicalFile("path","content-type");
        }

        //this sends files in the byte format
        [Route("byte-file")]
        public FileContentResult ShowFile()
        {
            String path = "C:\\Users\\Banhi\\Downloads\\Govt_certificates";
            string name = "feeReciept.pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(@$"{path}/{name}");

            return new FileContentResult(bytes, "application/pdf");
            //shorthand: File(byte array,"content-type");
        }
    }
}
