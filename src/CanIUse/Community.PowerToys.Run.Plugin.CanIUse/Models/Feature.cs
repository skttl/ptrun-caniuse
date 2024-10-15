using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

public class Feature
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("stats")]
    public BrowserStatDictionary Stats { get; set; } = new BrowserStatDictionary();

    [JsonPropertyName("usage_perc_y")]
    public decimal UsagePercent { get; set; }

    [JsonPropertyName("usage_perc_a")]
    public decimal UsagePercentPartial { get; set; }
}
