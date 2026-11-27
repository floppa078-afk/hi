using System.Diagnostics;
using System.IO;

namespace DarkProcessManager;

public class ProcessManager : IDisposable
{
    private Process? _app1Process;
    private Process? _app2Process;

    public event Action<string>? LogReceived;
    public event Action? ProcessesStopped;

    public bool IsRunning =>
        (_app1Process is { HasExited: false }) ||
        (_app2Process is { HasExited: false });

    public void Start(string mode, string app1Args, string app2Args)
    {
        Stop();
        var binDir = Path.Combine(AppContext.BaseDirectory, "bin");
        Directory.CreateDirectory(binDir);

        if (mode is "Both" or "App1Only")
            _app1Process = StartSingleProcess(Path.Combine(binDir, "app1.exe"), app1Args, "DPI-Core");

        if (mode is "Both" or "App2Only")
            _app2Process = StartSingleProcess(Path.Combine(binDir, "app2.exe"), app2Args, "Estonia-Proxy");
    }

    private Process? StartSingleProcess(string exePath, string arguments, string tag)
    {
        if (!File.Exists(exePath))
        {
            LogReceived?.Invoke($"[{tag}] File not found: {exePath}");
            return null;
        }

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments,
            WorkingDirectory = Path.GetDirectoryName(exePath)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        process.OutputDataReceived += (_, e) => { if (!string.IsNullOrEmpty(e.Data)) LogReceived?.Invoke($"[{tag}] {e.Data}"); };
        process.ErrorDataReceived += (_, e) => { if (!string.IsNullOrEmpty(e.Data)) LogReceived?.Invoke($"[{tag} ERR] {e.Data}"); };
        process.Exited += (_, _) => { if (!IsRunning) ProcessesStopped?.Invoke(); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        LogReceived?.Invoke($"[{tag}] Started (PID {process.Id})");
        return process;
    }

    public void Stop()
    {
        KillProcess(ref _app1Process, "DPI-Core");
        KillProcess(ref _app2Process, "Estonia-Proxy");
    }

    private void KillProcess(ref Process? process, string tag)
    {
        if (process == null) return;
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                process.WaitForExit(1500);
                LogReceived?.Invoke($"[{tag}] Stopped.");
            }
        }
        catch { }
        finally
        {
            process.Dispose();
            process = null;
        }
    }

    public void Dispose() => Stop();
}
