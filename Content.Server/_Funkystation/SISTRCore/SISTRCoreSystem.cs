using System.Linq;
using Content.Server.Power.EntitySystems;
using Content.Shared._Funkystation.SISTRTerminal;
using Content.Shared.Radio;
using Content.Shared.Station.Components;

namespace Content.Server._Funkystation.SistrCore;

/// <summary>
/// tracks whether the physical sis/tr core is alive and powered
/// </summary>
public sealed partial class SistrCoreSystem : EntitySystem
{
    [Dependency] private PowerReceiverSystem _power = null!;

    private bool GridHasFunctionalCore(EntityUid grid)
    {
        var query = EntityQueryEnumerator<SistrCoreComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var xform))
        {
            if (xform.GridUid != grid)
                continue;

            if (!_power.IsPowered(uid))
                continue;

            return true;
        }

        return false;
    }

    // this is some bullshit LOL
    public bool StationHasFunctionalCore(EntityUid station)
    {
        return TryComp<StationDataComponent>(station, out var data) && data.Grids.Any(GridHasFunctionalCore);
    }

    [SubscribeLocalEvent]
    private void OnReceiveRadio(Entity<SistrCoreComponent> ent, ref RadioReceiveEvent args)
    {
        if (ent.Owner == args.RadioSource)
            return;

        if (!TryComp<SistrTerminalComponent>(ent, out var terminal))
            return;

        // todo: we'll be smarter about this later
        terminal.RadioMessages.Add($"{Name(args.MessageSource)}: {args.Message}");
        Dirty<SistrTerminalComponent>((ent.Owner, terminal));
    }
}
