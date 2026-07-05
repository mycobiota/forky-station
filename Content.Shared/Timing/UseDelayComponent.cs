using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Timing;

/// <summary>
/// Timer that creates a cooldown each time an object is activated/used.
/// Can support additional, separate cooldown timers on the object by passing a unique ID with the system methods.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
[Access(typeof(UseDelaySystem))]
public sealed partial class UseDelayComponent : Component
{
    [DataField]
    public Dictionary<string, UseDelayInfo> Delays = [];

    /// <summary>
    /// Default delay time.
    /// </summary>
    /// <remarks>
    /// This is only used at MapInit and should not be expected
    /// to reflect the length of the default delay after that.
    /// Use <see cref="UseDelaySystem.TryGetDelayInfo"/> instead.
    /// </remarks>
    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Whether to hide the hand UI cooldown indicator for all delays.
    /// </summary>
    /// <remarks>
    /// Note that all delays on the component will not have a UI indicator
    /// if this is set to true.
    /// </remarks>
    [DataField]
    public bool Hidden = false;
}

[Serializable, NetSerializable]
public sealed class UseDelayComponentState : IComponentState
{
    public Dictionary<string, UseDelayInfo> Delays = new();
}

[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class UseDelayInfo
{
    [DataField]
    public TimeSpan Length { get; set; }
    [DataField]
    public TimeSpan StartTime { get; set; }
    [DataField]
    public TimeSpan EndTime { get; set; }
    [DataField]
    public bool Hidden { get; set; }

    public UseDelayInfo(TimeSpan length, TimeSpan startTime = default, TimeSpan endTime = default, bool hidden = false)
    {
        Length = length;
        StartTime = startTime;
        EndTime = endTime;
        Hidden = hidden;
    }
}
