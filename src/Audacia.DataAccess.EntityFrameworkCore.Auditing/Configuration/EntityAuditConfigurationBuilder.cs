using System;
using System.Linq.Expressions;
using Audacia.Core.Extensions;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// EntityAuditConfigurationBuilder class.
/// </summary>
/// <typeparam name="TEntity">Entity type of <see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</typeparam>
public class EntityAuditConfigurationBuilder<TEntity> : TypeAuditConfigurationBuilder
    where TEntity : class
{
    /// <summary>
    /// Main constructor.
    /// </summary>
    public EntityAuditConfigurationBuilder()
        : base(typeof(TEntity))
    {
    }

    /// <summary>
    /// Assigns value of ignore.
    /// </summary>
    /// <param name="ignore">Value of ignore.</param>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    public EntityAuditConfigurationBuilder<TEntity> Ignore(bool ignore = true)
    {
        InternalIgnore = ignore;

        return this;
    }

    /// <summary>
    /// Assigns value of strategy.
    /// </summary>
    /// <param name="strategy">Value of strategy.</param>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    public EntityAuditConfigurationBuilder<TEntity> Strategy(AuditStrategy strategy)
    {
        InternalAuditStrategy = strategy;

        return this;
    }

    /// <summary>
    /// Assigns value of friendlyTypeName.
    /// </summary>
    /// <param name="friendlyTypeName">Value of friendlyTypeName.</param>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    public EntityAuditConfigurationBuilder<TEntity> FriendlyName(string friendlyTypeName)
    {
        InternalFriendlyName = friendlyTypeName;

        return this;
    }

    /// <summary>
    /// Assigns value of nternalDescriptionFactory.
    /// </summary>
    /// <param name="descriptionFactory">Value of descriptionFactory.</param>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    public EntityAuditConfigurationBuilder<TEntity> Description(Func<TEntity, string> descriptionFactory)
    {
        InternalDescriptionFactory = entity => descriptionFactory((entity as TEntity)!);

        return this;
    }

    /// <summary>
    /// Add property details using the given selector.
    /// </summary>
    /// <typeparam name="TProperty">Type of Property.</typeparam>
    /// <param name="propertySelector">PropertySelector delegate.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{TEntity, TProperty}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public PropertyAuditConfigurationBuilder<TEntity, TProperty> Property<TProperty>(
        Expression<Func<TEntity, TProperty>>
            propertySelector)
    {
        var propertyInfo = ExpressionExtensions.GetPropertyInfo(propertySelector);

        ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

        if (!Properties.TryGetValue(propertyInfo.Name, out var propertyBuilder))
        {
            propertyBuilder = new PropertyAuditConfigurationBuilder<TEntity, TProperty>(propertyInfo);
            Properties.Add(propertyInfo.Name, propertyBuilder);
        }

        return (PropertyAuditConfigurationBuilder<TEntity, TProperty>)propertyBuilder;
    }

    /// <summary>
    /// Creates a propertyAuditConfigurationBuilder using given propertySelector expression.
    /// </summary>
    /// <typeparam name="TProperty">Type of property.</typeparam>
    /// <param name="propertySelector">PropertySelector expression.</param>
    /// <param name="propertyBuilderAction">PropertyBuilderAction delegate.</param>
    /// <returns><see cref="EntityAuditConfigurationBuilder{TEntity}"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1551:Call the more overloaded method from other overloads.", Justification = "Return types are different.")]
    public EntityAuditConfigurationBuilder<TEntity> Property<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        Action<PropertyAuditConfigurationBuilder<TEntity, TProperty>> propertyBuilderAction)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilderAction, nameof(propertyBuilderAction));

        var builder = Property(propertySelector);

        propertyBuilderAction(builder);

        return this;
    }
}