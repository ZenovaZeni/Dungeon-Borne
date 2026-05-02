using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    public sealed class DamageNumberSpawner : MonoBehaviour
    {
        [SerializeField] private TextMeshPro damageNumberPrefab;

        public void Spawn(Vector3 position, float amount)
        {
            if (damageNumberPrefab == null || amount <= 0f)
            {
                return;
            }

            var number = Instantiate(damageNumberPrefab, position, Quaternion.identity);
            number.text = Mathf.RoundToInt(amount).ToString();
        }
    }
}
