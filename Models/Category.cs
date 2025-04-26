using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookManagementPrj.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [JsonIgnore]
        public List<Book>? Books { get; set; }
    }

}
