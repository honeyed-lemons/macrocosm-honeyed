using Content.Shared._MACRO.Body.Components;
using Content.Shared.Body;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Robust.Shared.Containers;

namespace Content.Shared._MACRO.Body.EntitySystems;

public sealed class EquipmentOrganSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EquipmentOrganComponent, MapInitEvent>(OnMapInit);

        SubscribeLocalEvent<EquipmentOrganComponent, OrganGotInsertedEvent>(OnGotInserted);
        SubscribeLocalEvent<EquipmentOrganComponent, OrganGotRemovedEvent>(OnGotRemoved);
    }

    private void OnMapInit(Entity<EquipmentOrganComponent> ent, ref MapInitEvent args)
    {
        _container.EnsureContainer<Container>(ent, ent.Comp.ContainerId);

        // Spawn equipment
        foreach (var (slot, equipmentItem) in ent.Comp.Equipment)
        {
            InsertEquipment(ent, slot, equipmentItem);
        }
    }

    private void InsertEquipment(
        Entity<EquipmentOrganComponent> ent,
        string slot,
        EquipmentItem equipmentItem)
    {
        if (!PredictedTrySpawnInContainer(
                equipmentItem.Prototype,
                ent.Owner,
                ent.Comp.ContainerId,
                out var item))
            return;

        EnsureComp<OrganAttachedComponent>(item.Value, out var attachedComponent);
        attachedComponent.AttachedOrgan = ent;

        var equipData = new StoredEquipmentData
        {
            Slot = slot,
            HandEquipment = equipmentItem.HandEquipment,
            Uid = GetNetEntity(item.Value),
        };

        ent.Comp.StoredEquipment.Add(equipData);

        Dirty(ent);
    }
    private void OnGotInserted(Entity<EquipmentOrganComponent> ent, ref OrganGotInsertedEvent args)
    {
        foreach (var equipmentData in ent.Comp.StoredEquipment)
        {
            var item = GetEntity(equipmentData.Uid);

            if (!item.Valid)
                continue;

            if (equipmentData.HandEquipment)
            {
                _hands.TryForcePickup(
                    args.Target,
                    item,
                    equipmentData.Slot,
                    checkActionBlocker: false);
            }
            else
            {
                _inventory.TryEquip(args.Target,
                    item,
                    equipmentData.Slot,
                    predicted:true,
                    silent: true,
                    force: true);
            }

            EnsureComp<UnremoveableComponent>(item);
        }
    }

    private void OnGotRemoved(Entity<EquipmentOrganComponent> ent, ref OrganGotRemovedEvent args)
    {
        if (TerminatingOrDeleted(ent))
            return;

        var container = _container.EnsureContainer<Container>(ent, ent.Comp.ContainerId);

        foreach (var equipmentData in ent.Comp.StoredEquipment)
        {
            var item = GetEntity(equipmentData.Uid);

            if (!item.Valid)
                continue;

            RemComp<UnremoveableComponent>(item);
            _container.Insert(item, container);
        }
    }
}
