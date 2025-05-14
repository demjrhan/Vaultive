namespace Project.DTOs;

public class WatchHistoryResponseDTO
{

    public DateTime WatchDate { get; set; }
    public int TimeLeftOf { get; set; }
    public string MediaTitle { get; set; } = null!;
}