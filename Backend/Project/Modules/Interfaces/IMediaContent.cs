using Project.Modules.Enumerations;

namespace Project.Modules.Interfaces;

public interface IMediaContent
{
    public string Title { get; set; }
    string Description { get; set; }
    DateTime ReleaseDate { get; }
    string OriginalLanguage { get; }
    string Country { get; }
    ICollection<Genre> Genre { get; set; }
}
