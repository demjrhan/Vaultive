namespace Project.Modules;

public class SubtitleLanguage
{
    public int Id { get; set; }
    public string Language { get; set; } = null!;

    public int SubtitleOptionId { get; set; }
    public SubtitleOption SubtitleOption { get; set; } = null!;
}
