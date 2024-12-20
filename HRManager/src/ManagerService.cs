using System.Net.Http.Json;
using Contracts;

namespace HRManager;

public class ManagerService {
    private readonly AbstractHRManager _hrManager;
    private readonly HttpClient _httpClient;
    
    private readonly List<Employee> teamLeads = new();
    private readonly List<Employee> juniors = new();
    private readonly List<Wishlist> teamLeadWishlists = new();
    private readonly List<Wishlist> juniorWishlists = new();
    
    private static readonly object expression = new();
    public ManagerService(AbstractHRManager hrManager, HttpClient httpClient) {
        _hrManager = hrManager;
        _httpClient = httpClient;
    }
    
    public Task HandleWishlistRequest(WishlistRequest wishlist) {
        switch (wishlist.Role.ToLower()) {
            case "teamlead":
                teamLeads.Add(new Employee(wishlist.EmployeeId, wishlist.EmployeeName));
                teamLeadWishlists.Add(new Wishlist(wishlist.EmployeeId, wishlist.DesiredEmployees));
                break;
            case "junior":
                juniors.Add(new Employee(wishlist.EmployeeId, wishlist.EmployeeName));
                juniorWishlists.Add(new Wishlist(wishlist.EmployeeId, wishlist.DesiredEmployees));
                break;
            default:
                throw new ArgumentException("Invalid role. Must be either 'TeamLead' or 'Junior'");
        }
        Console.WriteLine(teamLeads.Count + ", " + juniors.Count);
        lock (expression) {
            if (teamLeads.Count == 5 && juniors.Count == 5) {
                Console.WriteLine("HRManager: Forming teams...");
                var teams = _hrManager.BuildOptimalTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
                SendTeams(teams);

                teamLeads.Clear();
                juniors.Clear();
                teamLeadWishlists.Clear();
                juniorWishlists.Clear();
            }
        }

        return Task.CompletedTask;
    }
    
    private void SendTeams(List<Team> teams) {
        var teamsData = new {
            Teams = teams
        };
        foreach (var team in teams) {
            Console.WriteLine(team.TeamLead.Name + " " + team.Junior.Name);
        }

        try {
            var response = _httpClient.PostAsJsonAsync("http://hrdirector:8087/hrdirector/api/teams", teamsData);
            Console.WriteLine("STATUS: " + response.Result.StatusCode);
        } catch (Exception e) {
            Console.WriteLine(e);
        }
    }
}