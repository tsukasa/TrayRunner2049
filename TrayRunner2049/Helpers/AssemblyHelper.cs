using System.Reflection;

namespace TrayRunner2049.Helpers;

public static class AssemblyHelper
{
    /// <summary>
    /// Internal cache for the application name.
    /// Reduces reflection calls.
    /// </summary>
    private static string? _applicationName;

    /// <summary>
    /// Returns the name of the application from the assembly attributes.
    /// This method first attempts to retrieve the product name from the AssemblyProductAttribute,
    /// and falls back to the assembly name if the product attribute is not available.
    /// The result is cached to improve performance on subsequent calls.
    /// </summary>
    /// <returns>The product name of the application if available, otherwise the assembly name. May return null if both are unavailable.</returns>
    /// <exception cref="System.Security.SecurityException">Thrown when the caller does not have the required permission to access assembly information.</exception>
    /// <exception cref="System.IO.FileLoadException">Thrown when the assembly file cannot be loaded.</exception>
    /// <exception cref="System.BadImageFormatException">Thrown when the assembly file is not a valid assembly.</exception>
    public static string? GetApplicationName()
    {
        if (string.IsNullOrEmpty(_applicationName))
        {
            var assembly = typeof(Program).Assembly;
            var attribute = assembly.GetCustomAttribute<System.Reflection.AssemblyProductAttribute>();
            _applicationName = attribute?.Product ?? assembly.GetName().Name;
        }

        return _applicationName;
    }
}