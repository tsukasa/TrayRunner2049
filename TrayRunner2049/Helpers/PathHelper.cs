namespace TrayRunner2049.Helpers;

public static class PathHelper
{
    private static string? _dataPath;

    /// <summary>
    /// Returns the path to the application data directory for TrayRunner2049.
    /// This method attempts to find a writable directory in the following order:
    /// 1. Current working directory
    /// 2. Executable directory
    /// 3. Local application data directory
    /// The result is cached for subsequent calls.
    /// </summary>
    /// <param name="filename">Optional filename to append to the data path. If null, returns the base data directory path.</param>
    /// <returns>The full path to the data directory, or the full path to the specified file within the data directory if filename is provided.</returns>
    /// <exception cref="IOException">Thrown when no writable directory can be found among the current directory, executable directory, or local application data directory.</exception>
    /// <exception cref="ArgumentException">Thrown when the filename contains invalid path characters.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the directories is denied.</exception>
    public static string GetDataPath(string? filename = null)
    {
        if (_dataPath == null)
        {
            string? currentDirectory = Directory.GetCurrentDirectory();
            string? executableDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string? localAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(TrayRunner2049));

            _dataPath = (
                TestPath(currentDirectory) ??
                TestPath(executableDirectory) ??
                TestPath(localAppDataDirectory)) ??
                throw new IOException(
                    $"{AssemblyHelper.GetApplicationName()} was unable to write to the working directory ({currentDirectory}), the executable's directory ({executableDirectory}), and the application data directory ({localAppDataDirectory}).  You will need to change the working directory of TrayRunner or move the TrayRunner files to a directory with write access.");
        }

        filename = filename != null ? Path.Combine(_dataPath, filename) : _dataPath;
        return filename;
    }

    /// <summary>
    /// Tests if the specified path is writable by creating and deleting a temporary test file.
    /// This method creates the directory if it doesn't exist, writes a test file named "batty.txt",
    /// and then deletes it to verify write permissions. If any operation fails, the method returns null.
    /// </summary>
    /// <param name="path">The directory path to test for write permissions. Can be null.</param>
    /// <returns>The original path if it is writable and accessible, or null if the path is not writable, null, or any I/O operation fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown when path is null and directory creation is attempted.</exception>
    private static string? TestPath(string? path)
    {
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path ?? throw new ArgumentNullException(nameof(path)));

            string testFilePath = Path.Combine(path, "batty.txt");
            File.WriteAllText(testFilePath, "like tears in the rain");
            File.Delete(testFilePath);
        }
        catch
        {
            path = null;
        }

        return path;
    }
}