using Dungeonborn.Characters;
using Dungeonborn.Audio;
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
            if (guaranteedDrop == null)
            {
                return;
            }

            var pickup = pickupPrefab != null
                ? Instantiate(pickupPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity)
                : CreateFallbackPickup(transform.position + Vector3.up * 0.2f);

            pickup.Configure(guaranteedDrop);
            PrototypeAudio.PlayLootDrop(pickup.transform.position);
        }

        private static LootPickup CreateFallbackPickup(Vector3 position)
        {
            var pickupObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pickupObject.name = "FallbackLootPickup";
            pickupObject.transform.position = position;
            pickupObject.transform.localScale = Vector3.one * 0.7f;
            pickupObject.GetComponent<Renderer>().material.color = new Color(1f, 0.72f, 0.18f);
            pickupObject.GetComponent<Collider>().isTrigger = true;
            return pickupObject.AddComponent<LootPickup>();
        }
    }
}
