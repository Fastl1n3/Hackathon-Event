using System;
using System.Collections.Generic;
using Hackathon;
using Hackathon.hrmanager;
using NUnit.Framework;

namespace HackathonTest {
    [TestFixture]
    public class HRManagerTest {
        private HRManager _hrManager;

        [SetUp]
        public void Setup() {
            _hrManager = new HRManager(new GaleShapleyTeamBuildingStrategy());
        }

        [Test]
        public void BuildOptimalTeams_ShouldReturnCorrectNumberOfTeams() {
            var juniors = new List<Employee> {
                new(11, "Junior1"),
                new(22, "Junior2")
            };
            var teamLeads = new List<Employee> {
                new(1, "TeamLead1"),
                new(2, "TeamLead2")
            };
            var juniorWishlists = new List<Wishlist> {
                new(11, new[] { 1, 2 }),
                new(22, new[] { 2, 1 })
            };
            var teamLeadWishlists = new List<Wishlist> {
                new(1, new[] { 11, 22 }),
                new(2, new[] { 22, 11 })
            };

            var teams = _hrManager.BuildOptimalTeams(juniors, teamLeads, juniorWishlists, teamLeadWishlists);
            Assert.AreEqual(2, teams.Count);
        }
        
        [Test]
        public void BuildOptimalTeams_ShouldReturnExpectedTeamDistribution() {
            // Arrange
            var juniors = new List<Employee> {
                new(11, "Junior1"),
                new(22, "Junior2"),
                new(33, "Junior3")
            };

            var teamLeads = new List<Employee> {
                new(1, "TeamLead1"),
                new(2, "TeamLead2"),
                new(3, "TeamLead3")
            };

            var juniorWishlists = new List<Wishlist> {
                new(11, new[] { 3, 1, 2 }), 
                new(22, new[] { 2, 3, 1 }),
                new(33, new[] { 3, 1, 2 }) 
            };

            var teamLeadWishlists = new List<Wishlist> {
                new(1, new[] { 11, 22, 33 }),
                new(2, new[] { 22, 33, 11 }), 
                new(3, new[] { 22, 11, 33 })
            };

            var expectedTeams = new List<Team> {
                new(teamLeads[2], juniors[0]),
                new(teamLeads[1], juniors[1]),
                new(teamLeads[0], juniors[2])
            };
            
            var teams = _hrManager.BuildOptimalTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
            for (int i = 0; i < teams.Count; i++) {
                Assert.AreEqual(expectedTeams[i].Junior.Id, teams[i].Junior.Id);
                Assert.AreEqual(expectedTeams[i].TeamLead.Id, teams[i].TeamLead.Id);
            }
        }
    }
}