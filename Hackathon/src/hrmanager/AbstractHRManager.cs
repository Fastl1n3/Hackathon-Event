using System.Collections.Generic;

namespace Hackathon {
    public abstract class AbstractHRManager {
        protected readonly ITeamBuildingStrategy teamBuildingStrategy;

        protected AbstractHRManager(ITeamBuildingStrategy teamBuildingStrategy) {
            this.teamBuildingStrategy = teamBuildingStrategy;
        }

        public abstract List<Team> BuildOptimalTeams(List<Employee> teamLeads, List<Employee> juniors, List<Wishlist> teamLeadWishlists, List<Wishlist> juniorWishlists);
    }
}