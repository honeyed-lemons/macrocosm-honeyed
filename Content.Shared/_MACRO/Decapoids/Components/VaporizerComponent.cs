using Content.Shared._MACRO.Decapoids;
using Content.Shared.Atmos;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._MACRO.Decapoids.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[AutoGenerateComponentPause]
public sealed partial class VaporizerComponent : Component
{
    /// <summary>
    /// Solution name.
    /// </summary>
    [DataField]
    public string LiquidTank = "waterTank";
    /// <summary>
    /// Expected Reagent to process into gas.
    /// </summary>
    [DataField]
    public ProtoId<ReagentPrototype> ExpectedReagent = "Water";

    [DataField]
    public Gas OutputGas = Gas.WaterVapor;

    [DataField]
    public FixedPoint2 MaxPressure = Atmospherics.OneAtmosphere * 10;

    [DataField]
    public float ReagentToMoles = 0.07f;

    [DataField]
    public FixedPoint2 ReagentPerSecond = 0.09;

    [DataField]
    public TimeSpan ProcessDelay = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// A percentage for how filled the liquid tank should be before it is considered "Low"
    /// </summary>
    [DataField]
    public float LowPercentage = 0.2f;

    [DataField(readOnly: true), ViewVariables(VVAccess.ReadOnly)]
    [AutoPausedField]
    public TimeSpan NextProcess = TimeSpan.Zero;

    [DataField, AutoNetworkedField]
    public VaporizerState State = VaporizerState.Empty;
}
