using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon {
    public class HRDirector {
        public double CalculateAverageHarmonicIndex(List<Team> teams, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
            double totalHarmonicSum = 0.0;
            foreach (var team in teams) {
                var teamLeadWishlist = teamLeadWishlists.First(w => w.EmployeeId == team.TeamLead.Id);
                var juniorWishlist = juniorWishlists.First(w => w.EmployeeId == team.Junior.Id);
                double harmonicValue = GetHarmonicMean(team, juniorWishlist, teamLeadWishlist);
                totalHarmonicSum += harmonicValue;
            }
            double averageHarmonicIndex = teams.Count > 0 ? totalHarmonicSum / teams.Count : 0;
            return averageHarmonicIndex;
        }

        private double GetHarmonicMean(Team team, Wishlist juniorWishlist, Wishlist teamLeadWishlist) {
            int juniorSatisfaction = GetSatisfaction(juniorWishlist, team.TeamLead.Id);
            int teamLeadSatisfaction = GetSatisfaction(teamLeadWishlist, team.Junior.Id);
            if (teamLeadSatisfaction == 0 || juniorSatisfaction == 0) {
                return 0;
            }

            return 2.0 * teamLeadSatisfaction * juniorSatisfaction / (teamLeadSatisfaction + juniorSatisfaction);
        }

        private int GetSatisfaction(Wishlist wishlist, int desiredEmployeeId) {
            int index = Array.IndexOf(wishlist.DesiredEmployees, desiredEmployeeId);
            return index >= 0 ? 20 - index : 0;
        }
    }
}