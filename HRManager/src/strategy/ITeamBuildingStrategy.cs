﻿namespace HRManager;

public interface ITeamBuildingStrategy {
    List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors,
        List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists);
}