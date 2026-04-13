using Content.Server.Speech.EntitySystems;
using Content.Shared.Whitelist;

namespace Content.Server.Speech.Components;

/// <summary>
/// Entities with this component require a certain piece of equipment in a certain slot in order to speak.
/// </summary>
[RegisterComponent]
[Access(typeof(SpeechRequiresEquipmentSystem))]
public sealed partial class SpeechRequiresEquipmentComponent : Component
{
    /// <summary>
    /// Slot and EntityWhitelist for the equipment in said slot that this entity requires to speak.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, EntityWhitelist> Equipment;

    [DataField]
    public LocId? FailMessage;
}
