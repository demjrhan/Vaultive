using System.Text.Json.Serialization;

namespace Project.DTOs;

public class WatchHistoryResponseDTO
{

    [JsonIgnore]
    public DateTime WatchDate { get; set; }

    public string WatchDateFormatted => WatchDate.ToString("dd-MM-yyyy");
    public int TimeLeftOf { get; set; }
    public string MediaTitle { get; set; } = null!;
}