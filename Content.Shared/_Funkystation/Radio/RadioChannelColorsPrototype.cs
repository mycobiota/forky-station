using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Funkystation.Radio;

[Prototype]
public sealed partial class RadioChannelColorsPrototype : IPrototype, IComparable<RadioChannelColorsPrototype>
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = null!;

    [DataField]
    public LocId Name { get; private set; } = string.Empty;

    [DataField(required: true)]
    public required Dictionary<ProtoId<RadioChannelPrototype>, Color> Colors;

    /// <summary>
    /// An order for the themes to be displayed in the UI
    /// </summary>
    [DataField]
    public int Order = 0;

    public int CompareTo(RadioChannelColorsPrototype? other)
    {
        return Order.CompareTo(other?.Order);
    }
}
