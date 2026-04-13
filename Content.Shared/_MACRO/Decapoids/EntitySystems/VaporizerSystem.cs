using Content.Shared._MACRO.Decapoids.Components;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Shared._MACRO.Decapoids.EntitySystems;

public sealed class VaporizerSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    private const int ExaminePriority = 1;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<VaporizerComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<VaporizerComponent> ent, ref ExaminedEvent args)
    {
        switch (ent.Comp)
        {
            case { State: VaporizerState.Normal }:
                args.PushMarkup(Loc.GetString("vaporizer-examine-state-normal"), ExaminePriority);
                break;
            case { State: VaporizerState.LowSolution }:
                args.PushMarkup(Loc.GetString("vaporizer-examine-state-low"), ExaminePriority);
                break;
            case { State: VaporizerState.BadSolution }:
                args.PushMarkup(Loc.GetString("vaporizer-examine-state-bad"), ExaminePriority);
                break;
            case { State: VaporizerState.Empty }:
                args.PushMarkup(Loc.GetString("vaporizer-examine-state-empty"), ExaminePriority);
                break;
        }
    }
    /// <summary>
    /// Get the fill state of a vaporizer's solution.
    /// </summary>
    /// <param name="ent">Vaporizer entity</param>
    /// <param name="solution">Solution to get fill level of.</param>
    /// <returns></returns>
    private static VaporizerState GetVaporizerState(Entity<VaporizerComponent> ent, Solution solution)
    {
        var vaporizer = ent.Comp;
        var state = VaporizerState.Empty;
        var consumeAmount = FixedPoint2.Zero;

        foreach (var reagent in solution.Contents)
        {
            if (reagent.Reagent.Prototype != vaporizer.ExpectedReagent)
                return VaporizerState.BadSolution;

            consumeAmount += reagent.Quantity;

            state = consumeAmount / solution.MaxVolume <= vaporizer.LowPercentage
                ? VaporizerState.LowSolution
                : VaporizerState.Normal;
        }

        return state;
    }
    /// <summary>
    /// Convert a portion of reagent inside of the vaporizer to gas.
    /// </summary>
    /// <param name="ent">Vaporizer to process.</param>
    /// <param name="gasTank">Gas Tank component to add to.</param>
    /// <param name="solutionManager">Solution Manager to get the solution from.</param>
    private void ProcessVaporizerTank(Entity<VaporizerComponent> ent, GasTankComponent gasTank, SolutionContainerManagerComponent solutionManager)
    {
        if (!_solution.TryGetSolution((ent, solutionManager), ent.Comp.LiquidTank, out var solutionEnt, out var solution))
            return;

        var state = GetVaporizerState(ent, solution);
        ent.Comp.State = state;
        // If the air pressure is less than max AND the state is low or normal
        if (gasTank.Air.Pressure < ent.Comp.MaxPressure && state is VaporizerState.LowSolution or VaporizerState.Normal)
        {
            // Split off the reagents consumed
            var reagentConsumed = _solution.SplitSolution(
                solutionEnt.Value,
                ent.Comp.ReagentPerSecond * ent.Comp.ProcessDelay.TotalSeconds);
            // Add gas to the gas tank
            gasTank.Air.AdjustMoles(ent.Comp.OutputGas, (float)reagentConsumed.Volume * ent.Comp.ReagentToMoles);
        }

        UpdateVisualState(ent, state);
    }

    private void UpdateVisualState(EntityUid uid, VaporizerState state, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref appearance))
            return;

        _appearance.SetData(uid, VaporizerVisuals.Indicator, state);
    }

    public override void Update(float frameTime)
    {
        var enumerator = EntityQueryEnumerator<VaporizerComponent, GasTankComponent, SolutionContainerManagerComponent>();

        while (enumerator.MoveNext(out var uid, out var vaporizer, out var gasTank, out var solutionManager))
        {
            if (_gameTiming.CurTime < vaporizer.NextProcess)
                continue;

            ProcessVaporizerTank((uid, vaporizer), gasTank, solutionManager);
            vaporizer.NextProcess = _gameTiming.CurTime + vaporizer.ProcessDelay;
        }
    }
}
