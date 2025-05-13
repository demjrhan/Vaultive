using Project.Modules.Enumerations;

namespace Project.Modules;

public class Movie : MediaContent
{
    public int Length { get; set; }

    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}
