namespace Project.Models;

public class WatchHistory
{
    public int Id { get; set; }

    public DateTime WatchDate { get; set; }
    public int TimeLeftOf { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MediaId { get; set; }
    public MediaContent MediaContent { get; set; } = null!;
    
}
