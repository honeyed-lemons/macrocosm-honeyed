using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._MACRO.Species.Kodepiia.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class KodepiiaScramblerComponent : Component
{
    [DataField]
    public EntityUid? ScramblerAction;

    [DataField]
    public string? ScramblerActionId = "ActionKodepiiaScrambler";

    [DataField]
    public SoundSpecifier ScramblerSound = new SoundPathSpecifier("/Audio/_MACRO/Voice/Kodepiia/kodescramble/kodescramble.ogg");

    [DataField]
    public LocId OnScrambleStart = "kodepiia-scramble-others";

    [DataField]
    public LocId OnScrambleCompleted = "kodepiia-scramble-self";
}
