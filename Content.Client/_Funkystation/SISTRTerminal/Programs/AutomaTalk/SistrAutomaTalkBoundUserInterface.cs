using Content.Shared._Funkystation.SistrCore;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.SISTRTerminal.Programs.AutomaTalk;

[UsedImplicitly]
public sealed partial class SistrAutomaTalkBoundUserInterface(EntityUid owner, Enum uiKey) : SistrProgramBui(owner, uiKey)
{
    private SistrAutomaTalk? _automaTalk;

    public override void CreateControl(out SistrProgramControl control)
    {
        _automaTalk = this.CreateDisposableControl<SistrAutomaTalk>();
        _automaTalk.ExitProgram += OnExit;
        control = _automaTalk;
    }

    private void OnExit()
    {
        ExitProgram?.Invoke(this);
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
