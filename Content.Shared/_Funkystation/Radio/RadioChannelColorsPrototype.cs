using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Funkystation.Radio;

[Prototype]
public sealed partial class RadioChannelColorsPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = null!;

    [DataField]
    public LocId Name { get; private set; } = string.Empty;

    [DataField(required: true)]
    public required Dictionary<ProtoId<RadioChannelPrototype>, Color> Colors;
}
