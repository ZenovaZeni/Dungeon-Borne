using Dungeonborn.Combat;
using UnityEngine;

namespace Dungeonborn.Loot
{
    public enum LootRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    [CreateAssetMenu(menuName = "Dungeonborn/Loot/Loot Item")]
    public sealed class LootItemDefinition : ScriptableObject
    {
        [SerializeField] private string itemId = "item";
        [SerializeField] private string displayName = "Item";
        [SerializeField] private LootRarity rarity = LootRarity.Common;
        [SerializeField] private LegendaryModifier legendaryModifier = LegendaryModifier.None;
        [SerializeField] private Color beamColor = Color.white;

        public string ItemId => itemId;
        public string DisplayName => displayName;
        public LootRarity Rarity => rarity;
        public LegendaryModifier LegendaryModifier => legendaryModifier;
        public Color BeamColor => beamColor;
    }
}
