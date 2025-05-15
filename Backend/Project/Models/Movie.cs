using Project.Models.Enumerations;

namespace Project.Models;

public class Movie : MediaContent
{
    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
}
