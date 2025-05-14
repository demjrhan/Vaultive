namespace Project.Models;

public class MediaContentStreamingService
{
    public string MediaTitle { get; set; } = null!;
    public int StreamingServiceId { get; set; }

    public MediaContent MediaContent { get; set; } = null!;
    public StreamingService StreamingService { get; set; } = null!;
}