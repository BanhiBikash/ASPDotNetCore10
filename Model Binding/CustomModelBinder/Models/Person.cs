using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CustomModelBinder.Models
{
    public class Person
    {
        //    public string? FirstName { get; set; }
        //    public string? LastName { get; set; }

        //    [Required(ErrorMessage = "Address is required.")]
        //    public string? Address { get; set; }

        //    [Required(ErrorMessage = "City is required.")]
        //    public string? City { get; set; }

        //    [Range(100000, 999999, ErrorMessage = "Pin must be a 6-digit number.")]
        //    public int? Pin { get; set; }

        [Required(ErrorMessage = "Person Name is required.")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Full Address is required.")]
        public string? FullAddress { get; set; }    
    }
}
