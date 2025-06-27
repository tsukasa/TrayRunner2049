using System.Diagnostics.CodeAnalysis;
using TrayRunner2049.Helpers;
using TrayRunner2049.Windows;

namespace TrayRunner2049;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string?[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        // Use with .net 10 and later... In .net 9 this causes
        // multiple issues:
        // - Dark mode looks bad at 96dpi
        // - Windows Forms Orchestration causes issues when trying to start multiple instances
        // Application.SetColorMode(SystemColorMode.System);

        // Exception logging
        Application.ThreadException += new ThreadExceptionEventHandler(MainWindow_UIThreadException);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        // Register additional encoding providers for .net 5 and later
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Check if a profile name is provided as an argument
        string profileName = null!;
        if (args.Length != 0)
            profileName = args[0]!;

        // Start the main window with the specified profile name
        MainWindow appWindow = new MainWindow(profileName);
        Application.Run(appWindow);
    }

    /// <summary>
    /// Exception handler for UI thread exceptions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal static void MainWindow_UIThreadException(object sender, ThreadExceptionEventArgs e)
    {
        HandleException(e.Exception);
    }

    /// <summary>
    /// Exception handler for unhandled exceptions in the application domain.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        HandleException(e.ExceptionObject as Exception);
    }

    /// <summary>
    /// Logic to handle exceptions by logging them to a file and showing a message box.
    /// </summary>
    /// <param name="ex"></param>
    internal static void HandleException(Exception? ex)
    {
        string str = $"{ex!.GetType().FullName}: {ex.Message}{Environment.NewLine}{ex.StackTrace}";
        File.AppendAllText(PathHelper.GetDataPath("TrayRunner2049.log"), str);
        MessageBox.Show(
            str,
            $@"{AssemblyHelper.GetApplicationName()} - {ex.GetType().FullName}",
            MessageBoxButtons.OK,
            MessageBoxIcon.Hand
        );
    }
}