using Robust.Shared.GameStates;

namespace Content.Shared.Chemisry.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlockInjectionComponent : Component
{
    /// <summary>
    /// LocId of the popup shown when injection is blocked.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string FailurePopup = "block-injection-default";
}
