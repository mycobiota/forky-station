using Robust.Shared.Configuration;

namespace Content.Shared._Funkystation.CCVar;

[CVarDefs]
public sealed class RadioChannelColorCvar
{
    public static readonly CVarDef<string> ChannelColorPreset =
        CVarDef.Create("funkystation.radio.channel_color_preset", "FunkyChannelColors", CVar.CLIENTONLY | CVar.ARCHIVE);
}
