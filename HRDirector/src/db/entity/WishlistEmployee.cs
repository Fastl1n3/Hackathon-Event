using System.ComponentModel.DataAnnotations.Schema;

namespace HRDirector;

[Table("wishlistemployee")]
public class WishlistEmployee {
    public int WishlistId { get; set; }
    public int DesiredEmployeeId { get; set; }
    public WishlistEntity Wishlist { get; set; } // primary key
    public EmployeeEntity DesiredEmployee { get; set; } // primary key
    public int Rank { get; set; }
}