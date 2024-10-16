using Community.PowerToys.Run.Plugin.CanIUse.Models;
using Community.PowerToys.Run.Plugin.CanIUse.Repositories;
using Community.PowerToys.Run.Plugin.CanIUse.Settings;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Wox.Infrastructure;
using Wox.Plugin;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.CanIUse
{
    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IPlugin, ISettingProvider, IDisposable
    {
        private readonly DataRepository _dataRepository = new DataRepository();

        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "5C1A49E431F74D66BC62FD214D834609";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "CanIUse";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Lookup browser support for different web features";

        private PluginInitContext? Context { get; set; }

        private bool Disposed { get; set; }

        public IEnumerable<PluginAdditionalOption> AdditionalOptions => CanIUseSettings.GetAdditionalOptions();

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search))
            {
                return new List<Result>()
                {
                    new Result
                    {
                        QueryTextDisplay = string.Empty,
                        Title = "Search caniuse.com",
                        SubTitle = Description,
                        IcoPath = $"Images\\caniuse.png"
                    }
                };
            }

            var results = new List<Result>();

            var data = _dataRepository.GetData(query.Search.ToLower().Trim())?.Take((int)CanIUseSettings.Instance.MaxResults);

            if (data is not null)
            {
                results.AddRange(data.Select(CreateResult));
            }

            if (results.Count == 0)
            {
                results.Add(new Result
                {
                    Title = "No results",
                    SubTitle = $"No results found on caniuse.com for \"{query.Search}\"",
                    IcoPath = $"Images\\caniuse.png"
                });
            }

            return results;
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }
        private string IconPath { get; set; } = "Images/caniuse.light.png";

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? "Images/caniuse.light.png" : "Images/caniuse.dark.png";

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);


        public Result CreateResult(DataMatch item)
        {
            return new Result
            {
                QueryTextDisplay = string.Empty,
                Title = $"{item.Feature.Title} " +
                $"{item.Feature.UsagePercent}%" +
                $"{(item.Feature.UsagePercentPartial != 0 ? $" + {item.Feature.UsagePercentPartial}% = {(item.Feature.UsagePercent + item.Feature.UsagePercentPartial)}%" : "")}",
                SubTitle = $"{item.Feature.Stats.ToString(CanIUseSettings.Instance.DefaultBrowsers)}{Environment.NewLine}{item.Feature.Description}",
                Action = action => Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, $"https://caniuse.com/#feat={item.Key}")
            };
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            CanIUseSettings.Instance.UpdateSettings(settings);
        }
    }
}
