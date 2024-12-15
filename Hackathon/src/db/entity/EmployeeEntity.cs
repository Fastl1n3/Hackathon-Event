using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hackathon.db.entity;

[Table("employee")]
public class EmployeeEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public List<WishlistEntity> Wishlists { get; set; } = new();
}