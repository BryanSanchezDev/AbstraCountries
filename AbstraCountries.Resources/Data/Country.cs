namespace AbstraCountries.Resources.Data
{
    public class Country
    {
        public string Id { get; set; } = string.Empty;
        public string CountryCode => Id;
        public string Name { get; set; } = string.Empty;
    }
}
