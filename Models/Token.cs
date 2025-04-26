using System.ComponentModel.DataAnnotations;

namespace BookManagementPrj.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string TokenValue { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}