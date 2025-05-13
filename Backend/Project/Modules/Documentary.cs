using Project.Modules.Enumerations;

namespace Project.Modules;

public class Documentary : MediaContent
{
    public int Length { get; set; }
    public ICollection<Topic> Topics { get; set; } = new HashSet<Topic>();
}
