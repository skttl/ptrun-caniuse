using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

public class CanIUseData
{
    [JsonPropertyName("data")]
    public Dictionary<string, Feature> Data { get; set; } = new Dictionary<string, Feature>();
}
