using Contracts;
using HRDirector.rabbitmq;

namespace HRDirector;

public class HRDirectorService {//: BackgroundService {
    private readonly DirectorDbContext _dbContext;
    private readonly DirectorRabbitMqConsumer _directorRabbitMqConsumer;
    public HRDirectorService(DirectorDbContext dbContext, DirectorRabbitMqConsumer directorRabbitMqConsumer) {
        _dbContext = dbContext;
        _directorRabbitMqConsumer = directorRabbitMqConsumer;
    }

    public void HandleTeams(TeamsRequest request) {
        var teamLeadWishlists = new List<Wishlist>();
        var juniorWishlists = new List<Wishlist>();
        var messageAccumulator = _directorRabbitMqConsumer.GetMessageAccumulator();
        while (messageAccumulator.GetMessages().Count != request.Teams.Count * 2) { }
        var allWishlists = new List<WishlistRequest>(messageAccumulator.GetMessages());
        messageAccumulator.ClearMessages();
        
        WishlistsRequestsToWishlists(allWishlists, juniorWishlists, teamLeadWishlists);
        Console.WriteLine("LEN: " + teamLeadWishlists.Count + ", " + juniorWishlists.Count);
        var harmonicMean = CalculateHarmonicMean(request.Teams, juniorWishlists, teamLeadWishlists);
        Console.WriteLine("Harmonic Mean: " + harmonicMean);
        teamLeadWishlists.Clear();
        juniorWishlists.Clear();
        WriteHackathon(request.Teams, harmonicMean, juniorWishlists, teamLeadWishlists);
    }
    
    private void WishlistsRequestsToWishlists(List<WishlistRequest> wishlists, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
        foreach (var wishlist in wishlists) {
            switch (wishlist.Role.ToLower()) {
                case "teamlead":
                    teamLeadWishlists.Add(new Wishlist(wishlist.EmployeeId, wishlist.DesiredEmployees));
                    break;
                case "junior":
                    juniorWishlists.Add(new Wishlist(wishlist.EmployeeId, wishlist.DesiredEmployees));
                    break;
                default:
                    throw new ArgumentException("Invalid role. Must be either 'TeamLead' or 'Junior'");
            }
        }
    }
    
    private double CalculateHarmonicMean(List<Team> teams, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
        double znam = 0.0;
        foreach (var team in teams) {
            var teamLeadWishlist = teamLeadWishlists.First(w => w.EmployeeId == team.TeamLead.Id);
            var juniorWishlist = juniorWishlists.First(w => w.EmployeeId == team.Junior.Id);
            znam += 1.0 / (GetSatisfaction(teams.Count, teamLeadWishlist, team.Junior.Id) + GetSatisfaction(teams.Count, juniorWishlist, team.TeamLead.Id));
        }
        return teams.Count / znam;
    }

    private int GetSatisfaction(int num, Wishlist wishlist, int desiredEmployeeId) {
        int index = Array.IndexOf(wishlist.DesiredEmployees, desiredEmployeeId);
        return num - index;
    }

    private void WriteHackathon(List<Team> teams, double harmonicMean, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
        var hackathonEntity = new HackathonEntity { HarmonicMean = harmonicMean };
        using var transaction = _dbContext.Database.BeginTransaction();
        try {
            foreach (var team in teams) {
                var teamleadEntity = _dbContext.Employees.Find(team.TeamLead.Id) ?? new EmployeeEntity {
                    Id = team.TeamLead.Id,
                    Name = team.TeamLead.Name,
                    Role = "teamlead"
                };
                var juniorEntity = _dbContext.Employees.Find(team.Junior.Id) ?? new EmployeeEntity {
                    Id = team.Junior.Id,
                    Name = team.Junior.Name,
                    Role = "junior"
                };
                var teamEntity = new TeamEntity {
                    Hackathon = hackathonEntity,
                    Teamlead = teamleadEntity,
                    Junior = juniorEntity
                };
                hackathonEntity.Teams.Add(teamEntity);
            }

            foreach (var wishlist in juniorWishlists) {
                SaveWishlist(hackathonEntity, wishlist);
            }

            foreach (var wishlist in teamLeadWishlists) {
                SaveWishlist(hackathonEntity, wishlist);
            }

            _dbContext.Hackathons.Add(hackathonEntity);
            _dbContext.SaveChanges();
            transaction.Commit();
        }
        catch {
            transaction.Rollback();
            throw;
        }
    }
    
    private void SaveWishlist(HackathonEntity hackathonEntity, Wishlist wishlist) {
        var employeeEntity = _dbContext.Employees.Find(wishlist.EmployeeId) ?? throw new ArgumentException("Employee not found");
        var wishlistEntity = new WishlistEntity { Hackathon = hackathonEntity, Employee = employeeEntity };

        for (int i = 0; i < wishlist.DesiredEmployees.Length; i++) {
            var desiredEmployeeId = wishlist.DesiredEmployees[i];
            var desiredEmployeeEntity = _dbContext.Employees.Find(desiredEmployeeId) ?? throw new ArgumentException("Employee not found");

            var wishlistEmployee = new WishlistEmployee {
                Wishlist = wishlistEntity,
                DesiredEmployee = desiredEmployeeEntity,
                Rank = wishlist.DesiredEmployees.Length - i
            };
            wishlistEntity.DesiredEmployees.Add(wishlistEmployee);
        }
        _dbContext.Wishlists.Add(wishlistEntity);
    }
}