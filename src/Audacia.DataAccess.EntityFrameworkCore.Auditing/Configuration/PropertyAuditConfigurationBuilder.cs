using System;
using System.Reflection;

namespace Audacia.DataAccess.EntityFrameworkCore.Auditing.Configuration;

/// <summary>
/// PropertyAuditConfigurationBuilder class.
/// </summary>
/// <param name="propertyInfo">Instance of <see cref="PropertyInfo"/>.</param>
public abstract class PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
{
    /// <summary>
    /// PropertyInfo <see cref="PropertyInfo"/>.
    /// </summary>
    internal PropertyInfo PropertyInfo = propertyInfo;

    /// <summary>
    /// Value of InternalIgnore. Indicates whether to ignore audit actions.
    /// </summary>
    internal bool? InternalIgnore;

    /// <summary>
    /// Value of InternalFriendlyName.
    /// </summary>
    internal string InternalFriendlyName = string.Empty;

    /// <summary>
    /// Value of InternalFriendlyValueLookupType.
    /// </summary>
    internal Type? InternalFriendlyValueLookupType;

    /// <summary>
    /// InternalFriendlyValueFactory delegate.
    /// </summary>
    internal Func<object, string>? InternalFriendlyValueFactory;
}

/// <summary>
/// PropertyAuditConfigurationBuilder class.
/// </summary>
/// <typeparam name="T">Factory type.</typeparam>
/// <typeparam name="TProperty">Property type.</typeparam>
public class PropertyAuditConfigurationBuilder<T, TProperty> : PropertyAuditConfigurationBuilder
    where T : class
{
    /// <summary>
    /// Assigns value of propertyInfo.
    /// </summary>
    /// <param name="propertyInfo">Instance of <see cref="PropertyInfo"/>. </param>
    public PropertyAuditConfigurationBuilder(PropertyInfo propertyInfo)
        : base(propertyInfo)
    {
    }

    /// <summary>
    /// Sets value of InternalFriendlyName.
    /// </summary>
    /// <param name="friendlyName">Value of friendlyName.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{T, TProperty}"/>. </returns>
    public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyName(string friendlyName)
    {
        InternalFriendlyName = friendlyName;

        return this;
    }

    /// <summary>
    /// Set the InternalFriendlyValueFactory.
    /// </summary>
    /// <param name="valueFactory">ValueFactory delegate.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{T, TProperty}"/>. </returns>
    public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyValue(
        Func<T, string> valueFactory)
    {
        InternalFriendlyValueFactory = o => valueFactory((o as T)!);

        return this;
    }

    /// <summary>
    /// Sets values of InternalFriendlyValueLookupType and InternalFriendlyValueFactory.
    /// </summary>
    /// <typeparam name="TValueLookup">Type of FriendlyLookup.</typeparam>
    /// <param name="valueFactory">ValueFactory delegate.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{T, TProperty}"/>. </returns>
    public PropertyAuditConfigurationBuilder<T, TProperty> FriendlyValue<TValueLookup>(
        Func<TValueLookup, string> valueFactory)
        where TValueLookup : class
    {
        InternalFriendlyValueLookupType = typeof(TValueLookup);
        InternalFriendlyValueFactory = o => valueFactory((o as TValueLookup)!);

        return this;
    }

    /// <summary>
    /// Set the value of InternalIgnore.
    /// </summary>
    /// <param name="shouldIgnore">Value of ignore.</param>
    /// <returns><see cref="PropertyAuditConfigurationBuilder{T, TProperty}"/>. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "AV1564:Parameter in public or internal member is of type bool or bool?", Justification = "Using booleans provides an easy to understand parameter.")]
    public PropertyAuditConfigurationBuilder<T, TProperty> Ignore(bool shouldIgnore = true)
    {
        InternalIgnore = shouldIgnore;

        return this;
    }
}