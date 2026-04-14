using Content.Shared._MACRO.Body.Components;
using Content.Shared.Body;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Content.Shared.Slippery;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Shared._MACRO.Body.EntitySystems;

public sealed class EquipmentOrganSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;

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
        foreach (var (slot, entityPrototype) in ent.Comp.Equipment)
        {
            InsertEquipment(ent, false, slot, entityPrototype);
        }
        // Spawn hand equipment
        foreach (var (handId, entityPrototype) in ent.Comp.HandEquipment)
        {
            InsertEquipment(ent, true, handId, entityPrototype);
        }
    }

    private void InsertEquipment(
        Entity<EquipmentOrganComponent> ent,
        bool hand,
        string slot,
        EntProtoId entityPrototype)
    {
        if (!PredictedTrySpawnInContainer(
                entityPrototype,
                ent.Owner,
                ent.Comp.ContainerId,
                out var item))
            return;

        if (hand == false)
            ent.Comp.StoredEquipment.Add(slot, item.Value);
        else
            ent.Comp.StoredHandEquipment.Add(slot, item.Value);

        Dirty(ent);
    }
    private void OnGotInserted(Entity<EquipmentOrganComponent> ent, ref OrganGotInsertedEvent args)
    {
        foreach (var (slot, item) in ent.Comp.StoredEquipment)
        {
            _inventory.TryEquip(args.Target, item, slot, predicted:true, silent: true, force: true);
            EnsureComp<UnremoveableComponent>(item);
        }

        foreach (var (handId, item)in ent.Comp.StoredHandEquipment)
        {
            _hands.TryForcePickup(args.Target, item, handId, checkActionBlocker: false);
            EnsureComp<UnremoveableComponent>(item);
        }
    }

    private void OnGotRemoved(Entity<EquipmentOrganComponent> ent, ref OrganGotRemovedEvent args)
    {
        var container = _container.EnsureContainer<Container>(ent, ent.Comp.ContainerId);

        foreach (var (_, item) in ent.Comp.StoredEquipment)
        {
            RemComp<UnremoveableComponent>(item);
            _container.Insert(item, container);
        }

        foreach (var (_, item) in ent.Comp.StoredHandEquipment)
        {
            RemComp<UnremoveableComponent>(item);
            _container.Insert(item, container);
        }
    }
}
