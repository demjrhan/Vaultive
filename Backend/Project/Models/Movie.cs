using Project.Models.Enumerations;

namespace Project.Models;

public class Movie : MediaContent
{
    public int Length { get; set; }

    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}
