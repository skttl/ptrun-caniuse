using Community.PowerToys.Run.Plugin.CanIUse.Helpers;
using Community.PowerToys.Run.Plugin.CanIUse.Models;
using Microsoft.PowerToys.Settings.UI.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Community.PowerToys.Run.Plugin.CanIUse.Settings;

internal sealed class CanIUseSettings
{
    private readonly bool _initialized;
    private static CanIUseSettings? instance;
    private static Browser[] FallbackDefaultBrowsers = { Browser.chrome, Browser.edge, Browser.firefox, Browser.safari, Browser.ios_saf };
    private static double FallbackDataRefreshInterval = 14;
    private static double FallbackMaxResults = 5;
    internal Browser[] DefaultBrowsers { get; set; } = FallbackDefaultBrowsers;
    internal double DataRefreshInterval { get; set; } = FallbackDataRefreshInterval;
    internal double MaxResults { get; set; } = FallbackMaxResults;

    private CanIUseSettings()
    {
        // Init class properties with default values
        UpdateSettings(null);
        _initialized = true;
    }

    internal static CanIUseSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CanIUseSettings();
            }

            return instance;
        }
    }
    internal static List<PluginAdditionalOption> GetAdditionalOptions()
    {
        
        var optionList = new List<PluginAdditionalOption>()
        {
            new PluginAdditionalOption()
            {
                Key = nameof(DataRefreshInterval),
                DisplayLabel = "Refresh caniuse.com data if older than",
                DisplayDescription = $"Determine the number of days the cache the data from caniuse.com for.",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Numberbox,
                NumberBoxMin = 0,
                NumberValue = FallbackDataRefreshInterval
            },
            new PluginAdditionalOption()
            {
                Key = nameof(MaxResults),
                DisplayLabel = "Maximum number of results",
                DisplayDescription = $"Determine the maximum number of results to show when searching.",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Numberbox,
                NumberBoxMin = 1,
                NumberValue = FallbackMaxResults
            }
        };

        foreach (var browserValue in Enum.GetValues(typeof(Browser)))
        {
            var browser = (Browser)browserValue;
            optionList.Add(new PluginAdditionalOption()
            {
                Key = $"{nameof(DefaultBrowsers)}_{browser.ToString()}",
                DisplayLabel = $"Show {browser.ToFriendlyName()} support in search results",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                TextValue = browser.ToString(),
                Value = FallbackDefaultBrowsers.Contains(browser)
            });
        }

        return optionList;
    }
    internal void UpdateSettings(PowerLauncherPluginSettings? settings)
    {
        if ((settings is null || settings.AdditionalOptions is null) & _initialized)
        {
            return;
        }

        DefaultBrowsers = settings?.AdditionalOptions?.Where(x => x.Key.StartsWith(nameof(DefaultBrowsers)) && x.Value).Select(x => (Browser)Enum.Parse(typeof(Browser), x.TextValue)).ToArray() 
            ?? FallbackDefaultBrowsers;

        DataRefreshInterval = (settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == nameof(DataRefreshInterval))?.NumberValue) ?? FallbackDataRefreshInterval;

        MaxResults = (settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == nameof(MaxResults))?.NumberValue) ?? FallbackMaxResults;
    }
}
