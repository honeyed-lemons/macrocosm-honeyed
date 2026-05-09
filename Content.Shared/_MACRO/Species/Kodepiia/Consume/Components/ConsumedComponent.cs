using Robust.Shared.GameStates;

namespace Content.Shared._MACRO.Species.Kodepiia.Consume.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ConsumedComponent : Component
{
    /// <summary>
    /// Consumed value, added to whenever a consumer consumes the consumed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ConsumedValue;
}
