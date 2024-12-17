using System.ComponentModel.DataAnnotations.Schema;

namespace HRDirector;

[Table("hackathon")]
public class HackathonEntity {
    public int Id { get; set; }
    public double HarmonicMean { get; set; }
    public List<TeamEntity> Teams { get; set; } = new();
}