namespace Employee;

public class WishlistGenerator {
    private readonly Random rnd;

    public WishlistGenerator(int seed) {
        rnd = new Random(seed);
    }
    public WishlistGenerator() {
        rnd = new Random();
    }
        
    public Wishlist GenerateWishlist(int employeeId, List<Employee> desiredEmployees) {
        int[] shuffledDesiredIds = desiredEmployees
            .Select(e => e.Id)
            .OrderBy(x => rnd.Next())
            .ToArray();
        
        
        return new Wishlist(employeeId, shuffledDesiredIds);
    }
}