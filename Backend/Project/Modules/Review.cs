namespace Project.Modules;

public class Review
{
    public int Id { get; set; }

    public double Rating { get; set; }
    public string Comment { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public MediaContent MediaContent { get; set; } = null!;
    
    public string MediaTitle { get; set; } = null!;
    
    public WatchHistory WatchHistory { get; set; } = null!;

}
