namespace Project.Models;

public class Episode
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public double Length { get; set; }
    public int SeasonNumber { get; set; }

    public string SeriesTitle { get; set; } = null!;
    public Series Series { get; set; } = null!;
}
