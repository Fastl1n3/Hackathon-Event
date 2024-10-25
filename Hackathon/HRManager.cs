using System.Collections.Generic;

namespace Hackathon {
    public class HRManager {
        private readonly ITeamBuildingStrategy teamBuildingStrategy;

        public HRManager(ITeamBuildingStrategy teamBuildingStrategy) {
            this.teamBuildingStrategy = teamBuildingStrategy;
        }

        public List<Team> FormOptimalTeams(List<Employee> juniors, List<Employee> teamLeads, List<Wishlist> juniorWishlists, List<Wishlist> teamLeadWishlists) {
            return teamBuildingStrategy.BuildTeams(juniors, teamLeads, juniorWishlists, teamLeadWishlists);
        }
        
    }
}