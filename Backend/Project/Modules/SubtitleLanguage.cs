namespace Project.Modules;

public class SubtitleLanguage
{
    public int Id { get; set; }
    public string Language { get; set; } = null!;

    public string MediaTitle { get; set; } = null!;
    public SubtitleOption SubtitleOption { get; set; } = null!;
}
