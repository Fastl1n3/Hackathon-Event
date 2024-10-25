﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon {
    public class GaleShapleyTeamBuildingStrategy : ITeamBuildingStrategy {
        public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, 
            List<Wishlist> teamLeadsWishlists, List<Wishlist> juniorsWishlists) {
            var teamLeadToJunior = new Dictionary<int, int>();
            var juniorDict = juniors.ToDictionary(j => j.Id);
            var teamLeadDict = teamLeads.ToDictionary(t => t.Id);
            var juniorProposals = juniors.ToDictionary(j => j.Id, j => 0);// Кол-во предложений, сделанных каждым джуном
            var freeJuniors = new Queue<int>(juniors.Select(j => j.Id));

            while (freeJuniors.Count > 0) {
                int juniorId = freeJuniors.Dequeue();
                var juniorWishlist = juniorsWishlists.First(w => w.EmployeeId == juniorId);

                // Джун делает предложение следующему по списку тимлиду
                int currentProposalIndex = juniorProposals[juniorId];
                if (currentProposalIndex >= juniorWishlist.DesiredEmployees.Length) {
                    continue;
                }

                int preferredTeamLeadId = juniorWishlist.DesiredEmployees[currentProposalIndex];
                juniorProposals[juniorId]++;

                var teamLeadWishlist = teamLeadsWishlists.First(w => w.EmployeeId == preferredTeamLeadId);

                if (!teamLeadToJunior.ContainsKey(preferredTeamLeadId)) { // свободен
                    teamLeadToJunior[preferredTeamLeadId] = juniorId;
                }
                else { // занят
                    int currentJuniorId = teamLeadToJunior[preferredTeamLeadId];

                    int currentPreferencePos = Array.IndexOf(teamLeadWishlist.DesiredEmployees, currentJuniorId);
                    int newPreferencePos = Array.IndexOf(teamLeadWishlist.DesiredEmployees, juniorId);

                    if (newPreferencePos < currentPreferencePos) {
                        teamLeadToJunior[preferredTeamLeadId] = juniorId;
                        freeJuniors.Enqueue(currentJuniorId); // Обратно в очередь лошару
                    }
                    else {
                        freeJuniors.Enqueue(juniorId);
                    }
                }
            }
            
            return teamLeadToJunior
                .Select(pair => new Team(
                    teamLeadDict[pair.Key],
                    juniorDict[pair.Value]))
                .ToList();
        }
    }
}