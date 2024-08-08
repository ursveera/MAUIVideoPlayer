using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Import necessary Windows API functions
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("shell32.dll", SetLastError = true)]
    static extern bool Shell_NotifyIcon(uint dwMessage, ref NotifyIconData lpData);

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_APPWINDOW = 0x00040000;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    private const uint NIM_ADD = 0x00000000;
    private const uint NIM_DELETE = 0x00000002;
    private const uint NIM_MODIFY = 0x00000001;
    private const uint NIF_ICON = 0x00000002;
    private const uint NIF_MESSAGE = 0x00000001;
    private const uint NIF_TIP = 0x00000004;

    private static FileSystemWatcher fileSystemWatcher;
    private static NotifyIconData notifyIconData;

    [StructLayout(LayoutKind.Sequential)]
    private struct NotifyIconData
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
    }

    static void Main(string[] args)
    {
        // Hide the console window from the taskbar
        IntPtr consoleWindow = GetConsoleWindow();
        int style = GetWindowLong(consoleWindow, GWL_EXSTYLE);
        SetWindowLong(consoleWindow, GWL_EXSTYLE, (style & ~WS_EX_APPWINDOW) | WS_EX_TOOLWINDOW);
        ShowWindow(consoleWindow, SW_HIDE);

        // Set up the NotifyIconData
        notifyIconData.cbSize = (uint)Marshal.SizeOf(typeof(NotifyIconData));
        notifyIconData.hWnd = consoleWindow;
        notifyIconData.uID = 1;
        notifyIconData.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
        notifyIconData.uCallbackMessage = 0x8000; // WM_USER + 1
        notifyIconData.hIcon = SystemIcons.Application.Handle;
        notifyIconData.szTip = "Console App Running";

        // Add icon to system tray
        Shell_NotifyIcon(NIM_ADD, ref notifyIconData);

        // Set up the FileSystemWatcher
        string pathToWatch = @"C:\Users\Ursve\Downloads\Tamanna Glb\OneDrive_1_8-8-2024\";
        string filter = "Veera.txt";

        fileSystemWatcher = new FileSystemWatcher
        {
            Path = pathToWatch,
            Filter = filter,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
        };

        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.Created += OnCreated;
        fileSystemWatcher.Deleted += OnDeleted;
        fileSystemWatcher.Renamed += OnRenamed;

        fileSystemWatcher.EnableRaisingEvents = true;

        // Start the background task
        Task.Run(() => BackgroundProcessLogicMethod());

        // Keep the application running
        Console.WriteLine("Press 'q' to quit the application.");
        while (Console.Read() != 'q') ;

        // Remove icon from system tray before exiting
        Shell_NotifyIcon(NIM_DELETE, ref notifyIconData);
    }

    private static void BackgroundProcessLogicMethod()
    {
        try
        {
            // Simulate a long-running background task
            Thread.Sleep(20000);
            Console.WriteLine("I was doing some work in the background.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"File Changed: {e.FullPath}");
    }

    private static void OnCreated(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"File Created: {e.FullPath}");
    }

    private static void OnDeleted(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"File Deleted: {e.FullPath}");
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
        Console.WriteLine($"File Renamed from {e.OldFullPath} to {e.FullPath}");
    }
}
