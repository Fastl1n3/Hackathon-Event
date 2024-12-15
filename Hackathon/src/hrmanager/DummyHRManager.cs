using System.Collections.Generic;

namespace Hackathon.hrmanager {
    public class DummyHRManager : AbstractHRManager {
        public DummyHRManager(ITeamBuildingStrategy teamBuildingStrategy) : base(teamBuildingStrategy) {
        }

        public override List<Team> BuildOptimalTeams(List<Employee> teamLeads, List<Employee> juniors, List<Wishlist> teamLeadWishlists, List<Wishlist> juniorWishlists) {
            return teamBuildingStrategy.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
        }
    }
}