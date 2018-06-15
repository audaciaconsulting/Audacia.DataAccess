using System;
using Microsoft.EntityFrameworkCore;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing
{
    public class DynamicAuditSinkFactory<TDbContext> : IAuditSinkFactory<TDbContext>
        where TDbContext : DbContext
    {
        private readonly Func<TDbContext, IAuditSink> _factory;

        public DynamicAuditSinkFactory(Func<TDbContext, IAuditSink> factory)
        {
            _factory = factory;
        }

        public IAuditSink Create(TDbContext context) => _factory.Invoke(context);
    }
}