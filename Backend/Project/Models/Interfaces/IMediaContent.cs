using Project.Models.Enumerations;

namespace Project.Models.Interfaces;

public interface IMediaContent
{
    public string Title { get; set; }
    string Description { get; set; }
    DateTime ReleaseDate { get; }
    string OriginalLanguage { get; }
    string Country { get; }
    ICollection<Genre> Genre { get; set; }
}
