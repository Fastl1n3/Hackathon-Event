namespace HRDirector;

public class HRDirectorService {
    private readonly DirectorDbContext _dbContext;

    public HRDirectorService(DirectorDbContext dbContext) {
        _dbContext = dbContext;
    }

    private double CalculateHarmonicMean(List<Team> teams, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
        double znam = 0.0;
        foreach (var team in teams) {
        //    Console.WriteLine("teamLead: " + team.TeamLead.Id + ", junior: " + team.Junior.Id);
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

    public void HandleTeams(TeamsRequest request) {
        var harmonicMean = CalculateHarmonicMean(request.Teams, request.JuniorWishlists, request.TeamLeadWishlists);
        Console.WriteLine("Harmonic Mean: " + harmonicMean);
        WriteHackathon(request.Teams, harmonicMean, request.JuniorWishlists, request.TeamLeadWishlists);
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