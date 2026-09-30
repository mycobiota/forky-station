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
    [Dependency] private SharedChatSystem _chat = null!;

    private static readonly ProtoId<RadioChannelPrototype> BinaryChannel = "Binary";

    // todo: loc string for this
    private const string StartMessage = "AutomaTalk ver.0.09.1\nType \"/exit\" to exit.";

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

    public override void Update()
    {
        base.Update();
        RefreshChatLog();
    }

    protected override void Open()
    {
        base.Open();
        RefreshChatLog();
        _automaTalk?.FocusInput();
        _automaTalk?.AddLine(StartMessage);
    }

    private void RefreshChatLog()
    {
        if (EntMan.TryGetComponent<SistrCoreComponent>(Owner, out var sistrCoreComponent))
        {
            _automaTalk?.Update(sistrCoreComponent.RadioMessages);
        }
    }
}
