using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class City
    {
        [Key] //Defines primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //A new key will be generated when a city is created
        //How something is generated sometimes needs to be defined but sql lite does it for us
        public int Id { get; set; }
        [Required(ErrorMessage = "Damn boy, can you at LEAST give me a name?")]
        [MaxLength(55, ErrorMessage = "This duud thinks he can get away with writing some buuullshit in the name, huh?")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Glad you had fun but you dont need to tell me your WHOLE life story, ya know?")]

        public string? Description { get; set; }
        public ICollection<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>();

        public City(string name) //Convey purpose, you always want a city to have a name
        {
            Name = name;
        }
    }
}
