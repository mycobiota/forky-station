using Content.Shared._Funkystation.SistrCore;
using Content.Shared.Radio;
using Content.Shared.Radio.EntitySystems;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client._Funkystation.SISTRTerminal.Programs.AutomaTalk;

[UsedImplicitly]
public sealed partial class SistrAutomaTalkBoundUserInterface(EntityUid owner, Enum uiKey) : SistrProgramBui(owner, uiKey)
{
    [Dependency] private SharedRadioSystem _radioSystem = null!;

    private static readonly ProtoId<RadioChannelPrototype> BinaryChannel = "Binary";

    public override string Name => "automaTalk";

    private SistrAutomaTalk? _automaTalk;

    public override void CreateControl(out SistrProgramControl control)
    {
        _automaTalk = this.CreateDisposableControl<SistrAutomaTalk>();
        _automaTalk.ExitProgram += OnExit;
        _automaTalk.SendMessage += OnSendMessage;
        control = _automaTalk;
    }

    private void OnExit()
    {
        ExitProgram?.Invoke(this);
    }

    private void OnSendMessage(string message)
    {
        var author = PlayerManager.LocalEntity ?? Owner;

        _radioSystem.SendRadioMessage(author, message, BinaryChannel, Owner);
    }

    public override void Update()
    {
        base.Update();

        if (EntMan.TryGetComponent<SistrCoreComponent>(Owner, out var sistrCoreComponent))
        {
            _automaTalk?.Update(sistrCoreComponent.RadioMessages);
        }
    }
}
