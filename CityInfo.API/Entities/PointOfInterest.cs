using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
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
        [ForeignKey("CityId")] //Not necessary but provides clarity for other developers
        public City? City { get; set; } //Provide relation between PointOfInterest and City,
        public int CityId { get; set; } // Forein Key

        public PointOfInterest(string name) { 
            Name = name;
        }
    }
}
