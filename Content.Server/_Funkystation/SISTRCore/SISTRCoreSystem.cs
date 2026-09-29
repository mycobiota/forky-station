using System.Globalization;
using System.Linq;
using Content.Server.Chat.Systems;
using Content.Server.Power.EntitySystems;
using Content.Server.Radio.EntitySystems;
using Content.Shared._Funkystation.SISTR;
using Content.Shared._Funkystation.SistrCore;
using Content.Shared.Station.Components;

namespace Content.Server._Funkystation.SistrCore;

/// <summary>
/// tracks whether the physical sis/tr core is alive and powered
/// </summary>
public sealed partial class SistrCoreSystem : EntitySystem
{
    [Dependency] private PowerReceiverSystem _power = null!;
    [Dependency] private RadioSystem _radio = null!;
    [Dependency] private ChatSystem _chat = null!;

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
    public void OnAutomaTalkChatMessage(Entity<SistrCoreComponent> ent, ref AutomaTalkChatMessage args)
    {
        bool shouldCapitalizeTheWordI = (!CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Parent.Name == "en")
                                        || (CultureInfo.CurrentCulture.IsNeutralCulture && CultureInfo.CurrentCulture.Name == "en");

        var sanitizedMessage = _chat.SanitizeMessageCapital(
            _chat.SanitizeMessageReplaceWords(args.Message.Trim()));

        if (shouldCapitalizeTheWordI)
            sanitizedMessage = _chat.SanitizeMessageCapitalizeTheWordI(sanitizedMessage, "i");

        _radio.SendRadioMessage(args.Actor, sanitizedMessage, args.Channel, ent);
    }
}
