using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audacia.Core;
using Audacia.DataAccess.EntityFrameworkCore.Extensions;
using Audacia.DataAccess.Tests.Helpers.Database;
using Audacia.DataAccess.Tests.Helpers.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Audacia.DataAccess.Tests.EntityFrameworkCore.Extensions;

public class QueryableExtensionsTests
{
    private readonly DummyDbContext _dbContext;

    public QueryableExtensionsTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DummyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new DummyDbContext(dbOptions);
    }
    
    [Fact]
    public async Task Total_count_populated_based_on_result_count()
    {
        var dataCount = new Random().Next(100);
        const int pageSize = 10;
        var query = Enumerable.Range(0, dataCount)
            .Select(_ => new Customer());
        await SeedDatabaseAsync(query);
        var pagingRequest = new PagingRequest(pageSize);

        var page = await _dbContext.Customers.ToPageAsync(pagingRequest);

        page.TotalRecords.Should().Be(dataCount);
    }
    
    [Fact]
    public async Task Number_of_pages_is_rounded_up_when_partial_page_is_filled()
    {
        const int dataCount = 11;
        const int pageSize = dataCount - 1;
        var query = Enumerable.Range(0, dataCount)
            .Select(_ => new Customer());
        await SeedDatabaseAsync(query);
        var pagingRequest = new PagingRequest(pageSize);

        var page = await _dbContext.Customers.ToPageAsync(pagingRequest);

        page.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Results_skipped_if_page_number_is_greater_than_one()
    {
        const int pageSize = 1;
        const int pageNumber = 2;
        var expectedExcludedRow = new Customer();
        var query = new List<Customer>
        {
            expectedExcludedRow,
            new Customer(),
            new Customer(),
            new Customer()
        };
        await SeedDatabaseAsync(query);
        var pagingRequest = new PagingRequest(pageSize, pageNumber);

        var page = await _dbContext.Customers.ToPageAsync(pagingRequest);

        page.Data.Should().NotContain(expectedExcludedRow);
    }

    [Fact]
    public async Task All_results_returned_if_no_page_size_specified()
    {
        var dataCount = new Random().Next(100);
        var pagingRequest = new PagingRequest();
        var query = Enumerable.Range(0, dataCount)
            .Select(_ => new Customer());
        await SeedDatabaseAsync(query);

        var page = await _dbContext.Customers.ToPageAsync(pagingRequest);

        page.Data.Should().HaveCount(dataCount);
    }

    [Fact]
    public async Task Sorts_results_before_applying_paging()
    {
        const int pageSize = 1;
        const int pageNumber = 2;
        var pagingRequest = new SortablePagingRequest(pageSize, pageNumber)
        {
            SortProperty = nameof(Customer.FirstName),
            Descending = true
        };
        var expectedExcludedRow = new Customer {FirstName = "A"};
        var query = new List<Customer>
        {
            new Customer {FirstName = "C"},
            new Customer {FirstName = "D"},
            expectedExcludedRow,
            new Customer {FirstName = "B"},
        };
        await SeedDatabaseAsync(query);

        var page = await _dbContext.Customers.ToPageAsync(pagingRequest);

        page.Data.Should().NotContain(expectedExcludedRow);
    }

    [Fact]
    public async Task Throws_exception_when_invalid_sort_property_provided()
    {
        const int pageSize = 1;
        var pagingRequest = new SortablePagingRequest(pageSize)
        {
            SortProperty = Guid.NewGuid().ToString()
        };
        var dataCount = new Random().Next(100);
        var query = Enumerable.Range(0, dataCount)
            .Select(_ => new Customer());
        await SeedDatabaseAsync(query);

        Func<Task<IPage<Customer>>> act = () => _dbContext.Customers.ToPageAsync(pagingRequest);

        await act.Should().ThrowExactlyAsync<ApplicationException>();
    }

    private async Task SeedDatabaseAsync(IEnumerable<Customer> query)
    {
        await _dbContext.Customers.AddRangeAsync(query);
        await _dbContext.SaveChangesAsync();
    }

}