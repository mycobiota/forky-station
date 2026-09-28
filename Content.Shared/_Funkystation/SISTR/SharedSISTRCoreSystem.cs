using Content.Shared._Funkystation.SistrCore;
using Content.Shared._Funkystation.SISTRTerminal;
using Content.Shared.Radio;

namespace Content.Shared._Funkystation.SISTR;

public sealed partial class SharedSistrCoreSystem : EntitySystem
{
    [Dependency] private SharedUserInterfaceSystem _ui = null!;

    [SubscribeLocalEvent]
    private void OnReceiveRadio(Entity<SistrCoreComponent> ent, ref RadioReceiveEvent args)
    {
        if (ent.Owner == args.RadioSource)
            return;

        if (ent.Comp.RadioMessages.Count >= ent.Comp.RadioMessages.Capacity)
            ent.Comp.RadioMessages.Dequeue();

        ent.Comp.RadioMessages.Enqueue($"{Name(args.MessageSource)}: {args.Message}");
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    public void OnAfterAutoHandleStateEvent(Entity<SistrCoreComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        if (_ui.TryGetOpenUi(ent.Owner, SistrAutomaTalkKey.Key, out var bui))
        {
            bui.Update();
        }
    }
}
