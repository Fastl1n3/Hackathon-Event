using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon {
    public class HRDirector {
        public double CalculateHarmonicMean(List<Team> teams, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
            double znam = 0.0;
            foreach (var team in teams) {
                var teamLeadWishlist = teamLeadWishlists.First(w => w.EmployeeId == team.TeamLead.Id);
                var juniorWishlist = juniorWishlists.First(w => w.EmployeeId == team.Junior.Id);
                znam += 1.0 / GetSatisfaction(juniorWishlist, team.TeamLead.Id);
                znam += 1.0 / GetSatisfaction(teamLeadWishlist, team.Junior.Id);
            }
            return (juniorWishlists.Count + teamLeadWishlists.Count) / znam;
        }

        private int GetSatisfaction(Wishlist wishlist, int desiredEmployeeId) {
            int index = Array.IndexOf(wishlist.DesiredEmployees, desiredEmployeeId);
            return index >= 0 ? 20 - index : 0;
        }
    }
}