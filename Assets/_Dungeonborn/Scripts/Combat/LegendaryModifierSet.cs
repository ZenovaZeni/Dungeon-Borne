using System;
using System.Collections.Generic;

namespace Dungeonborn.Combat
{
    [Serializable]
    public sealed class LegendaryModifierSet
    {
        private readonly HashSet<LegendaryModifier> modifiers = new HashSet<LegendaryModifier>();

        public LegendaryModifierSet()
        {
        }

        public LegendaryModifierSet(params LegendaryModifier[] startingModifiers)
        {
            foreach (var modifier in startingModifiers)
            {
                Add(modifier);
            }
        }

        public void Add(LegendaryModifier modifier)
        {
            if (modifier != LegendaryModifier.None)
            {
                modifiers.Add(modifier);
            }
        }

        public bool Has(LegendaryModifier modifier)
        {
            return modifier != LegendaryModifier.None && modifiers.Contains(modifier);
        }
    }
}
