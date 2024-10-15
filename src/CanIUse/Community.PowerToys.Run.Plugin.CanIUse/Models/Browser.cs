using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.CanIUse.Models;

[JsonConverter(typeof(JsonStringEnumConverter<Browser>))]
public enum Browser
{
    ie,
    edge,
    firefox,
    chrome,
    safari,
    opera,
    ios_saf,
    op_mini,
    android,
    bb,
    op_mob,
    and_chr,
    and_ff,
    ie_mob,
    and_uc,
    samsung,
    and_qq,
    baidu,
    kaios
}
