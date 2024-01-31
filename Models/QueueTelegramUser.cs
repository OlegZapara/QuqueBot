using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuqueBot.Models;

[PrimaryKey(nameof(UsersId), nameof(QueuesId))]
public class QueueTelegramUser
{
    [Required]
    [Key, Column(Order = 1)]
    [ForeignKey(nameof(Queue))]
    public long QueuesId { get; set; }
    
    public Queue Queue { get; set; }

    [Required]
    [Key, Column(Order = 2)]
    [ForeignKey(nameof(User))]
    public long UsersId { get; set; }
    
    public TelegramUser User { get; set; }

    [Required]
    public int Position { get; set; }
}