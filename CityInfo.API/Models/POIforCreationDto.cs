namespace CityInfo.API.Models
{
    public class POIforCreationDto //Seperate dto for creating, updating and returnig resources
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
