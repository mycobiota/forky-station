using Content.Shared.Radio;
using Robust.Shared.Serialization;

namespace Content.Shared._Funkystation.SISTRTerminal;

public sealed partial class SharedSistrTerminalSystem : EntitySystem
{
    [SubscribeLocalEvent]
    private void OnReceiveRadio(Entity<SistrTerminalComponent> ent, ref RadioReceiveEvent args)
    {
        if (ent.Owner == args.RadioSource)
            return;

        ent.Comp.Messages.Add($"{Name(args.MessageSource)}: {args.Message}");
    }
}

[Serializable, NetSerializable]
public enum SistrTerminalUiKey
{
    Key,
}
