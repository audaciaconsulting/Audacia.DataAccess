using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// TypeAuditConfigurationBuilder class.
/// </summary>
public abstract class TypeAuditConfigurationBuilder(Type typeOfEntity)
{
    /// <summary>
    /// Value of Properties.
    /// </summary>
    internal readonly IDictionary<string, PropertyAuditConfigurationBuilder> Properties =
        new Dictionary<string, PropertyAuditConfigurationBuilder>();

    /// <summary>
    /// Value of InternalIgnore.
    /// </summary>
    internal bool? InternalIgnore;

    /// <summary>
    /// InternalDescriptionFactory delegate.
    /// </summary>
    internal Func<object, string>? InternalDescriptionFactory;

    /// <summary>
    /// Value of InternalAuditStrategy.
    /// </summary>
    internal AuditStrategy? InternalAuditStrategy;

    /// <summary>
    /// Value of InternalFriendlyName.
    /// </summary>
    internal string InternalFriendlyName = string.Empty;

    /// <summary>
    /// Gets the value of TypeOfEntity.
    /// </summary>
    public Type TypeOfEntity { get; } = typeOfEntity;
}

/// <summary>
/// TypeAuditConfigurationBuilder class.
/// </summary>
/// <typeparam name="T">Type of <see cref="TypeAuditConfigurationBuilder{T}"/>.</typeparam>
public class TypeAuditConfigurationBuilder<T> : TypeAuditConfigurationBuilder
    where T : class
{
    /// <summary>
    /// Assigns value of typeOfEntity.
    /// </summary>
    public TypeAuditConfigurationBuilder()
        : base(typeof(T))
    {
    }

    /// <summary>
    /// Sets the value of InternalIgnore.
    /// </summary>
    /// <param name="shouldIgnore">Value of InternalIgnore.</param>
    /// <returns><see cref="TypeAuditConfigurationBuilder{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    public TypeAuditConfigurationBuilder<T> Ignore(bool shouldIgnore = true)
    {
        InternalIgnore = shouldIgnore;

        return this;
    }

    /// <summary>
    /// Sets the value of Strategy.
    /// </summary>
    /// <param name="strategy">value of strategy.</param>
    /// <returns><see cref="TypeAuditConfigurationBuilder{T}"/>.</returns>
    public TypeAuditConfigurationBuilder<T> Strategy(AuditStrategy strategy)
    {
        InternalAuditStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Sets InternalDescriptionFactory.
    /// </summary>
    /// <param name="descriptionFactory">DescriptionFactory delegate.</param>
    /// <returns><see cref="TypeAuditConfigurationBuilder{T}"/>.</returns>
    public TypeAuditConfigurationBuilder<T> Description(Func<T, string> descriptionFactory)
    {
        InternalDescriptionFactory = entity => descriptionFactory((entity as T)!);

        return this;
    }

    /// <summary>
    /// Sets propertyInfo and creates a new <see cref="PropertyAuditConfigurationBuilder{T,TProperty}" />.
    /// </summary>
    /// <typeparam name="TProperty">Type of property.</typeparam>
    /// <param name="propertySelector">PropertySelector expression.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{T,TProperty}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public PropertyAuditConfigurationBuilder<T, TProperty> Property<TProperty>(
        Expression<Func<T, TProperty>>
            propertySelector)
    {
        var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertySelector);

        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        if (!Properties.TryGetValue(propertyInfo.Name, out var propertyBuilder))
        {
            propertyBuilder = new PropertyAuditConfigurationBuilder<T, TProperty>(propertyInfo);
            Properties.Add(propertyInfo.Name, propertyBuilder);
        }

        return (PropertyAuditConfigurationBuilder<T, TProperty>)propertyBuilder;
    }

    /// <summary>
    /// Creates a new <see cref="PropertyAuditConfigurationBuilder{T,TProperty}" />.
    /// </summary>
    /// <typeparam name="TProperty">Type of property.</typeparam>
    /// <param name="propertySelector">PropertySelector expression.</param>
    /// <param name="propertyBuilderAction">PropertyBuilderAction delegate.</param>
    /// <returns><see cref="TypeAuditConfigurationBuilder{T}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public TypeAuditConfigurationBuilder<T> Property<TProperty>(
        Expression<Func<T, TProperty>> propertySelector, 
        Action<PropertyAuditConfigurationBuilder<T, TProperty>> propertyBuilderAction)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilderAction, nameof(propertyBuilderAction));
        var builder = Property(propertySelector);

        propertyBuilderAction(builder);

        return this;
    }
}