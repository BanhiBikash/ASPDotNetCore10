using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    /// <summary>
    /// This is the person entity class. It represents a person in the system. It can be used to store information about a person, such as their name, age, and address. This class can be extended to include additional properties and methods as needed.
    /// </summary>
    public class Person
    {
        public Guid? PersonID { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }  

        public string? Gender { get; set; }

        public string? Address { get; set; }    

        public Guid? CountryID { get; set; }
    }
}
