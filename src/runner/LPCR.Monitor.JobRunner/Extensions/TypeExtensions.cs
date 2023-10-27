using System;
using System.Linq;

namespace LPCR.Monitor.JobRunner.Extensions;

/// <summary>
/// Provides extensions for the native <see cref="Type"/> class.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Check if the given source type implements the given interface type.
    /// </summary>
    /// <typeparam name="TInterface">Interface type.</typeparam>
    /// <param name="sourceType">Source type.</param>
    /// <returns>True if the source type implements the given generic type.</returns>
    /// <exception cref="ArgumentNullException">Source type or given interface type is null.</exception>
    /// <exception cref="ArgumentException">Given interface type is not an interface.</exception>
    public static bool ImplementsInterface<TInterface>(this Type sourceType)
    {
        ArgumentNullException.ThrowIfNull(sourceType, nameof(sourceType));

        Type interfaceType = typeof(TInterface);

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"The given interface type '{interfaceType.FullName}' is not an interface.");
        }

        if (interfaceType.IsGenericType)
        {
            return sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }
        else
        {
            return sourceType.GetInterfaces().Any(x => x == interfaceType);
        }
    }

    /// <summary>
    /// Check if the given source type implements the given interface type.
    /// </summary>
    /// <param name="sourceType">Source type.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <returns>True if the source type implements the given generic type.</returns>
    /// <exception cref="ArgumentNullException">Source type or given interface type is null.</exception>
    /// <exception cref="ArgumentException">Given interface type is not an interface.</exception>
    public static bool ImplementsInterface(this Type sourceType, Type interfaceType)
    {
        ArgumentNullException.ThrowIfNull(sourceType, nameof(sourceType));

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"The given interface type '{interfaceType.FullName}' is not an interface.");
        }

        if (interfaceType.IsGenericType)
        {
            return sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }
        else
        {
            return sourceType.GetInterfaces().Any(x => x == interfaceType);
        }
    }
}
