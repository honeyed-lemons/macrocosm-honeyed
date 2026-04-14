using Robust.Shared.Prototypes;

namespace Content.Shared._MACRO.Body.Components;

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class EquipmentOrganComponent : Component
{
    /// <summary>
    /// The hand ID and entity to place in said hand.
    /// </summary>
    [DataField]
    public Dictionary<string, EntProtoId> HandEquipment = new();

    /// <summary>
    /// The slot and entity to place in said slot.
    /// </summary>
    [DataField]
    public Dictionary<string, EntProtoId> Equipment = new();


    /// <summary>
    /// A list of all regular equipment spawned by this component.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<string, EntityUid> StoredEquipment = [];

    /// <summary>
    /// A list of all hand equipment spawned by this component.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<string, EntityUid> StoredHandEquipment = [];

    /// <summary>
    /// The container ID used to store equipment.
    /// </summary>
    [DataField]
    public string ContainerId = "item-action-item-container";
}
