using Community.PowerToys.Run.Plugin.CanIUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Helpers;

public static class BrowserExtensions
{
    public static Dictionary<Browser, string> BrowserNames = new Dictionary<Browser, string>
    {
        { Browser.android, "Android Browser" },
        { Browser.and_chr, "Chrome for Android" },
        { Browser.and_ff, "Firefox for Android" },
        { Browser.and_qq, "QQ Browser" },
        { Browser.and_uc, "UC Browser for Android" },
        { Browser.baidu, "Baidu" },
        { Browser.bb, "BB Browser" },
        { Browser.chrome, "Chrome" },
        { Browser.edge, "Edge" },
        { Browser.firefox, "Firefox" },
        { Browser.ie, "IE" },
        { Browser.ie_mob, "IE Mobile" },
        { Browser.ios_saf, "iOS Safari" },
        { Browser.kaios, "KaiOS Browser" },
        { Browser.op_mini, "Opera Mini" },
        { Browser.op_mob, "Opera Mobile" },
        { Browser.opera, "Opera" },
        { Browser.safari, "Safari" },
        { Browser.samsung, "Samsung Internet" }
    };
    public static string ToFriendlyName(this Browser browser)
    {
        return BrowserNames.TryGetValue(browser, out var s) ? s : browser.ToString();
    }
}
