using System.ComponentModel.DataAnnotations;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._MACRO.Body.Components;
/// <summary>
/// Organs with this component equip entities to certain slots that persist when the organ is removed.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EquipmentOrganComponent : Component
{
    /// <summary>
    /// The equipment to place in said slot.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, EquipmentItem> Equipment;

    /// <summary>
    /// A list of all regular equipment spawned by this component.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public List<StoredEquipmentData> StoredEquipment = [];

    /// <summary>
    /// The container ID used to store equipment.
    /// </summary>
    [DataField]
    public string ContainerId = "item-action-item-container";
}

[Serializable, NetSerializable]
public record struct StoredEquipmentData
{
    public string Slot;
    public bool HandEquipment;
    public NetEntity Uid;
};

[DataDefinition, Serializable, NetSerializable]
public partial struct EquipmentItem(bool handEquipment, EntProtoId prototype)
{
    [DataField]
    public bool HandEquipment = handEquipment;
    [DataField]
    public EntProtoId Prototype = prototype;

    public EquipmentItem() : this(false, string.Empty) { }
}
