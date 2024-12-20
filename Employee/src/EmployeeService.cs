using Contracts;
using Employee.rabbitmq;

namespace Employee;

public class EmployeeService {
    private readonly EmployeeRabbitMqService _mqService;
    
    private readonly WishlistGenerator _wishlistGenerator = new(); 

    public EmployeeService(EmployeeRabbitMqService mqService) {
        _mqService = mqService;
    }

    public void GoToHackathon() {
        var wishlistRequest = CreateWishlistRequest();
        Console.WriteLine("Wishlist created.");
        _mqService.SendWishlistAsync(wishlistRequest);
        Console.WriteLine("Wishlist submitted.");
    }

    private WishlistRequest CreateWishlistRequest() {
        List<Employee> partners;
        var id = int.Parse(Environment.GetEnvironmentVariable("APP_ID") ?? throw new InvalidOperationException());
        var type = Environment.GetEnvironmentVariable("APP_TYPE");
        var name = Environment.GetEnvironmentVariable("name") ?? "";
        Console.WriteLine("Wishlist for id: " + id + ", type: " + type + ", name: " + name);
        if (type == "teamlead") {
            partners = CsvEmployeeReader.ReadCsv("resources/Juniors5.csv");
        } else if (type == "junior") {
            partners = CsvEmployeeReader.ReadCsv("resources/Teamleads5.csv");
        }
        else {
            throw new ArgumentException("type must be teamlead or junior");
        }
        var wishlist = _wishlistGenerator.GenerateWishlist(id, partners);
        Console.WriteLine(wishlist.EmployeeId+", " + wishlist.DesiredEmployees[0]);
        return new WishlistRequest(id, name, type, wishlist.DesiredEmployees);
    }
}
