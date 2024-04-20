using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Viribus_Unitis.Models
{
    public class Response
    {
        [Key] public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public string Phone { get; set; }
        public string? Comment { get; set; }
    }
}
