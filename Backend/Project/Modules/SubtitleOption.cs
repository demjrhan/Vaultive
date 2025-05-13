namespace Project.Modules;

public class SubtitleOption
{
    public int Id { get; set; }

    public ICollection<SubtitleLanguage> SubtitleLanguages { get; set; } = new HashSet<SubtitleLanguage>();

    public string MediaTitle { get; set; } = null!;
    public MediaContent MediaContent { get; set; } = null!;
}
