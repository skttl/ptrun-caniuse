using Community.PowerToys.Run.Plugin.CanIUse.Models;
using Community.PowerToys.Run.Plugin.CanIUse.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wox.Infrastructure;

namespace Community.PowerToys.Run.Plugin.CanIUse.Repositories;

public class DataRepository
{
    private static string absPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    private static string dataFn = Path.Combine(absPath, "data.json") ?? string.Empty;

    public static void WriteFile(string filename, string data)
    {
        File.WriteAllText(filename, data);
    }

    public static string ReadFile(string filename)
    {
        return File.ReadAllText(filename);
    }

    public IEnumerable<DataMatch>? GetData(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return null;
        }

        if (!File.Exists(dataFn) || (File.GetLastWriteTime(dataFn).AddDays(CanIUseSettings.Instance.DataRefreshInterval) < DateTime.Now))
        {
            DownloadData().GetAwaiter().GetResult();
        }

        var data = JsonSerializer.Deserialize<CanIUseData>(ReadFile(dataFn));

        if (data?.Data == null) {
            return null;
        }

        var ordered = data.Data
            .Select(x => new DataMatch(x, search))
            .Where(x => x.MatchResult.IsSearchPrecisionScoreMet())
            .OrderByDescending(x => x.MatchResult.RawScore);

        return ordered;
    }

    public async Task DownloadData()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/Fyrd/caniuse/master/data.json");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<CanIUseData>(jsonData);
                WriteFile(dataFn, JsonSerializer.Serialize(data));
            }
        }
    }
}
