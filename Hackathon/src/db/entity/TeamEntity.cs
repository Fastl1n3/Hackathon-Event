using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.db.entity;

[Table("team")]
public class TeamEntity {
    public int Id { get; set; }
    
    public HackathonEntity Hackathon { get; set; }
    
    [ForeignKey("teamlead_id")]
    public EmployeeEntity Teamlead { get; set; }
    
    [ForeignKey("junior_id")]
    public EmployeeEntity Junior { get; set; }
}