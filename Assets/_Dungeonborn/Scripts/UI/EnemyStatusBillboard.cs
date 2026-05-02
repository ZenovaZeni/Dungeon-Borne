using Dungeonborn.Characters;
using Dungeonborn.Enemies;
using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    [RequireComponent(typeof(Damageable))]
    public sealed class EnemyStatusBillboard : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0f, 2.35f, 0f);
        [SerializeField] private float labelFontSize = 1.4f;
        [SerializeField] private float barWidth = 1.4f;
        [SerializeField] private float barHeight = 0.12f;

        private Damageable damageable;
        private EnemyBrain enemyBrain;
        private Transform labelRoot;
        private TextMeshPro label;
        private Transform healthFill;
        private global::UnityEngine.Camera mainCamera;

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            enemyBrain = GetComponent<EnemyBrain>();
            CreateBillboard();
        }

        private void LateUpdate()
        {
            if (damageable == null || damageable.IsDead)
            {
                if (labelRoot != null)
                {
                    labelRoot.gameObject.SetActive(false);
                }

                return;
            }

            if (labelRoot != null && !labelRoot.gameObject.activeSelf)
            {
                labelRoot.gameObject.SetActive(true);
            }

            mainCamera ??= global::UnityEngine.Camera.main;
            if (mainCamera != null && labelRoot != null)
            {
                labelRoot.rotation = Quaternion.LookRotation(labelRoot.position - mainCamera.transform.position, Vector3.up);
            }

            var healthPercent = Mathf.Clamp01(damageable.CurrentHealth / damageable.MaxHealth);
            healthFill.localScale = new Vector3(barWidth * healthPercent, barHeight, 0.04f);
            healthFill.localPosition = new Vector3(-(barWidth - healthFill.localScale.x) * 0.5f, -0.26f, 0f);
        }

        private void CreateBillboard()
        {
            var labelObject = new GameObject("Enemy Name");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = offset;
            labelRoot = labelObject.transform;
            label = labelObject.AddComponent<TextMeshPro>();
            label.alignment = TextAlignmentOptions.Center;
            label.fontSize = labelFontSize;
            label.color = Color.white;
            label.text = enemyBrain != null ? enemyBrain.DisplayName : gameObject.name;

            var barBack = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barBack.name = "Enemy Health Back";
            barBack.transform.SetParent(labelObject.transform);
            barBack.transform.localPosition = new Vector3(0f, -0.26f, 0.01f);
            barBack.transform.localScale = new Vector3(barWidth, barHeight, 0.04f);
            Destroy(barBack.GetComponent<Collider>());
            barBack.GetComponent<Renderer>().material.color = new Color(0.08f, 0.02f, 0.02f);

            var barFill = GameObject.CreatePrimitive(PrimitiveType.Cube);
            barFill.name = "Enemy Health Fill";
            barFill.transform.SetParent(labelObject.transform);
            barFill.transform.localPosition = new Vector3(0f, -0.26f, 0f);
            barFill.transform.localScale = new Vector3(barWidth, barHeight, 0.04f);
            Destroy(barFill.GetComponent<Collider>());
            barFill.GetComponent<Renderer>().material.color = new Color(0.9f, 0.08f, 0.06f);
            healthFill = barFill.transform;
        }
    }
}
