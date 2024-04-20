using System.ComponentModel.DataAnnotations;
using Viribus_Unitis.Models;

namespace Viribus_Unitis.ViewModel
{
    public class RequestViewModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string? Token { get; set; }
        public List<int> Tags { get; set; }
        public string? UserName { get; set; }
    }
}
