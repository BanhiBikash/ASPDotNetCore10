using Microsoft.AspNetCore.Mvc;

namespace Model_Class.Models
{
    public class Students
    {
        //this changes default value to take from Query instaed of route
        [FromQuery]
        public string? Name { get; set; }

        //No changes since Route is anyway preferred
        [FromRoute]
        public int? Id { get; set; }

        //No changes since Route is anyway preferred
        [FromRoute]
        public string? City { get; set; }
    }
}
