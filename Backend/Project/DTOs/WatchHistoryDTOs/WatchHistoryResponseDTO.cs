using System.Text.Json.Serialization;

namespace Project.DTOs.WatchHistoryDTOs;

public class WatchHistoryResponseDTO
{

    public DateOnly WatchDate { get; set; }

    public int TimeLeftOf { get; set; }
    public string MediaTitle { get; set; } = null!;
    public int MediaId { get; set; } 

}