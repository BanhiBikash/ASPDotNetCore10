using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Gender
    {
        public string? GenderName { get; set; }
        public int? GenderKey { get; set; }
        public bool? Benefits { get; set; }

        public List<Person> Persons {get; set; }
    }
}
