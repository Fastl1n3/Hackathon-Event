using System.Collections.Generic;

namespace Hackathon {
    public class DummyTeamBuildingStrategy : ITeamBuildingStrategy{
        public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, 
            List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists) {
            if (teamLeads.Count != juniors.Count) {
                throw new System.ArgumentException("teamsLeads count must be equal to juniors count");
            }
            List<Team> teams = new List<Team>();
            for (int i = 0; i < teamLeads.Count; i++) {
                teams.Add(new Team(teamLeads[i], juniors[i]));
            }
            return teams;
        }
    }
}