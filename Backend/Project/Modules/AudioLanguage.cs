namespace Project.Modules;

public class AudioLanguage
{
    public int Id { get; set; }
    public string Language { get; set; } = null!;

    public string MediaTitle { get; set; } = null!;
    public AudioOption AudioOption { get; set; } = null!;
}