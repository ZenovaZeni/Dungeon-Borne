using Dungeonborn.Combat;
using UnityEngine;

namespace Dungeonborn.Characters
{
    public sealed class PlayerLegendaryModifiers : MonoBehaviour
    {
        private readonly LegendaryModifierSet modifiers = new LegendaryModifierSet();

        public bool Has(LegendaryModifier modifier)
        {
            return modifiers.Has(modifier);
        }

        public void Add(LegendaryModifier modifier)
        {
            modifiers.Add(modifier);
        }
    }
}
