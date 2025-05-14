using Project.Models.Enumerations;

namespace Project.Models;

public class Series : MediaContent
{
    public double AvgEpisodeLength { get; private set; }

    public ICollection<Episode> Episodes { get; set; } = new HashSet<Episode>();
    public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
    

 
}
