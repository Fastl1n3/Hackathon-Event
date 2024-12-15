using System;
using System.Collections.Generic;
using Hackathon.db.context;
using Hackathon.db.entity;

namespace Hackathon;

public class Hackathon {
    private readonly AbstractHRManager _hrManager;
    private readonly HRDirector _hrDirector;
    private readonly HackathonDbContext _dbContext;
        
    public Hackathon(AbstractHRManager hrManager, HRDirector hrDirector, HackathonDbContext hackathonDbContext) {
        _hrManager = hrManager;
        _hrDirector = hrDirector;
        _dbContext = hackathonDbContext;
    }
        
    public double Start(List<Employee> juniors, List<Employee> teamLeads, 
        List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
            
        var dreamTeams = _hrManager.BuildOptimalTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
        double harmonicMean = _hrDirector.CalculateHarmonicMean(dreamTeams, juniorWishlists, teamLeadWishlists);
      //  WriteHackathon(dreamTeams, harmonicMean, juniorWishlists, teamLeadWishlists);
        return harmonicMean;
    }

    private void WriteHackathon(List<Team> teams, double harmonicMean, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
        Console.WriteLine("ЧЕК writeDb");

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