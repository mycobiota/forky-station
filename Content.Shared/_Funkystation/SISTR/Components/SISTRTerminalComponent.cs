using Content.Shared._Funkystation.SISTR.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared._Funkystation.SISTRTerminal.Components;

[RegisterComponent]
public sealed partial class SistrTerminalComponent : Component
{
    [DataField, AlwaysPushInheritance]
    public List<ProtoId<SistrProgramPrototype>> Programs;
}
