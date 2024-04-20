using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Viribus_Unitis.Models
{
    public class Tag
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Request> Requests { get; set; } = new List<Request>();
    }
}
