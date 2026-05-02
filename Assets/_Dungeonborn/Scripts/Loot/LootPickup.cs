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
        [SerializeField] private float pickupRadius = 1.1f;

        private bool pickedUp;

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
            TryPickupNearestPlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (pickedUp || item == null || !other.CompareTag("Player"))
            {
                return;
            }

            Pickup(other.gameObject);
        }

        private void TryPickupNearestPlayer()
        {
            if (pickedUp || item == null)
            {
                return;
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }

            var distanceSquared = (player.transform.position - transform.position).sqrMagnitude;
            if (distanceSquared <= pickupRadius * pickupRadius)
            {
                Pickup(player);
            }
        }

        private void Pickup(GameObject player)
        {
            pickedUp = true;

            if (player.TryGetComponent<PlayerLegendaryModifiers>(out var modifiers))
            {
                modifiers.Add(item.LegendaryModifier);
            }

            Destroy(gameObject);
        }
    }
}
