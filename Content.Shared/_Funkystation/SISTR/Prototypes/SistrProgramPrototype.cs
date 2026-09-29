using Robust.Shared.Prototypes;

namespace Content.Shared._Funkystation.SISTR.Prototypes;

[Prototype]
public sealed partial class SistrProgramPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = null!;

    [DataField]
    public string Name { get; private set; } = string.Empty;

    [DataField]
    public Enum Key { get; private set; }

    [DataField("description")]
    public LocId LocalizedDescription { get; private set; } = string.Empty;
}
