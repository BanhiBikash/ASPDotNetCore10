using Microsoft.AspNetCore.Mvc;


namespace Form_URLEncoded_and_Form_Data.Models
{
    public class Students
    {
        //this takes value in accordnace to precedence
        public string? Name { get; set; }

        //this takes value in accordnace to precedence
        public int? Id { get; set; }

        //this takes value in accordnace to precedence
        public string? City { get; set; }
    }
}
