using Community.PowerToys.Run.Plugin.CanIUse.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

public class BrowserStatDictionary : Dictionary<Browser, BrowserVersionStatDictionary>
{
    public string ToString(Browser[]? browsers = null)
    {
        browsers = browsers ?? new[] { Browser.chrome, Browser.firefox, Browser.safari, Browser.edge };

        var stats = this.Where(x => browsers.Contains(x.Key));

        return string.Join(", ", stats.Select(x => $"{x.Key.ToFriendlyName()} {x.Value.GetSupport()}"));
    }
}
