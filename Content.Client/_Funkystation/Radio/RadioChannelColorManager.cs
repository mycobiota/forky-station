using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared._Funkystation.CCVar;
using Content.Shared._Funkystation.Radio;
using Content.Shared.Radio;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Client._Funkystation.Radio;

public sealed partial class RadioChannelColorManager : IPostInjectInit
{
    [Dependency] private IPrototypeManager _prototypeManager = null!;
    [Dependency] private IConfigurationManager _cfg = null!;
    [Dependency] private ILogManager _logManager = null!;

    private readonly CVarDef<string> _colorPresetCvar = RadioChannelColorCvar.ChannelColorPreset;

    private ISawmill _sawmill = null!;

    public bool TryGetRadioChannelColor(ProtoId<RadioChannelPrototype> channel, [NotNullWhen(true)] out Color? color)
    {
        var colorPresetId = _cfg.GetCVar(_colorPresetCvar);
        if (!_prototypeManager.TryIndex<RadioChannelColorsPrototype>(colorPresetId, out var channelColors))
        {
            _sawmill.Warning("No such radio channel color preset \"{colorPresetId}\" exists.", colorPresetId);
        }
        else if (channelColors.Colors.TryGetValue(channel, out var channelColor))
        {
            color = channelColor;
            return true;
        }

        if (_prototypeManager.TryIndex(channel, out var channelProto))
        {
            color = channelProto.Color;
            return true;
        }

        _sawmill.Warning("Tried to get the color of an unknown channel \"{channel}\".", channel);
        color = null;
        return false;
    }

    public void PostInject()
    {
        _sawmill = _logManager.GetSawmill("radio_channel_color_manager");
    }
}
