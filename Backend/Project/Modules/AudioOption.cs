namespace Project.Modules;

public class AudioOption
{
    public int Id { get; set; }

    public ICollection<AudioLanguage> AudioLanguages { get; set; } = new HashSet<AudioLanguage>();

    public string MediaTitle { get; set; } = null!;
    public MediaContent MediaContent { get; set; } = null!;
}
