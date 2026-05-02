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
            number.fontSize = 4.6f;
            number.color = amount >= 25f ? new Color(1f, 0.25f, 0.15f) : new Color(1f, 0.92f, 0.15f);
            number.transform.localScale = Vector3.one * (amount >= 25f ? 1.25f : 1f);
        }

        private static TextMeshPro CreateFallbackDamageNumber(Vector3 position)
        {
            var numberObject = new GameObject("FallbackDamageNumber");
            numberObject.transform.position = position;
            var number = numberObject.AddComponent<TextMeshPro>();
            number.alignment = TextAlignmentOptions.Center;
            number.fontSize = 4.6f;
            number.color = new Color(1f, 0.92f, 0.15f);
            numberObject.AddComponent<FloatingDamageNumber>();
            return number;
        }
    }
}
