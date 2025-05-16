using Project.Models.Enumerations;

namespace Project.Models;

public class Documentary : MediaContent
{
    public ICollection<Topic> Topics { get; set; } = new HashSet<Topic>();
}
