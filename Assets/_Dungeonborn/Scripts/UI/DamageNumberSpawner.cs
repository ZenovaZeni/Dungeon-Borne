using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    public sealed class DamageNumberSpawner : MonoBehaviour
    {
        [SerializeField] private TextMeshPro damageNumberPrefab;

        public void Spawn(Vector3 position, float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            var number = damageNumberPrefab != null
                ? Instantiate(damageNumberPrefab, position, Quaternion.identity)
                : CreateFallbackDamageNumber(position);

            number.text = Mathf.RoundToInt(amount).ToString();
        }

        private static TextMeshPro CreateFallbackDamageNumber(Vector3 position)
        {
            var numberObject = new GameObject("FallbackDamageNumber");
            numberObject.transform.position = position;
            var number = numberObject.AddComponent<TextMeshPro>();
            number.alignment = TextAlignmentOptions.Center;
            number.fontSize = 3f;
            number.color = Color.white;
            numberObject.AddComponent<FloatingDamageNumber>();
            return number;
        }
    }
}
