using System.Linq;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.Preferences;

public partial class HumanoidCharacterProfile
{

    /// <summary>
    /// Picks a random species from a weighted species list.
    /// <param name="weightedSpecies">List of species with weights to pick from.</param>
    /// <param name="ignoredSpecies">Species to exclude from randomizer.</param>
    /// </summary>
    public static SpeciesPrototype RandomSpeciesWeighted(
        ProtoId<WeightedRandomSpeciesPrototype> weightedSpecies,
        HashSet<string>? ignoredSpecies = null)
    {
        var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
        var random = IoCManager.Resolve<IRobustRandom>();

        var weights = prototypeManager.Index(weightedSpecies);
        // If there's a species blacklist in play, remove every single blacklisted species
        if (ignoredSpecies != null)
        {
            foreach (var pickedSpecies in ignoredSpecies)
            {
                weights.Weights.Remove(pickedSpecies);
            }
        }

        return prototypeManager.Index<SpeciesPrototype>(weights.Pick(random));
    }
}
