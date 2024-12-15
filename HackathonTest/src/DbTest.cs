using System.Collections.Generic;
using System.Linq;
using Hackathon.db.context;
using Hackathon.db.entity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HackathonTest;

[TestFixture]
public class DbTest {
    public static HackathonDbContext CreateInMemoryDbContext() {
        var options = new DbContextOptionsBuilder<HackathonDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new HackathonDbContext(options);

        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
    
    [Test]
    public void WriteAndReadHackathon_ShouldPersistAndRetrieveData() {
        using var context = CreateInMemoryDbContext();
        var teamlead = new EmployeeEntity { Id = 2, Name = "Bob", Role = "teamlead" };
        var junior = new EmployeeEntity { Id = 1, Name = "Alice", Role = "junior" };
        
        var hackathon = new HackathonEntity
        {
            HarmonicMean = 5.5,
            Teams = new List<TeamEntity>
            {
                new() {
                    Teamlead = teamlead,
                    Junior = junior
                }
            }
        };

        var wishlist = new WishlistEntity
        {
            Hackathon = hackathon,
            Employee = junior,
            DesiredEmployees = new List<WishlistEmployee>
            {
                new() {
                    DesiredEmployee = teamlead,
                    Rank = 1
                }
            }
        };
        
        context.Hackathons.Add(hackathon);
        context.Wishlists.Add(wishlist);
        context.SaveChanges();
        
        var savedHackathon = context.Hackathons
            .Include(h => h.Teams)
            .ThenInclude(t => t.Teamlead)
            .Include(h => h.Teams)
            .ThenInclude(t => t.Junior)
            .First();

        Assert.AreEqual(5.5, savedHackathon.HarmonicMean);
        Assert.AreEqual(1, savedHackathon.Teams.Count);

        var savedWishlist = context.Wishlists
            .Include(w => w.DesiredEmployees)
            .ThenInclude(we => we.DesiredEmployee)
            .First();

        Assert.AreEqual(1, savedWishlist.DesiredEmployees.Count);
        Assert.AreEqual(2, savedWishlist.DesiredEmployees.First().DesiredEmployee.Id);
        Assert.AreEqual(1, savedWishlist.DesiredEmployees.First().Rank);
    }

    [Test]
    public void WriteAndReadMultipleHackathons_ShouldCalculateAverageHarmonicMean() {
        using var context = CreateInMemoryDbContext();
        context.Hackathons.AddRange(
            new HackathonEntity { HarmonicMean = 4.0 },
            new HackathonEntity { HarmonicMean = 6.0 }
        );
        context.SaveChanges();
        
        var averageHarmonicMean = context.Hackathons.Average(h => h.HarmonicMean);
        
        Assert.AreEqual(5.0, averageHarmonicMean);
    }
}