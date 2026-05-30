using Content.Server.Actions;
using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared._MACRO.Species.Kodepiia;
using Content.Shared._MACRO.Species.Kodepiia.Components;
using Content.Shared.Body;
using Content.Shared.DoAfter;
using Content.Shared.Humanoid;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Preferences;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server._MACRO.Species.Kodepiia;

public sealed partial class KodepiiaScramblerSystem : SharedKodepiiaScramblerSystem
{
    [Dependency] private ActionsSystem _actionsSystem = default!;
    [Dependency] private HumanoidProfileSystem _humanoidProfile = default!;
    [Dependency] private SharedVisualBodySystem _visualBody = default!;
    [Dependency] private PopupSystem _popup = default!;
    [Dependency] private DoAfterSystem _doAfter = default!;
    [Dependency] private SharedAudioSystem _audio = default!;
    private const string OnScrambleStart = "kodepiia-scramble-others";
    private const string OnScrambleCompleted = "kodepiia-scramble-self";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<KodepiiaScramblerComponent, KodepiiaScramblerEvent>(Scramble);
        SubscribeLocalEvent<KodepiiaScramblerComponent, KodepiiaScramblerDoAfterEvent>(OnScrambleDoAfter);
    }
    private void Scramble(Entity<KodepiiaScramblerComponent> ent, ref KodepiiaScramblerEvent args)
    {
        var doargs = new DoAfterArgs(EntityManager, ent, 4, new KodepiiaScramblerDoAfterEvent(), ent)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
        };
        var popupOthers = Loc.GetString(OnScrambleStart, ("name", Identity.Entity(ent, EntityManager)), ("ent", ent));
        _popup.PopupEntity(popupOthers, ent, Filter.Pvs(ent).RemovePlayersByAttachedEntity(ent), true, PopupType.MediumCaution);
        _audio.PlayPvs(ent.Comp.ScramblerSound, ent);
        _doAfter.TryStartDoAfter(doargs);
        args.Handled = true;
    }

    private void OnScrambleDoAfter(Entity<KodepiiaScramblerComponent> ent, ref KodepiiaScramblerDoAfterEvent args)
    {
        if (args.Cancelled)
        {
            _actionsSystem.SetCooldown(ent.Comp.ScramblerAction, TimeSpan.FromSeconds(10));
            return;
        }

        if (args.Handled)
            return;

        if (!TryComp<HumanoidProfileComponent>(ent, out var humanoid))
            return;

        var popupSelf = Loc.GetString(OnScrambleCompleted, ("name", Identity.Entity(ent, EntityManager)));
        var profile = HumanoidCharacterProfile.RandomWithSpecies(humanoid.Species);

        _visualBody.ApplyProfileTo(ent.Owner, profile);
        _humanoidProfile.ApplyProfileTo(ent.Owner, profile);
        _popup.PopupEntity(popupSelf, ent, ent);
        args.Handled = true;
    }
}
