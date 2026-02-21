namespace Services
{
    public class City
    {
        public string CityName { get; set; }
        public string CityUniqueCode { get; set; }
        public DateTime DateAndTime { get; set; }

        public int TemperatureFahrenheit { get; set; }
    }
    public class CitiesService
    {
        public List<string> Cities; 

        public CitiesService()
        {
            Cities = new List<string>()
            {
                        //new City() { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = DateTime.Now, TemperatureFahrenheit = 75 },
                        //new City() { CityUniqueCode = "LA", CityName = "Los Angeles", DateAndTime = DateTime.Now, TemperatureFahrenheit = 85 },
                        //new City() { CityUniqueCode = "CHI", CityName = "Chicago", DateAndTime = DateTime.Now, TemperatureFahrenheit = 70 },
                        //new City() { CityUniqueCode = "HOU", CityName = "Houston", DateAndTime = DateTime.Now, TemperatureFahrenheit = 90 },
                        //new City() { CityUniqueCode = "PHX", CityName = "Phoenix", DateAndTime = DateTime.Now, TemperatureFahrenheit = 100 }
                        "Kolkata",
                        "London",
                        "Paris",
                        "Tokyo"
            };
        }

        public List<string> GetCities()
        {
            return Cities;
        }
    }
}
