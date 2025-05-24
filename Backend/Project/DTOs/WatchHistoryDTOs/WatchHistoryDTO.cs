
namespace Project.DTOs.WatchHistoryDTOs;

public class WatchHistoryDTO
{

    public DateOnly WatchDate { get; set; }

    public int TimeLeftOf { get; set; }
    public string MediaTitle { get; set; } = null!;
    public int MediaId { get; set; } 

}