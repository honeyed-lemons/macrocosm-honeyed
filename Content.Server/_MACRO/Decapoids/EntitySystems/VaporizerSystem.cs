using Content.Shared._MACRO.Decapoids;
using Content.Shared._MACRO.Decapoids.Components;
using Content.Shared._MACRO.Decapoids.EntitySystems;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Server._MACRO.Decapoids.EntitySystems;

public sealed class VaporizerSystem : SharedVaporizerSystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void AdjustTankMoles(VaporizerComponent vaporizer, GasTankComponent gasTank, float volumeConsumed)
    {
        gasTank.Air.AdjustMoles(vaporizer.OutputGas, volumeConsumed * vaporizer.ReagentToMoles);
    }
}
