<Window x:Class="DarkProcessManager.MainWindow"
        xmlns="http://microsoft.com"
        xmlns:x="http://microsoft.com"
        Title="Estonia VPN &amp; DPI Bypass"
        Width="980" Height="720" MinWidth="780" MinHeight="560"
        WindowStartupLocation="CenterScreen"
        Background="#121212" Foreground="#F2F2F2"
        Closing="MainWindow_Closing" StateChanged="MainWindow_StateChanged">

    <Window.Resources>
        <SolidColorBrush x:Key="CardBrush" Color="#1E1E1E"/>
        <SolidColorBrush x:Key="AccentBrush" Color="#00C853"/>
        <SolidColorBrush x:Key="MutedBrush" Color="#A0A0A0"/>

        <Style TargetType="TextBlock">
            <Setter Property="Foreground" Value="#F2F2F2"/>
            <Setter Property="VerticalAlignment" Value="Center"/>
        </Style>
        <Style TargetType="TextBox">
            <Setter Property="Foreground" Value="#F2F2F2"/>
            <Setter Property="Background" Value="#292929"/>
            <Setter Property="BorderBrush" Value="#404040"/>
            <Setter Property="Padding" Value="8,6"/>
            <Setter Property="CaretBrush" Value="#00C853"/>
            <Setter Property="Margin" Value="0,4,0,10"/>
        </Style>
        <Style TargetType="ComboBox">
            <Setter Property="Foreground" Value="#121212"/>
            <Setter Property="Background" Value="#E0E0E0"/>
            <Setter Property="Padding" Value="7"/>
            <Setter Property="Margin" Value="0,4,0,10"/>
        </Style>
        <Style TargetType="Button">
            <Setter Property="Foreground" Value="#F2F2F2"/>
            <Setter Property="Background" Value="#303030"/>
            <Setter Property="BorderBrush" Value="#484848"/>
            <Setter Property="Padding" Value="12,8"/>
            <Setter Property="Margin" Value="4"/>
            <Setter Property="Cursor" Value="Hand"/>
        </Style>
        <Style TargetType="TabItem">
            <Setter Property="Foreground" Value="#121212"/>
            <Setter Property="Padding" Value="15,8"/>
        </Style>
        <Style TargetType="ListBox">
            <Setter Property="Foreground" Value="#F2F2F2"/>
            <Setter Property="Background" Value="#171717"/>
            <Setter Property="BorderBrush" Value="#353535"/>
        </Style>
        <Style TargetType="CheckBox">
            <Setter Property="Foreground" Value="#F2F2F2"/>
            <Setter Property="Margin" Value="0,4,0,10"/>
        </Style>
    </Window.Resources>

    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <DockPanel Grid.Row="0" Margin="0,0,0,14">
            <StackPanel>
                <TextBlock Text="Estonia Shield" FontSize="26" FontWeight="SemiBold"/>
                <TextBlock Text="Hybrid DPI Bypass + Estonia Location Router" Foreground="{StaticResource MutedBrush}" Margin="0,4,0,0"/>
            </StackPanel>
            <TextBlock x:Name="DataLocationText" DockPanel.Dock="Right" Foreground="{StaticResource MutedBrush}" TextWrapping="Wrap" MaxWidth="380" HorizontalAlignment="Right"/>
        </DockPanel>

        <TabControl Grid.Row="1" Background="#171717" BorderBrush="#353535">
            <!-- Вкладка 1: Главная -->
            <TabItem Header="Dashboard">
                <ScrollViewer VerticalScrollBarVisibility="Auto">
                    <StackPanel Margin="20">
                        <Border Background="{StaticResource CardBrush}" CornerRadius="12" Padding="24" Margin="0,0,0,14">
                            <StackPanel HorizontalAlignment="Center">
                                <TextBlock x:Name="StatusText" Text="Disconnected" FontSize="18" FontWeight="SemiBold" HorizontalAlignment="Center"/>
                                <TextBlock x:Name="SessionTimerText" Text="00:00:00" FontSize="36" FontWeight="Light" Margin="0,10,0,16" HorizontalAlignment="Center"/>
                                <Button x:Name="StartStopButton" Content="Connect" Width="200" Height="56" FontSize="20" FontWeight="SemiBold" Background="{StaticResource AccentBrush}" Foreground="#101010" BorderThickness="0" Click="StartStopButton_Click"/>
                            </StackPanel>
                        </Border>

                        <Border Background="{StaticResource CardBrush}" CornerRadius="12" Padding="18">
                            <StackPanel>
                                <TextBlock Text="Connection Mode" FontSize="16" FontWeight="SemiBold"/>
                                <ComboBox x:Name="ModeComboBox" SelectedIndex="0">
                                    <ComboBoxItem Content="Hybrid: Estonia Proxy + DPI Bypass (Recommended)" Tag="Both"/>
                                    <ComboBoxItem Content="Estonia Proxy Only (All blocked apps &amp; geo-blocks)" Tag="App2Only"/>
                                    <ComboBoxItem Content="DPI Bypass Only (No IP change)" Tag="App1Only"/>
                                </ComboBox>

                                <CheckBox x:Name="SystemProxyCheckBox" Content="Automatically set Windows System Proxy (127.0.0.1:10809) when connected" IsChecked="True"/>

                                <TextBlock Text="DPI Bypass Arguments (bin\app1.exe = winws.exe)" FontSize="14" FontWeight="SemiBold" Margin="0,8,0,0"/>
                                <TextBox x:Name="App1ArgumentsTextBox"/>

                                <TextBlock Text="Proxy Core Arguments (bin\app2.exe = xray.exe)" FontSize="14" FontWeight="SemiBold"/>
                                <TextBox x:Name="App2ArgumentsTextBox"/>

                                <Button Content="Save Settings" HorizontalAlignment="Left" Click="SaveSettingsButton_Click"/>
                            </StackPanel>
                        </Border>
                    </StackPanel>
                </ScrollViewer>
            </TabItem>

            <!-- Вкладка 2: Эстония и DNS -->
            <TabItem Header="Estonia &amp; DNS">
                <ScrollViewer VerticalScrollBarVisibility="Auto">
                    <StackPanel Margin="20">
                        <Border Background="{StaticResource CardBrush}" CornerRadius="12" Padding="18" Margin="0,0,0,14">
                            <StackPanel>
                                <TextBlock Text="Free Estonia Subscription URL (GitHub Raw)" FontSize="16" FontWeight="SemiBold"/>
                                <TextBlock Text="Automatically fetches, filters EE/Estonia nodes, pings them and picks the fastest one on Connect." Foreground="{StaticResource MutedBrush}" Margin="0,2,0,6"/>
                                <TextBox x:Name="SubscriptionUrlTextBox"/>

                                <TextBlock Text="OR Paste Manual VLESS Link (Optional - overrides auto-fetch)" FontSize="15" FontWeight="SemiBold" Margin="0,6,0,0"/>
                                <TextBox x:Name="CustomVlessTextBox" ToolTip="vless://..."/>
                            </StackPanel>
                        </Border>

                        <Border Background="{StaticResource CardBrush}" CornerRadius="12" Padding="18">
                            <StackPanel>
                                <TextBlock Text="Custom DNS Server" FontSize="16" FontWeight="SemiBold"/>
                                <TextBlock Text="Examples: 1.1.1.1 (Cloudflare), 8.8.8.8 (Google), 9.9.9.9 (Quad9)" Foreground="{StaticResource MutedBrush}" Margin="0,2,0,6"/>
                                <TextBox x:Name="DnsTextBox"/>
                                <Button Content="Save Network Settings" HorizontalAlignment="Left" Click="SaveSettingsButton_Click"/>
                            </StackPanel>
                        </Border>
                    </StackPanel>
                </ScrollViewer>
            </TabItem>

            <!-- Вкладка 3: Редактор list-general.txt -->
            <TabItem Header="Domains (list-general.txt)">
                <Grid Margin="18">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                        <RowDefinition Height="Auto"/>
                    </Grid.RowDefinitions>

                    <StackPanel Grid.Row="0" Margin="0,0,0,8">
                        <TextBlock x:Name="ListFilePathText" Foreground="{StaticResource MutedBrush}" TextWrapping="Wrap"/>
                        <CheckBox x:Name="SplitTunnelCheckBox" Content="Route ONLY domains in list-general.txt through Estonia (uncheck to route ALL traffic)" IsChecked="False" Margin="0,6,0,0"/>
                    </StackPanel>

                    <DockPanel Grid.Row="1" Margin="0,0,0,8">
                        <Button Content="Add Domain" DockPanel.Dock="Right" Click="AddListLineButton_Click"/>
                        <TextBox x:Name="ListSearchTextBox" Margin="0,4,8,8" ToolTip="Type domain to search or add (e.g. discord.com)" TextChanged="ListSearchTextBox_TextChanged"/>
                    </DockPanel>

                    <ListBox x:Name="DomainListBox" Grid.Row="2" SelectionMode="Extended"/>

                    <DockPanel Grid.Row="3" Margin="0,10,0,0">
                        <TextBlock x:Name="ListCountText" Foreground="{StaticResource MutedBrush}" VerticalAlignment="Center"/>
                        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" DockPanel.Dock="Right">
                            <Button Content="Remove Selected" Click="RemoveListLinesButton_Click"/>
                            <Button Content="Save Domain List" Background="{StaticResource AccentBrush}" Foreground="#101010" FontWeight="SemiBold" Click="SaveListButton_Click"/>
                        </StackPanel>
                    </DockPanel>
                </Grid>
            </TabItem>
