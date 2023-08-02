using System;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

public interface IPropertyAuditConfiguration
{
    Type FriendlyValueLookupType { get; }
    Func<object, string> FriendlyValueFactory { get; }
    bool Ignore { get; }
    string FriendlyName { get; }
    IProperty Property { get; }
}