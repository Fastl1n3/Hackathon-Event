using System.ComponentModel.DataAnnotations.Schema;

namespace HRDirector;

[Table("wishlist")]
public class WishlistEntity {
    public int Id { get; set; }
    public HackathonEntity Hackathon { get; set; }
    public EmployeeEntity Employee { get; set; }
    public List<WishlistEmployee> DesiredEmployees { get; set; } = new();
}