Use code with caution.

---

### 8. `MainWindow.xaml.cs`
Создай этот файл в корне репозитория и вставь код целиком:

```csharp
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace DarkProcessManager;

public partial class MainWindow : Window
{
    private readonly ProcessManager _processManager = new();
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
    private readonly ObservableCollection<string> _domains = new();
    private ICollectionView? _domainView;
    private DateTime _connectedAt;
    private bool _isConnected;

    private readonly string _dataDir;
    private readonly string _settingsPath;
    private readonly string _listPath;

    private System.Windows.Forms.NotifyIcon? _trayIcon;
    private bool _isExiting;

    [DllImport("wininet.dll")]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

    public MainWindow()
    {
        InitializeComponent();

        _dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EstoniaShield");
        Directory.CreateDirectory(_dataDir);
        _settingsPath = Path.Combine(_dataDir, "settings.json");
        _listPath = Path.Combine(_dataDir, "list-general.txt");

        DataLocationText.Text = $"Config folder:\n{_dataDir}";
        ListFilePathText.Text = $"File: {_listPath}";

        _domainView = CollectionViewSource.GetDefaultView(_domains);
        _domainView.Filter = item => string.IsNullOrWhiteSpace(ListSearchTextBox.Text) ||
                                     (item?.ToString()?.Contains(ListSearchTextBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) ?? false);
        DomainListBox.ItemsSource = _domainView;

        _processManager.LogReceived += AppendLog;
        _processManager.ProcessesStopped += () => Dispatcher.Invoke(OnDisconnectedUI);
        _timer.Tick += (_, _) => SessionTimerText.Text = (DateTime.Now - _connectedAt).ToString(@"hh\:mm\:ss");

        AppDomain.CurrentDomain.ProcessExit += (_, _) => CleanupOnExit();

        InitTrayIcon();
        LoadSettings();
        LoadDomainList();
    }

    private void InitTrayIcon()
    {
        _trayIcon = new System.Windows.Forms.NotifyIcon
        {
            Icon = System.Drawing.SystemIcons.Shield,
            Visible = true,
            Text = "Estonia Shield"
        };
        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("Open Window", null, (_, _) => ShowFromTray());
        menu.Items.Add("Exit", null, (_, _) => { _isExiting = true; Close(); });
        _trayIcon.ContextMenuStrip = menu;
        _trayIcon.DoubleClick += (_, _) => ShowFromTray();
    }

    private void ShowFromTray()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private async void StartStopButton_Click(object sender, RoutedEventArgs e)
    {
        if (_isConnected)
        {
            Disconnect();
            return;
        }

        SaveSettings();
        SaveDomainList();

        StartStopButton.IsEnabled = false;
        StatusText.Text = "Connecting to Estonia...";
        StatusText.Foreground = Brushes.Gold;

        try
        {
            var mode = (ModeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Both";
            var binDir = Path.Combine(AppContext.BaseDirectory, "bin");
            Directory.CreateDirectory(binDir);

            if (mode is "Both" or "App2Only")
            {
                var configPath = Path.Combine(binDir, "config.json");
                var activeListPath = SplitTunnelCheckBox.IsChecked == true ? _listPath : "";
                await XrayHelper.PrepareFastestEstoniaConfigAsync(
                    SubscriptionUrlTextBox.Text,
                    CustomVlessTextBox.Text,
                    DnsTextBox.Text,
                    activeListPath,
                    configPath,
                    AppendLog
                );
            }

            var app1Args = App1ArgumentsTextBox.Text.Replace("{LIST}", $"\"{_listPath}\"");
            var app2Args = App2ArgumentsTextBox.Text;

            _processManager.Start(mode, app1Args, app2Args);

            if ((mode is "Both" or "App2Only") && SystemProxyCheckBox.IsChecked == true)
            {
                SetWindowsSystemProxy(true, "127.0.0.1:10809");
            }

            _isConnected = true;
            _connectedAt = DateTime.Now;
            _timer.Start();

            StatusText.Text = "Connected (Estonia)";
            StatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 83));
            StartStopButton.Content = "Disconnect";
            StartStopButton.Background = new SolidColorBrush(Color.FromRgb(229, 57, 53));
        }
        catch (Exception ex)
        {
            AppendLog($"[ERROR] {ex.Message}");
            Disconnect();
        }
        finally
        {
            StartStopButton.IsEnabled = true;
        }
    }

    private void Disconnect()
    {
        _processManager.Stop();
        SetWindowsSystemProxy(false, "");
        OnDisconnectedUI();
    }

    private void OnDisconnectedUI()
    {
        _isConnected = false;
        _timer.Stop();
        SessionTimerText.Text = "00:00:00";
        StatusText.Text = "Disconnected";
        StatusText.Foreground = Brushes.White;
        StartStopButton.Content = "Connect";
        StartStopButton.Background = new SolidColorBrush(Color.FromRgb(0, 200, 83));
    }

    private static void SetWindowsSystemProxy(bool enable, string proxyServer)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (key == null) return;
            key.SetValue("ProxyEnable", enable ? 1 : 0, RegistryValueKind.DWord);
            if (enable) key.SetValue("ProxyServer", proxyServer, RegistryValueKind.String);
            InternetSetOption(IntPtr.Zero, 39, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, 37, IntPtr.Zero, 0);
        }
        catch { }
    }

    private void LoadSettings()
    {
        if (File.Exists(_settingsPath))
        {
            try
            {
                var s = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_settingsPath));
                if (s != null)
                {
                    ModeComboBox.SelectedIndex = s.ModeIndex;
                    SystemProxyCheckBox.IsChecked = s.UseSystemProxy;
                    SplitTunnelCheckBox.IsChecked = s.SplitTunnelOnly;
                    App1ArgumentsTextBox.Text = s.App1Args;
                    App2ArgumentsTextBox.Text = s.App2Args;
                    SubscriptionUrlTextBox.Text = s.SubscriptionUrl;
                    CustomVlessTextBox.Text = s.CustomVless;
                    DnsTextBox.Text = s.Dns;
                    return;
                }
            }
            catch { }
        }

        // Значения по умолчанию при первом запуске
        App1ArgumentsTextBox.Text = "--wf-tcp=80,443 --wf-udp=443,50000-50100 --filter-tcp=443 --hostlist={LIST} --dpi-desync=fake,disorder2 --dpi-desync-autottl=2 --dpi-desync-fooling=md5sig";
        App2ArgumentsTextBox.Text = "run -c config.json";
        SubscriptionUrlTextBox.Text = "https://raw.githubusercontent.com/Au1rxx/free-vpn-subscriptions/main/output/v2ray-base64.txt";
        DnsTextBox.Text = "1.1.1.1";
    }

    private void SaveSettings()
    {
        var s = new AppSettings
        {
            ModeIndex = ModeComboBox.SelectedIndex,
            UseSystemProxy = SystemProxyCheckBox.IsChecked == true,
            SplitTunnelOnly = SplitTunnelCheckBox.IsChecked == true,
            App1Args = App1ArgumentsTextBox.Text,
            App2Args = App2ArgumentsTextBox.Text,
            SubscriptionUrl = SubscriptionUrlTextBox.Text,
            CustomVless = CustomVlessTextBox.Text,
            Dns = DnsTextBox.Text
        };
        File.WriteAllText(_settingsPath, JsonSerializer.Serialize(s, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void LoadDomainList()
    {
        _domains.Clear();
        if (!File.Exists(_listPath))
        {
            File.WriteAllLines(_listPath, new[] { "discord.com", "discordapp.com", "discord.gg", "youtube.com", "googlevideo.com", "chatgpt.com", "openai.com" });
        }
        foreach (var line in File.ReadAllLines(_listPath).Where(l => !string.IsNullOrWhiteSpace(l)))
            _domains.Add(line.Trim());
        UpdateListCount();
    }

    private void SaveDomainList()
    {
        File.WriteAllLines(_listPath, _domains);
        UpdateListCount();
    }

    private void UpdateListCount() => ListCountText.Text = $"Total domains: {_domains.Count}";

    private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        SaveSettings();
        AppendLog("[System] Settings saved.");
    }

    private void ListSearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => _domainView?.Refresh();

    private void AddListLineButton_Click(object sender, RoutedEventArgs e)
    {
        var text = ListSearchTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(text) && !_domains.Contains(text))
        {
            _domains.Add(text);
            ListSearchTextBox.Clear();
            UpdateListCount();
        }
    }

    private void RemoveListLinesButton_Click(object sender, RoutedEventArgs e)
    {
        foreach (var item in DomainListBox.SelectedItems.Cast<string>().ToList())
            _domains.Remove(item);
        UpdateListCount();
    }

    private void SaveListButton_Click(object sender, RoutedEventArgs e)
    {
        SaveDomainList();
AppendLog("[System] Domain list (list-general.txt) saved.");
}
private void ClearLogsButton_Click(object sender, RoutedEventArgs e) => ConsoleTextBox.Clear();
private void AppendLog(string message)
{
Dispatcher.Invoke(() =>
{
ConsoleTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
ConsoleTextBox.ScrollToEnd();
});
}
private void MainWindow_StateChanged(object? sender, EventArgs e)
{
if (WindowState == WindowState.Minimized) Hide();
}
private void MainWindow_Closing(object? sender, CancelEventArgs e)
{
if (!_isExiting)
{
e.Cancel = true;
Hide();
return;
}
CleanupOnExit();
}
private void CleanupOnExit()
{
_processManager.Dispose();
SetWindowsSystemProxy(false, "");
if (_trayIcon != null)
{
_trayIcon.Visible = false;
_trayIcon.Dispose();
}
}
private class AppSettings
{
public int ModeIndex { get; set; }
public bool UseSystemProxy { get; set; } = true;
public bool SplitTunnelOnly { get; set; }
public string App1Args { get; set; } = "";
public string App2Args { get; set; } = "";
public string SubscriptionUrl { get; set; } = "";
public string CustomVless { get; set; } = "";
public string Dns { get; set; } = "1.1.1.1";
}
}
