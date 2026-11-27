using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace DarkProcessManager;

public static class XrayHelper
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(15) };

    public static async Task<string> PrepareFastestEstoniaConfigAsync(
        string subscriptionUrl,
        string customVlessUri,
        string dnsServer,
        string listGeneralPath,
        string outputConfigPath,
        Action<string> log)
    {
        string? selectedLink = null;

        if (!string.IsNullOrWhiteSpace(customVlessUri) &&
            customVlessUri.Trim().StartsWith("vless://", StringComparison.OrdinalIgnoreCase))
        {
            selectedLink = customVlessUri.Trim();
            log("[Estonia-Proxy] Using manually provided VLESS link.");
        }
        else
        {
            log("[Estonia-Proxy] Fetching free subscription list...");
            var raw = await Client.GetStringAsync(subscriptionUrl.Trim());
            var text = TryDecodeBase64(raw);

            var estoniaLinks = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(l => l.StartsWith("vless://", StringComparison.OrdinalIgnoreCase) &&
                           (l.Contains("EE", StringComparison.OrdinalIgnoreCase) ||
                            l.Contains("Estonia", StringComparison.OrdinalIgnoreCase) ||
                            l.Contains("Tallinn", StringComparison.OrdinalIgnoreCase) ||
                            l.Contains("🇪🇪")))
                .ToList();

            if (estoniaLinks.Count == 0)
                throw new Exception("No Estonia (EE) VLESS nodes found in subscription.");

            log($"[Estonia-Proxy] Found {estoniaLinks.Count} Estonia nodes. Testing ping...");
            long bestPing = long.MaxValue;

            foreach (var link in estoniaLinks.Take(20))
            {
                if (!Uri.TryCreate(link, UriKind.Absolute, out var uri)) continue;
                var ping = await MeasureTcpPingAsync(uri.Host, uri.Port);
                if (ping >= 0 && ping < bestPing)
                {
                    bestPing = ping;
                    selectedLink = link;
                }
            }

            if (selectedLink == null)
                throw new Exception("All tested Estonia nodes timed out.");

            log($"[Estonia-Proxy] Fastest Estonia node selected ({bestPing} ms).");
        }

        BuildXrayConfig(selectedLink, dnsServer, listGeneralPath, outputConfigPath);
        return selectedLink;
    }

    private static string TryDecodeBase64(string input)
    {
        var compact = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray())
            .Replace('-', '+').Replace('_', '/');
        var mod = compact.Length % 4;
        if (mod > 0) compact = compact.PadRight(compact.Length + (4 - mod), '=');
        try { return Encoding.UTF8.GetString(Convert.FromBase64String(compact)); }
        catch { return input; }
    }

    private static async Task<long> MeasureTcpPingAsync(string host, int port)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1500));
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(host, port, cts.Token);
            return sw.ElapsedMilliseconds;
        }
        catch { return -1; }
    }

    private static void BuildXrayConfig(string vlessUri, string dnsServer, string listPath, string outPath)
    {
        var uri = new Uri(vlessUri);
        var q = HttpUtility.ParseQueryString(uri.Query);
        var domains = File.Exists(listPath)
            ? File.ReadAllLines(listPath).Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#"))
                  .Select(l => $"domain:{l.Trim()}").ToArray()
            : Array.Empty<string>();

        var config = new
        {
            log = new { loglevel = "warning" },
            dns = new { servers = new[] { string.IsNullOrWhiteSpace(dnsServer) ? "1.1.1.1" : dnsServer } },
            inbounds = new object[]
            {
                new { tag = "socks", port = 10808, listen = "127.0.0.1", protocol = "socks", settings = new { udp = true } },
                new { tag = "http", port = 10809, listen = "127.0.0.1", protocol = "http" }
            },
            outbounds = new object[]
            {
                new
                {
                    tag = "proxy",
                    protocol = "vless",
                    settings = new
                    {
                        vnext = new[] { new { address = uri.Host, port = uri.Port, users = new[] { new { id = uri.UserInfo, encryption = "none", flow = q["flow"] ?? "" } } } }
                    },
                    streamSettings = new
                    {
                        network = q["type"] ?? "tcp",
                        security = q["security"] ?? "none",
                        realitySettings = q["security"] == "reality" ? new { show = false, fingerprint = q["fp"] ?? "chrome", serverName = q["sni"] ?? uri.Host, publicKey = q["pbk"] ?? "", shortId = q["sid"] ?? "" } : null,
                        tlsSettings = q["security"] == "tls" ? new { serverName = q["sni"] ?? uri.Host, fingerprint = q["fp"] ?? "chrome" } : null
                    }
                },
                new { tag = "direct", protocol = "freedom" }
            },
            routing = new
            {
                domainStrategy = "IPIfNonMatch",
                rules = domains.Length > 0
                    ? new object[] { new { type = "field", domain = domains, outboundTag = "proxy" }, new { type = "field", network = "tcp,udp", outboundTag = "direct" } }
                    : new object[] { new { type = "field", network = "tcp,udp", outboundTag = "proxy" } }
            }
        };

        Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
        File.WriteAllText(outPath, JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }));
    }
}
