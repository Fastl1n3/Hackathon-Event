using System.Collections.Concurrent;
using System.Net.Http.Json;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HRManager;

[Route("hrmanager/api")]
[ApiController]
public class ManagerController : ControllerBase {
    private static readonly ConcurrentQueue<Employee> TeamLeads = new();
    private static readonly ConcurrentQueue<Employee> Juniors = new();
    private static readonly ConcurrentQueue<Wishlist> TeamLeadWishlists = new();
    private static readonly ConcurrentQueue<Wishlist> JuniorWishlists = new();
    
    private readonly AbstractHRManager _hrManager;
    private readonly HttpClient _httpClient;

    public ManagerController(AbstractHRManager hrManager, HttpClient httpClient) {
        _hrManager = hrManager;
        _httpClient = httpClient;
    }
    
    [HttpPost("wishlist")]
    public async Task<IActionResult> SubmitWishlist([FromBody] WishlistRequest request) {
        Console.WriteLine("Receiving wishlist...");
        var employee = new Employee(request.EmployeeId, request.EmployeeName);

        if (request.Role.Equals("Teamlead", StringComparison.OrdinalIgnoreCase)) {
            TeamLeads.Enqueue(employee);
            TeamLeadWishlists.Enqueue(new Wishlist(request.EmployeeId, request.DesiredEmployees));
        }
        else if (request.Role.Equals("Junior", StringComparison.OrdinalIgnoreCase)) {
            Juniors.Enqueue(employee);
            JuniorWishlists.Enqueue(new Wishlist(request.EmployeeId, request.DesiredEmployees));
        }
        else {
            return BadRequest("Invalid role. Must be either 'TeamLead' or 'Junior'.");
        }

        if (TeamLeads.Count == 5 && Juniors.Count == 5) {
            Console.WriteLine("Sending teams...");
            await DistributePreferencesAsync();
        }
        return Ok("Wishlist submitted successfully.");
    }

    private async Task DistributePreferencesAsync() {
        var teams = _hrManager.BuildOptimalTeams(TeamLeads.ToList(), Juniors.ToList(), TeamLeadWishlists.ToList(), JuniorWishlists.ToList());
        var teamsData = new {
            Teams = teams,
            JuniorWishlists = JuniorWishlists.ToList(),
            TeamLeadWishlists = TeamLeadWishlists.ToList()
        };

        var response = await _httpClient.PostAsJsonAsync("http://hrdirector:8087/hrdirector/api/teams", teamsData);
        Console.WriteLine("STATUS: " + response.StatusCode);
        TeamLeads.Clear();
        Juniors.Clear();
        TeamLeadWishlists.Clear();
        JuniorWishlists.Clear();
        
        Console.WriteLine(!response.IsSuccessStatusCode
            ? $"Failed to send data: {await response.Content.ReadAsStringAsync()}"
            : "Distribution successfully sent to HR Director.");
    }
}
