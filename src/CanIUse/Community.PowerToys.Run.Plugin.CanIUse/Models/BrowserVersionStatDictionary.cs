using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

public class BrowserVersionStatDictionary : Dictionary<string, string>
{
    private string GetSupportedRange(string token)
    {

        var supported = this.Where(x => x.Value.StartsWith(token)).ToList();
        if (supported.Count > 1)
        {
            return $"{supported.First().Key}-{supported.Last().Key}{(supported.Last().Key.Equals(this.Last().Key) ? "+" : "")}";
        }
        else if (supported.Count > 0)
        {
            return $"{supported.Last().Key}{(supported.Last().Key.Equals(this.Last().Key) ? "+" : "")}";
        }

        return "N/A";
    }

    public string GetSupportedRange() => GetSupportedRange("y");
    public string GetPartiallySupportedRange() => GetSupportedRange("p");

    public string GetSupport()
    {
        var supported = GetSupportedRange();
        var partiallySupported = GetPartiallySupportedRange();
        return $"{supported}{(partiallySupported != "N/A" ? $" ({partiallySupported}*)" : "")}";
    }
}
