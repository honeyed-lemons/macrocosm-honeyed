using Content.Shared.Inventory;
using Content.Shared.Speech;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chat;

/// <summary>
/// Similar to <seealso cref="TransformSpeakerNameEvent"/>, but for changing the speech
/// sounds of a speaking entity.
/// </summary>
public sealed class TransformSpeakerVoiceEvent(EntityUid sender) : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;
    public EntityUid Sender = sender;
    public ProtoId<SpeechSoundsPrototype>? SpeechSounds;
}
