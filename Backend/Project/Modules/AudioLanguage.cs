namespace Project.Modules;

public class AudioLanguage
{
    public int Id { get; set; }
    public string Language { get; set; } = null!;

    public int AudioOptionId { get; set; }
    public AudioOption AudioOption { get; set; } = null!;
}
