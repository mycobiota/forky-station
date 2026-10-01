using Content.Shared._Funkystation.SistrCore.Components;
using Content.Shared._Funkystation.SISTRTerminal;
using Content.Shared.Radio;

namespace Content.Shared._Funkystation.SISTR;

public sealed partial class SharedSistrCoreSystem : EntitySystem
{
    [Dependency] private SharedUserInterfaceSystem _ui = null!;

    public const int AutomaTalkHistoryLength = 15;

    [SubscribeLocalEvent]
    private void OnReceiveRadio(Entity<SistrCoreComponent> ent, ref RadioReceiveEvent args)
    {
        var formatted = Loc.GetString("automatalk-wrap-message",
            ("name", Name(args.MessageSource)),
            ("message", args.Message));

        while (ent.Comp.RadioMessages.Count >= AutomaTalkHistoryLength)
        {
            ent.Comp.RadioMessages.Dequeue();
        }

        ent.Comp.RadioMessages.Enqueue(formatted);
        Dirty(ent);

        _ui.ServerSendUiMessage(ent.Owner, SistrAutomaTalkKey.Key, new AutomaTalkChatMessage(formatted, args.Channel.ID));
    }
}
