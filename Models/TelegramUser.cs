using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuqueBot.Models;

public class TelegramUser
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(50)]
    public string? Username { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
    
    public List<Queue> Queues { get; set; }
    
    [Required]
    public UserRating Rating { get; set; }
}