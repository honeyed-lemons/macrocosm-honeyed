using Content.Shared.Speech;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server._MACRO.Speech.Components;

/// <summary>
/// When put on a piece of clothing, modifies the wearer's
/// speech sounds
/// </summary>
[RegisterComponent]
public sealed partial class SpeechSoundComponent : Component
{
    [DataField]
    public ProtoId<SpeechSoundsPrototype>? SpeechSounds;

    [DataField]
    public ProtoId<SpeechVerbPrototype>? SpeechVerb;
}
