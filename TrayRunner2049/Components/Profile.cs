using System.Drawing.Imaging;
using System.Text;
using TrayRunner2049.Helpers;

namespace TrayRunner2049.Components;

/// <summary>
/// Represents a profile for an application that can be started by TrayRunner2049.
/// Also contains static helper methods to load and save profiles from/to files.
/// </summary>
public class Profile
{
    /// <summary>
    /// Name of the profile, which is also used as the file name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Full path to the executable file that should be started.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Additional command-line arguments that should be passed to the executable.
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// Working directory for the executable.
    /// </summary>
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// Should the application be automatically restarted if it crashes/exits?
    /// </summary>
    public bool AutoRestart { get; set; }

    /// <summary>
    /// An image used as the icon for the profile in the window and tray.
    /// </summary>
    public Image? Image { get; set; }

    /// <summary>
    /// Encoding used for the standard output and error streams of the process.
    /// </summary>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// Constructor for the Profile class.
    /// </summary>
    public Profile()
    {
        Encoding = Encoding.UTF8;
    }

    /// <summary>
    /// Loads a profile definition from a file and returns a new Profile object.
    /// The file should contain key-value pairs separated by '=' characters.
    /// If an associated image file exists (same name with ".image" extension), it will be loaded as well.
    /// </summary>
    /// <param name="fileName">The full path to the file containing the profile definition</param>
    /// <returns>A new Profile object populated with the values from the file</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified profile file does not exist.</exception>
    /// <exception cref="Exception">Thrown when the file does not contain valid profile data or when encoding parsing fails.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the profile file or image file is denied.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while reading the profile or image files.</exception>
    /// <exception cref="ArgumentException">Thrown when an invalid encoding name is specified in the profile.</exception>
    public static Profile FromFile(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException(fileName);

        Dictionary<string, string> kvPairs = new Dictionary<string, string>();
        Image profileIcon = null!;

        // Fill the dictionary with the key/value pairs from the file.
        // Invalid lines are automatically being skipped.
        foreach (string line in File.ReadLines(fileName))
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains("="))
                continue;

            string[] elements = line.Split('=', 2);

            string key = elements[0].Trim();
            string value = elements[1].Trim();

            kvPairs.Add(key, value);
        }

        if (kvPairs.Count == 0)
            throw new Exception(
                $"Given input file does not seem to contain {AssemblyHelper.GetApplicationName()} profile.");

        Profile loadedProfile = new Profile();

        foreach (var kvp in kvPairs)
        {
            switch (kvp.Key.ToLowerInvariant())
            {
                case "name":
                    loadedProfile.Name = kvp.Value;
                    break;
                case "filename":
                    loadedProfile.FileName = kvp.Value;
                    break;
                case "arguments":
                    loadedProfile.Arguments = kvp.Value;
                    break;
                case "workingdirectory":
                    loadedProfile.WorkingDirectory = kvp.Value;
                    break;
                case "autorestart":
                    loadedProfile.AutoRestart = kvp.Value == "true";
                    break;
                case "encoding":
                    loadedProfile.Encoding = Encoding.GetEncoding(kvp.Value);
                    break;
            }
        }

        if (File.Exists($"{fileName}.image"))
        {
            // Loading via Image.FromFile keeps the file locked!
            using(var memoryStream = new MemoryStream(File.ReadAllBytes($"{fileName}.image")))
            {
                profileIcon = Image.FromStream(memoryStream);
                loadedProfile.Image = profileIcon;
            }
        }

        return loadedProfile;
    }

    /// <summary>
    /// Deletes the log file for the current profile.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the log file is denied.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory containing the log file is not found.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while deleting the file.</exception>
    public void DeleteLogFile()
    {
        File.Delete(PathHelper.GetDataPath($"{Name}.log"));
    }

    /// <summary>
    /// Loads all profiles from the data path and returns them as a list.
    /// Profiles that fail to load are skipped and an error message is written to the console.
    /// </summary>
    /// <returns>A list of successfully loaded Profile objects from the data directory.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the data directory is denied.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the data directory is not found.</exception>
    public static List<Profile> GetProfilesInDataPath()
    {
        List<Profile> profileList = new List<Profile>();

        string[] files = Directory.GetFiles(PathHelper.GetDataPath(), "*.trp");

        foreach (string file in files)
        {
            try
            {
                profileList.Add(FromFile(file));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading profile from {file}: {ex.Message}");
            }
        }

        return profileList;
    }

    /// <summary>
    /// Saves the profile to a file in the data path.
    /// The profile data is saved as key-value pairs, and if an image is associated with the profile,
    /// it is saved as a separate PNG file with the same name plus ".image" extension.
    /// </summary>
    /// <exception cref="Exception">Thrown when the profile name is null, empty, or whitespace.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the data directory or files is denied.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the data directory is not found.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs while writing the profile or image files.</exception>
    /// <exception cref="ArgumentException">Thrown when the encoding name is invalid.</exception>
    public void SaveToFile()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new Exception("Profile name cannot be empty.");

        string fileName = PathHelper.GetDataPath($"{Name}.trp");

        // Do not use serialization here!
        // Manually writing/loading the properties of the profile allows
        // for both backwards compatibility and future extensions.
        using (StreamWriter writer = new StreamWriter(fileName, false))
        {
            writer.WriteLine($"Name={Name}");
            writer.WriteLine($"FileName={FileName}");
            writer.WriteLine($"Arguments={Arguments}");
            writer.WriteLine($"WorkingDirectory={WorkingDirectory}");
            writer.WriteLine($"AutoRestart={AutoRestart.ToString().ToLowerInvariant()}");
            writer.WriteLine($"Encoding={Encoding.WebName}"); // Same as EncodingInfo.Name!
        }

        if (Image != null)
        {
            string imagePath = PathHelper.GetDataPath($"{fileName}.image");

            if (File.Exists(imagePath))
                File.Delete(imagePath);

            Image.Save(imagePath, ImageFormat.Png);
        }
    }
}