using System.ComponentModel.DataAnnotations;

namespace CitiesManager.web.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; }

        public string? CityName { get; set; }
    }
}
