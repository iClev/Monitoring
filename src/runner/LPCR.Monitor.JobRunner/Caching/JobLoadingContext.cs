using System;
using System.Reflection;
using System.Runtime.Loader;

namespace LPCR.Monitor.JobRunner.Caching;

/// <summary>
/// Provides a mechanism to load job assemblies in a single context.
/// </summary>
internal sealed class JobLoadingContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public JobLoadingContext(string jobAssembliesPath)
    {
        _resolver = new(jobAssembliesPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return !string.IsNullOrWhiteSpace(assemblyPath) ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

        return !string.IsNullOrWhiteSpace(libraryPath) ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }
}
