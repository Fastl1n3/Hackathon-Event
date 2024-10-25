using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon {
    public class WishlistGenerator {
        private readonly Random rnd;

        public WishlistGenerator(int seed) {
            rnd = new Random(seed);
        }
        
        public List<Wishlist> GenerateWishlists(List<Employee> employees, List<Employee> desiredEmployees) {
            List<Wishlist> wishlists = new List<Wishlist>();
            foreach (var employee in employees) {
                int[] shuffledDesiredIds = desiredEmployees
                    .Select(e => e.Id)
                    .OrderBy(x => rnd.Next())
                    .ToArray();
                
                wishlists.Add(new Wishlist(employee.Id, shuffledDesiredIds));
            }
            return wishlists;
        }
    }
}