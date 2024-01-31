using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuqueBot.Models;

public class Queue
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [MaxLength(50)]
    public string? Name { get; set; }
    
    public List<TelegramUser> Users { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public bool IsMixed { get; set; } = false;
    
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    public long MessageId { get; set; }
}