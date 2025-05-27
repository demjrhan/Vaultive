using Project.DTOs.MediaContentDTOs;

namespace Project.DTOs.WatchHistoryDTOs;

public class WatchHistoryDetailedDTO
{
    public int Id { get; set; }
    
    public DateOnly WatchDate { get; set; }
    
    public int TimeLeftOf { get; set; }
    
    public MediaContentDTO MediaContent { get; set; }
}