using Project.Models.Enumerations;

namespace Project.Models;

public class Documentary : MediaContent
{
    public int Length { get; set; }
    public ICollection<Topic> Topics { get; set; } = new HashSet<Topic>();
}
