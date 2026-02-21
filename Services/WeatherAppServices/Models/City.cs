namespace WeatherAppServices.Models
{
    public class City
    {
        public string CityName { get; set; }
        public string CityUniqueCode { get; set; }
        public DateTime DateAndTime { get; set; }
        public int TemperatureFahrenheit { get; set; }
    }
}
