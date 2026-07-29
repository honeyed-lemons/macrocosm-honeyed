using Robust.Shared.Configuration;

namespace Content.Shared._MACRO.CCVars;

[CVarDefs]
public sealed class MacroCCVars
{
    /// <summary>
    ///     The prototype to use for announcer weights.
    /// </summary>
    public static readonly CVarDef<string> AnnouncerWeightPrototype =
        CVarDef.Create("macro.announcer_weight_prototype", "Announcers", CVar.SERVERONLY);

    /// <summary>
    ///     The prototype to use for random species weights on entities that use randomized species..
    /// </summary>
    public static readonly CVarDef<string> RandomSpeciesWeightPrototype =
        CVarDef.Create("macro.random_species_weight_prototype", "VisitorSpeciesWeights", CVar.SERVERONLY);
}
