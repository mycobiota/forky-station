using Content.Shared.Radio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Funkystation.SISTR;

[Serializable, NetSerializable]
public sealed class AutomaTalkChatMessage(string message, ProtoId<RadioChannelPrototype> channel) : BoundUserInterfaceMessage
{
    public readonly string Message = message;
    public readonly ProtoId<RadioChannelPrototype> Channel = channel;
}
