using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon;

public class HRDirector {
    public double CalculateHarmonicMean(List<Team> teams, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
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
}