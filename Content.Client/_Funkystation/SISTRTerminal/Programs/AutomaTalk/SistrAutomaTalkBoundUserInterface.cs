using Content.Shared._Funkystation.SISTR;
using Content.Shared._Funkystation.SistrCore.Components;
using Content.Shared.Chat;
using Content.Shared.Radio;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._Funkystation.SISTRTerminal.Programs.AutomaTalk;

[UsedImplicitly]
public sealed partial class SistrAutomaTalkBoundUserInterface(EntityUid owner, Enum uiKey) : SistrProgramBui(owner, uiKey)
{
    private static readonly ProtoId<RadioChannelPrototype> BinaryChannel = "Binary";

    private static readonly string StartMessage =
        Loc.GetString("automatalk-start-message",
            ("capacity", SharedSistrCoreSystem.AutomaTalkHistoryLength));

    private SistrAutomaTalk? _automaTalk;

    public override void CreateControl(out SistrProgramControl control)
    {
        _automaTalk = this.CreateDisposableControl<SistrAutomaTalk>();
        _automaTalk.ExitProgram += OnExit;
        _automaTalk.SendMessage += OnSendMessage;
        control = _automaTalk;
    }

    private void OnSendMessage(string message)
    {
        SendMessage(new AutomaTalkChatMessage(message, BinaryChannel));
    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
        if (message is not AutomaTalkChatMessage automaTalkMsg)
            return;

        _automaTalk?.UpdateOnMessageReceived(automaTalkMsg.Message);
    }

    protected override void Open()
    {
        base.Open();
        RefreshChatLog();
        _automaTalk?.FocusInput();
        _automaTalk?.AddText(StartMessage);
    }

    private void RefreshChatLog()
    {
        if (EntMan.TryGetComponent<SistrCoreComponent>(Owner, out var sistrCoreComponent))
        {
            _automaTalk?.RefreshChatLog(sistrCoreComponent.RadioMessages);
        }
    }
}
