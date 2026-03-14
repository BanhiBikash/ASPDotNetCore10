using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Gender
    {
        public string? GenderName { get; set; }

        public int GenderKey { get; set; }  

        public bool? Benefits { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }
    }
}
