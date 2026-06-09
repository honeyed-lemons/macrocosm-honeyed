using Content.Shared._MACRO.Species.Kodepiia.Components;
using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._MACRO.Species.Kodepiia.Consume;

public abstract partial class SharedConsumeSystem : EntitySystem
{

    [Dependency] private SharedActionsSystem _actionsSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<Components.ConsumeActionComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<Components.ConsumeActionComponent, ComponentShutdown>(OnShutdown);
    }

    public void OnShutdown(Entity<Components.ConsumeActionComponent> ent, ref ComponentShutdown args)
    {
        _actionsSystem.RemoveAction(ent.Owner, ent.Comp.ConsumeAction);
    }

    public void OnStartup(Entity<Components.ConsumeActionComponent> ent, ref ComponentStartup args)
    {
        _actionsSystem.AddAction(ent, ref ent.Comp.ConsumeAction, ent.Comp.ConsumeActionId);
    }
}

public sealed partial class ConsumeEvent : EntityTargetActionEvent;

[Serializable, NetSerializable]
public sealed partial class ConsumeDoAfterEvent : SimpleDoAfterEvent;