using Project.DTOs.MediaContentDTOs;
using Project.DTOs.SubscriptionDTOs;

namespace Project.DTOs.StreamingServiceDTOs;

public class StreamingServiceDetailedDTO
{
    public StreamingServiceDTO Service { get; set; }
    public ICollection<MediaContentDTO> MediaContents { get; set; }
    public ICollection<SubscriptionDTO> Subscriptions { get; set; }
}