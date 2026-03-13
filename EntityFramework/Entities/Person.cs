using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Entities
{
    /// <summary>
    /// This is the person entity class. It represents a person in the system. It can be used to store information about a person, such as their name, age, and address. This class can be extended to include additional properties and methods as needed.
    /// </summary>
    public class Person
    {
        [Key]
        public Guid? PersonID { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(60)]
        public string? Address { get; set; }

        [StringLength(5)]
        public string? CountryID { get; set; }

        [Range(100000,999999)]
        public int? Pin { get; set; }
    }
}
