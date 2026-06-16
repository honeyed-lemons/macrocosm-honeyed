using Robust.Shared.Configuration;

namespace Content.Shared._MACRO.CCVar;

/// <summary>
/// Contains miscellaneous CCVars used in content.
/// </summary>
public sealed partial class MacroCCVars
{
    /// <summary>
    ///     How many times an entity must be consumed before they gib
    ///     12 by default, set to 0 to disable.
    /// </summary>
    public static readonly CVarDef<int> ConsumptionGibThreshold =
        CVarDef.Create("consumption.gibthreshold", 12, CVar.SERVERONLY);
}
