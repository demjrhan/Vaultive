using Project.Models.Enumerations;

namespace Project.Models;

public class ShortFilm : MediaContent
{
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public string SchoolName { get; set; } = null!;

}