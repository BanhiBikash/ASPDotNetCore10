using System.ComponentModel.DataAnnotations;

namespace CitiesManager.web.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; }

        [Required(ErrorMessage ="City Name is required")]
        public string? CityName { get; set; }
    }
}
