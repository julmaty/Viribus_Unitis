using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Viribus_Unitis.Models
{
    public class Request
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public int UserId  { get; set; }
        public DateTime CreatedDate { get; set; }
        public Description Description { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        [JsonIgnore]
        public List<Response> Responses { get; set; } = new List<Response>();
    }
}
