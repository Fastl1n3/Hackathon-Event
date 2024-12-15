using System.Collections.Generic;

namespace Hackathon.hrmanager {
    public class HRManager : AbstractHRManager{
        public HRManager(ITeamBuildingStrategy teamBuildingStrategy) : base(teamBuildingStrategy) {
        }

        public override List<Team> BuildOptimalTeams(List<Employee> teamLeads, List<Employee> juniors, List<Wishlist> teamLeadWishlists, List<Wishlist> juniorWishlists) {
            return teamBuildingStrategy.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
        }
    }
}