using Content.Shared._Funkystation.SistrCore;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client._Funkystation.SISTRTerminal.Programs;

[UsedImplicitly]
public sealed partial class SistrAutomaTalkBoundUserInterface(EntityUid owner, Enum uiKey) : SistrProgramBui(owner, uiKey)
{
    private SistrAutomaTalk? _automaTalk;

    protected override void Open()
    {
        base.Open();
        _automaTalk = this.CreateDisposableControl<SistrAutomaTalk>();
        _automaTalk.ExitProgram += OnExit;
    }

    public override SistrProgramControl? CreateSistrProgram()
    {
        Open();
        return _automaTalk;
    }

    private void OnExit()
    {
        _automaTalk?.Orphan();
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
