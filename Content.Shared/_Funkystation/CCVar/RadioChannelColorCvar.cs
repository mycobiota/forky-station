using Robust.Shared.Configuration;

namespace Content.Shared._Funkystation.CCVar;

[CVarDefs]
public sealed class RadioChannelColorCvar
{
    /// <summary>
    /// Specifies the ProtoId of the radio channel color preset/theme the player is using.
    /// </summary>
    public static readonly CVarDef<string> ChannelColorPreset =
        CVarDef.Create("funkystation.radio.channel_color_preset", "FunkyChannelColors", CVar.CLIENTONLY | CVar.ARCHIVE);
}
