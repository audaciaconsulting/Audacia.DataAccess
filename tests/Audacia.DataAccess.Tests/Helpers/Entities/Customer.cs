using System.Collections.Generic;

namespace Audacia.DataAccess.Tests.Helpers.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}