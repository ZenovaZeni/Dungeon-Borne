using Dungeonborn.Characters;
using UnityEngine;

namespace Dungeonborn.Loot
{
    [RequireComponent(typeof(Damageable))]
    public sealed class LootDropper : MonoBehaviour
    {
        [SerializeField] private LootPickup pickupPrefab;
        [SerializeField] private LootItemDefinition guaranteedDrop;

        private void Awake()
        {
            GetComponent<Damageable>().Died.AddListener(DropLoot);
        }

        private void DropLoot()
        {
            if (pickupPrefab == null || guaranteedDrop == null)
            {
                return;
            }

            var pickup = Instantiate(pickupPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            pickup.Configure(guaranteedDrop);
        }
    }
}
