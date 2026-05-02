using Dungeonborn.Characters;
using TMPro;
using UnityEngine;

namespace Dungeonborn.Loot
{
    public sealed class LootPickup : MonoBehaviour
    {
        [SerializeField] private LootItemDefinition item;
        [SerializeField] private TextMeshPro label;
        [SerializeField] private float rotateSpeed = 90f;

        public void Configure(LootItemDefinition configuredItem)
        {
            item = configuredItem;
            if (label != null && item != null)
            {
                label.text = item.DisplayName;
                label.color = item.BeamColor;
            }
        }

        private void Start()
        {
            Configure(item);
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (item == null || !other.CompareTag("Player"))
            {
                return;
            }

            if (other.TryGetComponent<PlayerLegendaryModifiers>(out var modifiers))
            {
                modifiers.Add(item.LegendaryModifier);
            }

            Destroy(gameObject);
        }
    }
}
