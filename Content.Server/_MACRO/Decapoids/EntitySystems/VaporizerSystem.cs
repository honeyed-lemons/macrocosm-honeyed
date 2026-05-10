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
    public override void AdjustTankMoles(VaporizerComponent vaporizer, GasTankComponent gasTank, float volumeConsumed)
    {
        gasTank.Air.AdjustMoles(vaporizer.OutputGas, volumeConsumed * vaporizer.ReagentToMoles);
    }
}
