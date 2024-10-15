using System.Collections.Generic;
using Wox.Infrastructure;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

public class DataMatch
{
    public DataMatch(KeyValuePair<string, Feature> item, string search)
    {
        Key = item.Key;
        Feature = item.Value;
        MatchResult = StringMatcher.FuzzySearch(search, $"{item.Key} {item.Value.Title} {item.Value.Description}");
    }
    public string Key { get; set; }
    public Feature Feature { get; set; }
    public MatchResult MatchResult { get; set; }

}
