using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Employee;

public class EmployeeService : BackgroundService {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    private readonly WishlistGenerator _wishlistGenerator = new WishlistGenerator(); 

    public EmployeeService(HttpClient httpClient, IConfiguration configuration) {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var wishlistRequest = CreateWishlistRequest();
        Console.WriteLine("Wishlist created.");
        var response = await _httpClient.PostAsJsonAsync("http://hrmanager:8086/hrmanager/api/wishlist", wishlistRequest, cancellationToken: stoppingToken);
        Console.WriteLine(!response.IsSuccessStatusCode
            ? $"Failed to send data: {await response.Content.ReadAsStringAsync(stoppingToken)}"
            : "Wishlist successfully sent to HR Manager.");
        
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
        return new WishlistRequest(id, name, type, wishlist.DesiredEmployees);
    }
}

public record WishlistRequest(int EmployeeId, string EmployeeName, string Role, int[] DesiredEmployees);