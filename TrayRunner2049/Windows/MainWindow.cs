using System.Diagnostics;
using System.Text;
using TrayRunner2049.Components;
using TrayRunner2049.Extensions;
using TrayRunner2049.Helpers;

namespace TrayRunner2049.Windows;

public partial class MainWindow : Form
{
    private bool _isStopButtonPressed;
    private Profile? _loadedProfile;
    private Process? _runningProcess;
    private Thread? _stdErrThread;
    private Thread? _stdOutThread;

    /// <summary>
    /// Initializes a new instance of the MainWindow class with default settings.
    /// Sets up dark mode, window/tray text, and notification icon.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        DarkModeCS.ExcludeFromProcessing(txtOutput);
        DarkModeCS.ExcludeFromProcessing(txtConsoleInput);
        DarkModeCS dm = new DarkModeCS(this);
        SetWindowAndTrayText();
        notifyIcon.Icon = new Icon(Icon!, 16, 16);
    }

    /// <summary>
    /// Initializes a new instance of the MainWindow class with an optional profile to load.
    /// If a valid profile name is provided, loads the profile and starts in minimized state.
    /// </summary>
    /// <param name="profileName">Optional name of the profile to load (without .trp extension). If null, opens with no profile loaded.</param>
    /// <exception cref="FileNotFoundException">Thrown when the specified profile file does not exist.</exception>
    /// <exception cref="Exception">Thrown when the profile file is invalid or corrupted.</exception>
    public MainWindow(string? profileName = null) : this()
    {
        if (profileName == null)
            return;

        profileName = PathHelper.GetDataPath($"{profileName}.trp");

        if (!File.Exists(profileName))
            return;

        LoadProfile(Profile.FromFile(profileName));
        WindowState = FormWindowState.Minimized;
    }

    /// <summary>
    /// Enables or disables UI controls based on whether a process is currently running.
    /// This method manages the state of input controls, buttons, and other UI elements to prevent
    /// invalid operations and provide appropriate user feedback.
    /// </summary>
    /// <param name="isRunning">True if a process is currently running, false otherwise</param>
    private void EnableControls(bool isRunning)
    {
        if (Disposing)
            return;

        txtConsoleInput.Enabled = isRunning;
        btnEnter.Enabled = isRunning;
        btnSettings.Enabled = !isRunning;
        btnStartProcess.Enabled = !isRunning && _loadedProfile != null && File.Exists(_loadedProfile.FileName);
        btnStopProcess.Enabled = isRunning;
        btnClear.Enabled = isRunning;
    }

    /// <summary>
    /// Event handler for when the running process exits.
    /// Cleans up process threads, handles auto-restart functionality, and updates UI controls.
    /// </summary>
    /// <param name="sender">The process that exited</param>
    /// <param name="e">Event arguments containing exit information</param>
    private void Process_Exited(object? sender, EventArgs e)
    {
        _runningProcess = null!;
        _stdErrThread?.Join();
        _stdOutThread?.Join();

        if (_loadedProfile!.AutoRestart && !_isStopButtonPressed)
            StartProcess();
        else
            try
            {
                Invoke((Action)(() => EnableControls(false)));
            }
            catch (ObjectDisposedException)
            {
                // Form is disposed, safe to ignore
            }
    }

    /// <summary>
    /// Reads output from a process stream (stdout or stderr) and updates the UI.
    /// This method runs in a separate thread to continuously read process output,
    /// logs it to a file, and displays it in the output text box with ANSI formatting support.
    /// </summary>
    /// <param name="reader">The StreamReader connected to the process output stream</param>
    /// <exception cref="ArgumentNullException">Thrown when reader is null</exception>
    /// <exception cref="IOException">Thrown when an error occurs reading from the stream</exception>
    /// <exception cref="InvalidOperationException">Thrown when the UI thread invoke fails</exception>
    private void Process_ReadProcessStream(StreamReader reader)
    {
        var buffer = new char[1024];
        int length;
        while ((length = reader.Read(buffer, 0, 1024)) > 0)
        {
            var output = new string(buffer, 0, length);
            File.AppendAllText(PathHelper.GetDataPath($"{_loadedProfile?.Name}.log"), output);
            txtOutput.Invoke((Action)(() => txtOutput.AppendAnsiText(output)));
        }
    }

    /// <summary>
    /// Loads a new profile into the main window and updates the UI accordingly.
    /// Sets the window title, clears output, updates icons, and configures controls based on the new profile.
    /// </summary>
    /// <param name="newProfile">The profile to load and apply to the window</param>
    /// <exception cref="ArgumentNullException">Thrown when newProfile is null</exception>
    /// <exception cref="OutOfMemoryException">Thrown when there's insufficient memory to create icons</exception>
    private void LoadProfile(Profile newProfile)
    {
        _loadedProfile = newProfile;

        SetWindowAndTrayText(_loadedProfile.Name);

        txtProfileName.Text = _loadedProfile.Name;
        txtOutput.Clear();
        txtConsoleInput.Text = "";

        if (_loadedProfile.Image == null)
        {
            Icon = new Icon(Icon!, 16, 16);
            notifyIcon.Icon = new Icon(Icon, 16, 16);
        }
        else
        {
            Icon = _loadedProfile.Image.ToIcon(16);
            notifyIcon.Icon = _loadedProfile.Image.ToIcon(16);
        }

        EnableControls(false);
    }

    /// <summary>
    /// Sets the text for the main window and the system tray icon.
    /// Combines the optional text with the application name to create a consistent title format.
    /// Do not set the text manually, use this method instead.
    /// </summary>
    /// <param name="text">Optional text to prepend to the application name. If null or empty, only shows the application name.</param>
    private void SetWindowAndTrayText(string? text = null)
    {
        var fullText = AssemblyHelper.GetApplicationName();

        if (!string.IsNullOrWhiteSpace(text))
            fullText = $"{text} - {fullText}";

        Text = fullText;
        notifyIcon.Text = fullText;
    }

    /// <summary>
    /// Starts a new process based on the currently loaded profile configuration.
    /// Creates separate threads for reading stdout and stderr, configures process settings,
    /// and handles error display if the process fails to start.
    /// </summary>
    /// <exception cref="Win32Exception">Thrown when the executable file cannot be found or started</exception>
    /// <exception cref="InvalidOperationException">Thrown when no profile is loaded or the profile is invalid</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when access to the executable is denied</exception>
    /// <exception cref="FileNotFoundException">Thrown when the executable file does not exist</exception>
    private void StartProcess()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = _loadedProfile?.FileName,
            Arguments = _loadedProfile?.Arguments,
            WorkingDirectory = _loadedProfile?.WorkingDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            StandardOutputEncoding = _loadedProfile?.Encoding,
            StandardErrorEncoding = _loadedProfile?.Encoding
        };

        if (!File.Exists(processStartInfo.FileName))
        {
            var path = Path.Combine(processStartInfo.WorkingDirectory!, processStartInfo.FileName!);
            if (File.Exists(path))
                processStartInfo.FileName = path;
        }

        try
        {
            var process = Process.Start(processStartInfo)!;
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            (_stdErrThread = new Thread(() => Process_ReadProcessStream(process.StandardError))).Start();
            (_stdOutThread = new Thread(() => Process_ReadProcessStream(process.StandardOutput)))
                .Start();
            _runningProcess = process;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                string.Format(
                    "An error occurred when attempting to start the process. Check the settings,{0}the path of the executable file, and appropriate permissions and try again...{0}{0}{1}",
                    Environment.NewLine, ex.Message), "Error: " + ex.GetType().FullName,
                MessageBoxButtons.OK, MessageBoxIcon.Hand);
            EnableControls(false);
        }
    }

    /// <summary>
    /// Forcibly terminates the currently running process if one exists.
    /// This method kills the process immediately without allowing graceful shutdown.
    /// </summary>
    /// <exception cref="Win32Exception">Thrown when the process cannot be terminated</exception>
    /// <exception cref="InvalidOperationException">Thrown when the process has already exited</exception>
    private void StopProcess()
    {
        _runningProcess?.Kill();
    }

    /// <summary>
    /// Event handler for the Enter button click.
    /// Sends the current input text to the running process and displays it in the output window.
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">Event arguments for the click event</param>
    /// <exception cref="InvalidOperationException">Thrown when no process is running or the process input stream is closed</exception>
    /// <exception cref="IOException">Thrown when writing to the process input stream fails</exception>
    private void btnEnter_Click(object sender, EventArgs e)
    {
        var inputText = txtConsoleInput.Text;
        txtOutput.AppendAnsiText(inputText + Environment.NewLine);
        _runningProcess?.StandardInput.WriteLine(inputText);
        _runningProcess?.StandardInput.Flush();
    }

    /// <summary>
    /// Event handler for the Start Process button click.
    /// Deletes the existing log file, starts the process, and updates the UI state.
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">Event arguments for the click event</param>
    private void btnStartProcess_Click(object sender, EventArgs e)
    {
        _loadedProfile?.DeleteLogFile();

        StartProcess();

        _isStopButtonPressed = false;
        EnableControls(true);
    }

    /// <summary>
    /// Event handler for the Stop Process button click.
    /// Stops the currently running process and sets the stop button pressed flag.
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">Event arguments for the click event</param>
    private void btnStopProcess_Click(object sender, EventArgs e)
    {
        StopProcess();
        _isStopButtonPressed = true;
    }

    /// <summary>
    /// Event handler for the main window closing event.
    /// Ensures any running process is stopped before the window closes.
    /// </summary>
    /// <param name="sender">The form that is closing</param>
    /// <param name="e">Event arguments for the form closing event</param>
    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_runningProcess == null)
            return;

        btnStopProcess.PerformClick();
    }

    /// <summary>
    /// Event handler for the main window load event.
    /// Initializes controls, sets focus, and auto-starts if a profile is loaded.
    /// </summary>
    /// <param name="sender">The form that loaded</param>
    /// <param name="e">Event arguments for the load event</param>
    private void MainWindow_Load(object sender, EventArgs e)
    {
        EnableControls(false);
        btnSettings.Focus();

        if (_loadedProfile == null)
            return;

        btnStartProcess.PerformClick();
        Hide();
    }

    /// <summary>
    /// Event handler for the main window resize event.
    /// Hides the window when it's minimized to show only in the system tray.
    /// </summary>
    /// <param name="sender">The form that was resized</param>
    /// <param name="e">Event arguments for the resize event</param>
    private void MainWindow_Resize(object sender, EventArgs e)
    {
        if (WindowState != FormWindowState.Minimized)
            return;

        Hide();
    }

    /// <summary>
    /// Event handler for the notification icon double-click event.
    /// Restores the window from the system tray and brings it to the foreground.
    /// </summary>
    /// <param name="sender">The notification icon that was double-clicked</param>
    /// <param name="e">Event arguments for the double-click event</param>
    private void notifyIcon_DoubleClick(object sender, EventArgs e)
    {
        TopMost = true;
        Show();
        WindowState = FormWindowState.Normal;
        TopMost = false;
    }

    /// <summary>
    /// Event handler for the Settings button click.
    /// Opens the settings window and loads any new profile that is saved.
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">Event arguments for the click event</param>
    private void btnSettings_Click(object sender, EventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow();
        settingsWindow.LoadProfile(_loadedProfile);

        if (settingsWindow.ShowDialog() != DialogResult.OK)
            return;

        Profile? newProfile = settingsWindow.CurrentProfile;
        LoadProfile(newProfile!);
    }

    /// <summary>
    /// Event handler for key down events in the console input text box.
    /// Handles the Enter key to submit input and clear the text box.
    /// </summary>
    /// <param name="sender">The text box that received the key press</param>
    /// <param name="e">Event arguments containing key information</param>
    private void txtConsoleInput_KeyDown(object sender, KeyEventArgs e)
    {
        // Enter key
        if(e.KeyValue == 13)
        {
            e.SuppressKeyPress = true;
            btnEnter.PerformClick();
            txtConsoleInput.Clear();
        }
    }

    /// <summary>
    /// Event handler for the Clear button click.
    /// Clears all text from the output window.
    /// </summary>
    /// <param name="sender">The button that was clicked</param>
    /// <param name="e">Event arguments for the click event</param>
    private void btnClear_Click(object sender, EventArgs e)
    {
        txtOutput.Clear();
    }

    /// <summary>
    /// Event handler for link clicks in the output text box.
    /// Opens clicked links in the default web browser or application.
    /// </summary>
    /// <param name="sender">The text box containing the clicked link</param>
    /// <param name="e">Event arguments containing the link text</param>
    /// <exception cref="Win32Exception">Thrown when the system cannot find the application to open the link</exception>
    /// <exception cref="FileNotFoundException">Thrown when the default application for the link type is not found</exception>
    private void txtOutput_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.LinkText!)
                { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open link: {e.LinkText}\n{ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}