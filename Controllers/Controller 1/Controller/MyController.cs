using Microsoft.AspNetCore.Mvc;

namespace Controller_1.Controller
{
    public class MyController
    {
        //route : /mymessage/
        [Route("/mymessage/")]
        public string MyMessage()
        {
            return "Hello from MyController in Controller 1!";

        }

        //route: /wish/
        [Route("/wish/")]
        public string Wish()
        {
           return "Good! Morning...";
        }

        //route: /: /home/
        [Route("/")]
        [Route("/home/")]
        public string HomeMessage()
        {
            return "We are at home";
        }

        [Route("/contact us/{phone:long:length(10,10)}")]
        public string Contact()
        {
            return $"Contacting";
        }
    }
}
