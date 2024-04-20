using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Viribus_Unitis.Models
{
    public class Description
    {
        [Key] public int Id { get; set; }
        public string Text { get; set; }
        public int RequestId { get; set; }
        [JsonIgnore]
        public Request Request { get; set; }
    }
}
