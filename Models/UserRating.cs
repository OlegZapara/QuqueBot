using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuqueBot.Models;

public class UserRating
{
    [Required]
    [Key]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public TelegramUser User { get; set; }

    [Required] 
    public int Rating { get; set; } = 0;

    [Required] 
    public int DailyIncrease { get; set; } = 100;

    [Required] 
    public int Credits { get; set; } = 100;

    [Required] 
    public int MaxCredits { get; set; } = 100;
    
    public UserRating(TelegramUser user)
    {
        UserId = user.Id;
        User = user;
    }

    public UserRating(long userId)
    {
        UserId = userId;
    }
}