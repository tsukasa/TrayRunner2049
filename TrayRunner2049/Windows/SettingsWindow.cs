using TrayRunner2049.Components;
using TrayRunner2049.Extensions;
using TrayRunner2049.Helpers;

namespace TrayRunner2049.Windows;

/// <summary>
/// Settings window for managing profiles in TrayRunner2049.
/// </summary>
public partial class SettingsWindow : Form
{
    public SettingsWindow()
    {
        InitializeComponent();
        DarkModeCS dm = new DarkModeCS(this);
        Reinit();
    }

    /// <summary>
    /// List of all profiles read from the data directory.
    /// </summary>
    private List<Profile> _profileList = new List<Profile>();
    
    /// <summary>
    /// Currently selected profile.
    /// </summary>
    private Profile? _currentProfile;
    
    /// <summary>
    /// Getter for the currently selected profile.
    /// </summary>
    public Profile? CurrentProfile => _currentProfile;

    private void PopulateEncodingComboBox()
    {
        cmbOutputEncoding.Items.Clear();
        foreach (System.Text.EncodingInfo enc in System.Text.Encoding.GetEncodings().OrderBy(e => e.Name))
        {
            cmbOutputEncoding.Items.Add(enc.Name);
        }

        cmbOutputEncoding.Text = System.Text.Encoding.UTF8.WebName;
    }
    
    /// <summary>
    /// Loads the list of profiles from the data directory and populates the combo box.
    /// </summary>
    private void LoadProfileList()
    {
        cmbProfile.Text = string.Empty;
        cmbProfile.Items.Clear();
        
        _currentProfile = null;

        _profileList = Profile.GetProfilesInDataPath();
        
        foreach (Profile profile in _profileList)
        {
            if(!string.IsNullOrWhiteSpace(profile.Name))
                cmbProfile.Items.Add(profile.Name);
        }
    }
    
    /// <summary>
    /// Method to set the image for the icon button.
    /// Performs automatic resizing to ensure proper display size.
    /// </summary>
    /// <param name="fileName">Full path to the selected image file</param>
    private void SetImage(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            return;

        try
        {
            // Try to load the image directly. If that fails,
            // we probably deal with something other than an image.
            // In that case, we will try to extract the icon from the file.
            Image newImage;
            try
            {
                newImage = Image.FromFile(fileName);
                newImage = newImage.Resize(16, 16);
            }
            catch (OutOfMemoryException)
            {
                Icon icon = Icon.ExtractAssociatedIcon(fileName)!;
                newImage = icon.ToBitmap();
                newImage = newImage.Resize(16, 16);
            }
            btnIcon.BackgroundImage = newImage;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    /// <summary>
    /// Populates the settings window's controls with the specified profile data.
    /// </summary>
    /// <param name="profile">Profile to load or null to clear the control data</param>
    public void LoadProfile(Profile? profile = null)
    {
        cmbProfile.Text = profile?.Name;
        txtFileName.Text = profile?.FileName;
        txtArguments.Text = profile?.Arguments;
        txtWorkingDirectory.Text = profile?.WorkingDirectory;
        cmbOutputEncoding.Text = profile?.Encoding.WebName;
        chkAutoRestart.Checked = profile is { AutoRestart: true } || false;
        btnIcon.BackgroundImage = profile?.Image;

        if (profile != null)
            _currentProfile = profile;
    }

    /// <summary>
    /// Helper function to reinitialize the settings window.
    /// Gets called from different places.
    /// </summary>
    private void Reinit()
    {
        LoadProfileList();
        PopulateEncodingComboBox();
        LoadProfile();
    }

    private void cmbProfile_SelectedIndexChanged(object sender, EventArgs e)
    {
        Profile? switchToProfile = _profileList.FirstOrDefault(i => i.Name == cmbProfile.Text);
        LoadProfile(switchToProfile);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        // Save the current profile settings to a file
        if (_currentProfile == null)
            _currentProfile = new Profile();
        
        _currentProfile.Name = cmbProfile.Text;
        _currentProfile.FileName = txtFileName.Text;
        _currentProfile.Arguments = txtArguments.Text;
        _currentProfile.WorkingDirectory = txtWorkingDirectory.Text;
        _currentProfile.Encoding = System.Text.Encoding.GetEncoding(cmbOutputEncoding.Text);
        _currentProfile.AutoRestart = chkAutoRestart.Checked;
        _currentProfile.Image = btnIcon.BackgroundImage;

        if (!string.IsNullOrWhiteSpace(_currentProfile.Name))
            _currentProfile.SaveToFile();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnIcon_Click(object sender, EventArgs e)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image Files|*.bmp;*.gif;*.exe;*.ico;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files (*.*)|*.*";
        openFileDialog.Title = "Select Icon File";
        
        if (openFileDialog.ShowDialog() == DialogResult.OK)
            SetImage(openFileDialog.FileName);
        
        Directory.SetCurrentDirectory(currentDirectory);
    }

    private void btnIcon_KeyUp(object sender, KeyEventArgs e)
    {
        // If the key pressed is not the Delete key, do nothing
        if (e.KeyValue != 46)
            return;

        btnIcon.BackgroundImage = null;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        string profileName = PathHelper.GetDataPath($"{cmbProfile.Text}.trp");

        if (File.Exists(profileName))
        {
            File.Delete(profileName);
            File.Delete($"{profileName}.image");
        }

        Reinit();
    }
}