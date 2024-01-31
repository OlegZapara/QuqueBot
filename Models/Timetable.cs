using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuqueBot.Models;

public class Timetable
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string WeekType { get; set; }

    public List<string> Days { get; set; }
}

public class TimetableDay
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public List<TimetableEntry> Entries { get; set; }
}

public class TimetableEntry
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime End { get; set; }
    
    [Required]
    public ClassType Type { get; set; }

    [Required]
    public string Url { get; set; }
}

public enum ClassType
{
    Lecture,
    Practice,
    Lab,
    Other
}