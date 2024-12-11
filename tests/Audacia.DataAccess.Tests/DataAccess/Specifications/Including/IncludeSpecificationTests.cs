using System;
using System.Threading.Tasks;
using Audacia.DataAccess.EntityFrameworkCore.SqlServer;
using Audacia.DataAccess.Specifications;
using Audacia.DataAccess.Tests.Helpers.Database;
using Audacia.DataAccess.Tests.Helpers.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Audacia.DataAccess.Tests.DataAccess.Specifications.Including;

[System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP002:Dispose the member as it is assigned with a created IDisposable", Justification = "Fixing this will introduce breaking changes.")]
public class IncludeSpecificationTests : IDisposable
{
    private readonly DummyDbContext _dbContext;

            .Options;

        // Seed in separate instance of context otherwise all seeded entities will be loaded
        using (var context = new DummyDbContext(databaseOptions))
        {
            context.Seed();
        }

        _dbContext = new DummyDbContext(databaseOptions);
        _repository = new ReadDataRepository<DummyDbContext>(_dbContext, new StoredProcedureBuilder());
    }

    [Fact]
    public async Task Single_Include_Sets_Navigation_Property()
    {
        var spec = QuerySpecification.WithInclude<Order>(a => a.With(o => o.Customer));

        var result = await _repository.GetAsync(spec);

        result?.Customer.Should().NotBeNull();
    }

    [Fact]
    public async Task Include_And_Then_Include_Sets_All_Navigation_Properties()
    {
        var spec = QuerySpecification.WithInclude<OrderItem>(a => 
            a.With(i => i.Order).Then(o => o!.Customer));

        var result = await _repository.GetAsync(spec);

        result?.Order?.Customer.Should().NotBeNull();
    }

    [Fact]
    public async Task Multiple_Includes_Sets_All_Navigation_Properties()
    {
        var spec = QuerySpecification
            .WithInclude<OrderItem>(a => a.With(i => i.Order))
            .WithInclude(a => a.With(i => i.Product));

        var result = await _repository.GetAsync(spec);

        result?.Order.Should().NotBeNull();
        result?.Product.Should().NotBeNull();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
