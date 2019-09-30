using System;
using System.Linq;
using System.Threading.Tasks;
using Audacia.DataAccess.EntityFrameworkCore.SqlServer;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Tests.Helpers.Database;
using Audacia.DataAccess.Tests.Helpers.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Audacia.DataAccess.Tests.DataAccess.Specifications.Ordering
{
    public class OrderSpecificationTests : IDisposable
    {
        private readonly DummyDbContext _dbContext;
        private readonly IReadableDataRepository _repository;

        public OrderSpecificationTests()
        {
            var dbOptions = new DbContextOptionsBuilder<DummyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Seed in separate instance of context otherwise all seeded entities will be loaded
            using (var context = new DummyDbContext(dbOptions))
            {
                context.Seed();
            }

            _dbContext = new DummyDbContext(dbOptions);
            _repository = new ReadDataRepository<DummyDbContext>(_dbContext, new StoredProcedureBuilder());
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


        [Fact]
        public async Task Single_Order_Performs_The_Correct_Ordering()
        {
            var spec = QuerySpecification
                .WithOrder<Customer>(s => s.Asc(c => c.FirstName));

            var results = (await _repository.GetAllAsync(spec)).ToList();

            results.First().FirstName.Should().Be("Alice");
            results.Last().FirstName.Should().Be("Bob");
        }

        [Fact]
        public async Task Multiple_Order_Performs_The_Correct_Ordering()
        {
            var spec = QuerySpecification
                .WithOrder<Product>(s => s.Asc(c => c.Price))
                .WithOrder(s => s.Desc(c => c.Description));

            var results = (await _repository.GetAllAsync(spec)).ToList();

            results[0].Description.Should().Be("Pencil");
            results[1].Description.Should().Be("Paper");
        }
    }
}